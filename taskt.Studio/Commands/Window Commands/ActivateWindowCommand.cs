﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.User32;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Window Commands")]
    [Description("This command activates an open window and brings it to the front.")]
    public class ActivateWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Window Name")]
        [InputSpecification("Select the name of the window to activate and bring forward.")]
        [SampleUsage("Untitled - Notepad || {vWindow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WindowName { get; set; }

        public ActivateWindowCommand()
        {
            CommandName = "ActivateWindowCommand";
            SelectionName = "Activate Window";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            string windowName = v_WindowName.ConvertUserVariableToString(engine);

            var targetWindows = User32Functions.FindTargetWindows(windowName);

            //loop each window
            foreach (var targetedWindow in targetWindows)
            {
                User32Functions.SetWindowState(targetedWindow, WindowState.SwShowNormal);
                User32Functions.SetForegroundWindow(targetedWindow);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultWindowControlGroupFor("v_WindowName", this, editor));

            return RenderedControls;
        }     

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Window '{v_WindowName}']";
        }
    }
}