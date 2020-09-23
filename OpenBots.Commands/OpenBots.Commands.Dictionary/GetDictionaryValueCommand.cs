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
using System.Xml.Serialization;

namespace OpenBots.Commands.Dictionary
{
    [Serializable]
    [Group("Dictionary Commands")]
    [Description("This command returns a dictionary value based on a specified key.")]
    public class GetDictionaryValueCommand : ScriptCommand
    {
        [PropertyDescription("Dictionary")]
        [InputSpecification("Specify the dictionary variable to get a value from.")]
        [SampleUsage("{vDictionary}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputDictionary { get; set; }
        [PropertyDescription("Key")]
        [InputSpecification("Specify the key to get the value for.")]
        [SampleUsage("SomeKey || {vKey}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Key { get; set; }
        [PropertyDescription("Output Value Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public GetDictionaryValueCommand()
        {
            CommandName = "GetDictionaryValueCommand";
            SelectionName = "Get Dictionary Value";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //Retrieve Dictionary by name
            var engine = (AutomationEngineInstance)sender;
            var vKey = v_Key.ConvertUserVariableToString(engine);

            //Declare local dictionary and assign output
            Dictionary<string,string> dict = (Dictionary<string,string>)v_InputDictionary.ConvertUserVariableToObject(engine);
            var dictValue = dict[vKey].ConvertUserVariableToString(engine);

            dictValue.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputDictionary", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Key", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_InputDictionary}' for Key '{v_Key}' - Store Value in '{v_OutputUserVariableName}']";
        }        
    }
}