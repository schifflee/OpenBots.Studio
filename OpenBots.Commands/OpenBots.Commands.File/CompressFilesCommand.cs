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
    [Description("This command compresses file(s) from a directory into a Zip file.")]
    public class CompressFilesCommand : ScriptCommand
    {
        [PropertyDescription("Source Directory Path")]
        [InputSpecification("Enter or Select the Path to the source directory.")]
        [SampleUsage(@"C:\temp || {ProjectPath}\temp || {vFileSourcePath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_DirectoryPathOrigin { get; set; }

        [PropertyDescription("Password (Optional)")]
        [InputSpecification("Define the password to use for file compression.")]
        [SampleUsage("password || {vPassword}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Password { get; set; }

        [PropertyDescription("Compressed File Directory Path")]
        [InputSpecification("Enter or Select the Folder Path to place the compressed file in.")]
        [SampleUsage(@"C:\temp || {ProjectPath}\temp || {vFilesPath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_PathDestination { get; set; }

        [PropertyDescription("Output Compressed File Path Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public CompressFilesCommand()
        {
            CommandName = "CompressFilesCommand";
            SelectionName = "Compress Files";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //get variable path to source file
            var vSourceDirectoryPathOrigin = v_DirectoryPathOrigin.ConvertUserVariableToString(engine);

            // get file path to destination files
            var vFilePathDestination = v_PathDestination.ConvertUserVariableToString(engine);

            // get password to extract files
            var vPassword = v_Password.ConvertUserVariableToString(engine);

            string[] filenames = Directory.GetFiles(vSourceDirectoryPathOrigin);

            string sourceDirectoryName = vSourceDirectoryPathOrigin.Split('\\').LastOrDefault();
            string compressedFileName = Path.Combine(vFilePathDestination, sourceDirectoryName + ".zip");
            using (ZipOutputStream OutputStream = new ZipOutputStream(IO.File.Create(compressedFileName)))
            {
                // Define a password for the file (if provided)
                OutputStream.Password = vPassword;

                // Define the compression level
                // 0 - store only to 9 - means best compression
                OutputStream.SetLevel(9);

                byte[] buffer = new byte[4096];

                foreach (string file in filenames)
                {

                    ZipEntry entry = new ZipEntry(Path.GetFileName(file));
                    entry.DateTime = DateTime.Now;
                    OutputStream.PutNextEntry(entry);

                    using (FileStream fs = IO.File.OpenRead(file))
                    {
                        int sourceBytes;

                        do
                        {
                            sourceBytes = fs.Read(buffer, 0, buffer.Length);
                            OutputStream.Write(buffer, 0, sourceBytes);
                        } while (sourceBytes > 0);
                    }
                }

                // Finish is important to ensure trailing information for a Zip file is appended.  Without this
                // the created file would be invalid.
                OutputStream.Finish();

                // Close is important to wrap things up and unlock the file.
                OutputStream.Close();                     
            }

            //Add File Path to the output variable
            compressedFileName.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //create standard group controls
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DirectoryPathOrigin", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Password", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_PathDestination", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Compress From '{v_DirectoryPathOrigin}' to '{v_PathDestination}' - " +
                $"Store Compressed File Path in '{v_OutputUserVariableName}']";
        }
    }
}