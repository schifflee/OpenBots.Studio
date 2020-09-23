using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenBots.Commands.System
{
    [Serializable]
    [Group("System Commands")]
    [Description("This command launches a remote desktop session.")]
    public class LaunchRemoteDesktopCommand : ScriptCommand
    {

        [PropertyDescription("Machine Name")]
        [InputSpecification("Define the name of the machine to log on to.")]
        [SampleUsage("myMachine || {vMachineName}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_MachineName { get; set; }

        [PropertyDescription("Username")]
        [InputSpecification("Define the username to use when connecting to the machine.")]
        [SampleUsage("myRobot || {vUsername}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_UserName { get; set; }

        [PropertyDescription("Password")]
        [InputSpecification("Define the password to use when connecting to the machine.")]
        [SampleUsage("password || {vPassword}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Password { get; set; }

        [PropertyDescription("RDP Window Width")]
        [InputSpecification("Define the width for the Remote Desktop Window.")]
        [SampleUsage("1000 || {vWidth}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_RDPWidth { get; set; }

        [PropertyDescription("RDP Window Height")]
        [InputSpecification("Define the height for the Remote Desktop Window.")]
        [SampleUsage("800 || {vHeight}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_RDPHeight { get; set; }

        public LaunchRemoteDesktopCommand()
        {
            CommandName = "LaunchRemoteDesktopCommand";
            SelectionName = "Launch Remote Desktop";
            CommandEnabled = true;
            CustomRendering = true;

            v_RDPWidth = SystemInformation.PrimaryMonitorSize.Width.ToString();
            v_RDPHeight = SystemInformation.PrimaryMonitorSize.Height.ToString();
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var machineName = v_MachineName.ConvertUserVariableToString(engine);
            var userName = v_UserName.ConvertUserVariableToString(engine);
            var password = v_Password.ConvertUserVariableToString(engine);
            var width = int.Parse(v_RDPWidth.ConvertUserVariableToString(engine));
            var height = int.Parse(v_RDPHeight.ConvertUserVariableToString(engine));

            var result = ((Form)engine.ScriptEngineUI).Invoke(new Action(() =>
            {
                engine.ScriptEngineUI.LaunchRDPSession(machineName, userName, password, width, height);
            }));
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MachineName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_UserName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultPasswordInputGroupFor("v_Password", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_RDPWidth", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_RDPHeight", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Machine '{v_MachineName}']";
        }
    }
}
