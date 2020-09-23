using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using OpenBots.Server;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenBots.Commands.Remote
{
    [Serializable]
    [Group("Remote Commands")]
    [Description("This command executes a task remotely on another OpenBots instance.")]
    public class RemoteAPICommand : ScriptCommand
    {
        [PropertyDescription("API Endpoint/Port")]
        [InputSpecification("Define the API endpoint or port enabled for local listening.")]
        [SampleUsage("example.com/hello || 192.168.2.200:19312 || {vMyUrl}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_BaseURL { get; set; }

        [PropertyDescription("Parameter Type")]
        [PropertyUISelectionOption("Get Engine Status")]
        [PropertyUISelectionOption("Restart OpenBots")]
        [InputSpecification("Select the appropriate parameter type.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_ParameterType { get; set; }

        [PropertyDescription("Request Timeout (Seconds)")]
        [InputSpecification("Enter the length of time to wait before the request times out.")]
        [SampleUsage("30 || {vTime}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_RequestTimeout { get; set; }

        [PropertyDescription("Output Response Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public RemoteAPICommand()
        {
            CommandName = "RemoteAPICommand";
            SelectionName = "Remote API";
            CommandEnabled = true;
            CustomRendering = true;
            v_ParameterType = "Get Engine Status";
            v_RequestTimeout = "30";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var server = v_BaseURL.ConvertUserVariableToString(engine);
            var paramType = v_ParameterType.ConvertUserVariableToString(engine);
            var timeout = int.Parse(v_RequestTimeout.ConvertUserVariableToString(engine)) * 1000;
                
            var response = LocalTCPClient.SendAutomationTask(server, paramType, timeout);
            response.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_BaseURL", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ParameterType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_RequestTimeout", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_ParameterType} on '{v_BaseURL}' - Store Response in '{v_OutputUserVariableName}']";
        }
    }

}
