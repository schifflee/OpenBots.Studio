using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Group = OpenBots.Core.Attributes.ClassAttributes.Group;

namespace OpenBots.Commands.RegEx
{
    [Serializable]
    [Group("Regex Commands")]
    [Description("This command replaces all the matches in a given text based on a Regex pattern.")]
    public class RegexReplaceCommand : ScriptCommand
    {

        [PropertyDescription("Text")]
        [InputSpecification("Select or provide text to apply Regex on.")]
        [SampleUsage("Hello || {vText}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputText { get; set; }

        [PropertyDescription("Regex Pattern")]
        [InputSpecification("Enter a Regex Pattern to apply to the input Text.")]
        [SampleUsage(@"^([\w\-]+) || {vPattern}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Regex { get; set; }

        [PropertyDescription("Replacement Text")]
        [InputSpecification("Selevt or provide text to replace the matches.")]
        [SampleUsage("Goodbye || {vReplacement}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]

        public string v_ReplacementText { get; set; }

        [PropertyDescription("Output Result Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public RegexReplaceCommand()
        {
            CommandName = "RegexReplaceCommand";
            SelectionName = "Regex Replace";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInputData = v_InputText.ConvertUserVariableToString(engine);
            string vRegex = v_Regex.ConvertUserVariableToString(engine);
            string vReplaceData = v_ReplacementText.ConvertUserVariableToString(engine);
            string resultData = Regex.Replace(vInputData, vRegex, vReplaceData);

            resultData.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputText", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Regex", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ReplacementText", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Text '{v_InputText}' - Regex Pattern '{v_Regex}' - Replacement '{v_ReplacementText}' - Store Result in '{v_OutputUserVariableName}']";
        }
    }
}
