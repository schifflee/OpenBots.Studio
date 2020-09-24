using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using OpenBots.UI.Forms;
using OpenBots.UI.Forms.ScriptBuilder_Forms;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Task Commands")]
    [Description("This command executes a Task.")]

    public class RunTaskCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Task File Path")]
        [InputSpecification("Enter or select a valid path to the Task file.")]
        [SampleUsage(@"C:\temp\mytask.json || {vScriptPath} || {ProjectPath}\mytask.json")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_taskPath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Assign Variables")]
        [InputSpecification("Select to assign variables to the Task.")]
        [SampleUsage("")]
        [Remarks("If selected, variables will be automatically generated from the Task's *Variable Manager*.")]
        public bool v_AssignVariables { get; set; }

        [XmlElement]
        [PropertyDescription("Task Variables")]
        [InputSpecification("Enter a VariableValue for each input variable.")]
        [SampleUsage("Hello World || {vVariableValue}")]
        [Remarks("For inputs, set VariableReturn to *No*. For outputs, set VariableReturn to *Yes*. " +
                 "Failure to assign a VariableReturn value will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_VariableAssignments { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private CheckBox _passParameters;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _assignmentsGridViewHelper;

        [XmlIgnore]
        public frmScriptEngine NewEngine { get; set; }

        public RunTaskCommand()
        {
            CommandName = "RunTaskCommand";
            SelectionName = "Run Task";
            CommandEnabled = true;
            CustomRendering = true;
            v_taskPath = "{ProjectPath}";

            v_VariableAssignments = new DataTable();
            v_VariableAssignments.Columns.Add("VariableName");
            v_VariableAssignments.Columns.Add("VariableValue");
            v_VariableAssignments.Columns.Add("VariableReturn");
            v_VariableAssignments.TableName = "RunTaskCommandInputParameters" + DateTime.Now.ToString("MMddyyhhmmss");

            _assignmentsGridViewHelper = new DataGridView();
            _assignmentsGridViewHelper.AllowUserToAddRows = true;
            _assignmentsGridViewHelper.AllowUserToDeleteRows = true;
            _assignmentsGridViewHelper.Size = new Size(400, 250);
            _assignmentsGridViewHelper.ColumnHeadersHeight = 30;
            _assignmentsGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _assignmentsGridViewHelper.DataSource = v_VariableAssignments;
            _assignmentsGridViewHelper.Hide();
        }

        public override void RunCommand(object sender)
        {
            AutomationEngineInstance currentScriptEngine = (AutomationEngineInstance)sender;
            var childTaskPath = v_taskPath.ConvertUserVariableToString(currentScriptEngine);

            frmScriptEngine parentEngine = (frmScriptEngine)currentScriptEngine.ScriptEngineUI;
            string parentTaskPath = currentScriptEngine.ScriptEngineUI.FilePath;
            int parentDebugLine = currentScriptEngine.ScriptEngineUI.DebugLineNumber;

            //create variable list
            var variableList = new List<ScriptVariable>();
            var variableReturnList = new List<ScriptVariable>();

            foreach (DataRow rw in v_VariableAssignments.Rows)
            {
                var variableName = (string)rw.ItemArray[0];
                object variableValue = null;

                if (((string)rw.ItemArray[1]).StartsWith("{") && ((string)rw.ItemArray[1]).EndsWith("}"))
                    variableValue = ((string)rw.ItemArray[1]).ConvertUserVariableToObject(currentScriptEngine);

                if (variableValue is string || variableValue == null)
                    variableValue = ((string)rw.ItemArray[1]).ConvertUserVariableToString(currentScriptEngine);

                var variableReturn = (string)rw.ItemArray[2];

                variableList.Add(new ScriptVariable
                {
                    VariableName = variableName.Replace("{", "").Replace("}", ""),
                    VariableValue = variableValue
                });

                if (variableReturn == "Yes")
                {
                    variableReturnList.Add(new ScriptVariable
                    {
                        VariableName = variableName.Replace("{", "").Replace("}", ""),
                        VariableValue = variableValue
                    });
                }
            }

            string projectPath = parentEngine.ProjectPath;
            NewEngine = new frmScriptEngine(childTaskPath, projectPath, (frmScriptBuilder)CurrentScriptBuilder, ((frmScriptBuilder)CurrentScriptBuilder).EngineLogger,
                variableList, null, currentScriptEngine.AppInstances, false, parentEngine.IsDebugMode);
    
            NewEngine.IsChildEngine = true;
            NewEngine.IsHiddenTaskEngine = true;

            if (IsSteppedInto)
            {                
                NewEngine.IsNewTaskSteppedInto = true;
                NewEngine.IsHiddenTaskEngine = false;
            }

            ((frmScriptBuilder)CurrentScriptBuilder).EngineLogger.Information("Executing Child Task: " + Path.GetFileName(childTaskPath));
            ((frmScriptEngine)currentScriptEngine.ScriptEngineUI).Invoke((Action)delegate()
            {
                ((frmScriptEngine)currentScriptEngine.ScriptEngineUI).TopMost = false;
            });
            Application.Run(NewEngine);
            
            if (NewEngine.ClosingAllEngines)
            {
                currentScriptEngine.ScriptEngineUI.ClosingAllEngines = true;
                currentScriptEngine.ScriptEngineUI.CloseWhenDone = true;
            }               

            //get new variable list from the new task engine after it finishes running
            var newVariableList = NewEngine.EngineInstance.VariableList;
            foreach (var variable in variableReturnList)
            {
                //check if the variables we wish to return are in the new variable list
                if (newVariableList.Exists(x => x.VariableName == variable.VariableName))
                {
                    //if yes, get that variable from the new list
                    ScriptVariable newTemp = newVariableList.Where(x => x.VariableName == variable.VariableName).FirstOrDefault();
                    //check if that variable previously existed in the current engine
                    if (currentScriptEngine.VariableList.Exists(x => x.VariableName == newTemp.VariableName))
                    {
                        //if yes, overwrite it
                        ScriptVariable currentTemp = currentScriptEngine.VariableList.Where(x => x.VariableName == newTemp.VariableName).FirstOrDefault();
                        currentScriptEngine.VariableList.Remove(currentTemp);
                    }
                    //Add to current engine variable list
                    currentScriptEngine.VariableList.Add(newTemp);
                }
            }

            //get updated app instance dictionary after the new engine finishes running
            currentScriptEngine.AppInstances = NewEngine.EngineInstance.AppInstances;

            //get errors from new engine (if any)
            var newEngineErrors = NewEngine.EngineInstance.ErrorsOccured;
            if (newEngineErrors.Count > 0)
            {
                currentScriptEngine.ChildScriptFailed = true;
                foreach (var error in newEngineErrors)
                {
                    currentScriptEngine.ErrorsOccured.Add(error);
                }
            }

            ((frmScriptBuilder)CurrentScriptBuilder).EngineLogger.Information("Resuming Parent Task: " + Path.GetFileName(parentTaskPath));
            if (parentEngine.IsDebugMode)
            {
                ((frmScriptEngine)currentScriptEngine.ScriptEngineUI).Invoke((Action)delegate()
                {
                    parentEngine.TopMost = true;
                    parentEngine.IsHiddenTaskEngine = true;

                    if ((IsSteppedInto || !NewEngine.IsHiddenTaskEngine) && !NewEngine.IsNewTaskResumed && !NewEngine.IsNewTaskCancelled)
                    {
                        parentEngine.CallBackForm.CurrentEngine = parentEngine;
                        parentEngine.CallBackForm.IsScriptSteppedInto = true;
                        parentEngine.IsHiddenTaskEngine = false;

                        //toggle running flag to allow for tab selection
                        parentEngine.CallBackForm.IsScriptRunning = false;
                        ((frmScriptBuilder)parentEngine.CallBackForm).OpenFile(parentTaskPath);
                        parentEngine.CallBackForm.IsScriptRunning = true;

                        parentEngine.UpdateLineNumber(parentDebugLine + 1);
                        parentEngine.AddStatus("Pausing Before Execution");
                    }
                    else if (NewEngine.IsNewTaskResumed)
                    {
                        parentEngine.CallBackForm.CurrentEngine = parentEngine;
                        parentEngine.IsNewTaskResumed = true;
                        parentEngine.IsHiddenTaskEngine = true;
                        parentEngine.CallBackForm.IsScriptSteppedInto = false;
                        parentEngine.CallBackForm.IsScriptPaused = false;
                        parentEngine.ResumeParentTask();
                    }
                    else if (NewEngine.IsNewTaskCancelled)
                        parentEngine.uiBtnCancel_Click(null, null);
                    else //child task never stepped into
                        parentEngine.IsHiddenTaskEngine = false;
                });
            }
            else
            {
                ((frmScriptEngine)currentScriptEngine.ScriptEngineUI).Invoke((Action)delegate()
                {
                    parentEngine.TopMost = true;
                });
            }          
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //create file path and helpers
            RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_taskPath", this));
            var taskPathControl = commandControls.CreateDefaultInputFor("v_taskPath", this);
            RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_taskPath", this, new Control[] { taskPathControl }, editor));
            RenderedControls.Add(taskPathControl);
            taskPathControl.TextChanged += TaskPathControl_TextChanged;

            _passParameters = new CheckBox();
            _passParameters.AutoSize = true;
            _passParameters.Text = "Assign Variables";
            _passParameters.Font = new Font("Segoe UI Light", 12);
            _passParameters.ForeColor = Color.White;
            _passParameters.DataBindings.Add("Checked", this, "v_AssignVariables", false, DataSourceUpdateMode.OnPropertyChanged);
            _passParameters.CheckedChanged += (sender, e) => PassParametersCheckbox_CheckedChanged(sender, e, editor);
            commandControls.CreateDefaultToolTipFor("v_AssignVariables", this, _passParameters);
            RenderedControls.Add(_passParameters);

            RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_VariableAssignments", this));
            RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_VariableAssignments", this, new Control[] { _assignmentsGridViewHelper }, editor));
            RenderedControls.Add(_assignmentsGridViewHelper);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Run '{v_taskPath}']";
        }

        private void TaskPathControl_TextChanged(object sender, EventArgs e)
        {
            _passParameters.Checked = false;
        }

        private void PassParametersCheckbox_CheckedChanged(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            AutomationEngineInstance currentScriptEngine = new AutomationEngineInstance(null);
            currentScriptEngine.VariableList.AddRange(editor.ScriptVariables);
            currentScriptEngine.ElementList.AddRange(editor.ScriptElements);

            var startFile = v_taskPath;
            if (startFile.Contains("{ProjectPath}"))
                startFile = startFile.Replace("{ProjectPath}", editor.ProjectPath);

            startFile = startFile.ConvertUserVariableToString(currentScriptEngine);
            
            var Sender = (CheckBox)sender;

            _assignmentsGridViewHelper.Visible = Sender.Checked;

            //load variables if selected and file exists
            if (Sender.Checked && File.Exists(startFile))
            {
                Script deserializedScript = Script.DeserializeFile(startFile);

                foreach (var variable in deserializedScript.Variables)
                {
                    DataRow[] foundVariables  = v_VariableAssignments.Select("VariableName = '" + "{" + variable.VariableName + "}" + "'");
                    if (foundVariables.Length == 0)
                        v_VariableAssignments.Rows.Add("{" + variable.VariableName + "}", variable.VariableValue, "No");
                }
                _assignmentsGridViewHelper.DataSource = v_VariableAssignments;

                for (int i = 0; i < _assignmentsGridViewHelper.Rows.Count - 1; i++)
                {
                    DataGridViewComboBoxCell returnComboBox = new DataGridViewComboBoxCell();
                    returnComboBox.Items.Add("Yes");
                    returnComboBox.Items.Add("No");
                    _assignmentsGridViewHelper.Rows[i].Cells[2] = returnComboBox;
                }
            }
            else if (!Sender.Checked)
            {
                v_VariableAssignments.Clear();
            }
        }       
    }
}
