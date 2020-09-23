using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Management;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OpenBots.Commands.System
{
    [Serializable]
    [Group("System Commands")]
    [Description("This command exclusively selects an OS variable.")]
    public class OSVariableCommand : ScriptCommand
    {
        [PropertyDescription("OS Variable")]
        [InputSpecification("Select an OS variable from one of the options.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_OSVariableName { get; set; }
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

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            var ActionNameComboBoxLabel = commandControls.CreateDefaultLabelFor("v_OSVariableName", this);
            VariableNameComboBox = (ComboBox)commandControls.CreateDropdownFor("v_OSVariableName", this);

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

            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));      

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