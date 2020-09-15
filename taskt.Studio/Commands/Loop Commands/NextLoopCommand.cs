﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;

namespace taskt.Commands
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