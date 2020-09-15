﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Management;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("System Commands")]
    [Description("This command exclusively selects an OS variable.")]
    public class OSVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("OS Variable")]
        [InputSpecification("Select an OS variable from one of the options.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_OSVariableName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output OS Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public ComboBox VariableNameComboBox;

        [XmlIgnore]
        [NonSerialized]
        public Label VariableValue;

        public OSVariableCommand()
        {
            CommandName = "OSVariableCommand";
            SelectionName = "OS Variable";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var systemVariable = v_OSVariableName.ConvertUserVariableToString(engine);

            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();

            foreach (ManagementObject result in results)
            {
                foreach (PropertyData prop in result.Properties)
                {
                    if (prop.Name == systemVariable.ToString())
                    {
                        var sysValue = prop.Value.ToString();
                        sysValue.StoreInUserVariable(engine, v_OutputUserVariableName);
                        return;
                    }
                }
            }
            throw new Exception("System Property '" + systemVariable + "' not found!");
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            var ActionNameComboBoxLabel = CommandControls.CreateDefaultLabelFor("v_OSVariableName", this);
            VariableNameComboBox = (ComboBox)CommandControls.CreateDropdownFor("v_OSVariableName", this);

            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();

            foreach (ManagementObject result in results)
            {
                foreach (PropertyData prop in result.Properties)
                    VariableNameComboBox.Items.Add(prop.Name);
            }

            VariableNameComboBox.SelectedValueChanged += VariableNameComboBox_SelectedValueChanged;
            RenderedControls.Add(ActionNameComboBoxLabel);
            RenderedControls.Add(VariableNameComboBox);

            VariableValue = new Label();
            VariableValue.Font = new Font("Segoe UI Semilight", 12, FontStyle.Bold);
            VariableValue.ForeColor = Color.White;
            RenderedControls.Add(VariableValue);

            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));      

            return RenderedControls;
        }

        private void VariableNameComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            var selectedValue = VariableNameComboBox.SelectedItem;

            if (selectedValue == null)
                return;

            ObjectQuery wql = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(wql);
            ManagementObjectCollection results = searcher.Get();

            foreach (ManagementObject result in results)
            {
                foreach (PropertyData prop in result.Properties)
                {
                    if (prop.Name == selectedValue.ToString())
                    {
                        VariableValue.Text = "[ex. " + prop.Value + "]";
                        return;
                    }
                }
            }
            VariableValue.Text = "[ex. **Item not found**]";
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Store OS Variable '{v_OSVariableName}' in '{v_OutputUserVariableName}']";
        }
    }


}