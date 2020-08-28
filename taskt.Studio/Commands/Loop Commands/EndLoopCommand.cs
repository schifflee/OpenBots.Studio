using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;

namespace taskt.Commands
{

    [Serializable]
    [Group("Loop Commands")]
    [Description("This command signifies the exit point of Loop command(s) and is required for all the Loop commands.")]
    public class EndLoopCommand : ScriptCommand
    {
        public EndLoopCommand()
        {
            DefaultPause = 0;
            CommandName = "EndLoopCommand";
            SelectionName = "End Loop";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return "End Loop";
        }
    }
}