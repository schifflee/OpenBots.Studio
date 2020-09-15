﻿using System;
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
using taskt.UI.CustomControls;

namespace taskt.Commands
{

    [Serializable]
    [Group("Folder Operation Commands")]
    [Description("This command creates a folder in a specified location.")]
    public class CreateFolderCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("New Folder Name")]
        [InputSpecification("Enter the name of the new folder.")]
        [SampleUsage("myFolderName || {vFolderName}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_NewFolderName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Directory Path")]
        [InputSpecification("Enter or Select the path to the directory to create the folder in.")]
        [SampleUsage(@"C:\temp\myfolder || {ProjectPath}\myfolder || {vTextFolderPath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)] 
        public string v_DestinationDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Delete Existing Folder")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the folder should be deleted first if it already exists.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_DeleteExisting { get; set; }

        public CreateFolderCommand()
        {
            CommandName = "CreateFolderCommand";
            SelectionName = "Create Folder";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //apply variable logic
            var destinationDirectory = v_DestinationDirectory.ConvertUserVariableToString(engine);
            var newFolder = v_NewFolderName.ConvertUserVariableToString(engine);

            var finalPath = Path.Combine(destinationDirectory, newFolder);
            //delete folder if it exists AND the delete option is selected 
            if (v_DeleteExisting == "Yes" && Directory.Exists(finalPath))
            {
                Directory.Delete(finalPath, true);
            }

            //create folder if it doesn't exist
            if (!Directory.Exists(finalPath))
            {
                Directory.CreateDirectory(finalPath);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_NewFolderName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DestinationDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_DeleteExisting", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $"[Folder Path '{v_DestinationDirectory}\\{v_NewFolderName}']";
        }
    }
}