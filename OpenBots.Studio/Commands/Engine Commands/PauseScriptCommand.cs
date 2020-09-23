using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Engine Commands")]
    [Description("This command pauses the script for a set amount of time specified in milliseconds.")]
    public class PauseScriptCommand : ScriptCommand
    {
        [PropertyDescription("Pause Time (Milliseconds)")]      
        [InputSpecification("Select or provide a specific amount of time in milliseconds.")]
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

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_PauseLength", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Pause for '{v_PauseLength} ms']";
        }
    }
}