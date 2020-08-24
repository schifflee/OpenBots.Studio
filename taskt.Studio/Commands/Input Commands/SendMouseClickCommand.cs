using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;
using taskt.Core.User32;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Input Commands")]
    [Description("This command simulates a mouse click of a given type.")]
    public class SendMouseClickCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Click Type")]
        [PropertyUISelectionOption("Left Click")]
        [PropertyUISelectionOption("Middle Click")]
        [PropertyUISelectionOption("Right Click")]
        [PropertyUISelectionOption("Left Down")]
        [PropertyUISelectionOption("Middle Down")]
        [PropertyUISelectionOption("Right Down")]
        [PropertyUISelectionOption("Left Up")]
        [PropertyUISelectionOption("Middle Up")]
        [PropertyUISelectionOption("Right Up")]
        [PropertyUISelectionOption("Double Left Click")]
        [InputSpecification("Indicate the type of click required.")]
        [SampleUsage("")]
        [Remarks("You can simulate a custom click by using multiple mouse click commands in succession, adding **Pause Command** in between where required.")]
        public string v_MouseClick { get; set; }

        public SendMouseClickCommand()
        {
            CommandName = "SendMouseClickCommand";
            SelectionName = "Send Mouse Click";
            CommandEnabled = true;
            CustomRendering = true;
            v_MouseClick = "Left Click";
        }

        public override void RunCommand(object sender)
        {
            var mousePosition = Cursor.Position;
            User32Functions.SendMouseClick(v_MouseClick, mousePosition.X, mousePosition.Y);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_MouseClick", this, editor));
       
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Click Type '{v_MouseClick}']";
        }
    }
}