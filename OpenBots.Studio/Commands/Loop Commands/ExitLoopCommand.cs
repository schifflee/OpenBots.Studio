using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Loop Commands")]
    [Description("This command signifies that the current loop should exit and resume execution outside the current loop.")]
    public class ExitLoopCommand : ScriptCommand
    {
        public ExitLoopCommand()
        {
            DefaultPause = 0;
            CommandName = "ExitLoopCommand";
            SelectionName = "Exit Loop";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return "Exit Loop";
        }
    }
}