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
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using taskt.Core.Documentation;
using taskt.Core.Enums;
using taskt.Core.IO;
using taskt.Core.Metrics;
using taskt.Core.Settings;
using taskt.Server;
using taskt.UI.Forms.ScriptBuilder_Forms;
using taskt.UI.Forms.Supplement_Forms;
using taskt.Utilities;

namespace taskt.UI.Forms
{
    public partial class frmSettings : ThemedForm
    {
        ApplicationSettings newAppSettings;
        public frmScriptBuilder scriptBuilderForm;
        public frmSettings(frmScriptBuilder sender)
        {
            scriptBuilderForm = sender;
            InitializeComponent();
        }

        private void frmSettings_Load(object sender, EventArgs e)
        {
            newAppSettings = new ApplicationSettings();
            newAppSettings = newAppSettings.GetOrCreateApplicationSettings();

            var serverSettings = newAppSettings.ServerSettings;
            chkServerEnabled.DataBindings.Add("Checked", serverSettings, "ServerConnectionEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAutomaticallyConnect.DataBindings.Add("Checked", serverSettings, "ConnectToServerOnStartup", false, DataSourceUpdateMode.OnPropertyChanged);
            chkRetryOnDisconnect.DataBindings.Add("Checked", serverSettings, "RetryServerConnectionOnFail", false, DataSourceUpdateMode.OnPropertyChanged);
            chkBypassValidation.DataBindings.Add("Checked", serverSettings, "BypassCertificateValidation", false, DataSourceUpdateMode.OnPropertyChanged);
            txtPublicKey.DataBindings.Add("Text", serverSettings, "ServerPublicKey", false, DataSourceUpdateMode.OnPropertyChanged);
            txtServerURL.DataBindings.Add("Text", serverSettings, "ServerURL", false, DataSourceUpdateMode.OnPropertyChanged);
            txtHttpsAddress.DataBindings.Add("Text", serverSettings, "HTTPServerURL", false, DataSourceUpdateMode.OnPropertyChanged);
            txtGUID.DataBindings.Add("Text", serverSettings, "HTTPGuid", false, DataSourceUpdateMode.OnPropertyChanged);

            var engineSettings = newAppSettings.EngineSettings;
            chkShowDebug.DataBindings.Add("Checked", engineSettings, "ShowDebugWindow", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAutoCloseWindow.DataBindings.Add("Checked", engineSettings, "AutoCloseDebugWindow", false, DataSourceUpdateMode.OnPropertyChanged);
            chkEnableLogging.DataBindings.Add("Checked", engineSettings, "EnableDiagnosticLogging", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAdvancedDebug.DataBindings.Add("Checked", engineSettings, "ShowAdvancedDebugOutput", false, DataSourceUpdateMode.OnPropertyChanged);
            chkTrackMetrics.DataBindings.Add("Checked", engineSettings, "TrackExecutionMetrics", false, DataSourceUpdateMode.OnPropertyChanged);
            txtCommandDelay.DataBindings.Add("Text", engineSettings, "DelayBetweenCommands", false, DataSourceUpdateMode.OnPropertyChanged);
            chkOverrideInstances.DataBindings.Add("Checked", engineSettings, "OverrideExistingAppInstances", false, DataSourceUpdateMode.OnPropertyChanged);
            chkAutoCalcVariables.DataBindings.Add("Checked", engineSettings, "AutoCalcVariables", false, DataSourceUpdateMode.OnPropertyChanged);

            cbxCancellationKey.DataSource = Enum.GetValues(typeof(Keys));
            cbxCancellationKey.DataBindings.Add("Text", engineSettings, "CancellationKey", false, DataSourceUpdateMode.OnPropertyChanged);

            SinkType loggingSinkType = engineSettings.LoggingSinkType;
            cbxSinkType.DataSource = Enum.GetValues(typeof(SinkType));           
            cbxSinkType.SelectedIndex = cbxSinkType.Items.IndexOf(loggingSinkType);

            LogEventLevel minLogLevel = engineSettings.MinLogLevel;
            cbxMinLogLevel.DataSource = Enum.GetValues(typeof(LogEventLevel));
            cbxMinLogLevel.SelectedIndex = cbxMinLogLevel.Items.IndexOf(minLogLevel);

            txtLogging1.DataBindings.Add("Text", engineSettings, "LoggingValue1", false, DataSourceUpdateMode.OnPropertyChanged);
            txtLogging2.DataBindings.Add("Text", engineSettings, "LoggingValue2", false, DataSourceUpdateMode.OnPropertyChanged);
            txtLogging3.DataBindings.Add("Text", engineSettings, "LoggingValue3", false, DataSourceUpdateMode.OnPropertyChanged);
            txtLogging4.DataBindings.Add("Text", engineSettings, "LoggingValue4", false, DataSourceUpdateMode.OnPropertyChanged);

            var listenerSettings = newAppSettings.ListenerSettings;
            chkAutoStartListener.DataBindings.Add("Checked", listenerSettings, "StartListenerOnStartup", false, DataSourceUpdateMode.OnPropertyChanged);
            chkEnableListening.DataBindings.Add("Checked", listenerSettings, "LocalListeningEnabled", false, DataSourceUpdateMode.OnPropertyChanged);
            chkRequireListenerKey.DataBindings.Add("Checked", listenerSettings, "RequireListenerAuthenticationKey", false, DataSourceUpdateMode.OnPropertyChanged);
            txtListeningPort.DataBindings.Add("Text", listenerSettings, "ListeningPort", false, DataSourceUpdateMode.OnPropertyChanged);
            txtAuthListeningKey.DataBindings.Add("Text", listenerSettings, "AuthKey", false, DataSourceUpdateMode.OnPropertyChanged);
            chkEnableWhitelist.DataBindings.Add("Checked", listenerSettings, "EnableWhitelist", false, DataSourceUpdateMode.OnPropertyChanged);
            txtWhiteList.DataBindings.Add("Text", listenerSettings, "IPWhiteList", false, DataSourceUpdateMode.OnPropertyChanged);

            SetupListeningUI();

            var clientSettings = newAppSettings.ClientSettings;
            chkAntiIdle.DataBindings.Add("Checked", clientSettings, "AntiIdleWhileOpen", false, DataSourceUpdateMode.OnPropertyChanged);
            txtAppFolderPath.DataBindings.Add("Text", clientSettings, "RootFolder", false, DataSourceUpdateMode.OnPropertyChanged);
            txtAttendedTaskFolder.DataBindings.Add("Text", clientSettings, "AttendedTasksFolder", false, DataSourceUpdateMode.OnPropertyChanged);
            chkInsertCommandsInline.DataBindings.Add("Checked", clientSettings, "InsertCommandsInline", false, DataSourceUpdateMode.OnPropertyChanged);
            chkSequenceDragDrop.DataBindings.Add("Checked", clientSettings, "EnableSequenceDragDrop", false, DataSourceUpdateMode.OnPropertyChanged);
            chkMinimizeToTray.DataBindings.Add("Checked", clientSettings, "MinimizeToTray", false, DataSourceUpdateMode.OnPropertyChanged);
            cboStartUpMode.DataBindings.Add("Text", clientSettings, "StartupMode", false, DataSourceUpdateMode.OnPropertyChanged);
            chkPreloadCommands.DataBindings.Add("Checked", clientSettings, "PreloadBuilderCommands", false, DataSourceUpdateMode.OnPropertyChanged);
            chkSlimActionBar.DataBindings.Add("Checked", clientSettings, "UseSlimActionBar", false, DataSourceUpdateMode.OnPropertyChanged);

            //get metrics
            bgwMetrics.RunWorkerAsync();

            LocalTCPClient.ListeningStarted += AutomationTCPListener_ListeningStarted;
            LocalTCPClient.ListeningStopped += AutomationTCPListener_ListeningStopped;
        }

        public delegate void AutomationTCPListener_StartedDelegate(object sender, EventArgs e);
        public delegate void AutomationTCPListener_StoppedDelegate(object sender, EventArgs e);
        private void AutomationTCPListener_ListeningStopped(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                var stoppedDelegate = new AutomationTCPListener_StoppedDelegate(AutomationTCPListener_ListeningStopped);
                Invoke(stoppedDelegate, new object[] { sender, e });
            }
            else
            {
                SetupListeningUI();
            }
        }

        private void AutomationTCPListener_ListeningStarted(object sender, EventArgs e)
        {
            if (InvokeRequired)
            {
                var startedDelegate = new AutomationTCPListener_StoppedDelegate(AutomationTCPListener_ListeningStarted);
                Invoke(startedDelegate, new object[] { sender, e });
            }
            else
            {
                SetupListeningUI();
            }

        }

        private void btnConnect_Click(object sender, EventArgs e)
        {
            SocketClient.InitializeScriptEngine(new frmScriptEngine());
            SocketClient.Connect(txtServerURL.Text);
        }


        private void btnAddAssembly_Click(object sender, EventArgs e)
        {
            //allow importing custom command assemblies
            OpenFileDialog newOpenDialog = new OpenFileDialog();

            if (newOpenDialog.ShowDialog() == DialogResult.OK)
            {
            }
        }

        private void uiBtnOpen_Click(object sender, EventArgs e)
        {
            Keys key = (Keys)Enum.Parse(typeof(Keys), cbxCancellationKey.Text);
            newAppSettings.EngineSettings.CancellationKey = key;

            if ((SinkType)cbxSinkType.SelectedItem == SinkType.File && string.IsNullOrEmpty(txtLogging1.Text.Trim()))
                newAppSettings.EngineSettings.LoggingValue1 = Path.Combine(Folders.GetFolder(FolderType.LogFolder), "OpenBots Engine Logs.txt");

            newAppSettings.Save(newAppSettings);
            
            SocketClient.InitializeScriptEngine(new frmScriptEngine());
            SocketClient.LoadSettings();
            Close();
        }

        private void btnUpdateCheck_Click(object sender, EventArgs e)
        {
            ManifestUpdate manifest = new ManifestUpdate();
            try
            {
                manifest = ManifestUpdate.GetManifest();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error getting manifest: " + ex.ToString());
                return;
            }

            if (manifest.RemoteVersionNewer)
            {
                frmUpdate frmUpdate = new frmUpdate(manifest);
                if (frmUpdate.ShowDialog() == DialogResult.OK)
                {
                    //move update exe to root folder for execution
                    var updaterExecutionResources = Application.StartupPath + "\\resources\\taskt.Updater.exe";
                    var updaterExecutableDestination = Application.StartupPath + "\\taskt.Updater.exe";

                    if (!File.Exists(updaterExecutionResources))
                    {
                        MessageBox.Show("taskt.Updater.exe not found in resources directory!");
                        return;
                    }
                    else
                    {
                        File.Copy(updaterExecutionResources, updaterExecutableDestination);
                    }

                    var updateProcess = new Process();
                    updateProcess.StartInfo.FileName = updaterExecutableDestination;
                    updateProcess.StartInfo.Arguments = manifest.PackageURL;
                    updateProcess.Start();
                    Application.Exit();
                }
            }
            else
            {
                MessageBox.Show("The application is currently up-to-date!", "No Updates Available", MessageBoxButtons.OK);
            }
        }

        private void tmrGetSocketStatus_Tick(object sender, EventArgs e)
        {

            lblStatus.Text = "Socket Status: " + SocketClient.GetSocketState();
            if (SocketClient.ConnectionException != string.Empty)
            {
                lblSocketException.Show();
                lblSocketException.Text = SocketClient.ConnectionException;
            }
            else
            {
                lblSocketException.Hide();
            }
        }

        private void btnCloseConnection_Click(object sender, EventArgs e)
        {
            SocketClient.Disconnect();
        }

        private void chkBypassValidation_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBypassValidation.Checked)
            {
                MessageBox.Show("Bypassing SSL Certificate Validation procedures is inherently insecure" +
                    " as the client will trust any server certificate. Please consider issuing proper SSL Certificates.",
                    "Warning - Insecure", MessageBoxButtons.OK);
            }
        }

        private void btnSelectFolder_Click(object sender, EventArgs e)
        {
            //prompt user to confirm they want to select a new folder
            var updateFolderRequest = MessageBox.Show("Would you like to change the default root folder that OpenBots uses" +
                " to store tasks and information? " + Environment.NewLine + Environment.NewLine + "Current Root Folder: " +
                txtAppFolderPath.Text, "Change Default Root Folder", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            //if user does not want to update folder then exit
            if (updateFolderRequest == DialogResult.No)
                return;

            //user folder browser to let user select top level folder
            using (var fbd = new FolderBrowserDialog())
            {

                //check if user selected a folder
                if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    //create references to old and new root folders
                    var oldRootFolder = txtAppFolderPath.Text;
                    var newRootFolder = Path.Combine(fbd.SelectedPath, "OpenBotsStudio");

                    //ask user to confirm
                    var confirmNewFolderSelection = MessageBox.Show("Please confirm the changes below:" +
                        Environment.NewLine + Environment.NewLine + "Old Root Folder: " + oldRootFolder +
                        Environment.NewLine + Environment.NewLine + "New Root Folder: " + newRootFolder,
                        "Change Default Root Folder", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);

                    //handle if user decides to cancel
                    if (confirmNewFolderSelection == DialogResult.Cancel)
                        return;

                    //ask if we should migrate the data
                    var migrateCopyData = MessageBox.Show("Would you like to attempt to move the data from" +
                        " the old folder to the new folder?  Please note, depending on how many files you have," +
                        " this could take a few minutes.", "Migrate Data?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                    //check if user wants to migrate data
                    if (migrateCopyData == DialogResult.Yes)
                    {
                        try
                        {
                            //find and copy files
                            foreach (string dirPath in Directory.GetDirectories(oldRootFolder, "*", SearchOption.AllDirectories))
                            {
                                Directory.CreateDirectory(dirPath.Replace(oldRootFolder, newRootFolder));
                            }
                            foreach (string newPath in Directory.GetFiles(oldRootFolder, "*.*", SearchOption.AllDirectories))
                            {
                                File.Copy(newPath, newPath.Replace(oldRootFolder, newRootFolder), true);
                            }

                            MessageBox.Show("Data Migration Complete", "Data Migration Complete", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);                            
                        }
                        catch (Exception ex)
                        {
                            //handle any unexpected errors
                            MessageBox.Show("An Error Occured during Data Migration Copy: " + ex.ToString());
                        }
                    }
                    //update textbox which will be updated once user selects "Ok"
                    txtAppFolderPath.Text = newRootFolder;
                    newAppSettings.Save(newAppSettings);
                }
            }
        }

        private void bgwMetrics_DoWork(object sender, DoWorkEventArgs e)
        {
            e.Result = new Metric().ExecutionMetricsSummary();
        }
        private void bgwMetrics_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Error != null)
            {
                if (e.Error is FileNotFoundException)
                {
                    lblGettingMetrics.Text = "Metrics Unavailable - Metrics are only available after running" +
                        " tasks which will generate metrics logs";
                }
                else
                {
                    lblGettingMetrics.Text = "Metrics Unavailable: " + e.Error.ToString();
                }
            }
            else
            {
                var metricsSummary = (List<ExecutionMetric>)(e.Result);

                if (metricsSummary.Count == 0)
                {
                    lblGettingMetrics.Text = "No Metrics Found";
                    lblGettingMetrics.Show();
                    tvExecutionTimes.Hide();
                    btnClearMetrics.Hide();
                }
                else
                {
                    lblGettingMetrics.Hide();
                    tvExecutionTimes.Show();
                    btnClearMetrics.Show();
                }

                foreach (var metric in metricsSummary)
                {
                    var rootNode = new TreeNode();
                    rootNode.Text = metric.FileName + " [" + metric.AverageExecutionTime + " avg.]";

                    foreach (var metricItem in metric.ExecutionData)
                    {
                        var subNode = new TreeNode();
                        subNode.Text = string.Join(" - ", metricItem.LoggedOn.ToString("MM/dd/yy hh:mm"), metricItem.ExecutionTime);
                        rootNode.Nodes.Add(subNode);
                    }
                    tvExecutionTimes.Nodes.Add(rootNode);
                }
            }
        }

        private void btnClearMetrics_Click(object sender, EventArgs e)
        {
            new Metric().ClearExecutionMetrics();
            bgwMetrics.RunWorkerAsync();
        }
        private void btnGenerateWikiDocs_Click(object sender, EventArgs e)
        {
            DocumentationGeneration docGeneration = new DocumentationGeneration();
            var docsRoot = docGeneration.GenerateMarkdownFiles();
            Process.Start(docsRoot);
        }

        private void btnLaunchAttendedMode_Click(object sender, EventArgs e)
        {
            var frmAttended = new frmAttendedMode(scriptBuilderForm.ScriptProject.ProjectName);
            frmAttended.Show();
            Close();
        }

        private void btnSelectAttendedTaskFolder_Click(object sender, EventArgs e)
        {
            using (var fbd = new FolderBrowserDialog())
            {
                if (fbd.ShowDialog() == DialogResult.OK && !string.IsNullOrWhiteSpace(fbd.SelectedPath))
                {
                    var newAttendedTaskFolder = Path.Combine(fbd.SelectedPath);
                    txtAttendedTaskFolder.Text = newAttendedTaskFolder;
                }
            }
        }

        private void btnLaunchDisplayManager_Click(object sender, EventArgs e)
        {
            frmDisplayManager displayManager = new frmDisplayManager();
            displayManager.Show();
            Close();
        }

        private void btnGetGUID_Click(object sender, EventArgs e)
        {
            var successfulConnection = HttpServerClient.TestConnection(txtHttpsAddress.Text);

            if (successfulConnection)
            {
                var pulledNewGUID = HttpServerClient.GetGuid();

                if (pulledNewGUID)
                {
                    newAppSettings = new ApplicationSettings();
                    newAppSettings = newAppSettings.GetOrCreateApplicationSettings();
                    txtHttpsAddress.Text = newAppSettings.ServerSettings.HTTPGuid.ToString();
                    MessageBox.Show("Connected Successfully! GUID will be reloaded automatically the next time settings is loaded!");
                }
                MessageBox.Show("Connected Successfully!");
            }
            else
            {
                MessageBox.Show("Unable To Connect!");
            }
        }

        private void btnGetBotGUID_Click(object sender, EventArgs e)
        {
            var newGUID = HttpServerClient.GetGuid();
        }

        private void btnTaskPublish_Click(object sender, EventArgs e)
        {
            if (File.Exists(scriptBuilderForm.ScriptFilePath))
            {
                HttpServerClient.PublishScript(scriptBuilderForm.ScriptFilePath, PublishType.ServerReference);
            }
            else
            {
                MessageBox.Show("Please open the task in order to publish it.");
            }
        }

        private void SetupListeningUI()
        {
            if (LocalTCPClient.IsListening)
            {
                lblListeningStatus.Text = $"Client is Listening at Endpoint '{LocalTCPClient.GetListeningAddress()}'.";
                btnStopListening.Enabled = true;
                btnStartListening.Enabled = false;
            }
            else
            {
                lblListeningStatus.Text = $"Client is Not Listening!";
                btnStopListening.Enabled = false;
                btnStartListening.Enabled = true;
            }
            lblListeningStatus.Show();
        }

        private void DisableListenerButtons()
        {
            btnStopListening.Enabled = false;
            btnStartListening.Enabled = false;
        }

        private void btnStartListening_Click(object sender, EventArgs e)
        {
            if (int.TryParse(txtListeningPort.Text, out var portNumber))
            {
                DisableListenerButtons();
                LocalTCPClient.InitializeScriptEngine(new frmScriptEngine());
                LocalTCPClient.StartListening(portNumber);
            }
        }

        private void btnStopListening_Click(object sender, EventArgs e)
        {
            DisableListenerButtons();
            LocalTCPClient.StopAutomationListener();
        }

        private void btnWhiteList_Click(object sender, EventArgs e)
        {
            //newAppSettings.ListenerSettings.IPWhiteList.Add(new WhiteListIPSettings("127.0.0.1"));
            //Supplement_Forms.frmGridView frmWhitelist = new Supplement_Forms.frmGridView(newAppSettings.ListenerSettings.IPWhiteList);
            //frmWhitelist.ShowDialog();
        }

        private void cbxSinkType_SelectedIndexChanged(object sender, EventArgs e)
        {
            newAppSettings.EngineSettings.LoggingSinkType = (SinkType)cbxSinkType.SelectedItem;
            switch (newAppSettings.EngineSettings.LoggingSinkType)
            {
                case SinkType.File:
                    LoadFileLoggingSettings();
                    break;
                case SinkType.HTTP:
                    LoadHTTPLoggingSettings();
                    break;
                case SinkType.SignalR:
                    LoadSignalRLoggingSettings();
                    break;
            }
        }

        private void LoadFileLoggingSettings()
        {
            lblLogging1.Text = "File Path: ";
            txtLogging1.Clear();
            txtLogging1.Text = Path.Combine(Folders.GetFolder(FolderType.LogFolder), "OpenBots Engine Logs.txt");
            btnFileManager.Visible = true;
           
            lblLogging2.Visible = false;
            txtLogging2.Visible = false;
            txtLogging2.Clear();

            lblLogging3.Visible = false;
            txtLogging3.Visible = false;
            txtLogging3.Clear();

            lblLogging4.Visible = false;
            txtLogging4.Visible = false;
            txtLogging4.Clear();
        }

        private void LoadHTTPLoggingSettings()
        {
            lblLogging1.Text = "URI: ";
            txtLogging1.Clear();
            btnFileManager.Visible = false;

            lblLogging2.Visible = false;
            txtLogging2.Visible = false;
            txtLogging2.Clear();

            lblLogging3.Visible = false;
            txtLogging3.Visible = false;
            txtLogging3.Clear();

            lblLogging4.Visible = false;
            txtLogging4.Visible = false;
            txtLogging4.Clear();
        }

        private void LoadSignalRLoggingSettings()
        {
            lblLogging1.Text = "URL: ";
            txtLogging1.Clear();
            btnFileManager.Visible = false;

            lblLogging2.Visible = true; //Hub
            txtLogging2.Visible = true;
            txtLogging2.Clear();

            lblLogging3.Visible = true; //Group Names
            txtLogging3.Visible = true;
            txtLogging3.Clear();

            lblLogging4.Visible = true; //User IDs
            txtLogging4.Visible = true;
            txtLogging4.Clear();
        }

        private void cbxMinLogLevel_SelectedIndexChanged(object sender, EventArgs e)
        {
            newAppSettings.EngineSettings.MinLogLevel = (LogEventLevel)cbxMinLogLevel.SelectedItem;
        }

        private void btnFileManager_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                txtLogging1.Text = ofd.FileName;
            }
        }
    }
}