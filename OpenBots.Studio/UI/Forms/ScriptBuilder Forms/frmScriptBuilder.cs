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
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using OpenBots.Commands;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.IO;
using OpenBots.Core.Script;
using OpenBots.Core.Settings;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using OpenBots.UI.CustomControls;
using OpenBots.UI.CustomControls.CustomUIControls;
using OpenBots.UI.Forms.Supplement_Forms;
using OpenBots.Utilities;
using Point = System.Drawing.Point;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form, IfrmScriptBuilder
    //Form tracks the overall configuration and enables script editing, saving, and running
    //Features ability to add, drag/drop reorder commands
    {
        #region Instance Variables
        private List<ListViewItem> _rowsSelectedForCopy;
        private List<ScriptVariable> _scriptVariables;
        private List<ScriptElement> _scriptElements;
        private List<AutomationCommand> _automationCommands;
        private bool _editMode;
        private ImageList _uiImages;
        private ApplicationSettings _appSettings;
        private DateTime _lastAntiIdleEvent;
        private int _reqdIndex;
        private int _selectedIndex = -1;
        private List<int> _matchingSearchIndex = new List<int>();
        private int _currentIndex = -1;
        private frmScriptBuilder _parentBuilder;
        private UIListView _selectedTabScriptActions;
        private string _scriptFilePath;
        public string ScriptFilePath
        {
            get
            {
                return _scriptFilePath;
            }
            set
            {
                _scriptFilePath = value;
                UpdateWindowTitle();
            }
        }
        public Project ScriptProject { get; set; }
        private string _scriptProjectPath;
        private string _mainFileName;
        private Point _lastClickPosition;
        private int _debugLine;
        public int DebugLine
        {
            get
            {
                return _debugLine;
            }
            set
            {
                _debugLine = value;
                if (_debugLine > 0)
                {
                    try
                    {
                        IsScriptRunning = true;
                        _selectedTabScriptActions.EnsureVisible(_debugLine - 1);
                    }
                    catch (Exception)
                    {
                        //log exception?
                    }
                }
                else if (_debugLine == 0)
                {
                    IsScriptRunning = false;
                    IsScriptSteppedOver = false;
                    IsScriptSteppedInto = false;
                    RemoveDebugTab();
                }

                _selectedTabScriptActions.Invalidate();
                //FormatCommandListView();

                if (IsScriptSteppedInto || IsScriptSteppedOver)
                    CreateDebugTab();
            }
        }
        private List<string> _notificationList = new List<string>();
        private DateTime _notificationExpires;
        private bool _isDisplaying;
        private string _notificationText;
        public IfrmScriptEngine CurrentEngine { get; set; }
        public bool IsScriptRunning { get; set; }
        public bool IsScriptPaused { get; set; }
        public bool IsScriptSteppedOver { get; set; }
        public bool IsScriptSteppedInto { get; set; }
        public bool IsUnhandledException { get; set; }
        public Logger EngineLogger { get; set; }
        private bool _isDebugMode;
        private TreeView _tvCommandsCopy;
        private string _txtCommandWatermark = "Type Here to Search";   
        public string HTMLElementRecorderURL { get; set; }
        private bool _isSequence;
        #endregion

        #region Form Events
        public frmScriptBuilder()
        {
            _selectedTabScriptActions = NewLstScriptActions();
            InitializeComponent();           
        }

        private void UpdateWindowTitle()
        {
            if (!string.IsNullOrEmpty(ScriptFilePath))
            {
                FileInfo scriptFileInfo = new FileInfo(ScriptFilePath);
                Text = "OpenBots Studio - (Project: " + ScriptProject.ProjectName + " - Script: " + scriptFileInfo.Name + ")";
            }
            else if (ScriptProject.ProjectName != null)
            {
                Text = "OpenBots Studio - (Project: " + ScriptProject.ProjectName + ")";
            }
            else
            {
                Text = "OpenBots Studio";
            }
        }

        private void frmScriptBuilder_Load(object sender, EventArgs e)
        {
            //load all commands
            _automationCommands = UIControlsHelper.GenerateCommandsandControls();

            //set controls double buffered
            foreach (Control control in Controls)
            {
                typeof(Control).InvokeMember("DoubleBuffered",
                    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
                    null, control, new object[] { true });
            }

            //get app settings
            _appSettings = new ApplicationSettings();
            _appSettings = _appSettings.GetOrCreateApplicationSettings();      

            string clientLoggerFilePath = Path.Combine(Folders.GetFolder(FolderType.LogFolder), "OpenBots Automation Client Logs.txt");
            Logger automationClientLogger = new Logging().CreateFileLogger(clientLoggerFilePath, Serilog.RollingInterval.Day);
            //Core.Sockets.SocketClient.Initialize();
            //Core.Sockets.SocketClient.associatedBuilder = this;

            //handle action bar preference
            //hide action panel

            if (_editMode)
            {
                tlpControls.RowStyles[0].SizeType = SizeType.Absolute;
                tlpControls.RowStyles[0].Height = 0;

                tlpControls.RowStyles[1].SizeType = SizeType.Absolute;
                tlpControls.RowStyles[1].Height = 81;
            }
            else if (_appSettings.ClientSettings.UseSlimActionBar)
            {
                tlpControls.RowStyles[1].SizeType = SizeType.Absolute;
                tlpControls.RowStyles[1].Height = 0;
            }
            else
            {
                tlpControls.RowStyles[0].SizeType = SizeType.Absolute;
                tlpControls.RowStyles[0].Height = 0;
            }

            //get scripts folder
            var rpaScriptsFolder = Folders.GetFolder(FolderType.ScriptsFolder);

            if (!Directory.Exists(rpaScriptsFolder))
            {
                frmDialog userDialog = new frmDialog("Would you like to create a folder to save your scripts in now? " +
                    "A script folder is required to save scripts generated with this application. " +
                    "The new script folder path would be '" + rpaScriptsFolder + "'.", "Unable to locate Script Folder!",
                    DialogType.YesNo, 0);

                if (userDialog.ShowDialog() == DialogResult.OK)
                {
                    Directory.CreateDirectory(rpaScriptsFolder);
                }
            }

            //get latest files for recent files list on load
            GenerateRecentFiles();

            //no height for status bar
            HideNotificationRow();

            //instantiate for script variables
            if (!_editMode)
            {
                _scriptVariables = new List<ScriptVariable>();
                _scriptElements = new List<ScriptElement>();
            }
            //pnlHeader.BackColor = Color.FromArgb(255, 214, 88);

            //instantiate and populate display icons for commands
            _uiImages = UIImage.UIImageList();

            //set image list
            _selectedTabScriptActions.SmallImageList = _uiImages;

            //set listview column size
            frmScriptBuilder_SizeChanged(null, null);

            var groupedCommands = _automationCommands.GroupBy(f => f.DisplayGroup);

            foreach (var cmd in groupedCommands)
            {
                TreeNode newGroup = new TreeNode(cmd.Key);

                foreach (var subcmd in cmd)
                {
                    TreeNode subNode = new TreeNode(subcmd.ShortName);

                    if (!subcmd.Command.CustomRendering)
                    {
                        subNode.ForeColor = Color.Red;
                    }
                    newGroup.Nodes.Add(subNode);
                }

                tvCommands.Nodes.Add(newGroup);
            }

            tvCommands.Sort();
            //tvCommands.ImageList = uiImages;

            _tvCommandsCopy = new TreeView();
            CopyTreeView(tvCommands, _tvCommandsCopy);
            txtCommandSearch.Text = _txtCommandWatermark;

            //start attended mode if selected
            if (_appSettings.ClientSettings.StartupMode == "Attended Task Mode")
            {
                WindowState = FormWindowState.Minimized;
                var frmAttended = new frmAttendedMode(ScriptProject.ProjectName);
                frmAttended.Show();
            }

        }

        private void frmScriptBuilder_FormClosing(object sender, FormClosingEventArgs e)
        {
            DialogResult result;

            if (_isSequence && uiScriptTabControl.TabPages[0].Text.Contains(" *") && DialogResult == DialogResult.Cancel)
            {
                result = MessageBox.Show($"Would you like to save the sequence before closing?",
                                         $"Save Sequence", MessageBoxButtons.YesNoCancel);

                if (result == DialogResult.Yes)
                    DialogResult = DialogResult.OK;
                else if (result == DialogResult.Cancel)
                    e.Cancel = true;

                return;
            }
            else if (_isSequence && DialogResult == DialogResult.OK)
                return;

            result = CheckForUnsavedScripts();
            if (result == DialogResult.Cancel)
                e.Cancel = true;
        }

        private void GenerateRecentFiles()
        {
            flwRecentFiles.Controls.Clear();

            var scriptPath = Folders.GetFolder(FolderType.ScriptsFolder);

            if (!Directory.Exists(scriptPath))
            {
                lblRecentFiles.Text = "Script Folder does not exist";
                lblFilesMissing.Text = "Directory Not Found: " + scriptPath;
                lblRecentFiles.ForeColor = Color.White;
                lblFilesMissing.ForeColor = Color.White;
                lblFilesMissing.Show();
                flwRecentFiles.Hide();
                return;
            }

            var directory = new DirectoryInfo(scriptPath);
            var recentFiles = directory.GetFiles()
                                       .OrderByDescending(file => file.LastWriteTime)
                                       .Select(f => f.Name);

            if (recentFiles.Count() == 0)
            {
                //Label noFilesLabel = new Label();
                //noFilesLabel.Text = "No Recent Files Found";
                //noFilesLabel.AutoSize = true;
                //noFilesLabel.ForeColor = Color.SteelBlue;
                //noFilesLabel.Font = lnkGitIssue.Font;
                //noFilesLabel.Margin = new Padding(0, 0, 0, 0);
                //flwRecentFiles.Controls.Add(noFilesLabel);
                lblRecentFiles.Text = "No Recent Files Found";
                lblRecentFiles.ForeColor = Color.White;
                lblFilesMissing.ForeColor = Color.White;
                lblFilesMissing.Show();
                flwRecentFiles.Hide();
            }
            else
            {
                foreach (var fil in recentFiles)
                {
                    if (flwRecentFiles.Controls.Count == 7)
                        return;

                    LinkLabel newFileLink = new LinkLabel
                    {
                        Text = fil,
                        AutoSize = true,
                        LinkColor = Color.AliceBlue,
                        Font = lnkGitIssue.Font,
                        Margin = new Padding(0, 0, 0, 0)
                    };
                    newFileLink.LinkClicked += NewFileLink_LinkClicked;
                    flwRecentFiles.Controls.Add(newFileLink);
                }
            }
        }

        private void frmScriptBuilder_Shown(object sender, EventArgs e)
        {
            Program.SplashForm.Hide();

            if (_editMode)
                return;

            AddProject();
            Notify("Welcome! Press 'Add Command' to get started!");
        }

        private void pnlControlContainer_Paint(object sender, PaintEventArgs e)
        {
            //Rectangle rect = new Rectangle(0, 0, pnlControlContainer.Width, pnlControlContainer.Height);
            //using (LinearGradientBrush brush = new LinearGradientBrush(rect, Color.White, Color.WhiteSmoke, LinearGradientMode.Vertical))
            //{
            //    e.Graphics.FillRectangle(brush, rect);
            //}

            //Pen steelBluePen = new Pen(Color.SteelBlue, 2);
            //Pen lightSteelBluePen = new Pen(Color.LightSteelBlue, 1);
            ////e.Graphics.DrawLine(steelBluePen, 0, 0, pnlControlContainer.Width, 0);
            //e.Graphics.DrawLine(lightSteelBluePen, 0, 0, pnlControlContainer.Width, 0);
            //e.Graphics.DrawLine(lightSteelBluePen, 0, pnlControlContainer.Height - 1, pnlControlContainer.Width, pnlControlContainer.Height - 1);
        }

        private void pbMainLogo_Click(object sender, EventArgs e)
        {
            frmAbout aboutForm = new frmAbout();
            aboutForm.Show();
        }

        private void frmScriptBuilder_SizeChanged(object sender, EventArgs e)
        {
            _selectedTabScriptActions.Columns[2].Width = Width - 340;
        }

        private void frmScriptBuilder_Resize(object sender, EventArgs e)
        {
            //check when minimized
            if ((WindowState == FormWindowState.Minimized) && (_appSettings.ClientSettings.MinimizeToTray))
            {
                _appSettings = new ApplicationSettings().GetOrCreateApplicationSettings();
                if (_appSettings.ClientSettings.MinimizeToTray)
                {
                    notifyTray.Visible = true;
                    notifyTray.ShowBalloonTip(3000);
                    ShowInTaskbar = false;
                }
            }
        }
        #endregion
        
        #region Bottom Notification Panel
        private void tmrNotify_Tick(object sender, EventArgs e)
        {
            if (_appSettings ==  null)
            {
                return;
            }

            if ((_notificationExpires < DateTime.Now) && (_isDisplaying))
            {
                HideNotification();
            }

            if ((_appSettings.ClientSettings.AntiIdleWhileOpen) && (DateTime.Now > _lastAntiIdleEvent.AddMinutes(1)))
            {
                PerformAntiIdle();
            }

            //check if notification is required
            if ((_notificationList.Count > 0) && (_notificationExpires < DateTime.Now))
            {
                var itemToDisplay = _notificationList[0];
                _notificationList.RemoveAt(0);
                _notificationExpires = DateTime.Now.AddSeconds(2);
                ShowNotification(itemToDisplay);
            }
        }

        public void Notify(string notificationText)
        {
            _notificationList.Add(notificationText);
        }

        private void ShowNotification(string textToDisplay)
        {
            _notificationText = textToDisplay;
            //lblStatus.Left = 20;
            //lblStatus.Text = textToDisplay;

            pnlStatus.SuspendLayout();
            //for (int i = 0; i < 30; i++)
            //{
            //    tlpControls.RowStyles[1].Height = i;
            //}
            ShowNotificationRow();
            pnlStatus.ResumeLayout();
            _isDisplaying = true;
        }

        private void HideNotification()
        {
            pnlStatus.SuspendLayout();
            //for (int i = 30; i > 0; i--)
            //{
            //    tlpControls.RowStyles[1].Height = i;
            //}
            HideNotificationRow();
            pnlStatus.ResumeLayout();
            _isDisplaying = false;
        }

        private void HideNotificationRow()
        {
            tlpControls.RowStyles[4].Height = 0;
        }

        private void ShowNotificationRow()
        {
            tlpControls.RowStyles[4].Height = 30;
        }

        private void PerformAntiIdle()
        {
            _lastAntiIdleEvent = DateTime.Now;
            var mouseMove = new SendMouseMoveCommand
            {
                v_XMousePosition = (Cursor.Position.X + 1).ToString(),
                v_YMousePosition = (Cursor.Position.Y + 1).ToString()
            };
            Notify("Anti-Idle Triggered");
        }

        private void notifyTray_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (_appSettings.ClientSettings.MinimizeToTray)
            {
                WindowState = FormWindowState.Normal;
                ShowInTaskbar = true;
                notifyTray.Visible = false;
            }
        }
        #endregion

        #region Create Command Logic
        private void AddNewCommand(string specificCommand = "")
        {
            //bring up new command configuration form
            frmCommandEditor newCommandForm = new frmCommandEditor(_automationCommands, GetConfiguredCommands())
            {
                CreationModeInstance = CreationMode.Add,
                ScriptVariables = _scriptVariables,
                ScriptElements = _scriptElements
            };
            if (specificCommand != "")
                newCommandForm.DefaultStartupCommand = specificCommand;

            newCommandForm.HTMLElementRecorderURL = HTMLElementRecorderURL;

            //if a command was selected
            if (newCommandForm.ShowDialog() == DialogResult.OK)
            {
                //add to listview
                CreateUndoSnapshot();
                AddCommandToListView(newCommandForm.SelectedCommand);              
            }

            if (newCommandForm.SelectedCommand.CommandName == "SeleniumElementActionCommand")
            {
                CreateUndoSnapshot();
                _scriptElements = newCommandForm.ScriptElements;
                HTMLElementRecorderURL = newCommandForm.HTMLElementRecorderURL;
            }
        }

        private List<ScriptCommand> GetConfiguredCommands()
        {
            List<ScriptCommand> ConfiguredCommands = new List<ScriptCommand>();
            foreach (ListViewItem item in _selectedTabScriptActions.Items)
            {
                ConfiguredCommands.Add(item.Tag as ScriptCommand);
            }
            return ConfiguredCommands;
        }
        #endregion

        #region TreeView Events
        private void tvCommands_DoubleClick(object sender, EventArgs e)
        {
            //handle double clicks outside
            if (tvCommands.SelectedNode == null)
                return;

            //exit if parent node is clicked
            if (tvCommands.SelectedNode.Parent == null)
                return;

            AddNewCommand(tvCommands.SelectedNode.Parent.Text + " - " + tvCommands.SelectedNode.Text);
        }

        private void tvCommands_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
            {
                e.Handled = true;
                tvCommands_DoubleClick(this, null);
            }
        }

        public void CopyTreeView(TreeView originalTreeView, TreeView copiedTreeView)
        {
            TreeNode copiedTreeNode;
            foreach (TreeNode originalTreeNode in originalTreeView.Nodes)
            {
                copiedTreeNode = new TreeNode(originalTreeNode.Text);
                CopyTreeViewNodes(copiedTreeNode, originalTreeNode);
                copiedTreeView.Nodes.Add(copiedTreeNode);
            }
        }

        public void CopyTreeViewNodes(TreeNode copiedTreeNode, TreeNode originalTreeNode)
        {
            TreeNode copiedChildNode;
            foreach (TreeNode originalChildNode in originalTreeNode.Nodes)
            {
                copiedChildNode = new TreeNode(originalChildNode.Text);
                copiedTreeNode.Nodes.Add(copiedChildNode);
            }
        }

        private void txtCommandSearch_TextChanged(object sender, EventArgs e)
        {
            if (txtCommandSearch.Text == _txtCommandWatermark)
                return;

            bool childNodefound = false;
            //blocks repainting tree until all controls are loaded
            tvCommands.BeginUpdate();
            tvCommands.Nodes.Clear();
            if (txtCommandSearch.Text != string.Empty)
            {
                foreach (TreeNode parentNodeCopy in _tvCommandsCopy.Nodes)
                {
                    TreeNode searchedParentNode = new TreeNode(parentNodeCopy.Text);
                    tvCommands.Nodes.Add(searchedParentNode);

                    foreach (TreeNode childNodeCopy in parentNodeCopy.Nodes)
                    {
                        if (childNodeCopy.Text.ToLower().Contains(txtCommandSearch.Text.ToLower()))
                        {
                            searchedParentNode.Nodes.Add(new TreeNode(childNodeCopy.Text));
                            childNodefound = true;
                        }
                    }
                    if (!childNodefound)
                    {
                        tvCommands.Nodes.Remove(searchedParentNode);
                    }
                    childNodefound = false;
                }
                tvCommands.ExpandAll();
            }
            else
            {
                foreach (TreeNode parentNodeCopy in _tvCommandsCopy.Nodes)
                {
                    tvCommands.Nodes.Add((TreeNode)parentNodeCopy.Clone());
                }
                tvCommands.CollapseAll();
            }
            //enables redrawing tree after all controls have been added
            tvCommands.EndUpdate();           
        }

        private void txtCommandSearch_Enter(object sender, EventArgs e)
        {
            if (txtCommandSearch.Text == _txtCommandWatermark)
            {
                txtCommandSearch.Text = "";
                txtCommandSearch.ForeColor = Color.Black;
            }           
        }

        private void txtCommandSearch_Leave(object sender, EventArgs e)
        {
            if (txtCommandSearch.Text == "")
            {
                txtCommandSearch.Text = _txtCommandWatermark;
                txtCommandSearch.ForeColor = Color.LightGray;
            }
        }
        #endregion

        #region Link Labels
        private void lnkGitProject_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/OpenBotsAI/OpenBots.Studio");
        }
        private void lnkGitLatestReleases_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/OpenBotsAI/OpenBots.Studio/releases");
        }
        private void lnkGitIssue_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://github.com/OpenBotsAI/OpenBots.Studio/issues/new");
        }
        private void lnkGitWiki_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://openbots.ai/api/execute-dll/");
        }
        private void NewFileLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel senderLink = (LinkLabel)sender;
            OpenFile(Path.Combine(Folders.GetFolder(FolderType.ScriptsFolder), senderLink.Text));
        }
        #endregion
    }
}

