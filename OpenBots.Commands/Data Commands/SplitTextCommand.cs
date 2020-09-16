using System;
using System.Collections.Generic;
using System.Linq;
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
    [Group("Data Commands")]
    [Description("This command splits a string by a delimiter and saves the result in a list.")]
    public class SplitTextCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Text Data")]
        [InputSpecification("Provide a variable or text value.")]
        [SampleUsage("Sample text, to be splitted by comma delimiter || {vTextData}")]
        [Remarks("Providing data of a type other than a 'String' will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputText { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text Delimiter")]
        [InputSpecification("Specify the character that will be used to split the text.")]
        [SampleUsage("[crLF] || [chars] || , || {vDelimiter}")]
        [Remarks("[crLF] can be used for line breaks and [chars] can be used to split each character.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SplitCharacter { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output List Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public SplitTextCommand()
        {
            CommandName = "SplitTextCommand";
            SelectionName = "Split Text";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var stringVariable = v_InputText.ConvertUserVariableToString(engine);
            var splitCharacter = v_SplitCharacter;

            if(v_SplitCharacter != "[crLF]" && v_SplitCharacter != "[chars]")
            {
                splitCharacter = v_SplitCharacter.ConvertUserVariableToString(engine);
            }
            List<string> splitString;
            if (splitCharacter == "[crLF]")
            {
                splitString = stringVariable.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
            }
            else if (splitCharacter == "[chars]")
            {
                splitString = new List<string>();
                var chars = stringVariable.ToCharArray();
                foreach (var c in chars)
                {
                    splitString.Add(c.ToString());
                }
            }
            else
            {
                splitString = stringVariable.Split(new string[] { splitCharacter }, StringSplitOptions.None).ToList();
            }

            splitString.StoreInUserVariable(engine, v_OutputUserVariableName);           
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputText", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SplitCharacter", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Split '{v_InputText}' by '{v_SplitCharacter}' - Store List in '{v_OutputUserVariableName}']";
        }
    }
}