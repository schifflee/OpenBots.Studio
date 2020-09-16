using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Misc Commands")]
    [Description("This command adds an in-line comment to the script.")]
    public class AddCodeCommentCommand : ScriptCommand
    {
        public AddCodeCommentCommand()
        {
            CommandName = "AddCodeCommentCommand";
            SelectionName = "Add Code Comment";
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
            return $"// Comment ['{v_Comment}']";
        }
    }
}