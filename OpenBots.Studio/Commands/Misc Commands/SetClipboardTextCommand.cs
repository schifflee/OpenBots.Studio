using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.User32;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Misc Commands")]
    [Description("This command sets text to the user's clipboard.")]
    public class SetClipboardTextCommand : ScriptCommand
    {

        [PropertyDescription("Text")]
        [InputSpecification("Select or provide the text to set on the clipboard.")]
        [SampleUsage("Hello || {vTextToSet}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_TextToSet { get; set; }

        public SetClipboardTextCommand()
        {
            CommandName = "SetClipboardTextCommand";
            SelectionName = "Set Clipboard Text";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var input = v_TextToSet.ConvertUserVariableToString(engine);

            User32Functions.SetClipboardText(input);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Text '{v_TextToSet}']";
        }
    }
}
