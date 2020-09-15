using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;
using taskt.Core.User32;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("System Commands")]
    [Description("This command performs a system action.")]
    public class SystemActionCommand : ScriptCommand
    {
        [XmlAttribute]
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

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ActionName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_ActionName}]";
        }
    }
}