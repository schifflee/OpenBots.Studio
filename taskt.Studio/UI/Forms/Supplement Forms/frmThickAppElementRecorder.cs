using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Forms;
using taskt.Commands;
using taskt.Core.Command;
using taskt.Core.Common;
using taskt.Core.Settings;
using taskt.Core.User32;
using taskt.UI.Forms.ScriptBuilder_Forms;
using taskt.UI.Supplement_Forms;
using taskt.Utilities;
using Enums = taskt.Core.Enums;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmThickAppElementRecorder : UIForm
    {
        public DataTable SearchParameters;
        public string LastItemClicked;
        public bool IsRecordingSequence { get; set; }
        public bool IsCommandItemSelected { get; set; }
        public frmScriptBuilder CallBackForm { get; set; }
        private List<ScriptCommand> _sequenceCommandList;

        private DataTable _parameterSettings;

        private string _windowName;
        private bool _isFirstRecordClick;
        private bool _isRecording;
        private bool _isHookStopped;
        private ApplicationSettings _appSettings;

        public frmThickAppElementRecorder()
        {
            _appSettings = new ApplicationSettings();
            _appSettings = _appSettings.GetOrCreateApplicationSettings();
            _isFirstRecordClick = true;

            InitializeComponent();
        }
        
        private void frmThickAppElementRecorder_Load(object sender, EventArgs e)
        {
            //create data source from windows
            cboWindowTitle.DataSource = Common.GetAvailableWindowNames();
        }

        private void pbRecord_Click(object sender, EventArgs e)
        {
            // this.WindowState = FormWindowState.Minimized;
            if (!_isRecording)
            {
                _isRecording = true;
               

                SearchParameters = new DataTable();
                SearchParameters.Columns.Add("Enabled");
                SearchParameters.Columns.Add("Parameter Name");
                SearchParameters.Columns.Add("Parameter Value");
                SearchParameters.TableName = DateTime.Now.ToString("UIASearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

                //clear all
                SearchParameters.Rows.Clear();

                //get window name and find window
                _windowName = cboWindowTitle.Text;
                IntPtr hWnd = User32Functions.FindWindow(_windowName);

                //check if window is found
                if (hWnd != IntPtr.Zero)
                {
                    //set window state and move to 0,0
                    User32Functions.SetWindowState(hWnd, Enums.WindowState.SwShowNormal);
                    User32Functions.SetForegroundWindow(hWnd);
                    User32Functions.SetWindowPosition(hWnd, 0, 0);

                    //start global hook and wait for left mouse down event
                    GlobalHook.StartEngineCancellationHook(Keys.F2);
                    GlobalHook.HookStopped += GlobalHook_HookStopped;
                    GlobalHook.StartElementCaptureHook(chkStopOnClick.Checked);
                    GlobalHook.MouseEvent += GlobalHook_MouseEvent;
                    GlobalHook.KeyDownEvent += GlobalHook_KeyDownEvent;       
                }

                if (IsRecordingSequence && _isFirstRecordClick)
                {
                    _isFirstRecordClick = false;
                    _sequenceCommandList = new List<ScriptCommand>();

                    frmThickAppElementRecorderSettings settingsForm = new frmThickAppElementRecorderSettings();
                    settingsForm.ShowDialog();

                    if (settingsForm.DialogResult == DialogResult.OK)
                    {
                        _parameterSettings = settingsForm.ParameterSettingsDT;
                    }
                    else
                    {
                        pbRecord_Click(null, null);
                        return;
                    }

                    ActivateWindowCommand activateWindowCommand = new ActivateWindowCommand
                    {
                        v_WindowName = _windowName
                    };
                    _sequenceCommandList.Add(activateWindowCommand);
                }

                if (!chkStopOnClick.Checked)
                {
                    lblDescription.Text = $"Recording. Press F2 to stop recording!";
                    MoveFormToBottomRight(this);
                    TopMost = true;
                }
                else
                    WindowState = FormWindowState.Minimized;
            }
            else
            {
                _isRecording = false;
                if (!chkStopOnClick.Checked)
                    lblDescription.Text = "Recording has stopped. Press F2 to save and close.";

                //remove wait for left mouse down event
                GlobalHook.MouseEvent -= GlobalHook_MouseEvent;
                GlobalHook.KeyDownEvent -= GlobalHook_KeyDownEvent;
            }          
        }

        private void GlobalHook_HookStopped(object sender, EventArgs e)
        {
            GlobalHook_MouseEvent(null, null);
            _isHookStopped = true;
            Close();
        }

        private void GlobalHook_MouseEvent(object sender, MouseCoordinateEventArgs e)
        {
            //mouse down has occured
            if (e != null)
            {
                //invoke UIA
                try
                {
                    LoadSearchParameters(e.MouseCoordinates);
                    if (IsRecordingSequence)
                        BuildElementClickActionCommand();
                }
                catch (Exception)
                {
                    lblDescription.Text = "Error cloning element. Please Try Again.";
                }
            }
            
            if (chkStopOnClick.Checked)
                Close();     
        }

        private void GlobalHook_KeyDownEvent(object sender, KeyDownEventArgs e)
        {
            //key down has occured
            if (e != null)
            {
                //invoke UIA
                try
                {
                    LoadSearchParameters(e.MouseCoordinates);

                    if (IsRecordingSequence && _isRecording)
                        BuildElementSetTextActionCommand(e.Key);
                }
                catch (Exception)
                {
                    lblDescription.Text = "Error cloning element. Please Try Again.";
                }
            }
        }

        private void LoadSearchParameters(Point point)
        {
            AutomationElement element = AutomationElement.FromPoint(point);
            AutomationElement.AutomationElementInformation elementProperties = element.Current;

            LastItemClicked = $"[Automation ID:{elementProperties.AutomationId}].[Name:{elementProperties.Name}].[Class:{elementProperties.ClassName}]";
            lblSubHeader.Text = LastItemClicked;

            //get properties from class via reflection
            PropertyInfo[] properties = typeof(AutomationElement.AutomationElementInformation).GetProperties();
            Array.Sort(properties, (x, y) => string.Compare(x.Name, y.Name));

            SearchParameters.Rows.Clear();

            if (IsRecordingSequence)
            {
                //loop through each property and get value from the element
                foreach (DataRow row in _parameterSettings.Rows) 
                {
                    foreach (PropertyInfo property in properties)
                    {
                        try
                        {
                            var propName = property.Name;
                            var propValue = property.GetValue(elementProperties, null);

                            //if property is a basic type then display
                            if ((propName == row[1].ToString()) && ((propValue is string) || (propValue is bool) || (propValue is int) || (propValue is double)))
                                SearchParameters.Rows.Add(row[0], propName, propValue);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error Iterating over properties in window: " + ex.ToString());
                        }
                    }
                }
            }
            else
            {                
                //loop through each property and get value from the element
                foreach (PropertyInfo property in properties)
                {
                    try
                    {
                        var propName = property.Name;
                        var propValue = property.GetValue(elementProperties, null);

                        //if property is a basic type then display
                        if ((propValue is string) || (propValue is bool) || (propValue is int) || (propValue is double))
                            SearchParameters.Rows.Add(false, propName, propValue);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Error Iterating over properties in window: " + ex.ToString());
                    }
                }
            }
        }

        private void pbRefresh_Click(object sender, EventArgs e)
        {
            //handle window refresh requests
            cboWindowTitle.DataSource = Common.GetAvailableWindowNames();
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {       
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void BuildElementClickActionCommand()
        {
            BuildWaitForElementActionCommand();

            var clickElementActionCommand = new UIAutomationCommand
            {
                v_WindowName = _windowName,
                v_UIASearchParameters = SearchParameters,
                v_AutomationType = "Click Element"
            };

            DataTable webActionDT = clickElementActionCommand.v_UIAActionParameters;
            DataRow clickTypeRow = webActionDT.NewRow();
            clickTypeRow["Parameter Name"] = "Click Type";
            clickTypeRow["Parameter Value"] = "Left Click";
            webActionDT.Rows.Add(clickTypeRow);

            _sequenceCommandList.Add(clickElementActionCommand);
        }

        private void BuildWaitForElementActionCommand()
        {
            var waitElementActionCommand = new UIAutomationCommand
            {
                v_WindowName = _windowName,
                v_UIASearchParameters = SearchParameters,
                v_AutomationType = "Wait For Element To Exist"
            };

            DataTable webActionDT = waitElementActionCommand.v_UIAActionParameters;
            DataRow timeoutRow = webActionDT.NewRow();
            timeoutRow["Parameter Name"] = "Timeout (Seconds)";
            timeoutRow["Parameter Value"] = "30";
            webActionDT.Rows.Add(timeoutRow);

            _sequenceCommandList.Add(waitElementActionCommand);
        }

        private void BuildElementSetTextActionCommand(Keys key)
        {
            bool toUpperCase = false;

            //determine if casing is needed
            if (GlobalHook.IsKeyDown(Keys.ShiftKey) && GlobalHook.IsKeyToggled(Keys.Capital))
                toUpperCase = false;
            else if (!GlobalHook.IsKeyDown(Keys.ShiftKey) && GlobalHook.IsKeyToggled(Keys.Capital))
                toUpperCase = true;
            else if (GlobalHook.IsKeyDown(Keys.ShiftKey) && !GlobalHook.IsKeyToggled(Keys.Capital))
                toUpperCase = true;
            else if (!GlobalHook.IsKeyDown(Keys.ShiftKey) && !GlobalHook.IsKeyToggled(Keys.Capital))
                toUpperCase = false;

            var buf = new StringBuilder(256);
            var keyboardState = new byte[256];

            if (toUpperCase)
                keyboardState[(int)Keys.ShiftKey] = 0xff;

            GlobalHook.ToUnicode((uint)key, 0, keyboardState, buf, 256, 0);

            var selectedKey = buf.ToString();

            if ((selectedKey == "") || (selectedKey == "\r"))
                selectedKey = key.ToString();

            //translate key press to sendkeys identifier
            if (selectedKey == GlobalHook.StopHookKey)
            {
                //STOP HOOK
                GlobalHook.StopHook();
                return;
            }
            else if (selectedKey == "Return")
                selectedKey = "ENTER";
            else if (selectedKey == "Space")
                selectedKey = " ";
            else if (selectedKey == "OemPeriod")
                selectedKey = ".";
            else if (selectedKey == "Oemcomma")
                selectedKey = ",";
            else if (selectedKey == "OemQuestion")
                selectedKey = "?";
            else if (selectedKey.Contains("ShiftKey"))
                return;

            //add braces
            if (selectedKey.Length > 1)
                selectedKey = "{" + selectedKey + "}";

            //generate sendkeys together
            if ((_sequenceCommandList.Count > 1) && (_sequenceCommandList[_sequenceCommandList.Count - 1] is UIAutomationCommand)
                && (_sequenceCommandList[_sequenceCommandList.Count - 1] as UIAutomationCommand).v_AutomationType == "Set Text")
            {
                var lastCreatedSendKeysCommand = (UIAutomationCommand)_sequenceCommandList[_sequenceCommandList.Count - 1];

                if (lastCreatedSendKeysCommand.v_UIAActionParameters.Rows.Count > 0 &&
                    lastCreatedSendKeysCommand.v_UIAActionParameters.Rows[0][1].ToString().Contains("{ENTER}"))
                {
                    BuildWaitForElementActionCommand();

                    //build keyboard command
                    var setTextElementActionCommand = new UIAutomationCommand
                    {
                        v_WindowName = _windowName,
                        v_UIASearchParameters = SearchParameters,
                        v_AutomationType = "Set Text"
                    };

                    DataTable webActionDT = setTextElementActionCommand.v_UIAActionParameters;
                    DataRow textToSetRow = webActionDT.NewRow();
                    textToSetRow["Parameter Name"] = "Text To Set";
                    textToSetRow["Parameter Value"] = selectedKey;
                    webActionDT.Rows.Add(textToSetRow);

                    _sequenceCommandList.Add(setTextElementActionCommand);
                }
                else
                {
                    //append chars to previously created command
                    //this makes editing easier for the user because only 1 command is issued rather than multiples
                    var previouslyInputChars = lastCreatedSendKeysCommand.v_UIAActionParameters.Rows[0][1].ToString();
                    lastCreatedSendKeysCommand.v_UIAActionParameters.Rows[0][1] = previouslyInputChars + selectedKey;
                }
            }
            else
            {
                BuildWaitForElementActionCommand();

                //build keyboard command
                var setTextElementActionCommand = new UIAutomationCommand
                {
                    v_WindowName = _windowName,
                    v_UIASearchParameters = SearchParameters,
                    v_AutomationType = "Set Text"
                };

                DataTable webActionDT = setTextElementActionCommand.v_UIAActionParameters;
                DataRow textToSetRow = webActionDT.NewRow();
                textToSetRow["Parameter Name"] = "Text To Set";
                textToSetRow["Parameter Value"] = selectedKey;
                webActionDT.Rows.Add(textToSetRow);

                _sequenceCommandList.Add(setTextElementActionCommand);
            }
        }

        private void FinalizeRecording()
        {
            string sequenceComment = $"Web Sequence Recorded {DateTime.Now}";

            var commentCommand = new AddCodeCommentCommand
            {
                v_Comment = sequenceComment
            };

            var sequenceCommand = new SequenceCommand
            {
                ScriptActions = _sequenceCommandList,
                v_Comment = sequenceComment
            };

            if (_appSettings.ClientSettings.InsertCommandsInline && IsCommandItemSelected)
            {
                CallBackForm.AddCommandToListView(sequenceCommand);
                CallBackForm.AddCommandToListView(commentCommand);
            }
            else
            {
                CallBackForm.AddCommandToListView(commentCommand);
                CallBackForm.AddCommandToListView(sequenceCommand);
            }
        }

        private void frmThickAppElementRecorder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsRecordingSequence && _isHookStopped)
            {
                GlobalHook.HookStopped -= GlobalHook_HookStopped;
                FinalizeRecording();
            }

            DialogResult = DialogResult.Cancel;
        }
    }
}
