using System;
using System.Collections.Generic;
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
    [Group("Dictionary Commands")]
    [Description("This command returns a dictionary value based on a specified key.")]
    public class GetDictionaryValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Dictionary")]
        [InputSpecification("Specify the dictionary variable to get a value from.")]
        [SampleUsage("{vDictionary}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputDictionary { get; set; }

        [XmlAttribute]
        [PropertyDescription("Key")]
        [InputSpecification("Specify the key to get the value for.")]
        [SampleUsage("SomeKey || {vKey}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Key { get; set; }

        [XmlAttribute]
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
        
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputDictionary", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Key", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_InputDictionary}' for Key '{v_Key}' - Store Value in '{v_OutputUserVariableName}']";
        }        
    }
}