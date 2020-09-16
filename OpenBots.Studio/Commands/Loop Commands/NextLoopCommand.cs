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
    [Description("This command enables user to break and exit from the current loop.")]
    public class NextLoopCommand : ScriptCommand
    {
        public NextLoopCommand()
        {
            DefaultPause = 0;
            CommandName = "NextLoopCommand";
            SelectionName = "Next Loop";
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
            return base.GetDisplayValue();
        }
    }
}