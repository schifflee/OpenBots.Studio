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
using System.Windows.Forms;

namespace OpenBots.Commands.TextFile
{
    [Serializable]
    [Group("Text File Commands")]
    [Description("This command writes specified data to an existing or newly created text file.")]
    public class WriteCreateTextFileCommand : ScriptCommand
    {

        [PropertyDescription("Text File Path")]
        [InputSpecification("Enter or select the text file path.")]
        [SampleUsage(@"C:\temp\myfile.txt || {ProjectPath}\myText.txt || {vTextFilePath}")]
        [Remarks("If the selected text file does not exist, a file with the provided name and location will be created.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }

        [PropertyDescription("Text")]
        [InputSpecification("Indicate the Text to write.")]
        [SampleUsage("Hello World! || {vText}")]
        [Remarks("[crLF] inserts a newline.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]       
        public string v_TextToWrite { get; set; }

        [PropertyDescription("Overwrite Option")]
        [PropertyUISelectionOption("Append")]
        [PropertyUISelectionOption("Overwrite")]
        [InputSpecification("Indicate whether this command should append the text to or overwrite all existing text " +
                            "in the file")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_Overwrite { get; set; }

        public WriteCreateTextFileCommand()
        {
            CommandName = "WriteCreateTextFileCommand";
            SelectionName = "Write/Create Text File";
            CommandEnabled = true;
            CustomRendering = true;
            v_Overwrite = "Append";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //convert variables
            var filePath = v_FilePath.ConvertUserVariableToString(engine);
            var outputText = v_TextToWrite.ConvertUserVariableToString(engine).Replace("[crLF]", Environment.NewLine);

            //append or overwrite as necessary
            if (v_Overwrite == "Append")
                File.AppendAllText(filePath, outputText);
            else
                File.WriteAllText(filePath, outputText);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TextToWrite", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Overwrite", this, editor));

            return RenderedControls;
        }


        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" ['{v_Overwrite}' to '{v_FilePath}']";
        }
    }
}