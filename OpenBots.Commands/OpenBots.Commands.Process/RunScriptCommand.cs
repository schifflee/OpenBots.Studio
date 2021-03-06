﻿using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using Diagnostics = System.Diagnostics;

namespace OpenBots.Commands.Process
{
    [Serializable]
    [Group("Programs/Process Commands")]
    [Description("This command runs a script or program and waits for it to exit before proceeding.")]

    public class RunScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Script Path")]
        [InputSpecification("Enter a fully qualified path to the script, including the script extension.")]
        [SampleUsage(@"C:\temp\myscript.ps1 || {vScriptPath} || {ProjectPath}\myscript.ps1")]
        [Remarks("This command differs from *Start Process* because this command blocks execution until the script has completed. " +
                 "If you do not want to stop while the script executes, consider using *Start Process* instead.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_ScriptPath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Script Type")]
        [PropertyUISelectionOption("Default")]
        [PropertyUISelectionOption("Powershell")]
        [PropertyUISelectionOption("Python")]
        [InputSpecification("Select the type of script you want to execute.")]
        [SampleUsage("")]
        [Remarks("Default executes with the system default for that file type.")]
        public string v_ScriptType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Arguments")]
        [InputSpecification("Enter any arguments as a single string.")]
        [SampleUsage("-message Hello -t 2 || {vArguments}")]
        [Remarks("This input is optional.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ScriptArgs { get; set; }

        public RunScriptCommand()
        {
            CommandName = "RunScriptCommand";
            SelectionName = "Run Script";
            CommandEnabled = true;
            CustomRendering = true;
            v_ScriptType = "Default";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            Diagnostics.Process scriptProc = new Diagnostics.Process();

            string scriptPath = v_ScriptPath.ConvertUserVariableToString(engine);
            string scriptArgs = v_ScriptArgs.ConvertUserVariableToString(engine);

            switch(v_ScriptType)
            {
                case "Powershell":
                    scriptProc.StartInfo = new Diagnostics.ProcessStartInfo()
                    {
                        FileName = "powershell.exe",
                        Arguments = $"-NoProfile -ExecutionPolicy unrestricted -file \"{scriptPath}\" " + scriptArgs,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    break;
                case "Python":
                    scriptProc.StartInfo = new Diagnostics.ProcessStartInfo()
                    {
                        FileName = "python.exe",
                        Arguments = $"\"{scriptPath}\" " + scriptArgs,
                        CreateNoWindow = true,
                        UseShellExecute = false,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    break;
                default:
                    scriptProc.StartInfo = new Diagnostics.ProcessStartInfo()
                    {
                        FileName = scriptPath,
                        WindowStyle = Diagnostics.ProcessWindowStyle.Hidden,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true
                    };
                    break;
            }
            scriptProc.Start();
            scriptProc.WaitForExit();

            scriptProc.StandardOutput.ReadToEnd();
            scriptProc.StandardError.ReadToEnd();
            scriptProc.Close();
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ScriptPath", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ScriptType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ScriptArgs", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Script Path '{v_ScriptPath}']";
        }
    }
}
