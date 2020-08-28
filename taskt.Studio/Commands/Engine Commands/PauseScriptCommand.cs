using System;
using System.Collections.Generic;
using System.Threading;
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

namespace taskt.Commands
{
    [Serializable]
    [Group("Engine Commands")]
    [Description("This command pauses the script for a set amount of time specified in milliseconds.")]

    public class PauseScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Pause Time (Milliseconds)")]      
        [InputSpecification("Enter a specific amount of time in milliseconds.")]
        [SampleUsage("1000 || {vTime}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_PauseLength { get; set; }

        public PauseScriptCommand()
        {
            CommandName = "PauseScriptCommand";
            SelectionName = "Pause Script";
            CommandEnabled = true;
            CustomRendering = true;
            v_PauseLength = "1000";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var userPauseLength = v_PauseLength.ConvertUserVariableToString(engine);
            var pauseLength = int.Parse(userPauseLength);
            Thread.Sleep(pauseLength);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_PauseLength", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [For '{v_PauseLength}' Milliseconds]";
        }
    }
}