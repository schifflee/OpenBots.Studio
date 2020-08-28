using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.Properties;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Database Commands")]
    [Description("This command allows you to perform a database query and apply the result to a dataset")]
    [UsesDescription("Use this command to select data from a database.")]
    [ImplementationDescription("This command implements 'OLEDB' to achieve automation.")]
    public class ExecuteDatabaseQueryCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Database Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Define Database Connection** command.")]
        [SampleUsage("MyBrowserInstance")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **Define Database Connection** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Define Query Execution Type")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUISelectionOption("Return Dataset")]
        [PropertyUISelectionOption("Execute NonQuery")]
        [PropertyUISelectionOption("Execute Stored Procedure")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_QueryType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Define Query to Execute")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Query { get; set; }

        [XmlElement]
        [PropertyDescription("Define Query Parameters")]
        [InputSpecification("")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_QueryParameters { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView QueryParametersGridView;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> QueryParametersControls;

        [XmlAttribute]
        [PropertyDescription("Output Dataset Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public ExecuteDatabaseQueryCommand()
        {
            CommandName = "ExecuteDatabaseQueryCommand";
            SelectionName = "Execute Database Query";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultDatabase";

            v_QueryParameters = new DataTable
            {
                TableName = "QueryParamTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            };

            v_QueryParameters.Columns.Add("Parameter Name");
            v_QueryParameters.Columns.Add("Parameter Value");
            v_QueryParameters.Columns.Add("Parameter Type");

        }

        public override void RunCommand(object sender)
        {
            //create engine, instance, query
            var engine = (AutomationEngineInstance)sender;
            var query = v_Query.ConvertUserVariableToString(engine);

            //define connection
            var databaseConnection = (OleDbConnection)v_InstanceName.GetAppInstance(engine);
            var queryExecutionType = v_QueryType.ConvertUserVariableToString(engine);

            //define commad
            var oleCommand = new OleDbCommand(query, databaseConnection);

            //add parameters
            foreach (DataRow rw in v_QueryParameters.Rows)
            {
                var parameterName = rw.Field<string>("Parameter Name").ConvertUserVariableToString(engine);
                var parameterValue = rw.Field<string>("Parameter Value").ConvertUserVariableToString(engine);
                var parameterType = rw.Field<string>("Parameter Type").ConvertUserVariableToString(engine);

                object convertedValue = null;
                //"STRING", "BOOLEAN", "DECIMAL", "INT16", "INT32", "INT64", "DATETIME", "DOUBLE", "SINGLE", "GUID", "BYTE", "BYTE[]"
                switch (parameterType)
                {
                    case "STRING":
                        convertedValue = parameterValue;
                        break;
                    case "BOOLEAN":
                        convertedValue = Convert.ToBoolean(parameterValue);
                        break;
                    case "DECIMAL":
                        convertedValue = Convert.ToDecimal(parameterValue);
                        break;
                    case "INT16":
                        convertedValue = Convert.ToInt16(parameterValue);
                        break;
                    case "INT32":
                        convertedValue = Convert.ToInt32(parameterValue);
                        break;
                    case "INT64":
                        convertedValue = Convert.ToInt64(parameterValue);
                        break;
                    case "DATETIME":
                        convertedValue = Convert.ToDateTime(parameterValue);
                        break;
                    case "DOUBLE":
                        convertedValue = Convert.ToDouble(parameterValue);
                        break;
                    case "SINGLE":
                        convertedValue = Convert.ToSingle(parameterValue);
                        break;
                    case "GUID":
                        convertedValue = Guid.Parse(parameterValue);
                        break;
                    case "BYTE":
                        convertedValue = Convert.ToByte(parameterValue);
                        break;
                    case "BYTE[]":
                        convertedValue = System.Text.Encoding.UTF8.GetBytes(parameterValue);
                        break;
                    default:
                        throw new NotImplementedException($"Parameter Type '{parameterType}' not implemented!");
                }

                oleCommand.Parameters.AddWithValue(parameterName, convertedValue);


            }

            if (queryExecutionType == "Return Dataset")
            {

                DataTable dataTable = new DataTable();
                OleDbDataAdapter adapter = new OleDbDataAdapter(oleCommand);
                adapter.SelectCommand = oleCommand;
                databaseConnection.Open();
                adapter.Fill(dataTable);
                databaseConnection.Close();

                
                dataTable.TableName = v_OutputUserVariableName;
                engine.DataTables.Add(dataTable);

                dataTable.StoreInUserVariable(engine, v_OutputUserVariableName);
           
            }
            else if (queryExecutionType == "Execute NonQuery")
            {
                databaseConnection.Open();
                var result = oleCommand.ExecuteNonQuery();
                databaseConnection.Close();

                result.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
            }
            else if(queryExecutionType == "Execute Stored Procedure")
            {
                oleCommand.CommandType = CommandType.StoredProcedure;
                databaseConnection.Open();
                var result = oleCommand.ExecuteNonQuery();
                databaseConnection.Close();
                result.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
            }
            else
            {
                throw new NotImplementedException($"Query Execution Type '{queryExecutionType}' not implemented.");
            }

        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));


            var queryControls = CommandControls.CreateDefaultInputGroupFor("v_Query", this, editor);
            var queryBox = (TextBox)queryControls[2];
            queryBox.Multiline = true;
            queryBox.Height = 150;
            RenderedControls.AddRange(queryControls);

            //set up query parameter controls
            QueryParametersGridView = new DataGridView();
            QueryParametersGridView.AllowUserToAddRows = true;
            QueryParametersGridView.AllowUserToDeleteRows = true;
            QueryParametersGridView.Size = new Size(400, 250);
            QueryParametersGridView.ColumnHeadersHeight = 30;
            QueryParametersGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            QueryParametersGridView.AutoGenerateColumns = false;
        

            var selectColumn = new DataGridViewComboBoxColumn();
            selectColumn.HeaderText = "Type";
            selectColumn.DataPropertyName = "Parameter Type";
            selectColumn.DataSource = new string[] { "STRING", "BOOLEAN", "DECIMAL", "INT16", "INT32", "INT64", "DATETIME", "DOUBLE", "SINGLE", "GUID", "BYTE", "BYTE[]" };
            QueryParametersGridView.Columns.Add(selectColumn);

            var paramNameColumn = new DataGridViewTextBoxColumn();
            paramNameColumn.HeaderText = "Name";
            paramNameColumn.DataPropertyName = "Parameter Name";
            QueryParametersGridView.Columns.Add(paramNameColumn);

            var paramValueColumn = new DataGridViewTextBoxColumn();
            paramValueColumn.HeaderText = "Value";
            paramValueColumn.DataPropertyName = "Parameter Value";
            QueryParametersGridView.Columns.Add(paramValueColumn);

            QueryParametersGridView.DataBindings.Add("DataSource", this, "v_QueryParameters", false, DataSourceUpdateMode.OnPropertyChanged);
         
            QueryParametersControls = new List<Control>();

            QueryParametersControls.Add(CommandControls.CreateDefaultLabelFor("v_QueryParameters", this));
            QueryParametersControls.AddRange(CommandControls.CreateUIHelpersFor("v_QueryParameters", this, new Control[] { QueryParametersGridView }, editor));

            CommandItemControl helperControl = new CommandItemControl();
            helperControl.Padding = new Padding(10, 0, 0, 0);
            helperControl.ForeColor = Color.AliceBlue;
            helperControl.Font = new Font("Segoe UI Semilight", 10);
            helperControl.Name = "add_param_helper";
            helperControl.CommandImage = Resources.command_database2;
            helperControl.CommandDisplay = "Add Parameter";
            helperControl.Click += (sender, e) => AddParameter(sender, e);
            QueryParametersControls.Add(helperControl);
            QueryParametersControls.Add(QueryParametersGridView);
            RenderedControls.AddRange(QueryParametersControls);



            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_QueryType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
            return RenderedControls;

        }

        private void AddParameter(object sender, EventArgs e)
        {
            v_QueryParameters.Rows.Add();
        }

        public override string GetDisplayValue()
        {
            return $"{base.GetDisplayValue()} - [{v_QueryType}, Apply Result to Variable '{v_OutputUserVariableName}', Instance Name: '{v_InstanceName}']";
        }
    }
}