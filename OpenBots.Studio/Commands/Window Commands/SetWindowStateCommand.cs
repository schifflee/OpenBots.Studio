using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.User32;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Window Commands")]
    [Description("This command sets a open window's state.")]
    public class SetWindowStateCommand : ScriptCommand
    {

        [PropertyDescription("Window Name")]
        [InputSpecification("Select the name of the window to set.")]
        [SampleUsage("Untitled - Notepad || Current Window || {vWindow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WindowName { get; set; }

        [PropertyDescription("Window State")]
        [PropertyUISelectionOption("Maximize")]
        [PropertyUISelectionOption("Minimize")]
        [PropertyUISelectionOption("Restore")]
        [InputSpecification("Select the appropriate window state.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_WindowState { get; set; }

        public SetWindowStateCommand()
        {
            CommandName = "SetWindowStateCommand";
            SelectionName = "Set Window State";
            CommandEnabled = true;
            CustomRendering = true;
            v_WindowName = "Current Window";
            v_WindowState = "Maximize";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //convert window name
            string windowName = v_WindowName.ConvertUserVariableToString(engine);

            var targetWindows = User32Functions.FindTargetWindows(windowName);

            //loop each window and set the window state
            foreach (var targetedWindow in targetWindows)
            {
                WindowState WINDOW_STATE = WindowState.SwShowNormal;
                switch (v_WindowState)
                {
                    case "Maximize":
                        WINDOW_STATE = WindowState.SwMaximize;
                        break;

                    case "Minimize":
                        WINDOW_STATE = WindowState.SwMinimize;
                        break;

                    case "Restore":
                        WINDOW_STATE = WindowState.SwRestore;
                        break;

                    default:
                        break;
                }

                User32Functions.SetWindowState(targetedWindow, WINDOW_STATE);
            }     
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultWindowControlGroupFor("v_WindowName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_WindowState", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Window '{v_WindowName}' - Window State '{v_WindowState}']";
        }
    }
}