using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using IO = System.IO;

namespace OpenBots.Commands.File
{
    [Serializable]
    [Group("File Operation Commands")]
    [Description("This command extracts file(s) from a Zip file.")]
    public class ExtractFilesCommand : ScriptCommand
    {
        [PropertyDescription("Source File Path")]
        [InputSpecification("Enter or Select the Path to the source zip file.")]
        [SampleUsage(@"C:\temp\myfile.zip || {ProjectPath}\myfile.zip || {vFileSourcePath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePathOrigin { get; set; }

        [PropertyDescription("Password (Optional)")]
        [InputSpecification("Define the password to use if required to extract files.")]
        [SampleUsage("password || {vPassword}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Password { get; set; }

        [PropertyDescription("Extracted File(s) Directory Path")]
        [InputSpecification("Enter or Select the Folder Path to move extracted file(s) to.")]
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
            //get variable path to source file
            var vSourceFilePathOrigin = v_FilePathOrigin.ConvertUserVariableToString(engine);

            // get file path to destination files
            var vFilePathDestination = v_PathDestination.ConvertUserVariableToString(engine);

            // get password to extract files
            var vPassword = v_Password.ConvertUserVariableToString(engine);
            FileStream fs = IO.File.OpenRead(vSourceFilePathOrigin);
            ZipFile file = new ZipFile(fs);

            if (!string.IsNullOrEmpty(vPassword))
            {
                // AES encrypted entries are handled automatically
                file.Password = vPassword;
            }

            foreach (ZipEntry zipEntry in file)
            {
                if (!zipEntry.IsFile)
                {
                    // Ignore directories
                    continue;
                }

                string entryFileName = zipEntry.Name;

                // 4K is optimum
                byte[] buffer = new byte[4096];
                Stream zipStream = file.GetInputStream(zipEntry);

                // Manipulate the output filename here as desired.
                string fullZipToPath = Path.Combine(vFilePathDestination, entryFileName);
                string directoryName = Path.GetDirectoryName(fullZipToPath);

                if (directoryName.Length > 0)
                    Directory.CreateDirectory(directoryName);

                // Unzip file in buffered chunks. This is just as fast as unpacking to a buffer the full size
                // of the file, but does not waste memory.
                // The "using" will close the stream even if an exception occurs.
                using (FileStream streamWriter = IO.File.Create(fullZipToPath))
                    StreamUtils.Copy(zipStream, streamWriter, buffer);
            }
           
            if (file != null)
            {
                file.IsStreamOwner = true;
                file.Close(); 

                //Get File Paths from the folder
                var filesList = Directory.GetFiles(vFilePathDestination, ".", SearchOption.AllDirectories).ToList();

                //Add File Paths to the output variable
                filesList.StoreInUserVariable(engine, v_OutputUserVariableName);
            }           
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //create standard group controls
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePathOrigin", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultPasswordInputGroupFor("v_Password", this, editor));
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