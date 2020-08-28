using System;
using System.Collections.Generic;
using System.Threading;
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
    [Group("Window Commands")]
    [Description("This command waits for a window to exist.")]
    public class WaitForWindowToExistCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Window Name")]
        [InputSpecification("Select the name of the window to wait for.")]
        [SampleUsage("Untitled - Notepad || Current Window || {vWindow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Timeout (Seconds)")]
        [InputSpecification("Specify how many seconds to wait before throwing an exception.")]
        [SampleUsage("30 || {vSeconds}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Timeout { get; set; }

        public WaitForWindowToExistCommand()
        {
            CommandName = "WaitForWindowToExistCommand";
            SelectionName = "Wait For Window To Exist";
            CommandEnabled = true;
            CustomRendering = true;
            v_WindowName = "Current Window";
            v_Timeout = "30";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            string windowName = v_WindowName.ConvertUserVariableToString(engine);
            var timeout = int.Parse(v_Timeout.ConvertUserVariableToString(engine));

            var endDateTime = DateTime.Now.AddSeconds(timeout);

            IntPtr hWnd = IntPtr.Zero;

            while (DateTime.Now < endDateTime)
            {
                hWnd = User32Functions.FindWindow(windowName);

                if (hWnd != IntPtr.Zero) //If found
                    break;

                Thread.Sleep(1000);
            }

            if (hWnd == IntPtr.Zero)
                throw new Exception("Window was not found in the allowed time!");
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultWindowControlGroupFor("v_WindowName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Timeout", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Window '{v_WindowName}' - Timeout '{v_Timeout}']";
        }
    }
}
