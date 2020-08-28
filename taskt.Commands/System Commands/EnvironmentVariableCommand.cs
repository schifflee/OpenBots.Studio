using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
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
    [Description("This command allows you to exclusively select a system/environment variable")]
    [UsesDescription("Use this command to exclusively retrieve a system variable")]
    [ImplementationDescription("")]
    public class EnvironmentVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Select the required environment variable")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select from one of the options")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_EnvVariableName { get; set; }

        [XmlAttribute]
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
            var environmentVariable = (string)v_EnvVariableName.ConvertUserVariableToString(engine);

            var variables = Environment.GetEnvironmentVariables();
            var envValue = (string)variables[environmentVariable];

            envValue.StoreInUserVariable(engine, v_OutputUserVariableName);


        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            var ActionNameComboBoxLabel = CommandControls.CreateDefaultLabelFor("v_EnvVariableName", this);
            VariableNameComboBox = (ComboBox)CommandControls.CreateDropdownFor("v_EnvVariableName", this);


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
            VariableValue.Font = new Font("Segoe UI Semilight", 10, FontStyle.Bold);
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


            var variable = Environment.GetEnvironmentVariables();
            var value = variable[selectedValue];

            VariableValue.Text = "[ex. " + value + "]";


        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + " [Apply '" + v_EnvVariableName + "' to Variable '" + v_OutputUserVariableName + "']";
        }
    }

}