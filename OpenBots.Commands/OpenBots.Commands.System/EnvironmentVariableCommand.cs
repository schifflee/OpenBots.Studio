using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OpenBots.Commands.System
{
    [Serializable]
    [Group("System Commands")]
    [Description("This command exclusively selects an environment variable.")]
    public class EnvironmentVariableCommand : ScriptCommand
    {
        [PropertyDescription("Environment Variable")]
        [InputSpecification("Select an evironment variable from one of the options.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_EnvVariableName { get; set; }
        [PropertyDescription("Output Environment Variable")]
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

        public EnvironmentVariableCommand()
        {
            CommandName = "EnvironmentVariableCommand";
            SelectionName = "Environment Variable";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var environmentVariable = v_EnvVariableName.ConvertUserVariableToString(engine);

            var variables = Environment.GetEnvironmentVariables();
            var envValue = (string)variables[environmentVariable];

            envValue.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            var ActionNameComboBoxLabel = commandControls.CreateDefaultLabelFor("v_EnvVariableName", this);
            VariableNameComboBox = (ComboBox)commandControls.CreateDropdownFor("v_EnvVariableName", this);

            foreach (DictionaryEntry env in Environment.GetEnvironmentVariables())
            {
                var envVariableKey = env.Key.ToString();
                var envVariableValue = env.Value.ToString();
                VariableNameComboBox.Items.Add(envVariableKey);
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

            var variable = Environment.GetEnvironmentVariables();
            var value = variable[selectedValue];

            VariableValue.Text = "[ex. " + value + "]";
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Store Environment Variable '{v_EnvVariableName}' in '{v_OutputUserVariableName}']";
        }
    }

}
