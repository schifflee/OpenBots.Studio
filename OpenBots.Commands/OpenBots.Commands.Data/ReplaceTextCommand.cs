using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OpenBots.Commands.Data
{
    [Serializable]
    [Group("Data Commands")]
    [Description("This command replaces an existing substring in a string and saves the result in a variable.")]
    public class ReplaceTextCommand : ScriptCommand
    {
        [PropertyDescription("Text Data")]
        [InputSpecification("Provide a variable or text value.")]
        [SampleUsage("Hello John || {vTextData}")]
        [Remarks("Providing data of a type other than a 'String' will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputText { get; set; }
        [PropertyDescription("Old Text")]
        [InputSpecification("Specify the old value of the text that will be replaced.")]
        [SampleUsage("Hello || {vOldText}")]
        [Remarks("'Hello' in 'Hello John' would be targeted for replacement.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_OldText { get; set; }
        [PropertyDescription("New Text")]
        [InputSpecification("Specify the new value to replace the old value.")]
        [SampleUsage("Hi || {vNewText}")]
        [Remarks("'Hi' would be replaced with 'Hello' to form 'Hi John'.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_NewText { get; set; }
        [PropertyDescription("Output Text Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public ReplaceTextCommand()
        {
            CommandName = "ReplaceTextCommand";
            SelectionName = "Replace Text";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //get full text
            string replacementVariable = v_InputText.ConvertUserVariableToString(engine);

            //get replacement text and value
            string replacementText = v_OldText.ConvertUserVariableToString(engine);
            string replacementValue = v_NewText.ConvertUserVariableToString(engine);

            //perform replacement
            replacementVariable = replacementVariable.Replace(replacementText, replacementValue);

            //store in variable
            replacementVariable.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //create standard group controls
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputText", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_OldText", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_NewText", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Replace '{v_OldText}' With '{v_NewText}'- Store Text in '{v_InputText}']";
        }
    }
}