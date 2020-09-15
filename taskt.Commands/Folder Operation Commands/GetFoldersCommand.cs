using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
    [Description("This command returns a list of folder directories from a specified location.")]
    public class GetFoldersCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Root Folder Path")]
        [InputSpecification("Enter or Select the path to the root folder to get its subdirectories.")]
        [SampleUsage(@"C:\temp\myfolder || {ProjectPath}\myfolder || {vTextFolderPath}")]
        [Remarks("{ProjectPath} is the directory path of the current project.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_SourceFolderPath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Folder Path(s) Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public GetFoldersCommand()
        {
            CommandName = "GetFoldersCommand";
            SelectionName = "Get Folders";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //apply variable logic
            var sourceFolder = v_SourceFolderPath.ConvertUserVariableToString(engine);

            //Get Subdirectories List
            var directoriesList = Directory.GetDirectories(sourceFolder).ToList();

            directoriesList.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFolderPath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_SourceFolderPath}' - Store Folder Path(s) in '{v_OutputUserVariableName}']";
        }
    }
}