using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Switch Commands")]
    [Description("This command specifies the end of a switch block.")]
    public class EndSwitchCommand : ScriptCommand
    {
        public EndSwitchCommand()
        {
            CommandName = "EndSwitchCommand";
            SelectionName = "End Switch";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //no execution required, used as a marker by the Automation Engine
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
