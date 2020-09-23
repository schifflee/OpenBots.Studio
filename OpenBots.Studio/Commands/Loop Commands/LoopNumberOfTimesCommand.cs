using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Loop Commands")]
    [Description("This command repeats the subsequent actions a specified number of times.")]
    public class LoopNumberOfTimesCommand : ScriptCommand
    {

        [PropertyDescription("Loop Count")]
        [InputSpecification("Enter the amount of times you would like to execute the encased commands.")]
        [SampleUsage("5 || {vLoopCount}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_LoopParameter { get; set; }

        [PropertyDescription("Start Index")]
        [InputSpecification("Enter the starting index of the loop.")]
        [SampleUsage("5 || {vStartIndex}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_LoopStart { get; set; }

        public LoopNumberOfTimesCommand()
        {
            CommandName = "LoopNumberOfTimesCommand";
            SelectionName = "Loop Number Of Times";
            CommandEnabled = true;
            CustomRendering = true;
            v_LoopStart = "0";
        }

        public override void RunCommand(object sender, ScriptAction parentCommand)
        {
            LoopNumberOfTimesCommand loopCommand = (LoopNumberOfTimesCommand)parentCommand.ScriptCommand;
            var engine = (AutomationEngineInstance)sender;

            int loopTimes;

            var loopParameter = loopCommand.v_LoopParameter.ConvertUserVariableToString(engine);
            loopTimes = int.Parse(loopParameter);

            int.TryParse(v_LoopStart.ConvertUserVariableToString(engine), out int startIndex);

            for (int i = startIndex; i < loopTimes; i++)
            {
                engine.ReportProgress("Starting Loop Number " + (i + 1) + "/" + loopTimes + " From Line " + loopCommand.LineNumber);

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                        return;

                    engine.ExecuteCommand(cmd);

                    if (engine.CurrentLoopCancelled)
                    {
                        engine.ReportProgress("Exiting Loop From Line " + loopCommand.LineNumber);
                        engine.CurrentLoopCancelled = false;
                        return;
                    }

                    if (engine.CurrentLoopContinuing)
                    {
                        engine.ReportProgress("Continuing Next Loop From Line " + loopCommand.LineNumber);
                        engine.CurrentLoopContinuing = false;
                        break;
                    }
                }
                engine.ReportProgress("Finished Loop From Line " + loopCommand.LineNumber);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_LoopParameter", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_LoopStart", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            if (v_LoopStart != "0")
            {
                return "Loop From (" + v_LoopStart + "+1) to " + v_LoopParameter;
            }
            else
            {
                return "Loop " +  v_LoopParameter + " Times";
            }
        }
    }
}