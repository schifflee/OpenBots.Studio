using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
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
    [Description("This command logs text data to either an engine file or a custom file.")]
    public class LogMessageCommand : ScriptCommand
    {
        [PropertyDescription("Write Log To")]
        [InputSpecification("Specify the corresponding logging option to save logs to Engine Logs or to a custom File.")]
        [SampleUsage(@"Engine Logs || C:\MyEngineLogs.txt || {vFileVariable}")]
        [Remarks("Selecting 'Engine Logs' will result in writing execution logs in the 'Engine Logs'. " +
            "The current Date and Time will be automatically appended to a local file if a custom file name is provided. " +
            "Logs are all saved in the OpenBots Studio Root Folder in the 'Logs' folder.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_LogFile { get; set; }
        [PropertyDescription("Log Text")]
        [InputSpecification("Specify the log text.")]
        [SampleUsage("Third Step is Complete || {vLogText}")]
        [Remarks("Provide only text data.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_LogText { get; set; }
        [PropertyDescription("Log Type")]
        [PropertyUISelectionOption("Verbose")]
        [PropertyUISelectionOption("Debug")]
        [PropertyUISelectionOption("Information")]
        [PropertyUISelectionOption("Warning")]
        [PropertyUISelectionOption("Error")]
        [PropertyUISelectionOption("Fatal")]
        [InputSpecification("Specify the log type.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_LogType { get; set; }

        public LogMessageCommand()
        {
            CommandName = "LogMessageCommand";
            SelectionName = "Log Message";
            CommandEnabled = true;
            CustomRendering = true;
            v_LogFile = "Engine Logs";
            v_LogType = "Information";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            //get text to log and log file name       
            var textToLog = v_LogText.ConvertUserVariableToString(engine);
            var loggerFilePath = v_LogFile.ConvertUserVariableToString(engine);

            //determine log file
            if (v_LogFile == "Engine Logs")
            {
                switch (v_LogType)
                {
                    case "Verbose":
                        LogLevel = LogEventLevel.Verbose;
                        break;
                    case "Debug":
                        LogLevel = LogEventLevel.Debug;
                        break;
                    case "Information":
                        LogLevel = LogEventLevel.Information;
                        break;
                    case "Warning":
                        LogLevel = LogEventLevel.Warning;
                        break;
                    case "Error":
                        LogLevel = LogEventLevel.Error;
                        break;
                    case "Fatal":
                        LogLevel = LogEventLevel.Fatal;
                        break;
                }
            }
            else
            {
                //create new logger and log to custom file
                using (var logger = new Logging().CreateFileLogger(loggerFilePath, RollingInterval.Infinite))
                {
                    switch (v_LogType)
                    {
                        case "Verbose":
                            logger.Verbose(textToLog);
                            break;
                        case "Debug":
                            logger.Debug(textToLog);
                            break;
                        case "Information":
                            logger.Information(textToLog);
                            break;
                        case "Warning":
                            logger.Warning(textToLog);
                            break;
                        case "Error":
                            logger.Error(textToLog);
                            break;
                        case "Fatal":
                            logger.Fatal(textToLog);
                            break;
                    }                  
                }
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //create standard group controls
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_LogFile", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_LogText", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_LogType", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            if (v_LogFile == "Engine Logs")
                return base.GetDisplayValue() + $" ['{v_LogType} - {v_LogText}']";
            else
                return base.GetDisplayValue() + $" ['{v_LogType} - {v_LogText}' to '{v_LogFile}']";
        }
    }
}
