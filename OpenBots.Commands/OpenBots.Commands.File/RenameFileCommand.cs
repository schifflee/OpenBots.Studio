using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using IO = System.IO;

namespace OpenBots.Commands.File
{
    [Serializable]
    [Group("File Operation Commands")]
    [Description("This command renames an existing file.")]
    public class RenameFileCommand : ScriptCommand
    {

        [PropertyDescription("File Path")]
        [InputSpecification("Enter or Select the path to the file.")]
        [SampleUsage(@"C:\temp\myfile.txt || {ProjectPath}\myfile.txt || {vTextFilePath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)] 
        public string v_SourceFilePath { get; set; }

        [PropertyDescription("New File Name (with extension)")]
        [InputSpecification("Specify new file name with extension.")]
        [SampleUsage("newfile.txt || {vNewFileName}")]
        [Remarks("Changing the file extension will not automatically convert files.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_NewName { get; set; }

        public RenameFileCommand()
        {
            CommandName = "RenameFileCommand";
            SelectionName = "Rename File";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //apply variable logic
            var sourceFile = v_SourceFilePath.ConvertUserVariableToString(engine);
            var newFileName = v_NewName.ConvertUserVariableToString(engine);

            //get source file name and info
            IO.FileInfo sourceFileInfo = new IO.FileInfo(sourceFile);

            //create destination
            var destinationPath = IO.Path.Combine(sourceFileInfo.DirectoryName, newFileName);

            //rename file
            IO.File.Move(sourceFile, destinationPath);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SourceFilePath", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_NewName", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Rename '{v_SourceFilePath}' to '{v_NewName}']";
        }
    }
}