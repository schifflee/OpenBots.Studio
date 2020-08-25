using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Commands
{
    [Serializable]
    [Group("System Commands")]
    [Description("This command allows you to launch a remote desktop session.")]
    public class LaunchRemoteDesktopCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Machine Name")]
        [InputSpecification("Define the name of the machine to log on to.")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_MachineName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Username")]
        [InputSpecification("Define the username to use when connecting to the machine.")]
        [SampleUsage("myRobot || {vUsername}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_UserName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Password")]
        [InputSpecification("Define the password to use when connecting to the machine.")]
        [SampleUsage("password || {vPassword}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Password { get; set; }

        [XmlAttribute]
        [PropertyDescription("RDP Window Width")]
        [InputSpecification("Define the width for the Remote Desktop Window.")]
        [SampleUsage("1000 || {vWidth}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_RDPWidth { get; set; }

        [XmlAttribute]
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

            var result = ((frmScriptEngine)engine.TasktEngineUI).Invoke(new Action(() =>
            {
                engine.TasktEngineUI.LaunchRDPSession(machineName, userName, password, width, height);
            }));
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MachineName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_UserName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultPasswordInputGroupFor("v_Password", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RDPWidth", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RDPHeight", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Machine '{v_MachineName}']";
        }
    }
}