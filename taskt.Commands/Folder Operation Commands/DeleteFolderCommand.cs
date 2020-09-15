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
    [Description("This command deletes a folder from a specified location.")]
    public class DeleteFolderCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Folder Path")]
        [InputSpecification("Enter or Select the path to the folder.")]
        [SampleUsage(@"C:\temp\myfolder || {ProjectPath}\myfolder  || {vTextFolderPath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)] 
        public string v_SourceFolderPath { get; set; }

        public DeleteFolderCommand()
        {
            CommandName = "DeleteFolderCommand";
            SelectionName = "Delete Folder";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //apply variable logic
            var sourceFolder = v_SourceFolderPath.ConvertUserVariableToString(engine);

            //delete folder
            Directory.Delete(sourceFolder, true);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFolderPath", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Delete '{v_SourceFolderPath}']";
        }
    }
}