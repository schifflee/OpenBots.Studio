﻿using System;
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
    [Group("Dictionary Commands")]
    [Description("This command creates a new Dictionary.")]
    public class CreateDictionaryCommand : ScriptCommand
    {
        [XmlElement]
        [PropertyDescription("Keys and Values")]
        [InputSpecification("Enter the Keys and Values required for the new dictionary.")]
        [SampleUsage("[FirstName | John] || [{vKey} | {vValue}]")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_ColumnNameDataTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Dictionary Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public CreateDictionaryCommand()
        {
            CommandName = "CreateDictionaryCommand";
            SelectionName = "Create Dictionary";
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

            Dictionary<string, string> outputDictionary = new Dictionary<string, string>();

            foreach (DataRow rwColumnName in v_ColumnNameDataTable.Rows)
            {
                outputDictionary.Add(
                    rwColumnName.Field<string>("Keys").ConvertUserVariableToString(engine), 
                    rwColumnName.Field<string>("Values").ConvertUserVariableToString(engine));
            }

            outputDictionary.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDataGridViewGroupFor("v_ColumnNameDataTable", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [With {v_ColumnNameDataTable.Rows.Count} Entries - Store Dictionary in '{v_OutputUserVariableName}']";
        }
    }
}