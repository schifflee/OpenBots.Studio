using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("If Commands")]
    [Description("This command declares the seperation between the actions based on the 'true' or 'false' condition.")]
    public class ElseCommand : ScriptCommand
    {
        public ElseCommand()
        {
            DefaultPause = 0;
            CommandName = "ElseCommand";
            SelectionName = "Else";
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
            return "Else";
        }
    }
}