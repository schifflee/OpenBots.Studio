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
    [Group("Engine Commands")]
    [Description("This command displays an engine context message to the user.")]
    public class ShowEngineContextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Close After X (Seconds)")]
        [InputSpecification("Specify how many seconds to display the message on screen. After the specified time," +
                            "\nthe message box will be automatically closed and script will resume execution.")]
        [SampleUsage("0 || 5 || {vSeconds})")]
        [Remarks("Set value to 0 to remain open indefinitely.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_AutoCloseAfter { get; set; }

        public ShowEngineContextCommand()
        {
            CommandName = "ShowEngineContextCommand";
            SelectionName = "Show Engine Context";
            CommandEnabled = true;
            CustomRendering = true;
            v_AutoCloseAfter = "0";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            int closeAfter = int.Parse(v_AutoCloseAfter.ConvertUserVariableToString(engine));

            if (engine.TasktEngineUI == null)
                return;

            //automatically close messageboxes for server requests
            if (engine.ServerExecution && closeAfter <= 0)
                v_AutoCloseAfter = "10";

            var result = ((frmScriptEngine)engine.TasktEngineUI).Invoke(new Action(() =>
                {
                    engine.TasktEngineUI.ShowEngineContext(engine.GetEngineContext(), closeAfter);
                }
            ));
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_AutoCloseAfter", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}
