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
using taskt.Server;
using taskt.UI.CustomControls;

namespace taskt.Commands
{

    [Serializable]
    [Group("Remote Commands")]
    [Description("This command executes a task remotely on another taskt instance.")]
    public class RemoteTaskCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("API Endpoint/Port")]
        [InputSpecification("Define the API endpoint or port enabled for local listening.")]
        [SampleUsage("example.com/hello || 192.168.2.200:19312 || {vMyUrl}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_BaseURL { get; set; }

        [XmlAttribute]
        [PropertyDescription("Parameter Type")]
        [PropertyUISelectionOption("Run Raw Script Data")]
        [PropertyUISelectionOption("Run Local File")]
        [PropertyUISelectionOption("Run Remote File")]
        [PropertyUISelectionOption("Run Command Json")]
        [InputSpecification("Select the appropriate parameter.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_ParameterType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Execution Preference")]
        [PropertyUISelectionOption("Continue Execution")]
        [PropertyUISelectionOption("Await For Result")]
        [InputSpecification("Select the appropriate execution preference.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_ExecuteAwait { get; set; }

        [XmlAttribute]
        [PropertyDescription("Script Parameter Data")]
        [InputSpecification("Specify the data, typically either raw data, a local file, or a remote file.")]
        [SampleUsage(@"hello || {vData} || C:\temp\myfile.json || {ProjectPath}\myfile.json || {vFilePath}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_Parameter { get; set; }

        [XmlAttribute]
        [PropertyDescription("Request Timeout (Seconds)")]
        [InputSpecification("Enter the length of time to wait before the request times out.")]
        [SampleUsage("30 || {vTime}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_RequestTimeout { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Response Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public RemoteTaskCommand()
        {
            CommandName = "RemoteTaskCommand";
            SelectionName = "Remote Task";
            CommandEnabled = true;
            CustomRendering = true;
            v_ParameterType = "Run Raw Script Data";
            v_ExecuteAwait = "Continue Execution";
            v_RequestTimeout = "30";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var server = v_BaseURL.ConvertUserVariableToString(engine);
            var paramType = v_ParameterType.ConvertUserVariableToString(engine);
            var parameter = v_Parameter.ConvertUserVariableToString(engine);
            var awaitPreference = v_ExecuteAwait.ConvertUserVariableToString(engine);
            var timeout = int.Parse(v_RequestTimeout.ConvertUserVariableToString(engine)) * 1000;

            var response = LocalTCPClient.SendAutomationTask(server, paramType, timeout, parameter, awaitPreference);
            response.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_BaseURL", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ParameterType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ExecuteAwait", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Parameter", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_RequestTimeout", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_ParameterType} on '{v_BaseURL}' - Store Response in '{v_OutputUserVariableName}']";

        }

    }

}

