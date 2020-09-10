using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.Core.Properties;
using taskt.Core.UI.CustomControls;
using taskt.Commands.Forms;

// to be reviewed
namespace taskt.Commands
{
    [Serializable]
    [Group("DataTable Commands")]
    [Description("This command adds a DataRow to a DataTable.")]

    public class AddDataRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("DataTable")]
        [InputSpecification("Enter an existing DataTable to add a DataRow to.")]
        [SampleUsage("{vDataTable}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataTable { get; set; }

        [XmlElement]
        [PropertyDescription("Data")]
        [InputSpecification("Enter Column Names and Data for each column in the DataRow.")]
        [SampleUsage("[ First Name | John ] || [ {vColumn} | {vData} ]")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_DataRowDataTable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private List<CreateDataTableCommand> _dataTableCreationCommands;

        public AddDataRowCommand()
        {
            CommandName = "AddDataRowCommand";
            SelectionName = "Add DataRow";
            CommandEnabled = true;
            CustomRendering = true;

            //initialize data table
            v_DataRowDataTable = new DataTable
            {
                TableName = "AddDataDataTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            };

            v_DataRowDataTable.Columns.Add("Column Name");
            v_DataRowDataTable.Columns.Add("Data");
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            DataTable Dt = (DataTable)v_DataTable.ConvertUserVariableToObject(engine);
            var newRow = Dt.NewRow();

            foreach (DataRow rw in v_DataRowDataTable.Rows)
            {
                var columnName = rw.Field<string>("Column Name").ConvertUserVariableToString(engine);
                var data = rw.Field<string>("Data").ConvertUserVariableToString(engine);
                newRow.SetField(columnName, data);
            }
            Dt.Rows.Add(newRow);

            Dt.StoreInUserVariable(engine, v_DataTable);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));
            RenderedControls.AddRange(commandControls.CreateDataGridViewGroupFor("v_DataRowDataTable", this, editor));

            CommandItemControl loadSchemaControl = new CommandItemControl();
            loadSchemaControl.ForeColor = Color.White;
            loadSchemaControl.Font = new Font("Segoe UI Semilight", 10);
            loadSchemaControl.CommandDisplay = "Load Column Names From Existing DataTable";
            loadSchemaControl.CommandImage = Resources.command_spreadsheet;
            loadSchemaControl.Click += LoadSchemaControl_Click;
            RenderedControls.Add(loadSchemaControl);

            _dataTableCreationCommands = editor.ConfiguredCommands.Where(f => f is CreateDataTableCommand)
                                                                 .Select(f => (CreateDataTableCommand)f)
                                                                 .ToList();

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Add {v_DataRowDataTable.Rows.Count} Field(s) to '{v_DataTable}']";
        }

        private void LoadSchemaControl_Click(object sender, EventArgs e)
        {
            DataTableVariableSelector selectionForm = new DataTableVariableSelector();
            selectionForm.Text = "Load Schema";
            selectionForm.lblHeader.Text = "Select a DataTable from the list";
            foreach (var item in _dataTableCreationCommands)
            {
                selectionForm.lstVariables.Items.Add(item.v_OutputUserVariableName);
            }

            var result = selectionForm.ShowDialog();

            if (result == DialogResult.OK)
            {
                var tableName = selectionForm.lstVariables.SelectedItem.ToString();
                var schema = _dataTableCreationCommands.Where(f => f.v_OutputUserVariableName == tableName).FirstOrDefault();

                v_DataRowDataTable.Rows.Clear();

                foreach (DataRow rw in schema.v_ColumnNameDataTable.Rows)
                {
                    v_DataRowDataTable.Rows.Add(rw.Field<string>("Column Name"), "");
                }
            }
        }
    }
}