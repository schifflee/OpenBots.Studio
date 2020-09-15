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
    [Description("This command returns an item (having a specific index) from a List.")]
    public class GetListItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("List")]
        [InputSpecification("Provide a List variable.")]
        [SampleUsage("{vList}")]
        [Remarks("Any type of variable other than List will cause error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ListName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Index")]
        [InputSpecification("Specify a valid List item index.")]
        [SampleUsage("0 || {vIndex}")]
        [Remarks("'0' is the index of the first item in a List. Providing an invalid or out-of-bounds index will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ItemIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output List Item Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public GetListItemCommand()
        {
            CommandName = "GetListItemCommand";
            SelectionName = "Get List Item";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var itemIndex = v_ItemIndex.ConvertUserVariableToString(engine);
            int index = int.Parse(itemIndex);
            //get variable by regular name
            var listVariable = v_ListName.ConvertUserVariableToObject(engine);

            //if still null then throw exception
            if (listVariable == null)
            {
                throw new System.Exception("Complex Variable '" + v_ListName + 
                    "' not found. Ensure the variable exists before attempting to modify it.");
            }

            dynamic listToIndex;
            if (listVariable is List<string>)
            {
                listToIndex = (List<string>)listVariable;
            }
            else if (listVariable is List<DataTable>)
            {
                listToIndex = (List<DataTable>)listVariable;
            }
            else if (listVariable is List<MailItem>)
            {
                listToIndex = (List<MailItem>)listVariable;
            }
            else if (listVariable is List<MimeMessage>)
            {
                listToIndex = (List<MimeMessage>)listVariable;
            }
            else if (listVariable is List<IWebElement>)
            {
                listToIndex = (List<IWebElement>)listVariable;
            }
            else if (
                (listVariable.ToString().StartsWith("[")) && 
                (listVariable.ToString().EndsWith("]")) && 
                (listVariable.ToString().Contains(","))
                )
            {
                //automatically handle if user has given a json array
                JArray jsonArray = JsonConvert.DeserializeObject(listVariable.ToString()) as JArray;

                var itemList = new List<string>();
                foreach (var jsonItem in jsonArray)
                {
                    var value = (JValue)jsonItem;
                    itemList.Add(value.ToString());
                }

                itemList.StoreInUserVariable(engine, v_ListName);
                listToIndex = itemList;
            }
            else
            {
                throw new System.Exception("Complex Variable List Type<T> Not Supported");
            }

            var item = listToIndex[index];

            ((object)item).StoreInUserVariable(engine, v_OutputUserVariableName);         
        }
        
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ListName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ItemIndex", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From Index '{v_ItemIndex}' of '{v_ListName}' - Store List Item in '{v_OutputUserVariableName}']";
        }       
    }
}