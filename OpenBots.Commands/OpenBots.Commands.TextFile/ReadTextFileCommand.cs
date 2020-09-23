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
    [Description("This command reads text data from a text file and stores it in a variable.")]
    public class ReadTextFileCommand : ScriptCommand
    {

        [PropertyDescription("Text File Path")]
        [InputSpecification("Enter or select the path to the text file.")]
        [SampleUsage(@"C:\temp\myfile.txt || {ProjectPath}\myText.txt || {vTextFilePath}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }

        [PropertyDescription("Output Text Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public ReadTextFileCommand()
        {
            CommandName = "ReadTextFileCommand";
            SelectionName = "Read Text File";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //convert variables
            var filePath = v_FilePath.ConvertUserVariableToString(engine);
            //read text from file
            var textFromFile = File.ReadAllText(filePath);
            //assign text to user variable
            textFromFile.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            RenderedControls.AddRange(
                commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor)
            );

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Read Text From '{v_FilePath}' - Store Text in '{v_OutputUserVariableName}']";
        }
    }
}