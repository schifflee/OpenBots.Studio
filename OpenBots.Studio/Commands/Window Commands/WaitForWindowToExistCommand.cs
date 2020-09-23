using System;
using System.Collections.Generic;
using System.Threading;
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
    [Group("Window Commands")]
    [Description("This command waits for a window to exist.")]
    public class WaitForWindowToExistCommand : ScriptCommand
    {
        [PropertyDescription("Window Name")]
        [InputSpecification("Select the name of the window to wait for.")]
        [SampleUsage("Untitled - Notepad || Current Window || {vWindow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WindowName { get; set; }

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

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultWindowControlGroupFor("v_WindowName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Timeout", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Window '{v_WindowName}' - Timeout '{v_Timeout}']";
        }
    }
}
