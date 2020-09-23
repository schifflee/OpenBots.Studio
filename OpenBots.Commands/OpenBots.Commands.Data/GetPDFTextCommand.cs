using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.IO;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace OpenBots.Commands.Data
{
    [Serializable]
    [Group("Data Commands")]
    [Description("This command reads all text from a PDF file and saves it into a variable.")]
    public class GetPDFTextCommand : ScriptCommand
    {
        [PropertyDescription("Source Type")]
        [PropertyUISelectionOption("File Path")]
        [PropertyUISelectionOption("File URL")]
        [InputSpecification("Select source type of PDF file.")]
        [SampleUsage("")]
        [Remarks("Select 'File Path' if the file is locally placed or 'File URL' to read a file from a web URL.")]
        public string v_FileSourceType { get; set; }

        [PropertyDescription("File Path / URL")]
        [InputSpecification("Specify the local path or URL to the applicable PDF file.")]
        [SampleUsage(@"C:\temp\myfile.pdf || https://temp.com/myfile.pdf || {vFilePath}")]
        [Remarks("Providing an invalid File Path/URL will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }

        [PropertyDescription("Output Text Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public GetPDFTextCommand()
        {
            CommandName = "GetPDFTextCommand";
            SelectionName = "Get PDF Text";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            //get variable path or URL to source file
            var vSourceFilePath = v_FilePath.ConvertUserVariableToString(engine);

            if (v_FileSourceType == "File URL")
            {
                //create temp directory
                var tempDir = Folders.GetFolder(FolderType.TempFolder);
                var tempFile = Path.Combine(tempDir, $"{ Guid.NewGuid()}.pdf");

                //check if directory does not exist then create directory
                if (!Directory.Exists(tempDir))
                {
                    Directory.CreateDirectory(tempDir);
                }

                // Create webClient to download the file for extraction
                var webclient = new WebClient();
                var uri = new Uri(vSourceFilePath);
                webclient.DownloadFile(uri, tempFile);

                // check if file is downloaded successfully
                if (File.Exists(tempFile))
                {
                    vSourceFilePath = tempFile;
                }

                // Free not needed resources
                uri = null;
                if (webclient != null)
                {
                    webclient.Dispose();
                    webclient = null;
                }
            }

            // Check if file exists before proceeding
            if (!File.Exists(vSourceFilePath))
            {
                throw new FileNotFoundException("Could not find file: " + vSourceFilePath);
            }

            //create process interface
            JavaInterface javaInterface = new JavaInterface();

            //get output from process
            var result = javaInterface.ExtractPDFText(vSourceFilePath);

            //apply to variable
            result.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //create standard group controls
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_FileSourceType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Extract Text From '{v_FilePath}' - Store Text in '{v_OutputUserVariableName}']";
        }
    }
}