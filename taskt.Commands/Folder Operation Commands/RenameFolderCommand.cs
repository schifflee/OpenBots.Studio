using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;

namespace taskt.Commands
{
    [Serializable]
    [Group("Folder Operation Commands")]
    [Description("This command renames an existing folder.")]
    public class RenameFolderCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Folder Path")]
        [InputSpecification("Enter or Select the path to the folder.")]
        [SampleUsage(@"C:\temp\myFolder || {ProjectPath}\myfolder || {vFolderPath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)] 
        public string v_SourceFolderPath { get; set; }

        [XmlAttribute]
        [PropertyDescription("New Folder Name")]
        [InputSpecification("Specify the new folder name.")]
        [SampleUsage("New Folder Name || {vNewFolderName}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_NewName { get; set; }

        public RenameFolderCommand()
        {
            CommandName = "RenameFolderCommand";
            SelectionName = "Rename Folder";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //apply variable logic
            var sourceFolder = v_SourceFolderPath.ConvertUserVariableToString(engine);
            var newFolderName = v_NewName.ConvertUserVariableToString(engine);

            //get source folder name and info
            DirectoryInfo sourceFolderInfo = new DirectoryInfo(sourceFolder);

            //create destination
            var destinationPath = Path.Combine(sourceFolderInfo.Parent.FullName, newFolderName);

            //rename folder
            Directory.Move(sourceFolder, destinationPath);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SourceFolderPath", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_NewName", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Rename '{v_SourceFolderPath}' to '{v_NewName}']";
        }
    }
}