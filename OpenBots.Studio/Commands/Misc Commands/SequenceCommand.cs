using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Script;
using OpenBots.Engine;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Misc Commands")]
    [Description("This command groups multiple actions together.")]
    public class SequenceCommand : ScriptCommand
    {
        public List<ScriptCommand> ScriptActions = new List<ScriptCommand>();

        public SequenceCommand()
        {
            CommandName = "SequenceCommand";
            SelectionName = "Sequence Command";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender, ScriptAction parentCommand)
        {
            var engine = (AutomationEngineInstance)sender;

            foreach (var item in ScriptActions)
            {
                //exit if cancellation pending
                if (engine.IsCancellationPending)
                    return;

                //only run if not commented
                if (!item.IsCommented)
                    item.RunCommand(engine);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{ScriptActions.Count()} Embedded Commands]";
        }
    }
}