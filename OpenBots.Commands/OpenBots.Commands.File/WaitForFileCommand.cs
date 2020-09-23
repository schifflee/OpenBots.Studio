using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using IO = System.IO;

namespace OpenBots.Commands.File
{
    [Serializable]
    [Group("File Operation Commands")]
    [Description("This command waits for a file to exist at a specified destination.")]
    public class WaitForFileCommand : ScriptCommand
    {

        [PropertyDescription("File Path")]
        [InputSpecification("Enter or Select the path to the file.")]
        [SampleUsage(@"C:\temp\myfile.txt || {ProjectPath}\myfile.txt || {vTextFilePath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FileName { get; set; }

        [PropertyDescription("Timeout")]
        [InputSpecification("Specify how many seconds to wait for the file to exist.")]
        [SampleUsage("10 || {vSeconds}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WaitTime { get; set; }

        public WaitForFileCommand()
        {
            CommandName = "WaitForFileCommand";
            SelectionName = "Wait For File";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //convert items to variables
            var fileName = v_FileName.ConvertUserVariableToString(engine);
            var pauseTime = int.Parse(v_WaitTime.ConvertUserVariableToString(engine));

            //determine when to stop waiting based on user config
            var stopWaiting = DateTime.Now.AddSeconds(pauseTime);

            //initialize flag for file found
            var fileFound = false;

            //while file has not been found
            while (!fileFound)
            {
                //if file exists at the file path
                if (IO.File.Exists(fileName))
                    fileFound = true;

                //test if we should exit and throw exception
                if (DateTime.Now > stopWaiting)
                    throw new Exception("File was not found in time!");

                //put thread to sleep before iterating
                Thread.Sleep(100);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FileName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_WaitTime", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Wait '{v_WaitTime}' Seconds for File '{v_FileName}' to Exist]";
        }
    }
}