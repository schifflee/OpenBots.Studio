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

namespace taskt.Commands
{
    [Serializable]
    [Group("Engine Commands")]
    [Description("This command sets delays between the execution of commands in a running instance.")]

    public class SetEngineDelayCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Command Delay Time (Milliseconds)")]
        [InputSpecification("Select or provide a specific amount of time in milliseconds.")]
        [SampleUsage("1000 || {vTime}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_EngineDelay { get; set; }

        public SetEngineDelayCommand()
        {
            CommandName = "SetEngineDelayCommand";
            SelectionName = "Set Engine Delay";
            CommandEnabled = true;
            CustomRendering = true;
            v_EngineDelay = "250";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var engineDelay = v_EngineDelay.ConvertUserVariableToString(engine);
            var delay = int.Parse(engineDelay);

            //update delay setting
            engine.EngineSettings.DelayBetweenCommands = delay;
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_EngineDelay", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Set Delay of '{v_EngineDelay} ms' Between Commands]";
        }
    }
}