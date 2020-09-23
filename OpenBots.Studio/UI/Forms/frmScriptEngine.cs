﻿//Copyright (c) 2019 Jason Bayldon
//Modifications - Copyright (c) 2020 OpenBots Inc.
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using OpenBots.Commands;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.IO;
using OpenBots.Core.Model.EngineModel;
using OpenBots.Core.Script;
using OpenBots.Core.Settings;
using OpenBots.Core.UI.Forms;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using OpenBots.Core.UI.DTOs;
using OpenBots.UI.Forms.ScriptBuilder_Forms;
using OpenBots.UI.Forms.Supplement_Forms;
using OpenBots.Utilities;
using OpenBots.Core.Properties;

namespace OpenBots.UI.Forms
{
    public partial class frmScriptEngine : ThemedForm, IfrmScriptEngine
    {
        //all variables used by this form
        #region Form Variables
        private EngineSettings _engineSettings;
        public string FilePath { get; set; }
        public string ProjectName { get; set; }
        public string JsonData { get; set; }
        public bool ServerExecution { get; set; }
        public IfrmScriptBuilder CallBackForm { get; set; }
        private bool _advancedDebug;
        public AutomationEngineInstance EngineInstance { get; set; }
        private List<ScriptVariable> _scriptVariableList;
        private List<ScriptElement> _scriptElementList;
        private Dictionary<string, object> _scriptAppInstanceDict;
        public string Result { get; set; }
        public bool IsNewTaskSteppedInto { get; set; }
        public bool IsNewTaskResumed { get; set; }
        public bool IsNewTaskCancelled { get; set; }
        public bool IsHiddenTaskEngine { get; set; }
        public int DebugLineNumber { get; set; }
        public bool IsDebugMode { get; set; }
        public bool CloseWhenDone { get; set; }
        public bool ClosingAllEngines { get; set; }
        public bool IsChildEngine { get; set; }
        public Logger ScriptEngineLogger { get; set; }
        #endregion

        //events and methods
        #region Form Events/Methods
        public frmScriptEngine(string pathToFile, string projectName, frmScriptBuilder builderForm, Logger engineLogger, List<ScriptVariable> variables = null, 
            List<ScriptElement> elements = null, Dictionary<string, object> appInstances = null, bool blnCloseWhenDone = false, bool isDebugMode = false)
        {
            InitializeComponent();

            ScriptEngineLogger = engineLogger;

            IsDebugMode = isDebugMode;

            if (variables != null)
                _scriptVariableList = variables;

            if (elements != null)
                _scriptElementList = elements;

            if (appInstances != null)
                _scriptAppInstanceDict = appInstances;

            CloseWhenDone = blnCloseWhenDone;

            //set callback form
            CallBackForm = builderForm;

            //set file
            FilePath = pathToFile;

            ProjectName = projectName;

            //get engine settings
            _engineSettings = new ApplicationSettings().GetOrCreateApplicationSettings().EngineSettings;

            if (isDebugMode)
            {
                _engineSettings.ShowDebugWindow = true;
                _engineSettings.ShowAdvancedDebugOutput = true;
            }

            //determine whether to show listbox or not
            _advancedDebug = _engineSettings.ShowAdvancedDebugOutput;

            //if listbox should be shown
            if (_advancedDebug)
            {
                lstSteppingCommands.Show();
                lblMainLogo.Show();
                pbBotIcon.Hide();
                lblAction.Hide();
            }
            else
            {
                lstSteppingCommands.Hide();
                lblMainLogo.Hide();
                pbBotIcon.Show();
                lblAction.Show();
            }

            //apply debug window setting
            if (!_engineSettings.ShowDebugWindow)
            {
                Visible = false;
                Opacity = 0;
            }

            //add hooks for hot key cancellation
            GlobalHook.HookStopped += new EventHandler(OnHookStopped);
            GlobalHook.StartEngineCancellationHook(_engineSettings.CancellationKey);
        }
        public frmScriptEngine()
        {
            InitializeComponent();

            //set file
            FilePath = null;

            ProjectName = "";

            //get engine settings
            _engineSettings = new ApplicationSettings().GetOrCreateApplicationSettings().EngineSettings;

            //initialize Logger
            switch (_engineSettings.LoggingSinkType)
            {
                case SinkType.File:
                    if (string.IsNullOrEmpty(_engineSettings.LoggingValue1.Trim()))
                        _engineSettings.LoggingValue1 = Path.Combine(Folders.GetFolder(FolderType.LogFolder), "OpenBots Engine Logs.txt");

                    ScriptEngineLogger = new Logging().CreateFileLogger(_engineSettings.LoggingValue1, Serilog.RollingInterval.Day,
                        _engineSettings.MinLogLevel);
                    break;
                case SinkType.HTTP:
                    ScriptEngineLogger = new Logging().CreateHTTPLogger(ProjectName, _engineSettings.LoggingValue1, _engineSettings.MinLogLevel);
                    break;
                case SinkType.SignalR:
                    string[] groupNames = _engineSettings.LoggingValue3.Split(',').Select(x => x.Trim()).ToArray();
                    string[] userIDs = _engineSettings.LoggingValue4.Split(',').Select(x => x.Trim()).ToArray();

                    ScriptEngineLogger = new Logging().CreateSignalRLogger(ProjectName, _engineSettings.LoggingValue1, _engineSettings.LoggingValue2,
                        groupNames, userIDs, _engineSettings.MinLogLevel);
                    break;
            }

            //determine whether to show listbox or not
            _advancedDebug = _engineSettings.ShowAdvancedDebugOutput;

            //if listbox should be shown
            if (_advancedDebug)
            {
                lstSteppingCommands.Show();
                lblMainLogo.Show();
                pbBotIcon.Hide();
                lblAction.Hide();
            }
            else
            {
                lstSteppingCommands.Hide();
                lblMainLogo.Hide();
                pbBotIcon.Show();
                lblAction.Show();
            }

            //apply debug window setting
            if (!_engineSettings.ShowDebugWindow)
            {
                Visible = false;
                Opacity = 0;
            }

            //add hooks for hot key cancellation
            GlobalHook.HookStopped += new EventHandler(OnHookStopped);
            GlobalHook.StartEngineCancellationHook(_engineSettings.CancellationKey);
        }

        private void frmProcessingStatus_Load(object sender, EventArgs e)
        {
            //move engine form to bottom right and bring to front
            if (_engineSettings.ShowDebugWindow)
            {
                BringToFront();
                MoveFormToBottomRight(this);
            }

            //start running
            EngineInstance = new AutomationEngineInstance(ScriptEngineLogger);

            if (IsNewTaskSteppedInto)
            {
                EngineInstance.PauseScript();
                uiBtnPause.Image = Resources.command_resume;
                uiBtnPause.DisplayText = "Resume";
                uiBtnStepOver.Visible = true;
                uiBtnStepInto.Visible = true;

                CallBackForm.CurrentEngine = this;

                //toggle running flag to allow for tab selection
                CallBackForm.IsScriptRunning = false;
                ((frmScriptBuilder)CallBackForm).OpenFile(FilePath); 
                CallBackForm.IsScriptRunning = true;
            }

            EngineInstance.ReportProgressEvent += Engine_ReportProgress;
            EngineInstance.ScriptFinishedEvent += Engine_ScriptFinishedEvent;
            EngineInstance.LineNumberChangedEvent += EngineInstance_LineNumberChangedEvent;
            EngineInstance.ScriptEngineUI = this;
            EngineInstance.ServerExecution = ServerExecution;

            if (JsonData == null)
            {
                EngineInstance.ExecuteScriptAsync(this, FilePath, _scriptVariableList, _scriptElementList, _scriptAppInstanceDict);
            }
            else
            {
                EngineInstance.ExecuteScriptJson(JsonData);
            }
        }

        /// <summary>
        /// Triggers the automation engine to stop based on a hooked key press
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnHookStopped(object sender, EventArgs e)
        {
            if (EngineInstance != null)
            {
                uiBtnCancel_Click(null, null);
                EngineInstance.CancelScript();
            }  
            GlobalHook.HookStopped -= new EventHandler(OnHookStopped);
        }
        #endregion

        //engine event handlers
        #region Engine Event Handlers
        /// <summary>
        /// Handles Progress Updates raised by Automation Engine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Engine_ReportProgress(object sender, ReportProgressEventArgs e)
        {
            AddStatus(e.ProgressUpdate, e.LoggerColor);
        }

        /// <summary>
        /// Handles Script Finished Event raised by Automation Engine
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Engine_ScriptFinishedEvent(object sender, ScriptFinishedEventArgs e)
        {
            switch (e.Result)
            {
                case ScriptFinishedResult.Successful:
                    AddStatus("Script Completed Successfully");
                    UpdateUI("debug info (success)");
                    if (IsChildEngine)
                        CloseWhenDone = true;
                    break;
                case ScriptFinishedResult.Error:
                    AddStatus("Error: " + e.Error, Color.Red);
                    AddStatus("Script Completed With Errors!");
                    UpdateUI("debug info (error)");
                    break;
                case ScriptFinishedResult.Cancelled:
                    AddStatus("Script Cancelled By User");
                    UpdateUI("debug info (cancelled)");
                    break;
                default:
                    break;
            }

            Result = EngineInstance.TaskResult;
            AddStatus("Total Execution Time: " + e.ExecutionTime.ToString());
            try
            {
                UpdateLineNumber(0);
            }
            catch(Exception) 
            { 
                /*Attemting to reset the debug line to 0 will occasionally produce an exception 
                 if the engine is improperly closed or interrupted during execution.*/
            }
            
            if(CloseWhenDone)
            {
                ((frmScriptEngine)EngineInstance.ScriptEngineUI).Invoke((Action)delegate () { Close(); });
            }
        }

        private void EngineInstance_LineNumberChangedEvent(object sender, LineNumberChangedEventArgs e)
        {
            UpdateLineNumber(e.CurrentLineNumber);
        }
        #endregion

        //delegates to marshal changes to UI
        #region Engine Delegates
        /// <summary>
        /// Delegate for adding progress reports
        /// </summary>
        /// <param name="message">The progress report string from Automation Engine</param>
        public delegate void AddStatusDelegate(string text, Color? statusColor = null);
        /// <summary>
        /// Adds a status to the listbox for debugging and display purposes
        /// </summary>
        /// <param name="text"></param>
        public void AddStatus(string text, Color? statusColor = null)
        {
            if (InvokeRequired)
            {
                var d = new AddStatusDelegate(AddStatus);
                Invoke(d, new object[] { text, statusColor });
            }
            else
            {
                if (text == "Pausing Before Execution" && !uiBtnStepOver.Visible)
                {                  
                    uiBtnPause_Click(null, null);
                    uiBtnStepOver.Visible = true;
                    uiBtnStepInto.Visible = true;
                    
                    if (IsHiddenTaskEngine)
                    {
                        //toggle running flag to allow for tab selection
                        CallBackForm.IsScriptRunning = false;
                        ((frmScriptBuilder)CallBackForm).OpenFile(FilePath); 
                        CallBackForm.IsScriptRunning = true;

                        CallBackForm.CurrentEngine = this;
                        IsNewTaskSteppedInto = true;
                        IsHiddenTaskEngine = false;
                        CallBackForm.IsScriptPaused = false;                       
                    }
                    CallBackForm.IsScriptPaused = false;
                    UpdateLineNumber(DebugLineNumber);
                }
                else if (text == "Pausing Before Exception")
                {
                    uiBtnPause_Click(null, null);
                    uiBtnStepOver.Visible = true;
                    uiBtnStepInto.Visible = true;
                    
                    if (IsHiddenTaskEngine)
                    {
                        CallBackForm.CurrentEngine = this;

                        //toggle running flag to allow for tab selection
                        CallBackForm.IsScriptRunning = false;
                        ((frmScriptBuilder)CallBackForm).OpenFile(FilePath);
                        CallBackForm.IsScriptRunning = true;

                        IsNewTaskSteppedInto = true;
                        IsHiddenTaskEngine = false;
                    }
                    CallBackForm.IsScriptPaused = false;
                    UpdateLineNumber(DebugLineNumber);
                }
                else
                {
                    //update status
                    lblAction.Text = text + "..";
                    SteppingCommandsItem commandsItem = new SteppingCommandsItem
                    {
                        Text = DateTime.Now.ToString("MM/dd/yy hh:mm:ss.fff") + " | " + text,
                        Color = statusColor ?? SystemColors.Highlight
                    };

                    lstSteppingCommands.Items.Add(commandsItem);
                    lstSteppingCommands.SelectedIndex = lstSteppingCommands.Items.Count - 1;
                }
            }
        }

        /// <summary>
        /// Delegate for updating UI after Automation Engine finishes
        /// </summary>
        /// <param name="message"></param>
        public delegate void UpdateUIDelegate(string message);
        /// <summary>
        /// Standard UI updates after automation is finished running
        /// </summary>
        /// <param name="mainLogoText"></param>
        private void UpdateUI(string mainLogoText)
        {
            if (InvokeRequired)
            {
                var d = new UpdateUIDelegate(UpdateUI);
                Invoke(d, new object[] { mainLogoText });
            }
            else
            {
                //set main logo text
                lblMainLogo.Text = mainLogoText;

                //hide and change buttons not required
                uiBtnPause.Visible = false;
                uiBtnStepOver.Visible = false;
                uiBtnStepInto.Visible = false;
                uiBtnCancel.Visible = true;
                uiBtnCancel.DisplayText = "Close";

                if ((!_advancedDebug) && (mainLogoText.Contains("(error)")))
                {
                    pbBotIcon.Image = Resources.error;
                }

                if (mainLogoText.Contains("(error)"))
                {
                    Theme.BgGradientStartColor = Color.OrangeRed;
                    Theme.BgGradientEndColor = Color.OrangeRed;
                    Invalidate();
                }
                else if (mainLogoText.Contains("(success)")) 
                {
                    Theme.BgGradientStartColor = Color.Green;
                    Theme.BgGradientEndColor = Color.Green;
                    Invalidate();                  
                }

                //begin auto close
                if ((_engineSettings.AutoCloseDebugWindow) || (ServerExecution))
                    tmrNotify.Enabled = true;
            }
        }

        /// <summary>
        /// Delegate for showing message box
        /// </summary>
        /// <param name="message"></param>
        public delegate void ShowMessageDelegate(string message, string title, DialogType dialogType, int closeAfter);
        /// <summary>
        /// Used by the automation engine to show a message to the user on-screen. If UI is not available, a standard messagebox will be invoked instead.
        /// </summary>
        public void ShowMessage(string message, string title, DialogType dialogType, int closeAfter)
        {
            if (InvokeRequired)
            {
                var d = new ShowMessageDelegate(ShowMessage);
                Invoke(d, new object[] { message, title, dialogType, closeAfter });
            }
            else
            {
                var confirmationForm = new frmDialog(message, title, dialogType, closeAfter);
                confirmationForm.ShowDialog();
            }
        }

        /// <summary>
        /// Delegate for showing engine context form
        /// </summary>
        /// <param name="message"></param>
        public delegate void ShowEngineContextDelegate(string context, int closeAfter);
        /// <summary>
        /// Used by the automation engine to show the engine context data
        /// </summary>
        public void ShowEngineContext(string context, int closeAfter)
        {
            if (InvokeRequired)
            {
                var d = new ShowEngineContextDelegate(ShowEngineContext);
                Invoke(d, new object[] { context, closeAfter });
            }
            else
            {
                var contextForm = new frmEngineContextViewer(context, closeAfter);
                contextForm.ShowDialog();
            }
        }

        public delegate List<string> ShowInputDelegate(InputCommand inputs);
        public List<string> ShowInput(InputCommand inputs)
        {
            if (InvokeRequired)
            {
                var d = new ShowInputDelegate(ShowInput);
                Invoke(d, new object[] { inputs });
                return null;
            }
            else
            {
                var inputForm = new frmUserInput
                {
                    InputCommand = inputs
                };
                var dialogResult = inputForm.ShowDialog();

                if (dialogResult == DialogResult.OK)
                {
                    var responses = new List<string>();
                    foreach (var ctrl in inputForm.InputControls)
                    {
                        if (ctrl is CheckBox)
                        {
                            var checkboxCtrl = (CheckBox)ctrl;
                            responses.Add(checkboxCtrl.Checked.ToString());
                        }
                        else
                            responses.Add(ctrl.Text);
                    }
                    return responses;
                }
                else
                    return null;
            }
        }

        public delegate List<ScriptVariable> ShowHTMLInputDelegate(string htmlTemplate);
        public List<ScriptVariable> ShowHTMLInput(string htmlTemplate)
        {
            if (InvokeRequired)
            {
                var d = new ShowHTMLInputDelegate(ShowHTMLInput);
                Invoke(d, new object[] { htmlTemplate });
                return null;
            }
            else
            {
                var inputForm = new frmHTMLDisplayForm
                {
                    TemplateHTML = htmlTemplate
                };
                var dialogResult = inputForm.ShowDialog();

                if (inputForm.Result == DialogResult.OK)
                {
                    var variables = inputForm.GetVariablesFromHTML("input");
                    variables.AddRange(inputForm.GetVariablesFromHTML("select"));
                    return variables;
                }
                else
                    return null;
            }
        }

        public delegate void UpdateLineNumberDelegate(int lineNumber);
        public void UpdateLineNumber(int lineNumber)
        {
            if (InvokeRequired)
            {
                var d = new UpdateLineNumberDelegate(UpdateLineNumber);
                Invoke(d, new object[] { lineNumber });
            }
            else
            {
                DebugLineNumber = lineNumber;

                if (CallBackForm != null && !IsHiddenTaskEngine)
                {
                    CallBackForm.DebugLine = lineNumber;
                }
            }
        }
        #endregion

        //various small UI methods
        #region UI Elements
        private void lblClose_MouseEnter(object sender, EventArgs e)
        {
            Cursor = Cursors.Hand;
        }

        private void lblClose_MouseLeave(object sender, EventArgs e)
        {
            Cursor = Cursors.Arrow;
        }

        private void autoCloseTimer_Tick(object sender, EventArgs e)
        {
            Close();
        }

        public delegate void uiBtnCancel_ClickDelegate(object sender, EventArgs e);
        public void uiBtnCancel_Click(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                var d = new uiBtnCancel_ClickDelegate(uiBtnCancel_Click);
                Invoke(d, new object[] { sender, e });
            }
            else
            {
                if (uiBtnCancel.DisplayText == "Close")
                {
                    UpdateLineNumber(0);
                    ClosingAllEngines = true;
                    Close();                   
                    return;
                }

                if (IsNewTaskSteppedInto || IsHiddenTaskEngine)
                {
                    IsNewTaskResumed = false;
                    IsNewTaskCancelled = true;
                }

                uiBtnPause.Visible = false;
                uiBtnCancel.Visible = false;
                uiBtnStepInto.Visible = false;
                uiBtnStepOver.Visible = false;
                lblKillProcNote.Text = "Cancelling...";               
                EngineInstance.ResumeScript();

                SteppingCommandsItem commandsItem = new SteppingCommandsItem
                {
                    Text = "[User Requested Cancellation]",
                    Color = Color.Black
                };
                lstSteppingCommands.Items.Add(commandsItem);
                lstSteppingCommands.SelectedIndex = lstSteppingCommands.Items.Count - 1;
                lblMainLogo.Text = "debug info (cancelling)";
                EngineInstance.CancelScript();
            }          
        }

        public delegate void uiBtnPause_ClickDelegate(object sender, EventArgs e);           
        public void uiBtnPause_Click(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                var d = new uiBtnPause_ClickDelegate(uiBtnPause_Click);
                Invoke(d, new object[] { sender, e });
            }
            else
            {
                if (uiBtnPause.DisplayText == "Pause")
                {
                    SteppingCommandsItem commandsItem = new SteppingCommandsItem
                    {
                        Text = "[User Requested Pause]",
                        Color = Color.Red
                    };
                    lstSteppingCommands.Items.Add(commandsItem);
                    uiBtnPause.Image = Resources.command_resume;
                    uiBtnPause.DisplayText = "Resume";
                    EngineInstance.PauseScript();
                }
                else if (uiBtnPause.DisplayText == "Resume")
                {
                    SteppingCommandsItem commandsItem = new SteppingCommandsItem
                    {
                        Text = "[User Requested Resume]",
                        Color = Color.Green
                    };
                    lstSteppingCommands.Items.Add(commandsItem);
                    uiBtnPause.Image = Resources.command_pause;
                    uiBtnPause.DisplayText = "Pause";
                    uiBtnStepOver.Visible = false;
                    uiBtnStepInto.Visible = false;
                    if (CallBackForm != null)
                    {
                        CallBackForm.IsScriptSteppedOver = false;
                        CallBackForm.IsScriptSteppedInto = false;
                    }
                    if (IsNewTaskSteppedInto || !IsHiddenTaskEngine)
                        IsNewTaskResumed = true;
                    EngineInstance.ResumeScript();
                }

                lstSteppingCommands.SelectedIndex = lstSteppingCommands.Items.Count - 1;
            }   
        }

        public delegate void ResumeParentTaskDelegate();
        public void ResumeParentTask()
        {
            if (InvokeRequired)
            {
                var d = new ResumeParentTaskDelegate(ResumeParentTask);
                Invoke(d, new object[] { });
            }
            else
            {
                uiBtnPause.Image = Resources.command_pause;
                uiBtnPause.DisplayText = "Pause";
                uiBtnStepOver.Visible = false;
                uiBtnStepInto.Visible = false;
                if (CallBackForm != null)
                {
                    CallBackForm.IsScriptSteppedOver = false;
                    CallBackForm.IsScriptSteppedInto = false;
                }
                if (IsNewTaskSteppedInto || !IsHiddenTaskEngine)
                    IsNewTaskResumed = true;
                EngineInstance.ResumeScript();
            }
        }

        public void uiBtnStepOver_Click(object sender, EventArgs e)
        {
            if (CallBackForm != null)
                CallBackForm.IsScriptSteppedOver = true;
            if (IsNewTaskSteppedInto)
                IsNewTaskResumed = false;
            EngineInstance.StepOverScript();
        }

        public void uiBtnStepInto_Click(object sender, EventArgs e)
        {
            if (CallBackForm != null)
                CallBackForm.IsScriptSteppedInto = true;
            if (IsNewTaskSteppedInto)
                IsNewTaskResumed = false;
            EngineInstance.StepIntoScript();
        }

        private void pbBotIcon_Click(object sender, EventArgs e)
        {
            //show debug if user clicks
            lblMainLogo.Show();
            lstSteppingCommands.Visible = !lstSteppingCommands.Visible;
        }

        private void lstSteppingCommands_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            MessageBox.Show(((SteppingCommandsItem)lstSteppingCommands.SelectedItem).Text, "Item Status");
        }

        #endregion UI Elements

        private void lstSteppingCommands_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index != -1)
            {
                SteppingCommandsItem item = lstSteppingCommands.Items[e.Index] as SteppingCommandsItem;

                if (item != null)
                {
                    if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                    {
                        e = new DrawItemEventArgs(e.Graphics, e.Font, e.Bounds, e.Index,
                                                  e.State ^ DrawItemState.Selected,
                                                  e.ForeColor, item.Color);

                        e.DrawBackground();
                        e.Graphics.DrawString(item.Text, e.Font, Brushes.White, e.Bounds);
                    }
                    else
                    {
                        e.DrawBackground();
                        e.Graphics.DrawString(item.Text, e.Font, new SolidBrush(item.Color),
                                              e.Bounds);
                    }
                    e.DrawFocusRectangle();
                }
            }                
        }
    }
}