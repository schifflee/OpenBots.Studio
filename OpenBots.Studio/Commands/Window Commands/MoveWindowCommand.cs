using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
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
    [Description("This command moves an open window to a specified location on screen.")]
    public class MoveWindowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Window Name")]
        [InputSpecification("Select the name of the window to move.")]
        [SampleUsage("Untitled - Notepad || Current Window || {vWindow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyDescription("X Position")]
        [InputSpecification("Input the new horizontal coordinate of the window. Starts from 0 on the left and increases going right.")]
        [SampleUsage("0 || {vXPosition}")]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range would be 0-1920.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowMouseCaptureHelper)]
        public string v_XMousePosition { get; set; }

        [XmlAttribute]
        [PropertyDescription("Y Position")]
        [InputSpecification("Input the new vertical coordinate of the window. Starts from 0 at the top and increases going down.")]
        [SampleUsage("0 || {vYPosition}")]
        [Remarks("This number is the pixel location on screen. Maximum value should be the maximum value allowed by your resolution. For 1920x1080, the valid range would be 0-1080.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowMouseCaptureHelper)]
        public string v_YMousePosition { get; set; }

        public MoveWindowCommand()
        {
            CommandName = "MoveWindowCommand";
            SelectionName = "Move Window";
            CommandEnabled = true;
            CustomRendering = true;
            v_WindowName = "Current Window";
            v_XMousePosition = "0";
            v_YMousePosition = "0";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            string windowName = v_WindowName.ConvertUserVariableToString(engine);
            var variableXPosition = v_XMousePosition.ConvertUserVariableToString(engine);
            var variableYPosition = v_YMousePosition.ConvertUserVariableToString(engine);

            var targetWindows = User32Functions.FindTargetWindows(windowName);

            //loop each window
            foreach (var targetedWindow in targetWindows)
            {
                User32Functions.SetWindowState(targetedWindow, WindowState.SwShowNormal);
            
                if (!int.TryParse(variableXPosition, out int xPos))
                    throw new Exception("X Position Invalid - " + v_XMousePosition);

                if (!int.TryParse(variableYPosition, out int yPos))
                    throw new Exception("Y Position Invalid - " + v_YMousePosition);

                User32Functions.SetWindowPosition(targetedWindow, xPos, yPos);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultWindowControlGroupFor("v_WindowName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_XMousePosition", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_YMousePosition", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Window '{v_WindowName}' - Target Coordinates '({v_XMousePosition},{v_YMousePosition})']";
        }
    }
}