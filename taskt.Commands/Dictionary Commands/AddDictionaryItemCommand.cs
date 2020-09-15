﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Script;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Dictionary Commands")]
    [Description("This command adds an item (key and value pair) to a Dictionary.")]
    public class AddDictionaryItemCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Dictionary")]
        [InputSpecification("Select the dictionary variable to add an item to.")]
        [SampleUsage("{vMyDictionary}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DictionaryName { get; set; }

        [XmlElement]
        [PropertyDescription("Keys and Values")]
        [InputSpecification("Enter Keys and Values required for the dictionary.")]
        [SampleUsage("[FirstName | John] || [{vKey} | {vValue}]")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_ColumnNameDataTable { get; set; }

        public AddDictionaryItemCommand()
        {
            CommandName = "AddDictionaryItemCommand";
            SelectionName = "Add Dictionary Item";
            CommandEnabled = true;
            CustomRendering = true;

            //initialize Datatable
            v_ColumnNameDataTable = new DataTable
            {
                TableName = "ColumnNamesDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            };

            v_ColumnNameDataTable.Columns.Add("Keys");
            v_ColumnNameDataTable.Columns.Add("Values");
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var dictionaryVariable = v_DictionaryName.ConvertUserVariableToObject(engine);

            Dictionary<string, string> outputDictionary = (Dictionary<string, string>)dictionaryVariable;

            foreach (DataRow rwColumnName in v_ColumnNameDataTable.Rows)
            {
                outputDictionary.Add(
                    rwColumnName.Field<string>("Keys").ConvertUserVariableToString(engine), 
                    rwColumnName.Field<string>("Values").ConvertUserVariableToString(engine));
            }
            outputDictionary.StoreInUserVariable(engine, v_DictionaryName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_DictionaryName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDataGridViewGroupFor("v_ColumnNameDataTable", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Add {v_ColumnNameDataTable.Rows.Count} Item(s) in '{v_DictionaryName}']";
        }      
    }
}