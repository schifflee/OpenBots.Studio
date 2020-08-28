using Microsoft.Office.Interop.Outlook;
using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
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
    [Group("List Commands")]
    [Description("This command returns the count of items contained in a List.")]
    public class GetListCountCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("List")]
        [InputSpecification("Provide a List variable.")]
        [SampleUsage("{vList}")]
        [Remarks("Providing any type of variable other than a List will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Count Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public GetListCountCommand()
        {
            CommandName = "GetListCountCommand";
            SelectionName = "Get List Count";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //get variable by regular name
            var listVariable = v_ListName.ConvertUserVariableToObject(engine);

            //if still null then throw exception
            if (listVariable == null)
            {
                throw new System.Exception("Complex Variable '" + v_ListName +
                    "' not found. Ensure the variable exists before attempting to modify it.");
            }

            dynamic listToCount; 
            if (listVariable is List<string>)
                listToCount = (List<string>)listVariable;
            else if (listVariable is List<DataTable>)
                listToCount = (List<DataTable>)listVariable;
            else if (listVariable is List<MailItem>)
                listToCount = (List<MailItem>)listVariable;
            else if (listVariable is List<MimeMessage>)
                listToCount = (List<MimeMessage>)listVariable;
            else if (listVariable is List<IWebElement>)
                listToCount = (List<IWebElement>)listVariable;
            else if (listVariable.ToString().StartsWith("[") && listVariable.ToString().EndsWith("]") && 
                     listVariable.ToString().Contains(","))
            {
                //automatically handle if user has given a json array
                JArray jsonArray = JsonConvert.DeserializeObject(listVariable.ToString()) as JArray;

                var itemList = new List<string>();
                foreach (var item in jsonArray)
                {
                    var value = (JValue)item;
                    itemList.Add(value.ToString());
                }

                itemList.StoreInUserVariable(engine, v_ListName);
                listToCount = itemList;
            }
            else
                throw new System.Exception("Complex Variable List Type<T> Not Supported");

            string count = listToCount.Count.ToString();
            count.StoreInUserVariable(engine, v_OutputUserVariableName);
        }
        
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ListName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_ListName}' - Store Count in '{v_OutputUserVariableName}']";
        }       
    }
}