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
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;
using IO = System.IO;

namespace OpenBots.Commands.File
{
    [Serializable]
    [Group("File Operation Commands")]
    [Description("This command extracts file(s) from a file having specific format.")]
    public class ExtractFilesCommand : ScriptCommand
    {
        [PropertyDescription("Source File Format")]
        [PropertyUISelectionOption("zip")]
        [PropertyUISelectionOption("7z")]
        [PropertyUISelectionOption("xz")]
        [PropertyUISelectionOption("bzip2")]
        [PropertyUISelectionOption("tar")]
        [PropertyUISelectionOption("wim")]
        [PropertyUISelectionOption("iso")]
        [InputSpecification("Select source file format.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_FileType { get; set; }

        [PropertyDescription("File Source Type")]
        [PropertyUISelectionOption("File Path")]
        [PropertyUISelectionOption("File URL")]
        [InputSpecification("Select file source type.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_FileSourceType { get; set; }

        [PropertyDescription("Source File Path / URL")]
        [InputSpecification("Enter or Select the Path / URL to the applicable file.")]
        [SampleUsage(@"C:\temp\myfile.zip || {ProjectPath}\myfile.zip || https://temp.com/myfile.zip || {vFileSourcePath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePathOrigin { get; set; }

        [PropertyDescription("Extracted File(s) Directory Path")]
        [InputSpecification("Enter or Select the Folder Path / URL to move extracted file(s) to.")]
        [SampleUsage(@"C:\temp || {ProjectPath}\temp || {vFilesPath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_PathDestination { get; set; }

        [PropertyDescription("Output Extracted File Path(s) List Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public ExtractFilesCommand()
        {
            CommandName = "ExtractFilesCommand";
            SelectionName = "Extract Files";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //get variable path or URL to source file
            var vSourceFilePathOrigin = v_FilePathOrigin.ConvertUserVariableToString(engine);

            // get file path to destination files
            var vFilePathDestination = v_PathDestination.ConvertUserVariableToString(engine);

            if (v_FileSourceType == "File URL")
            {
                //create temp directory
                var tempDir = Folders.GetFolder(FolderType.TempFolder);
                var tempFile = IO.Path.Combine(tempDir, $"{ Guid.NewGuid()}." + v_FileType);

                //check if directory does not exist then create directory
                if (!IO.Directory.Exists(tempDir))
                {
                    IO.Directory.CreateDirectory(tempDir);
                }

                // Create webClient to download the file for extraction
                var webclient = new WebClient();
                var uri = new Uri(vSourceFilePathOrigin);
                webclient.DownloadFile(uri, tempFile);

                // check if file is downloaded successfully
                if (IO.File.Exists(tempFile))
                {
                    vSourceFilePathOrigin = tempFile;
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
            if (!IO.File.Exists(vSourceFilePathOrigin))
                throw new IO.FileNotFoundException("Could not find file: " + vSourceFilePathOrigin);

            // Get 7Z app
            var zPath = IO.Path.Combine(IO.Path.GetDirectoryName(Application.ExecutablePath), "Resources", "7z.exe");

            // If the directory doesn't exist, create it.
            if (!IO.Directory.Exists(vFilePathDestination))
                IO.Directory.CreateDirectory(vFilePathDestination);

            var result = "";
            Process process = new Process();

            try
            {
                var temp = Guid.NewGuid();
                //Extract in temp to get list files and directories and delete
                ProcessStartInfo pro = new ProcessStartInfo();
                pro.WindowStyle = ProcessWindowStyle.Hidden;
                pro.UseShellExecute = false;
                pro.FileName = zPath;
                pro.RedirectStandardOutput = true;
                pro.Arguments = "x " + vSourceFilePathOrigin + " -o" + vFilePathDestination + "/" + temp + " -aoa";
                process.StartInfo = pro;
                process.Start();
                process.WaitForExit();
                string[] dirPaths = IO.Directory.GetDirectories(vFilePathDestination + "/" + temp, "*", IO.SearchOption.TopDirectoryOnly);
                string[] filePaths = IO.Directory.GetFiles(vFilePathDestination + "/" + temp, "*", IO.SearchOption.TopDirectoryOnly);

                foreach (var item in dirPaths)
                {
                    result = result + item + Environment.NewLine;
                }
                foreach (var item in filePaths)
                {
                    result = result + item + Environment.NewLine;
                }
                result = result.Replace("/" + temp, "");
                IO.Directory.Delete(vFilePathDestination + "/" + temp, true);

                //Extract 
                pro.Arguments = "x " + vSourceFilePathOrigin + " -o" + vFilePathDestination + " -aoa";
                process.StartInfo = pro;
                process.Start();
                process.WaitForExit();

                result.StoreInUserVariable(engine, v_OutputUserVariableName);
            }
            catch (Exception Ex)
            {
                process.Kill();
                throw Ex;
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //create standard group controls
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_FileType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_FileSourceType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePathOrigin", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_PathDestination", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Extract From '{v_FilePathOrigin}' to '{v_PathDestination}' - " +
                $"Store Extracted File Path(s) List in '{v_OutputUserVariableName}']";
        }
    }
}