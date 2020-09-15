using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;

namespace taskt.Commands
{
    [Serializable]
    [Group("Error Handling Commands")]
    [Description("This command rethrows an exception caught in a catch block.")]
    public class RethrowCommand : ScriptCommand
    {
        public RethrowCommand()
        {
            CommandName = "RethrowCommand";
            SelectionName = "Rethrow";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            throw new Exception("Rethrowing Original Exception");
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