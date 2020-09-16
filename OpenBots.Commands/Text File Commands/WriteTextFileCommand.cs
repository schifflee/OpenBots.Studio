using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Text File Commands")]
    [Description("This command writes specified data to a text file.")]
    public class WriteTextFileCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("File Path")]
        [InputSpecification("Enter or Select the File Path.")]
        [SampleUsage(@"C:\temp\myfile.txt || {ProjectPath}\myText.txt || {vTextFilePath}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_FilePath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text")]
        [InputSpecification("Indicate the Text to write.")]
        [SampleUsage("Hello World! || {vText}")]
        [Remarks("[crLF] inserts a newline.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_TextToWrite { get; set; }

        [XmlAttribute]
        [PropertyDescription("Overwrite Option")]
        [PropertyUISelectionOption("Append")]
        [PropertyUISelectionOption("Overwrite")]
        [InputSpecification("Indicate whether this command should append the text to or overwrite all existing text " +
                            "in the file")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_Overwrite { get; set; } = "Append";

        public WriteTextFileCommand()
        {
            CommandName = "WriteTextFileCommand";
            SelectionName = "Write Text File";
            CommandEnabled = true;
            CustomRendering = true;
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