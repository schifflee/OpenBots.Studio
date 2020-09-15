using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.User32;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Misc Commands")]
    [Description("This command sets text to the user's clipboard.")]
    public class SetClipboardTextCommand : ScriptCommand
    {
        [XmlAttribute]
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

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Text '{v_TextToSet}']";
        }
    }
}
