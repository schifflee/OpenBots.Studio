using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;

namespace taskt.Commands
{
    [Serializable]
    [Group("Variable Commands")]
    [Description("This command modifies a variable.")]
    public class SetVariableCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Input Value")]
        [InputSpecification("Enter the input value for the variable.")]
        [SampleUsage("Hello || {vNum} || {vNum}+1")]
        [Remarks("You can use variables in input if you encase them within braces {vValue}. You can also perform basic math operations.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Input { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Data Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public SetVariableCommand()
        {
            CommandName = "SetVariableCommand";
            SelectionName = "Set Variable";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            dynamic input = v_Input.ConvertUserVariableToString(engine);

            if (input == v_Input && input.StartsWith("{") && input.EndsWith("}"))
                input = v_Input.ConvertUserVariableToObject(engine);

            ((object)input).StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Input", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Set '{v_Input}' to Variable '{v_OutputUserVariableName}']";
        }
    }
}