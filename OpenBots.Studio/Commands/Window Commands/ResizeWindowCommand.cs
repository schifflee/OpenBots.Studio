using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Newtonsoft.Json;
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
    [Description("This command resizes an open window to a specified size.")]
    public class ResizeWindowCommand : ScriptCommand
    {
        [PropertyDescription("Window Name")]
        [InputSpecification("Select the name of the window to resize.")]
        [SampleUsage("Untitled - Notepad || Current Window || {vWindow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WindowName { get; set; }

        [PropertyDescription("Width (Pixels)")]
        [InputSpecification("Input the new width size of the window.")]
        [SampleUsage("800 || {vWidth}")]
        [Remarks("Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid width range would be 0-1920.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_XWindowSize { get; set; }

        [PropertyDescription("Height (Pixels)")]
        [InputSpecification("Input the new height size of the window.")]
        [SampleUsage("500 || {vHeight}")]
        [Remarks("Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid height range would be 0-1080.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_YWindowSize { get; set; }

        [JsonIgnore]
        [NonSerialized]
        public ComboBox WindowNameControl;
        public ResizeWindowCommand()
        {
            CommandName = "ResizeWindowCommand";
            SelectionName = "Resize Window";
            CommandEnabled = true;
            CustomRendering = true;
            v_WindowName = "Current Window";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            string windowName = v_WindowName.ConvertUserVariableToString(engine);
            var variableXSize = v_XWindowSize.ConvertUserVariableToString(engine);
            var variableYSize = v_YWindowSize.ConvertUserVariableToString(engine);

            var targetWindows = User32Functions.FindTargetWindows(windowName);

            //loop each window and set the window state
            foreach (var targetedWindow in targetWindows)
            {
                User32Functions.SetWindowState(targetedWindow, WindowState.SwShowNormal);

                if (!int.TryParse(variableXSize, out int xPos))
                    throw new Exception("Width Invalid - " + v_XWindowSize);

                if (!int.TryParse(variableYSize, out int yPos))
                    throw new Exception("Height Invalid - " + v_YWindowSize);

                User32Functions.SetWindowSize(targetedWindow, xPos, yPos);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultWindowControlGroupFor("v_WindowName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_XWindowSize", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_YWindowSize", this, editor));
      
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Window '{v_WindowName}' - Target Size '({v_XWindowSize},{v_YWindowSize})']";
        }
    }
}