using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Data = System.Data;

namespace OpenBots.Commands.DataTable
{
    [Serializable]
    [Group("DataTable Commands")]
    [Description("This command creates a DataTable with the Column Names provided.")]

    public class CreateDataTableCommand : ScriptCommand
    {

        [PropertyDescription("Column Names")]
        [InputSpecification("Enter the Column Names required for each column of data.")]
        [SampleUsage("MyColumn || {vColumn}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public Data.DataTable v_ColumnNameDataTable { get; set; }

        [PropertyDescription("Output DataTable Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public CreateDataTableCommand()
        {
            CommandName = "CreateDataTableCommand";
            SelectionName = "Create DataTable";
            CommandEnabled = true;
            CustomRendering = true;

            //initialize data table
            v_ColumnNameDataTable = new Data.DataTable
            {
                TableName = "ColumnNamesDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            };

            v_ColumnNameDataTable.Columns.Add("Column Name");
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            Data.DataTable Dt = new Data.DataTable();

            foreach(DataRow rwColumnName in v_ColumnNameDataTable.Rows)
            {
                Dt.Columns.Add(rwColumnName.Field<string>("Column Name").ConvertUserVariableToString(engine));
            }

            Dt.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDataGridViewGroupFor("v_ColumnNameDataTable", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [With {v_ColumnNameDataTable.Rows.Count} Column(s) - Store DataTable in '{v_OutputUserVariableName}']";
        }
    }
}