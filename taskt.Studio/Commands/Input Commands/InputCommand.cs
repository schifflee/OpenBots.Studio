using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Common;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.Core.Properties;
using taskt.Core.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Commands
{
    [Serializable]
    [Group("Input Commands")]
    [Description("This command provides the user with a form to input and store a collection of data.")]
    public class InputCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Header Name")]
        [InputSpecification("Define the header to be displayed on the input form.")]
        [SampleUsage("Please Provide Input || {vHeader}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputHeader { get; set; }

        [XmlAttribute]
        [PropertyDescription("Input Directions")]
        [InputSpecification("Define the directions to give to the user.")]
        [SampleUsage("Directions: Please fill in the following fields || {vDirections}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputDirections { get; set; }

        [XmlElement]
        [PropertyDescription("Input Parameters")]
        [InputSpecification("Define the required input parameters.")]
        [SampleUsage("[TextBox | Name | 500,30 | John | {vName}]")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_UserInputConfig { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _userInputGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private CommandItemControl _addRowControl;

        public InputCommand()
        {
            CommandName = "InputCommand";
            SelectionName = "Prompt for Input";
            CommandEnabled = true;
            CustomRendering = true;

            v_UserInputConfig = new DataTable();
            v_UserInputConfig.TableName = DateTime.Now.ToString("UserInputParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
            v_UserInputConfig.Columns.Add("Type");
            v_UserInputConfig.Columns.Add("Label");
            v_UserInputConfig.Columns.Add("Size");
            v_UserInputConfig.Columns.Add("DefaultValue");
            v_UserInputConfig.Columns.Add("StoreInVariable");

            v_InputHeader = "Please Provide Input";
            v_InputDirections = "Directions: Please fill in the following fields";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            if (engine.TasktEngineUI == null)
            {
                engine.ReportProgress("UserInput Supported With UI Only");
                MessageBox.Show("UserInput Supported With UI Only", "UserInput Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //create clone of original
            dynamic clonedCommand = Common.Clone(this);

            //translate variable
            clonedCommand.v_InputHeader = ((string)clonedCommand.v_InputHeader).ConvertUserVariableToString(engine);
            clonedCommand.v_InputDirections = ((string)clonedCommand.v_InputDirections).ConvertUserVariableToString(engine);

            //translate variables for each label
            foreach (DataRow rw in clonedCommand.v_UserInputConfig.Rows)
            {
                rw["DefaultValue"] = rw["DefaultValue"].ToString().ConvertUserVariableToString(engine);
                var targetVariable = rw["StoreInVariable"] as string;

                if (string.IsNullOrEmpty(targetVariable))
                {
                    var newMessage = new ShowMessageCommand();
                    newMessage.v_Message = "User Input question '" + rw["Label"] + "' is missing variables to apply results to! " + 
                                           "Results for the item will not be tracked. To fix this, assign a variable in the designer!";
                    newMessage.v_AutoCloseAfter = "10";
                    newMessage.RunCommand(sender);
                }
            }

            //invoke ui for data collection
            var result = ((frmScriptEngine)engine.TasktEngineUI).Invoke(new Action(() =>
            {
                //get input from user
              var userInputs = ((frmScriptEngine)engine.TasktEngineUI).ShowInput(clonedCommand);

                //check if user provided input
                if (userInputs != null)
                {
                    //loop through each input and assign
                    for (int i = 0; i < userInputs.Count; i++)
                    {                       
                        //get target variable
                        var targetVariable = v_UserInputConfig.Rows[i]["StoreInVariable"] as string;

                        //store user data in variable
                        if (!string.IsNullOrEmpty(targetVariable))
                            ((object)userInputs[i]).StoreInUserVariable(engine, targetVariable);
                    }
                }
            }));
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            _userInputGridViewHelper = new DataGridView();
            _userInputGridViewHelper.KeyDown += UserInputDataGridView_KeyDown;
            _userInputGridViewHelper.DataBindings.Add("DataSource", this, "v_UserInputConfig", false, DataSourceUpdateMode.OnPropertyChanged);

            var typefield = new DataGridViewComboBoxColumn();
            typefield.Items.Add("TextBox");
            typefield.Items.Add("CheckBox");
            typefield.Items.Add("ComboBox");
            typefield.HeaderText = "Input Type";
            typefield.DataPropertyName = "Type";
            _userInputGridViewHelper.Columns.Add(typefield);

            var field = new DataGridViewTextBoxColumn();
            field.HeaderText = "Input Label";
            field.DataPropertyName = "Label";
            _userInputGridViewHelper.Columns.Add(field);

            field = new DataGridViewTextBoxColumn();
            field.HeaderText = "Input Size (X,Y)";
            field.DataPropertyName = "Size";
            _userInputGridViewHelper.Columns.Add(field);

            field = new DataGridViewTextBoxColumn();
            field.HeaderText = "Default Value";
            field.DataPropertyName = "DefaultValue";
            _userInputGridViewHelper.Columns.Add(field);

            field = new DataGridViewTextBoxColumn();
            field.HeaderText = "Assigned Variable";
            field.DataPropertyName = "StoreInVariable";
            _userInputGridViewHelper.Columns.Add(field);

            _userInputGridViewHelper.ColumnHeadersHeight = 30;
            _userInputGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _userInputGridViewHelper.AllowUserToAddRows = false;
            _userInputGridViewHelper.AllowUserToDeleteRows = false;

            _addRowControl = new CommandItemControl();
            _addRowControl.Padding = new Padding(10, 0, 0, 0);
            _addRowControl.ForeColor = Color.AliceBlue;
            _addRowControl.Font = new Font("Segoe UI Semilight", 10);
            _addRowControl.CommandImage = Resources.command_input;
            _addRowControl.CommandDisplay = "Add Input Parameter";
            _addRowControl.Click += (sender, e) => AddInputParameter(sender, e, editor);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputHeader", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputDirections", this, editor));

            RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_UserInputConfig", this));
            RenderedControls.Add(_addRowControl);
            RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_UserInputConfig", this, new Control[] { _userInputGridViewHelper }, editor));
            RenderedControls.Add(_userInputGridViewHelper);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Header '{v_InputHeader}']";
        }

        private void AddInputParameter(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            var newRow = v_UserInputConfig.NewRow();
            newRow["Size"] = "500,30";
            v_UserInputConfig.Rows.Add(newRow);
        }

        private void UserInputDataGridView_KeyDown(object sender, KeyEventArgs e)
        {
            if (_userInputGridViewHelper.SelectedRows.Count > 0)
                _userInputGridViewHelper.Rows.RemoveAt(_userInputGridViewHelper.SelectedCells[0].RowIndex);
        }
    }
}