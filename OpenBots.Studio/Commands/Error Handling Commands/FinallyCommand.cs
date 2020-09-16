using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Script;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Error Handling Commands")]
    [Description("This command defines a block of commands which are always executed after a try/catch block.")]
    public class FinallyCommand : ScriptCommand
    {
        public FinallyCommand()
        {
            CommandName = "FinallyCommand";
            SelectionName = "Finally";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender, ScriptAction parentCommand)
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