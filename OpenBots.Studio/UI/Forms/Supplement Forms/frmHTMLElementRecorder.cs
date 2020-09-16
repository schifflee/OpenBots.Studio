using Gecko;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using OpenBots.Commands;
using OpenBots.Core.Command;
using OpenBots.Core.Script;
using OpenBots.Core.Settings;
using OpenBots.UI.Forms.ScriptBuilder_Forms;
using OpenBots.UI.Supplement_Forms;
using OpenBots.Utilities;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmHTMLElementRecorder : UIForm
    {
        public List<ScriptElement> ScriptElements { get; set; }
        public DataTable SearchParameters { get; set; }
        public string LastItemClicked { get; set; }
        public string StartURL { get; set; }
        public bool IsRecordingSequence { get; set; }
        public bool IsCommandItemSelected { get; set; }
        public frmScriptBuilder CallBackForm { get; set; }
        private List<ScriptCommand> _sequenceCommandList;

        private string _browserInstanceName;
        private string _browserEngineType;
        private DataTable _parameterSettings;

        private bool _isFirstRecordClick;
        private string _homeURL = "https://www.google.com/"; //TODO replace with openbots url;
        private string _xPath;
        private string _name;
        private string _id;
        private string _tagName;
        private string _className;
        private string _linkText;
        private List<string> _cssSelectors;
        private bool _isRecording;
        private bool _isHookStopped;
        private SeleniumCreateBrowserCommand _createBrowserCommand;
        private ApplicationSettings _appSettings;
        private Point _lastSavedPoint;

        public frmHTMLElementRecorder(string startURL)
        {
            _appSettings = new ApplicationSettings();
            _appSettings = _appSettings.GetOrCreateApplicationSettings();

            _isFirstRecordClick = true;
            if (string.IsNullOrEmpty(startURL))
                StartURL = _homeURL;
            else
                StartURL = startURL;

            InitializeComponent();

            Directory.SetCurrentDirectory(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
            Xpcom.Initialize("Firefox");
            wbElementRecorder.Navigate(StartURL);
            tbURL.Text = StartURL;
            tbURL.Refresh();
        }

        private void frmHTMLElementRecorder_Load(object sender, EventArgs e)
        {
        }

        private void pbRecord_Click(object sender, EventArgs e)
        {
            if (!_isRecording)
            {
                _isRecording = true;
                TopMost = true;
                if (!chkStopOnClick.Checked)
                    lblDescription.Text = "Recording. Press F2 to save and close.";

                SearchParameters = new DataTable();
                SearchParameters.Columns.Add("Enabled");
                SearchParameters.Columns.Add("Parameter Name");
                SearchParameters.Columns.Add("Parameter Value");
                SearchParameters.TableName = DateTime.Now.ToString("UIASearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

                //clear all
                SearchParameters.Rows.Clear();

                //start global hook and wait for left mouse down event
                GlobalHook.StartEngineCancellationHook(Keys.F2);
                GlobalHook.HookStopped += GlobalHook_HookStopped;
                GlobalHook.StartElementCaptureHook(chkStopOnClick.Checked);
                wbElementRecorder.DomClick += wbElementRecorder_DomClick;
                wbElementRecorder.DomKeyDown += WbElementRecorder_DomKeyDown;

                if (IsRecordingSequence && _isFirstRecordClick)
                {
                    _isFirstRecordClick = false;
                    _sequenceCommandList = new List<ScriptCommand>();

                    frmHTMLElementRecorderSettings settingsForm = new frmHTMLElementRecorderSettings();
                    settingsForm.txtBrowserInstanceName.Text = "DefaultBrowser";
                    settingsForm.cbxBrowserEngineType.SelectedIndex = 0;
                    settingsForm.ShowDialog();

                    if (settingsForm.DialogResult == DialogResult.OK)
                    {
                        _browserInstanceName = settingsForm.txtBrowserInstanceName.Text;
                        _browserEngineType = settingsForm.cbxBrowserEngineType.SelectedItem.ToString();
                        _parameterSettings = settingsForm.ParameterSettingsDT;
                    }
                    else
                    {
                        pbRecord_Click(null, null);
                        return;
                    }

                    if (_browserEngineType != "None")
                    {
                        _createBrowserCommand = new SeleniumCreateBrowserCommand
                        {
                            v_InstanceName = _browserInstanceName,
                            v_EngineType = _browserEngineType,
                            v_URL = wbElementRecorder.Url.ToString()
                        };
                    }                  
                }
            }
            else
            {
                _isRecording = false;
                if (!chkStopOnClick.Checked)
                    lblDescription.Text = "Recording has stopped. Press F2 to save and close.";

                //remove wait for left mouse down event
                wbElementRecorder.DomClick -= wbElementRecorder_DomClick;
                wbElementRecorder.DomKeyDown -= WbElementRecorder_DomKeyDown;
            }
        }

        private void GlobalHook_HookStopped(object sender, EventArgs e)
        {
            wbElementRecorder_DomClick(null, null);
            _isHookStopped = true;
            Close();
        }

        private void wbElementRecorder_DomClick(object sender, DomMouseEventArgs e)
        {
            //mouse down has occured
            if (e != null)
            {
                try
                {
                    _lastSavedPoint = new Point(e.ClientX, e.ClientY);
                    LoadSearchParameters(_lastSavedPoint);
                    if (IsRecordingSequence && _isRecording)
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

        private void WbElementRecorder_DomKeyDown(object sender, DomKeyEventArgs e)
        {
            //mouse down has occured
            if (e != null)
            {
                try
                {
                    LoadSearchParameters(_lastSavedPoint);
                    if (IsRecordingSequence && _isRecording)
                        BuildElementSetTextActionCommand(e.KeyCode);
                }
                catch (Exception)
                {
                    lblDescription.Text = "Error cloning element. Please Try Again.";
                }
            }
        }

        private void LoadSearchParameters(Point point)
        {
            GeckoElement element = wbElementRecorder.DomDocument.ElementFromPoint(point.X, point.Y);

            string savedId = element.GetAttribute("id");
            string uniqueId = Guid.NewGuid().ToString();
            element.SetAttribute("id", uniqueId);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(element.OwnerDocument.GetElementsByTagName("html")[0].OuterHtml);
            element.SetAttribute("id", savedId);
            HtmlNode node = doc.GetElementbyId(uniqueId);

            _xPath = node.XPath.Replace("[1]", "");
            _name = element.GetAttribute("name") == null ? "" : element.GetAttribute("name");
            _id = element.GetAttribute("id") == null ? "" : element.GetAttribute("id"); ;
            _tagName = element.TagName;
            _className = element.GetAttribute("class") == null ? "" : element.GetAttribute("class");
            _linkText = element.TagName.ToLower() != "a" ? "" : element.TextContent;
            _cssSelectors = GetCSSSelectors(element);

            string cssSelectorString = string.Join(", ", GetCSSSelectors(element));

            LastItemClicked = $"[XPath:{_xPath}].[ID:{_id}].[Name:{_name}].[Tag Name:{_tagName}].[Class:{_className}].[Link Text:{_linkText}].[CSS Selector:{cssSelectorString}]";
            lblSubHeader.Text = LastItemClicked;

            if (IsRecordingSequence)
            {
                SearchParameters.Rows.Clear();
                foreach (DataRow row in _parameterSettings.Rows)
                {
                    switch (row[1].ToString())
                    {
                        case "XPath":
                            SearchParameters.Rows.Add(row[0], "XPath", _xPath);
                            break;
                        case "ID":
                            SearchParameters.Rows.Add(row[0], "ID", _id);
                            break;
                        case "Name":
                            SearchParameters.Rows.Add(row[0], "Name", _name);
                            break;
                        case "Tag Name":
                            SearchParameters.Rows.Add(row[0], "Tag Name", _tagName);
                            break;
                        case "Class Name":
                            SearchParameters.Rows.Add(row[0], "Class Name", _className);
                            break;
                        case "Link Text":
                            SearchParameters.Rows.Add(row[0], "Link Text", _linkText);
                            break;
                        case "CSS Selector":
                            for (int i = 0; i < _cssSelectors.Count; i++)
                                SearchParameters.Rows.Add(row[0], $"CSS Selector {i + 1}", _cssSelectors[i]);
                            break;
                    }
                }               
            }
            else
            {
                SearchParameters.Rows.Clear();
                SearchParameters.Rows.Add(true, "XPath", _xPath);
                SearchParameters.Rows.Add(false, "ID", _id);
                SearchParameters.Rows.Add(false, "Name", _name);
                SearchParameters.Rows.Add(false, "Tag Name", _tagName);
                SearchParameters.Rows.Add(false, "Class Name", _className);
                SearchParameters.Rows.Add(false, "Link Text", _linkText);
                for (int i = 0; i < _cssSelectors.Count; i++)
                    SearchParameters.Rows.Add(false, $"CSS Selector {i + 1}", _cssSelectors[i]);
            }
        }

        private void pbHome_Click(object sender, EventArgs e)
        {
            wbElementRecorder.Navigate(_homeURL);
            if (IsRecordingSequence && _isRecording)
                BuildNavigateToURLCommand(_homeURL);
        }

        private void pbRefresh_Click(object sender, EventArgs e)
        {
            wbElementRecorder.Reload();
            if (IsRecordingSequence && _isRecording)
            {
                var refreshBrowserCommand = new SeleniumRefreshCommand
                {
                    v_InstanceName = _browserInstanceName
                };
                _sequenceCommandList.Add(refreshBrowserCommand);
            }
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void pbGo_Click(object sender, EventArgs e)
        {
            wbElementRecorder.Navigate(tbURL.Text);
            if (IsRecordingSequence && _isRecording)
                BuildNavigateToURLCommand(tbURL.Text);
        }

        private void pbBack_Click(object sender, EventArgs e)
        {
            wbElementRecorder.GoBack();
            if (IsRecordingSequence && _isRecording)
            {
                var navigateBackBrowserCommand = new SeleniumNavigateBackCommand
                {
                    v_InstanceName = _browserInstanceName
                };
                _sequenceCommandList.Add(navigateBackBrowserCommand);
            }
        }

        private void pbForward_Click(object sender, EventArgs e)
        {
            wbElementRecorder.GoForward();
            if (IsRecordingSequence && _isRecording)
            {
                var navigateForwardBrowserCommand = new SeleniumNavigateForwardCommand
                {
                    v_InstanceName = _browserInstanceName
                };
                _sequenceCommandList.Add(navigateForwardBrowserCommand);
            }
        }

        private void pbSave_Click(object sender, EventArgs e)
        {
            frmAddElement addElementForm = new frmAddElement("", SearchParameters);
            addElementForm.ScriptElements = ScriptElements;
            addElementForm.ShowDialog();

            if (addElementForm.DialogResult == DialogResult.OK)
            {
                ScriptElement newElement = new ScriptElement()
                {
                    ElementName = addElementForm.txtElementName.Text.Replace("<", "").Replace(">", ""),
                    ElementValue = addElementForm.ElementValueDT
                };

                ScriptElements.Add(newElement);
            }
        }

        private void pbElements_Click(object sender, EventArgs e)
        {
            frmScriptElements scriptElementForm = new frmScriptElements();
            scriptElementForm.ScriptElements = ScriptElements;
            scriptElementForm.ShowDialog();

            if (scriptElementForm.DialogResult == DialogResult.OK)
                ScriptElements = scriptElementForm.ScriptElements;
        }

        private void tbURL_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                pbGo_Click(null, null);
                e.Handled = true;
                e.SuppressKeyPress = true;
            }
        }

        private void frmHTMLElementRecorder_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (wbElementRecorder != null)
            {
                StartURL = wbElementRecorder.Url.ToString();
                wbElementRecorder.Dispose();
                wbElementRecorder = null;

                if (IsRecordingSequence && _isHookStopped)
                    FinalizeRecording();
            }          
            DialogResult = DialogResult.Cancel;
        }

        private void wbElementRecorder_Navigated(object sender, GeckoNavigatedEventArgs e)
        {
            tbURL.Text = wbElementRecorder.Url.ToString();
        }

        private void wbElementRecorder_CreateWindow(object sender, GeckoCreateWindowEventArgs e)
        {
            //force popups to open in the same browser window
            ((GeckoWebBrowser)sender).Navigate(e.Uri);
            e.Cancel = true;
        }

        private List<string> GetCSSSelectors(GeckoElement element)
        {
            var attributes = element.Attributes;
            string tagName = element.TagName.ToLower();
            List<string> attributeList = new List<string>();

            foreach (var attribute in attributes)
                attributeList.Add($"{tagName}[{attribute.NodeName}='{attribute.NodeValue}']");

            return attributeList;
        }
        
        private void BuildNavigateToURLCommand(string url)
        {
            var navigateToURLCommand = new SeleniumNavigateToURLCommand
            {
                v_InstanceName = _browserInstanceName,
                v_URL = url
            };
            _sequenceCommandList.Add(navigateToURLCommand);
        }

        private void BuildElementClickActionCommand()
        {
            BuildWaitForElementActionCommand();

            var clickElementActionCommand = new SeleniumElementActionCommand
            {
                v_InstanceName = _browserInstanceName,
                v_SeleniumSearchParameters = SearchParameters,
                v_SeleniumElementAction = "Invoke Click"
            };
            _sequenceCommandList.Add(clickElementActionCommand);
        }

        private void BuildWaitForElementActionCommand()
        {
            var waitElementActionCommand = new SeleniumElementActionCommand
            {
                v_InstanceName = _browserInstanceName,
                v_SeleniumSearchParameters = SearchParameters,
                v_SeleniumElementAction = "Wait For Element To Exist"
            };

            DataTable webActionDT = waitElementActionCommand.v_WebActionParameterTable;
            DataRow timeoutRow = webActionDT.NewRow();
            timeoutRow["Parameter Name"] = "Timeout (Seconds)";
            timeoutRow["Parameter Value"] = "30";
            webActionDT.Rows.Add(timeoutRow);

            _sequenceCommandList.Add(waitElementActionCommand);
        }

        private void BuildElementSetTextActionCommand(uint key)
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

            GlobalHook.ToUnicode(key, 0, keyboardState, buf, 256, 0);

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
                return;

            //generate sendkeys together
            if ((_sequenceCommandList.Count > 1) && (_sequenceCommandList[_sequenceCommandList.Count - 1] is SeleniumElementActionCommand) 
                && (_sequenceCommandList[_sequenceCommandList.Count - 1] as SeleniumElementActionCommand).v_SeleniumElementAction == "Set Text")
            {
                var lastCreatedSendKeysCommand = (SeleniumElementActionCommand)_sequenceCommandList[_sequenceCommandList.Count - 1];

                if (lastCreatedSendKeysCommand.v_WebActionParameterTable.Rows.Count > 0 && 
                    lastCreatedSendKeysCommand.v_WebActionParameterTable.Rows[0][1].ToString().Contains("{ENTER}"))
                {
                    BuildWaitForElementActionCommand();

                    //build keyboard command
                    var setTextElementActionCommand = new SeleniumElementActionCommand
                    {
                        v_InstanceName = _browserInstanceName,
                        v_SeleniumSearchParameters = SearchParameters,
                        v_SeleniumElementAction = "Set Text"
                    };

                    DataTable webActionDT = setTextElementActionCommand.v_WebActionParameterTable;
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
                    var previouslyInputChars = lastCreatedSendKeysCommand.v_WebActionParameterTable.Rows[0][1].ToString();
                    lastCreatedSendKeysCommand.v_WebActionParameterTable.Rows[0][1] = previouslyInputChars + selectedKey;
                }
            }
            else
            {
                BuildWaitForElementActionCommand();

                //build keyboard command
                var setTextElementActionCommand = new SeleniumElementActionCommand
                {
                    v_InstanceName = _browserInstanceName,
                    v_SeleniumSearchParameters = SearchParameters,
                    v_SeleniumElementAction = "Set Text"
                };

                DataTable webActionDT = setTextElementActionCommand.v_WebActionParameterTable;
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

            var closeBrowserCommand = new SeleniumCloseBrowserCommand
            {
                v_InstanceName = _browserInstanceName
            };
          
            var sequenceCommand = new SequenceCommand 
            { 
                ScriptActions = _sequenceCommandList,
                v_Comment = sequenceComment
            };

            if (_appSettings.ClientSettings.InsertCommandsInline && IsCommandItemSelected)
            {
                if (_browserEngineType != "None")
                    CallBackForm.AddCommandToListView(closeBrowserCommand);

                CallBackForm.AddCommandToListView(sequenceCommand);

                if (_browserEngineType != "None")
                    CallBackForm.AddCommandToListView(_createBrowserCommand);

                CallBackForm.AddCommandToListView(commentCommand);
            }
            else
            {
                CallBackForm.AddCommandToListView(commentCommand);

                if (_browserEngineType != "None")
                    CallBackForm.AddCommandToListView(_createBrowserCommand);

                CallBackForm.AddCommandToListView(sequenceCommand);

                if (_browserEngineType != "None")
                    CallBackForm.AddCommandToListView(closeBrowserCommand);
            }            
        }
    }
}
