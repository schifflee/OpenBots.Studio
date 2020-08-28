using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;

namespace taskt.Commands
{
    [Serializable]
    [Group("Task Commands")]
    [Description("This command stops the currently running task.")]

    public class StopCurrentTaskCommand : ScriptCommand
    {
        public StopCurrentTaskCommand()
        {
            CommandName = "StopCurrentTaskCommand";
            SelectionName = "Stop Current Task";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}