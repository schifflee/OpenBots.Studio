using Newtonsoft.Json.Linq;
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
    [Group("Data Commands")]
    [Description("This command parses a JSON array into a list.")]
    public class ParseJSONArrayCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("JSON Array")]
        [InputSpecification("Provide a variable or JSON array value.")]
        [SampleUsage("[{\"rect\":{\"length\":10, \"width\":5}}] || {vArrayVariable}")]
        [Remarks("Providing data of a type other than a 'JSON Array' will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_JsonArrayName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output List Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public ParseJSONArrayCommand()
        {
            CommandName = "ParseJSONArrayCommand";
            SelectionName = "Parse JSON Array";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            //get variablized input
            var variableInput = v_JsonArrayName.ConvertUserVariableToString(engine);

            //create objects
            JArray arr;
            List<string> resultList = new List<string>();

            //parse json
            try
            {
                arr = JArray.Parse(variableInput);
            }
            catch (Exception ex)
            {
                throw new Exception("Error Occured Selecting Tokens: " + ex.ToString());
            }
 
            //add results to result list since list<string> is supported
            foreach (var result in arr)
            {
                resultList.Add(result.ToString());
            }

            resultList.StoreInUserVariable(engine, v_OutputUserVariableName);           
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_JsonArrayName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Parse '{v_JsonArrayName}' - Store List in '{v_OutputUserVariableName}']";
        }
    }
}