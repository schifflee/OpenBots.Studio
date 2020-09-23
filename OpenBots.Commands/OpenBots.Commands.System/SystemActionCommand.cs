using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.User32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;

namespace OpenBots.Commands.System
{
    [Serializable]
    [Group("System Commands")]
    [Description("This command performs a system action.")]
    public class SystemActionCommand : ScriptCommand
    {

        [PropertyDescription("System Action")]
        [PropertyUISelectionOption("Shutdown")]
        [PropertyUISelectionOption("Restart")]
        [PropertyUISelectionOption("Logoff")]
        [PropertyUISelectionOption("Lock Screen")]
        [InputSpecification("Select a system action from one of the options.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_ActionName { get; set; }

        public SystemActionCommand()
        {
            CommandName = "SystemActionCommand";
            SelectionName = "System Action";
            CommandEnabled = true;
            CustomRendering = true;
            v_ActionName = "Shutdown";
        }

        public override void RunCommand(object sender)
        {
            switch (v_ActionName)
            {
                case "Shutdown":
                    Process.Start("shutdown", "/s /t 0");
                    break;
                case "Restart":
                    Process.Start("shutdown", "/r /t 0");
                    break;
                case "Logoff":
                    User32Functions.WindowsLogOff();
                    break;
                case "Lock Screen":
                    User32Functions.LockWorkStation();
                    break;
                default:
                    break;
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ActionName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_ActionName}]";
        }
    }
}