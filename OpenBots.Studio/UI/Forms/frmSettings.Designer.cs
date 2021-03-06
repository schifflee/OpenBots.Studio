﻿namespace OpenBots.UI.Forms
{
    partial class frmSettings
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSettings));
            this.btnConnect = new System.Windows.Forms.Button();
            this.chkRetryOnDisconnect = new System.Windows.Forms.CheckBox();
            this.lblManageSettings = new System.Windows.Forms.Label();
            this.chkAutomaticallyConnect = new System.Windows.Forms.CheckBox();
            this.chkServerEnabled = new System.Windows.Forms.CheckBox();
            this.txtServerURL = new System.Windows.Forms.TextBox();
            this.lblServerURL = new System.Windows.Forms.Label();
            this.txtPublicKey = new System.Windows.Forms.TextBox();
            this.lblConnectKey = new System.Windows.Forms.Label();
            this.chkEnableLogging = new System.Windows.Forms.CheckBox();
            this.chkAutoCloseWindow = new System.Windows.Forms.CheckBox();
            this.chkShowDebug = new System.Windows.Forms.CheckBox();
            this.uiBtnOpen = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.lblAutomationEngine = new System.Windows.Forms.Label();
            this.lblApplicationSettings = new System.Windows.Forms.Label();
            this.chkAntiIdle = new System.Windows.Forms.CheckBox();
            this.btnUpdates = new System.Windows.Forms.Button();
            this.chkAdvancedDebug = new System.Windows.Forms.CheckBox();
            this.uiSettingTabs = new OpenBots.UI.CustomControls.CustomUIControls.UITabControl();
            this.tabAppSettings = new System.Windows.Forms.TabPage();
            this.chkSlimActionBar = new System.Windows.Forms.CheckBox();
            this.chkPreloadCommands = new System.Windows.Forms.CheckBox();
            this.btnLaunchDisplayManager = new System.Windows.Forms.Button();
            this.lblStartupMode = new System.Windows.Forms.Label();
            this.cboStartUpMode = new System.Windows.Forms.ComboBox();
            this.btnSelectAttendedTaskFolder = new System.Windows.Forms.Button();
            this.lblAttendedTasksFolder = new System.Windows.Forms.Label();
            this.txtAttendedTaskFolder = new System.Windows.Forms.TextBox();
            this.btnLaunchAttendedMode = new System.Windows.Forms.Button();
            this.chkMinimizeToTray = new System.Windows.Forms.CheckBox();
            this.chkSequenceDragDrop = new System.Windows.Forms.CheckBox();
            this.btnGenerateWikiDocs = new System.Windows.Forms.Button();
            this.chkInsertCommandsInline = new System.Windows.Forms.CheckBox();
            this.btnClearMetrics = new System.Windows.Forms.Button();
            this.lblScriptExecutionMetrics = new System.Windows.Forms.Label();
            this.lblGettingMetrics = new System.Windows.Forms.Label();
            this.tvExecutionTimes = new System.Windows.Forms.TreeView();
            this.btnSelectFolder = new System.Windows.Forms.Button();
            this.lblRootFolder = new System.Windows.Forms.Label();
            this.txtAppFolderPath = new System.Windows.Forms.TextBox();
            this.tabDebugSettings = new System.Windows.Forms.TabPage();
            this.btnFileManager = new System.Windows.Forms.Button();
            this.cbxMinLogLevel = new System.Windows.Forms.ComboBox();
            this.lblMinLogLevel = new System.Windows.Forms.Label();
            this.txtLogging4 = new System.Windows.Forms.TextBox();
            this.lblLogging4 = new System.Windows.Forms.Label();
            this.lblLogging3 = new System.Windows.Forms.Label();
            this.txtLogging3 = new System.Windows.Forms.TextBox();
            this.txtLogging2 = new System.Windows.Forms.TextBox();
            this.lblLogging2 = new System.Windows.Forms.Label();
            this.txtLogging1 = new System.Windows.Forms.TextBox();
            this.lblLogging1 = new System.Windows.Forms.Label();
            this.cbxSinkType = new System.Windows.Forms.ComboBox();
            this.lblSinkType = new System.Windows.Forms.Label();
            this.lblLoggingSettings = new System.Windows.Forms.Label();
            this.chkAutoCalcVariables = new System.Windows.Forms.CheckBox();
            this.lblEndScriptHotKey = new System.Windows.Forms.Label();
            this.cbxCancellationKey = new System.Windows.Forms.ComboBox();
            this.chkOverrideInstances = new System.Windows.Forms.CheckBox();
            this.lblDelay = new System.Windows.Forms.Label();
            this.txtCommandDelay = new System.Windows.Forms.TextBox();
            this.chkTrackMetrics = new System.Windows.Forms.CheckBox();
            this.tabServerSettings = new System.Windows.Forms.TabPage();
            this.btnTaskPublish = new System.Windows.Forms.Button();
            this.lblClientGUIDDesc = new System.Windows.Forms.Label();
            this.lblServerSettingsDesc = new System.Windows.Forms.Label();
            this.lblHTTPSDesc = new System.Windows.Forms.Label();
            this.btnGetGUID = new System.Windows.Forms.Button();
            this.lblClientGUID = new System.Windows.Forms.Label();
            this.txtGUID = new System.Windows.Forms.TextBox();
            this.lblHTTPSServerURL = new System.Windows.Forms.Label();
            this.txtHttpsAddress = new System.Windows.Forms.TextBox();
            this.chkBypassValidation = new System.Windows.Forms.CheckBox();
            this.btnCloseConnection = new System.Windows.Forms.Button();
            this.lblSocketException = new System.Windows.Forms.Label();
            this.lblServerSettings = new System.Windows.Forms.Label();
            this.lblStatus = new System.Windows.Forms.Label();
            this.tabLocalListener = new System.Windows.Forms.TabPage();
            this.txtWhiteList = new System.Windows.Forms.TextBox();
            this.chkEnableWhitelist = new System.Windows.Forms.CheckBox();
            this.chkAutoStartListener = new System.Windows.Forms.CheckBox();
            this.lblListeningPort = new System.Windows.Forms.Label();
            this.txtListeningPort = new System.Windows.Forms.TextBox();
            this.lblListeningStatus = new System.Windows.Forms.Label();
            this.btnStopListening = new System.Windows.Forms.Button();
            this.btnStartListening = new System.Windows.Forms.Button();
            this.chkRequireListenerKey = new System.Windows.Forms.CheckBox();
            this.lblAuthKey = new System.Windows.Forms.Label();
            this.txtAuthListeningKey = new System.Windows.Forms.TextBox();
            this.chkEnableListening = new System.Windows.Forms.CheckBox();
            this.lblLocalListenerSettingsDesc = new System.Windows.Forms.Label();
            this.lblLocalListenerSettings = new System.Windows.Forms.Label();
            this.tlpSettings = new System.Windows.Forms.TableLayoutPanel();
            this.pnlSettings = new System.Windows.Forms.Panel();
            this.tmrGetSocketStatus = new System.Windows.Forms.Timer(this.components);
            this.bgwMetrics = new System.ComponentModel.BackgroundWorker();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).BeginInit();
            this.uiSettingTabs.SuspendLayout();
            this.tabAppSettings.SuspendLayout();
            this.tabDebugSettings.SuspendLayout();
            this.tabServerSettings.SuspendLayout();
            this.tabLocalListener.SuspendLayout();
            this.tlpSettings.SuspendLayout();
            this.pnlSettings.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnConnect
            // 
            this.btnConnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnConnect.Location = new System.Drawing.Point(16, 529);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(75, 33);
            this.btnConnect.TabIndex = 17;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = true;
            this.btnConnect.Visible = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // chkRetryOnDisconnect
            // 
            this.chkRetryOnDisconnect.AutoSize = true;
            this.chkRetryOnDisconnect.BackColor = System.Drawing.Color.Transparent;
            this.chkRetryOnDisconnect.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRetryOnDisconnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkRetryOnDisconnect.Location = new System.Drawing.Point(15, 498);
            this.chkRetryOnDisconnect.Name = "chkRetryOnDisconnect";
            this.chkRetryOnDisconnect.Size = new System.Drawing.Size(154, 19);
            this.chkRetryOnDisconnect.TabIndex = 15;
            this.chkRetryOnDisconnect.Text = "Retry If Connection Fails";
            this.chkRetryOnDisconnect.UseVisualStyleBackColor = false;
            this.chkRetryOnDisconnect.Visible = false;
            // 
            // lblManageSettings
            // 
            this.lblManageSettings.BackColor = System.Drawing.Color.Transparent;
            this.lblManageSettings.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblManageSettings.ForeColor = System.Drawing.Color.White;
            this.lblManageSettings.Location = new System.Drawing.Point(6, 43);
            this.lblManageSettings.Name = "lblManageSettings";
            this.lblManageSettings.Size = new System.Drawing.Size(452, 28);
            this.lblManageSettings.TabIndex = 14;
            this.lblManageSettings.Text = "Manage settings used by the application";
            // 
            // chkAutomaticallyConnect
            // 
            this.chkAutomaticallyConnect.AutoSize = true;
            this.chkAutomaticallyConnect.BackColor = System.Drawing.Color.Transparent;
            this.chkAutomaticallyConnect.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutomaticallyConnect.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutomaticallyConnect.Location = new System.Drawing.Point(12, 127);
            this.chkAutomaticallyConnect.Name = "chkAutomaticallyConnect";
            this.chkAutomaticallyConnect.Size = new System.Drawing.Size(158, 24);
            this.chkAutomaticallyConnect.TabIndex = 13;
            this.chkAutomaticallyConnect.Text = "Check In On Startup";
            this.chkAutomaticallyConnect.UseVisualStyleBackColor = false;
            // 
            // chkServerEnabled
            // 
            this.chkServerEnabled.AutoSize = true;
            this.chkServerEnabled.BackColor = System.Drawing.Color.Transparent;
            this.chkServerEnabled.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkServerEnabled.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkServerEnabled.Location = new System.Drawing.Point(12, 102);
            this.chkServerEnabled.Name = "chkServerEnabled";
            this.chkServerEnabled.Size = new System.Drawing.Size(205, 24);
            this.chkServerEnabled.TabIndex = 12;
            this.chkServerEnabled.Text = "Server Connection Enabled";
            this.chkServerEnabled.UseVisualStyleBackColor = false;
            // 
            // txtServerURL
            // 
            this.txtServerURL.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtServerURL.Location = new System.Drawing.Point(14, 407);
            this.txtServerURL.Name = "txtServerURL";
            this.txtServerURL.Size = new System.Drawing.Size(371, 29);
            this.txtServerURL.TabIndex = 11;
            this.txtServerURL.Visible = false;
            // 
            // lblServerURL
            // 
            this.lblServerURL.AutoSize = true;
            this.lblServerURL.BackColor = System.Drawing.Color.Transparent;
            this.lblServerURL.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerURL.ForeColor = System.Drawing.Color.SlateGray;
            this.lblServerURL.Location = new System.Drawing.Point(14, 381);
            this.lblServerURL.Name = "lblServerURL";
            this.lblServerURL.Size = new System.Drawing.Size(232, 16);
            this.lblServerURL.TabIndex = 9;
            this.lblServerURL.Text = "Server URL ex. ws://localhost:port/ws)\r";
            this.lblServerURL.Visible = false;
            // 
            // txtPublicKey
            // 
            this.txtPublicKey.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtPublicKey.Location = new System.Drawing.Point(16, 462);
            this.txtPublicKey.Name = "txtPublicKey";
            this.txtPublicKey.Size = new System.Drawing.Size(371, 29);
            this.txtPublicKey.TabIndex = 19;
            this.txtPublicKey.Visible = false;
            // 
            // lblConnectKey
            // 
            this.lblConnectKey.AutoSize = true;
            this.lblConnectKey.BackColor = System.Drawing.Color.Transparent;
            this.lblConnectKey.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.749999F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConnectKey.ForeColor = System.Drawing.Color.SlateGray;
            this.lblConnectKey.Location = new System.Drawing.Point(14, 439);
            this.lblConnectKey.Name = "lblConnectKey";
            this.lblConnectKey.Size = new System.Drawing.Size(86, 16);
            this.lblConnectKey.TabIndex = 18;
            this.lblConnectKey.Text = "Connect Key:";
            this.lblConnectKey.Visible = false;
            // 
            // chkEnableLogging
            // 
            this.chkEnableLogging.AutoSize = true;
            this.chkEnableLogging.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableLogging.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableLogging.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkEnableLogging.Location = new System.Drawing.Point(10, 80);
            this.chkEnableLogging.Name = "chkEnableLogging";
            this.chkEnableLogging.Size = new System.Drawing.Size(212, 25);
            this.chkEnableLogging.TabIndex = 14;
            this.chkEnableLogging.Text = "Enable Diagnostic Logging";
            this.chkEnableLogging.UseVisualStyleBackColor = false;
            // 
            // chkAutoCloseWindow
            // 
            this.chkAutoCloseWindow.AutoSize = true;
            this.chkAutoCloseWindow.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoCloseWindow.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoCloseWindow.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutoCloseWindow.Location = new System.Drawing.Point(10, 57);
            this.chkAutoCloseWindow.Name = "chkAutoCloseWindow";
            this.chkAutoCloseWindow.Size = new System.Drawing.Size(275, 25);
            this.chkAutoCloseWindow.TabIndex = 13;
            this.chkAutoCloseWindow.Text = "Automatically Close Debug Window";
            this.chkAutoCloseWindow.UseVisualStyleBackColor = false;
            // 
            // chkShowDebug
            // 
            this.chkShowDebug.AutoSize = true;
            this.chkShowDebug.BackColor = System.Drawing.Color.Transparent;
            this.chkShowDebug.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkShowDebug.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkShowDebug.Location = new System.Drawing.Point(10, 35);
            this.chkShowDebug.Name = "chkShowDebug";
            this.chkShowDebug.Size = new System.Drawing.Size(177, 25);
            this.chkShowDebug.TabIndex = 12;
            this.chkShowDebug.Text = "Show Debug Window";
            this.chkShowDebug.UseVisualStyleBackColor = false;
            // 
            // uiBtnOpen
            // 
            this.uiBtnOpen.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOpen.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOpen.DisplayText = "Ok";
            this.uiBtnOpen.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOpen.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOpen.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOpen.Image")));
            this.uiBtnOpen.IsMouseOver = false;
            this.uiBtnOpen.Location = new System.Drawing.Point(3, 719);
            this.uiBtnOpen.Name = "uiBtnOpen";
            this.uiBtnOpen.Size = new System.Drawing.Size(45, 49);
            this.uiBtnOpen.TabIndex = 13;
            this.uiBtnOpen.TabStop = false;
            this.uiBtnOpen.Text = "Ok";
            this.uiBtnOpen.Click += new System.EventHandler(this.uiBtnOpen_Click);
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(3, 1);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(125, 45);
            this.lblMainLogo.TabIndex = 14;
            this.lblMainLogo.Text = "settings";
            // 
            // lblAutomationEngine
            // 
            this.lblAutomationEngine.AutoSize = true;
            this.lblAutomationEngine.BackColor = System.Drawing.Color.Transparent;
            this.lblAutomationEngine.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutomationEngine.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblAutomationEngine.Location = new System.Drawing.Point(6, 6);
            this.lblAutomationEngine.Name = "lblAutomationEngine";
            this.lblAutomationEngine.Size = new System.Drawing.Size(181, 30);
            this.lblAutomationEngine.TabIndex = 15;
            this.lblAutomationEngine.Text = "Automation Engine";
            // 
            // lblApplicationSettings
            // 
            this.lblApplicationSettings.AutoSize = true;
            this.lblApplicationSettings.BackColor = System.Drawing.Color.Transparent;
            this.lblApplicationSettings.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblApplicationSettings.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblApplicationSettings.Location = new System.Drawing.Point(6, 6);
            this.lblApplicationSettings.Name = "lblApplicationSettings";
            this.lblApplicationSettings.Size = new System.Drawing.Size(184, 30);
            this.lblApplicationSettings.TabIndex = 21;
            this.lblApplicationSettings.Text = "Application Settings";
            // 
            // chkAntiIdle
            // 
            this.chkAntiIdle.AutoSize = true;
            this.chkAntiIdle.BackColor = System.Drawing.Color.Transparent;
            this.chkAntiIdle.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAntiIdle.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAntiIdle.Location = new System.Drawing.Point(10, 36);
            this.chkAntiIdle.Name = "chkAntiIdle";
            this.chkAntiIdle.Size = new System.Drawing.Size(222, 25);
            this.chkAntiIdle.TabIndex = 20;
            this.chkAntiIdle.Text = "Anti-Idle (while app is open)";
            this.chkAntiIdle.UseVisualStyleBackColor = false;
            // 
            // btnUpdates
            // 
            this.btnUpdates.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnUpdates.Location = new System.Drawing.Point(12, 338);
            this.btnUpdates.Name = "btnUpdates";
            this.btnUpdates.Size = new System.Drawing.Size(207, 27);
            this.btnUpdates.TabIndex = 22;
            this.btnUpdates.Text = "Check For Updates";
            this.btnUpdates.UseVisualStyleBackColor = true;
            this.btnUpdates.Click += new System.EventHandler(this.btnUpdateCheck_Click);
            // 
            // chkAdvancedDebug
            // 
            this.chkAdvancedDebug.AutoSize = true;
            this.chkAdvancedDebug.BackColor = System.Drawing.Color.Transparent;
            this.chkAdvancedDebug.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAdvancedDebug.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAdvancedDebug.Location = new System.Drawing.Point(10, 102);
            this.chkAdvancedDebug.Name = "chkAdvancedDebug";
            this.chkAdvancedDebug.Size = new System.Drawing.Size(344, 25);
            this.chkAdvancedDebug.TabIndex = 23;
            this.chkAdvancedDebug.Text = "Show Advanced Debug Logs During Execution";
            this.chkAdvancedDebug.UseVisualStyleBackColor = false;
            // 
            // uiSettingTabs
            // 
            this.uiSettingTabs.Controls.Add(this.tabAppSettings);
            this.uiSettingTabs.Controls.Add(this.tabDebugSettings);
            this.uiSettingTabs.Controls.Add(this.tabServerSettings);
            this.uiSettingTabs.Controls.Add(this.tabLocalListener);
            this.uiSettingTabs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.uiSettingTabs.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.uiSettingTabs.Location = new System.Drawing.Point(3, 73);
            this.uiSettingTabs.Name = "uiSettingTabs";
            this.uiSettingTabs.SelectedIndex = 0;
            this.uiSettingTabs.Size = new System.Drawing.Size(634, 640);
            this.uiSettingTabs.TabIndex = 25;
            // 
            // tabAppSettings
            // 
            this.tabAppSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabAppSettings.Controls.Add(this.chkSlimActionBar);
            this.tabAppSettings.Controls.Add(this.chkPreloadCommands);
            this.tabAppSettings.Controls.Add(this.btnLaunchDisplayManager);
            this.tabAppSettings.Controls.Add(this.lblStartupMode);
            this.tabAppSettings.Controls.Add(this.cboStartUpMode);
            this.tabAppSettings.Controls.Add(this.btnSelectAttendedTaskFolder);
            this.tabAppSettings.Controls.Add(this.lblAttendedTasksFolder);
            this.tabAppSettings.Controls.Add(this.txtAttendedTaskFolder);
            this.tabAppSettings.Controls.Add(this.btnLaunchAttendedMode);
            this.tabAppSettings.Controls.Add(this.chkMinimizeToTray);
            this.tabAppSettings.Controls.Add(this.chkSequenceDragDrop);
            this.tabAppSettings.Controls.Add(this.btnGenerateWikiDocs);
            this.tabAppSettings.Controls.Add(this.chkInsertCommandsInline);
            this.tabAppSettings.Controls.Add(this.btnClearMetrics);
            this.tabAppSettings.Controls.Add(this.lblScriptExecutionMetrics);
            this.tabAppSettings.Controls.Add(this.lblGettingMetrics);
            this.tabAppSettings.Controls.Add(this.tvExecutionTimes);
            this.tabAppSettings.Controls.Add(this.btnSelectFolder);
            this.tabAppSettings.Controls.Add(this.lblRootFolder);
            this.tabAppSettings.Controls.Add(this.txtAppFolderPath);
            this.tabAppSettings.Controls.Add(this.lblApplicationSettings);
            this.tabAppSettings.Controls.Add(this.chkAntiIdle);
            this.tabAppSettings.Controls.Add(this.btnUpdates);
            this.tabAppSettings.Location = new System.Drawing.Point(4, 30);
            this.tabAppSettings.Name = "tabAppSettings";
            this.tabAppSettings.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabAppSettings.Size = new System.Drawing.Size(626, 606);
            this.tabAppSettings.TabIndex = 0;
            this.tabAppSettings.Text = "Application";
            // 
            // chkSlimActionBar
            // 
            this.chkSlimActionBar.AutoSize = true;
            this.chkSlimActionBar.BackColor = System.Drawing.Color.Transparent;
            this.chkSlimActionBar.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSlimActionBar.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkSlimActionBar.Location = new System.Drawing.Point(10, 124);
            this.chkSlimActionBar.Name = "chkSlimActionBar";
            this.chkSlimActionBar.Size = new System.Drawing.Size(162, 25);
            this.chkSlimActionBar.TabIndex = 42;
            this.chkSlimActionBar.Text = "Use Slim Action Bar";
            this.chkSlimActionBar.UseVisualStyleBackColor = false;
            // 
            // chkPreloadCommands
            // 
            this.chkPreloadCommands.AutoSize = true;
            this.chkPreloadCommands.BackColor = System.Drawing.Color.Transparent;
            this.chkPreloadCommands.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkPreloadCommands.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkPreloadCommands.Location = new System.Drawing.Point(10, 145);
            this.chkPreloadCommands.Name = "chkPreloadCommands";
            this.chkPreloadCommands.Size = new System.Drawing.Size(332, 25);
            this.chkPreloadCommands.TabIndex = 41;
            this.chkPreloadCommands.Text = "Load Commands at Startup (Reduces Flicker)";
            this.chkPreloadCommands.UseVisualStyleBackColor = false;
            this.chkPreloadCommands.Visible = false;
            // 
            // btnLaunchDisplayManager
            // 
            this.btnLaunchDisplayManager.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLaunchDisplayManager.Location = new System.Drawing.Point(225, 371);
            this.btnLaunchDisplayManager.Name = "btnLaunchDisplayManager";
            this.btnLaunchDisplayManager.Size = new System.Drawing.Size(207, 27);
            this.btnLaunchDisplayManager.TabIndex = 40;
            this.btnLaunchDisplayManager.Text = "Display Manager for VMs";
            this.btnLaunchDisplayManager.UseVisualStyleBackColor = true;
            this.btnLaunchDisplayManager.Click += new System.EventHandler(this.btnLaunchDisplayManager_Click);
            // 
            // lblStartupMode
            // 
            this.lblStartupMode.AutoSize = true;
            this.lblStartupMode.BackColor = System.Drawing.Color.Transparent;
            this.lblStartupMode.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStartupMode.ForeColor = System.Drawing.Color.SlateGray;
            this.lblStartupMode.Location = new System.Drawing.Point(10, 281);
            this.lblStartupMode.Name = "lblStartupMode";
            this.lblStartupMode.Size = new System.Drawing.Size(70, 17);
            this.lblStartupMode.TabIndex = 39;
            this.lblStartupMode.Text = "Start Mode";
            // 
            // cboStartUpMode
            // 
            this.cboStartUpMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboStartUpMode.FormattingEnabled = true;
            this.cboStartUpMode.Items.AddRange(new object[] {
            "Builder Mode",
            "Attended Task Mode"});
            this.cboStartUpMode.Location = new System.Drawing.Point(13, 301);
            this.cboStartUpMode.Name = "cboStartUpMode";
            this.cboStartUpMode.Size = new System.Drawing.Size(219, 29);
            this.cboStartUpMode.TabIndex = 38;
            // 
            // btnSelectAttendedTaskFolder
            // 
            this.btnSelectAttendedTaskFolder.Location = new System.Drawing.Point(504, 249);
            this.btnSelectAttendedTaskFolder.Name = "btnSelectAttendedTaskFolder";
            this.btnSelectAttendedTaskFolder.Size = new System.Drawing.Size(42, 30);
            this.btnSelectAttendedTaskFolder.TabIndex = 37;
            this.btnSelectAttendedTaskFolder.Text = "...";
            this.btnSelectAttendedTaskFolder.UseVisualStyleBackColor = true;
            this.btnSelectAttendedTaskFolder.Click += new System.EventHandler(this.btnSelectAttendedTaskFolder_Click);
            // 
            // lblAttendedTasksFolder
            // 
            this.lblAttendedTasksFolder.AutoSize = true;
            this.lblAttendedTasksFolder.BackColor = System.Drawing.Color.Transparent;
            this.lblAttendedTasksFolder.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAttendedTasksFolder.ForeColor = System.Drawing.Color.SlateGray;
            this.lblAttendedTasksFolder.Location = new System.Drawing.Point(9, 230);
            this.lblAttendedTasksFolder.Name = "lblAttendedTasksFolder";
            this.lblAttendedTasksFolder.Size = new System.Drawing.Size(130, 17);
            this.lblAttendedTasksFolder.TabIndex = 36;
            this.lblAttendedTasksFolder.Text = "Attended Tasks Folder";
            // 
            // txtAttendedTaskFolder
            // 
            this.txtAttendedTaskFolder.Location = new System.Drawing.Point(12, 249);
            this.txtAttendedTaskFolder.Name = "txtAttendedTaskFolder";
            this.txtAttendedTaskFolder.Size = new System.Drawing.Size(490, 29);
            this.txtAttendedTaskFolder.TabIndex = 35;
            // 
            // btnLaunchAttendedMode
            // 
            this.btnLaunchAttendedMode.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLaunchAttendedMode.Location = new System.Drawing.Point(225, 338);
            this.btnLaunchAttendedMode.Name = "btnLaunchAttendedMode";
            this.btnLaunchAttendedMode.Size = new System.Drawing.Size(207, 27);
            this.btnLaunchAttendedMode.TabIndex = 34;
            this.btnLaunchAttendedMode.Text = "Launch Attended Mode";
            this.btnLaunchAttendedMode.UseVisualStyleBackColor = true;
            this.btnLaunchAttendedMode.Click += new System.EventHandler(this.btnLaunchAttendedMode_Click);
            // 
            // chkMinimizeToTray
            // 
            this.chkMinimizeToTray.AutoSize = true;
            this.chkMinimizeToTray.BackColor = System.Drawing.Color.Transparent;
            this.chkMinimizeToTray.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkMinimizeToTray.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkMinimizeToTray.Location = new System.Drawing.Point(10, 102);
            this.chkMinimizeToTray.Name = "chkMinimizeToTray";
            this.chkMinimizeToTray.Size = new System.Drawing.Size(193, 25);
            this.chkMinimizeToTray.TabIndex = 33;
            this.chkMinimizeToTray.Text = "Minimize to System Tray";
            this.chkMinimizeToTray.UseVisualStyleBackColor = false;
            // 
            // chkSequenceDragDrop
            // 
            this.chkSequenceDragDrop.AutoSize = true;
            this.chkSequenceDragDrop.BackColor = System.Drawing.Color.Transparent;
            this.chkSequenceDragDrop.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkSequenceDragDrop.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkSequenceDragDrop.Location = new System.Drawing.Point(10, 80);
            this.chkSequenceDragDrop.Name = "chkSequenceDragDrop";
            this.chkSequenceDragDrop.Size = new System.Drawing.Size(355, 25);
            this.chkSequenceDragDrop.TabIndex = 32;
            this.chkSequenceDragDrop.Text = "Allow Drag and Drop into Sequence Commands";
            this.chkSequenceDragDrop.UseVisualStyleBackColor = false;
            // 
            // btnGenerateWikiDocs
            // 
            this.btnGenerateWikiDocs.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnGenerateWikiDocs.Location = new System.Drawing.Point(12, 371);
            this.btnGenerateWikiDocs.Name = "btnGenerateWikiDocs";
            this.btnGenerateWikiDocs.Size = new System.Drawing.Size(207, 27);
            this.btnGenerateWikiDocs.TabIndex = 31;
            this.btnGenerateWikiDocs.Text = "Generate Documentation";
            this.btnGenerateWikiDocs.UseVisualStyleBackColor = true;
            this.btnGenerateWikiDocs.Click += new System.EventHandler(this.btnGenerateWikiDocs_Click);
            // 
            // chkInsertCommandsInline
            // 
            this.chkInsertCommandsInline.AutoSize = true;
            this.chkInsertCommandsInline.BackColor = System.Drawing.Color.Transparent;
            this.chkInsertCommandsInline.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkInsertCommandsInline.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkInsertCommandsInline.Location = new System.Drawing.Point(10, 58);
            this.chkInsertCommandsInline.Name = "chkInsertCommandsInline";
            this.chkInsertCommandsInline.Size = new System.Drawing.Size(365, 25);
            this.chkInsertCommandsInline.TabIndex = 30;
            this.chkInsertCommandsInline.Text = "New Commands Insert Below Selected Command";
            this.chkInsertCommandsInline.UseVisualStyleBackColor = false;
            // 
            // btnClearMetrics
            // 
            this.btnClearMetrics.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClearMetrics.Location = new System.Drawing.Point(11, 558);
            this.btnClearMetrics.Name = "btnClearMetrics";
            this.btnClearMetrics.Size = new System.Drawing.Size(108, 25);
            this.btnClearMetrics.TabIndex = 29;
            this.btnClearMetrics.Text = "Clear Metrics";
            this.btnClearMetrics.UseVisualStyleBackColor = true;
            this.btnClearMetrics.Visible = false;
            this.btnClearMetrics.Click += new System.EventHandler(this.btnClearMetrics_Click);
            // 
            // lblScriptExecutionMetrics
            // 
            this.lblScriptExecutionMetrics.AutoSize = true;
            this.lblScriptExecutionMetrics.BackColor = System.Drawing.Color.Transparent;
            this.lblScriptExecutionMetrics.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScriptExecutionMetrics.ForeColor = System.Drawing.Color.SlateGray;
            this.lblScriptExecutionMetrics.Location = new System.Drawing.Point(13, 409);
            this.lblScriptExecutionMetrics.Name = "lblScriptExecutionMetrics";
            this.lblScriptExecutionMetrics.Size = new System.Drawing.Size(248, 17);
            this.lblScriptExecutionMetrics.TabIndex = 28;
            this.lblScriptExecutionMetrics.Text = "Script Execution Metrics (Last 10 per Script)";
            // 
            // lblGettingMetrics
            // 
            this.lblGettingMetrics.AccessibleRole = System.Windows.Forms.AccessibleRole.ButtonDropDownGrid;
            this.lblGettingMetrics.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblGettingMetrics.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblGettingMetrics.Location = new System.Drawing.Point(12, 427);
            this.lblGettingMetrics.Name = "lblGettingMetrics";
            this.lblGettingMetrics.Size = new System.Drawing.Size(534, 127);
            this.lblGettingMetrics.TabIndex = 27;
            this.lblGettingMetrics.Text = "Getting Metrics...";
            // 
            // tvExecutionTimes
            // 
            this.tvExecutionTimes.Location = new System.Drawing.Point(12, 427);
            this.tvExecutionTimes.Name = "tvExecutionTimes";
            this.tvExecutionTimes.Size = new System.Drawing.Size(534, 127);
            this.tvExecutionTimes.TabIndex = 26;
            this.tvExecutionTimes.Visible = false;
            // 
            // btnSelectFolder
            // 
            this.btnSelectFolder.Location = new System.Drawing.Point(504, 197);
            this.btnSelectFolder.Name = "btnSelectFolder";
            this.btnSelectFolder.Size = new System.Drawing.Size(42, 30);
            this.btnSelectFolder.TabIndex = 25;
            this.btnSelectFolder.Text = "...";
            this.btnSelectFolder.UseVisualStyleBackColor = true;
            this.btnSelectFolder.Click += new System.EventHandler(this.btnSelectFolder_Click);
            // 
            // lblRootFolder
            // 
            this.lblRootFolder.AutoSize = true;
            this.lblRootFolder.BackColor = System.Drawing.Color.Transparent;
            this.lblRootFolder.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRootFolder.ForeColor = System.Drawing.Color.SlateGray;
            this.lblRootFolder.Location = new System.Drawing.Point(9, 178);
            this.lblRootFolder.Name = "lblRootFolder";
            this.lblRootFolder.Size = new System.Drawing.Size(168, 17);
            this.lblRootFolder.TabIndex = 24;
            this.lblRootFolder.Text = "OpenBots Studio Root Folder";
            // 
            // txtAppFolderPath
            // 
            this.txtAppFolderPath.Location = new System.Drawing.Point(12, 197);
            this.txtAppFolderPath.Name = "txtAppFolderPath";
            this.txtAppFolderPath.Size = new System.Drawing.Size(490, 29);
            this.txtAppFolderPath.TabIndex = 23;
            // 
            // tabDebugSettings
            // 
            this.tabDebugSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabDebugSettings.Controls.Add(this.btnFileManager);
            this.tabDebugSettings.Controls.Add(this.cbxMinLogLevel);
            this.tabDebugSettings.Controls.Add(this.lblMinLogLevel);
            this.tabDebugSettings.Controls.Add(this.txtLogging4);
            this.tabDebugSettings.Controls.Add(this.lblLogging4);
            this.tabDebugSettings.Controls.Add(this.lblLogging3);
            this.tabDebugSettings.Controls.Add(this.txtLogging3);
            this.tabDebugSettings.Controls.Add(this.txtLogging2);
            this.tabDebugSettings.Controls.Add(this.lblLogging2);
            this.tabDebugSettings.Controls.Add(this.txtLogging1);
            this.tabDebugSettings.Controls.Add(this.lblLogging1);
            this.tabDebugSettings.Controls.Add(this.cbxSinkType);
            this.tabDebugSettings.Controls.Add(this.lblSinkType);
            this.tabDebugSettings.Controls.Add(this.lblLoggingSettings);
            this.tabDebugSettings.Controls.Add(this.chkAutoCalcVariables);
            this.tabDebugSettings.Controls.Add(this.lblEndScriptHotKey);
            this.tabDebugSettings.Controls.Add(this.cbxCancellationKey);
            this.tabDebugSettings.Controls.Add(this.chkOverrideInstances);
            this.tabDebugSettings.Controls.Add(this.lblDelay);
            this.tabDebugSettings.Controls.Add(this.txtCommandDelay);
            this.tabDebugSettings.Controls.Add(this.chkTrackMetrics);
            this.tabDebugSettings.Controls.Add(this.lblAutomationEngine);
            this.tabDebugSettings.Controls.Add(this.chkShowDebug);
            this.tabDebugSettings.Controls.Add(this.chkAdvancedDebug);
            this.tabDebugSettings.Controls.Add(this.chkAutoCloseWindow);
            this.tabDebugSettings.Controls.Add(this.chkEnableLogging);
            this.tabDebugSettings.Location = new System.Drawing.Point(4, 30);
            this.tabDebugSettings.Name = "tabDebugSettings";
            this.tabDebugSettings.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabDebugSettings.Size = new System.Drawing.Size(626, 624);
            this.tabDebugSettings.TabIndex = 1;
            this.tabDebugSettings.Text = "Automation Engine";
            // 
            // btnFileManager
            // 
            this.btnFileManager.Location = new System.Drawing.Point(590, 336);
            this.btnFileManager.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnFileManager.Name = "btnFileManager";
            this.btnFileManager.Size = new System.Drawing.Size(27, 28);
            this.btnFileManager.TabIndex = 52;
            this.btnFileManager.Text = "...";
            this.btnFileManager.UseVisualStyleBackColor = true;
            this.btnFileManager.Click += new System.EventHandler(this.btnFileManager_Click);
            // 
            // cbxMinLogLevel
            // 
            this.cbxMinLogLevel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxMinLogLevel.FormattingEnabled = true;
            this.cbxMinLogLevel.Location = new System.Drawing.Point(446, 298);
            this.cbxMinLogLevel.Name = "cbxMinLogLevel";
            this.cbxMinLogLevel.Size = new System.Drawing.Size(142, 29);
            this.cbxMinLogLevel.TabIndex = 51;
            this.cbxMinLogLevel.SelectedIndexChanged += new System.EventHandler(this.cbxMinLogLevel_SelectedIndexChanged);
            // 
            // lblMinLogLevel
            // 
            this.lblMinLogLevel.AutoSize = true;
            this.lblMinLogLevel.BackColor = System.Drawing.Color.Transparent;
            this.lblMinLogLevel.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMinLogLevel.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblMinLogLevel.Location = new System.Drawing.Point(293, 301);
            this.lblMinLogLevel.Name = "lblMinLogLevel";
            this.lblMinLogLevel.Size = new System.Drawing.Size(147, 21);
            this.lblMinLogLevel.TabIndex = 50;
            this.lblMinLogLevel.Text = "Minimum Log Level:";
            // 
            // txtLogging4
            // 
            this.txtLogging4.Location = new System.Drawing.Point(142, 432);
            this.txtLogging4.Name = "txtLogging4";
            this.txtLogging4.Size = new System.Drawing.Size(445, 29);
            this.txtLogging4.TabIndex = 49;
            this.txtLogging4.Visible = false;
            // 
            // lblLogging4
            // 
            this.lblLogging4.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lblLogging4.AutoSize = true;
            this.lblLogging4.BackColor = System.Drawing.Color.Transparent;
            this.lblLogging4.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogging4.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblLogging4.Location = new System.Drawing.Point(6, 436);
            this.lblLogging4.Name = "lblLogging4";
            this.lblLogging4.Size = new System.Drawing.Size(89, 21);
            this.lblLogging4.TabIndex = 48;
            this.lblLogging4.Text = "User IDs (,):";
            this.lblLogging4.Visible = false;
            // 
            // lblLogging3
            // 
            this.lblLogging3.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.lblLogging3.AutoSize = true;
            this.lblLogging3.BackColor = System.Drawing.Color.Transparent;
            this.lblLogging3.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogging3.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblLogging3.Location = new System.Drawing.Point(6, 403);
            this.lblLogging3.Name = "lblLogging3";
            this.lblLogging3.Size = new System.Drawing.Size(131, 21);
            this.lblLogging3.TabIndex = 47;
            this.lblLogging3.Text = "Group Names (,): ";
            this.lblLogging3.Visible = false;
            // 
            // txtLogging3
            // 
            this.txtLogging3.Location = new System.Drawing.Point(142, 400);
            this.txtLogging3.Name = "txtLogging3";
            this.txtLogging3.Size = new System.Drawing.Size(445, 29);
            this.txtLogging3.TabIndex = 46;
            this.txtLogging3.Visible = false;
            // 
            // txtLogging2
            // 
            this.txtLogging2.Location = new System.Drawing.Point(142, 367);
            this.txtLogging2.Name = "txtLogging2";
            this.txtLogging2.Size = new System.Drawing.Size(445, 29);
            this.txtLogging2.TabIndex = 45;
            this.txtLogging2.Visible = false;
            // 
            // lblLogging2
            // 
            this.lblLogging2.AutoSize = true;
            this.lblLogging2.BackColor = System.Drawing.Color.Transparent;
            this.lblLogging2.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogging2.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblLogging2.Location = new System.Drawing.Point(6, 370);
            this.lblLogging2.Name = "lblLogging2";
            this.lblLogging2.Size = new System.Drawing.Size(43, 21);
            this.lblLogging2.TabIndex = 44;
            this.lblLogging2.Text = "Hub:";
            this.lblLogging2.Visible = false;
            // 
            // txtLogging1
            // 
            this.txtLogging1.Location = new System.Drawing.Point(142, 335);
            this.txtLogging1.Name = "txtLogging1";
            this.txtLogging1.Size = new System.Drawing.Size(445, 29);
            this.txtLogging1.TabIndex = 43;
            // 
            // lblLogging1
            // 
            this.lblLogging1.AutoSize = true;
            this.lblLogging1.BackColor = System.Drawing.Color.Transparent;
            this.lblLogging1.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogging1.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblLogging1.Location = new System.Drawing.Point(6, 338);
            this.lblLogging1.Name = "lblLogging1";
            this.lblLogging1.Size = new System.Drawing.Size(72, 21);
            this.lblLogging1.TabIndex = 42;
            this.lblLogging1.Text = "File Path:";
            // 
            // cbxSinkType
            // 
            this.cbxSinkType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxSinkType.FormattingEnabled = true;
            this.cbxSinkType.Location = new System.Drawing.Point(142, 298);
            this.cbxSinkType.Name = "cbxSinkType";
            this.cbxSinkType.Size = new System.Drawing.Size(142, 29);
            this.cbxSinkType.TabIndex = 41;
            this.cbxSinkType.SelectedIndexChanged += new System.EventHandler(this.cbxSinkType_SelectedIndexChanged);
            // 
            // lblSinkType
            // 
            this.lblSinkType.AutoSize = true;
            this.lblSinkType.BackColor = System.Drawing.Color.Transparent;
            this.lblSinkType.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSinkType.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblSinkType.Location = new System.Drawing.Point(6, 301);
            this.lblSinkType.Name = "lblSinkType";
            this.lblSinkType.Size = new System.Drawing.Size(78, 21);
            this.lblSinkType.TabIndex = 40;
            this.lblSinkType.Text = "Sink Type:";
            // 
            // lblLoggingSettings
            // 
            this.lblLoggingSettings.AutoSize = true;
            this.lblLoggingSettings.BackColor = System.Drawing.Color.Transparent;
            this.lblLoggingSettings.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoggingSettings.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblLoggingSettings.Location = new System.Drawing.Point(6, 262);
            this.lblLoggingSettings.Name = "lblLoggingSettings";
            this.lblLoggingSettings.Size = new System.Drawing.Size(160, 30);
            this.lblLoggingSettings.TabIndex = 39;
            this.lblLoggingSettings.Text = "Logging Settings";
            // 
            // chkAutoCalcVariables
            // 
            this.chkAutoCalcVariables.AutoSize = true;
            this.chkAutoCalcVariables.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoCalcVariables.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoCalcVariables.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutoCalcVariables.Location = new System.Drawing.Point(10, 167);
            this.chkAutoCalcVariables.Name = "chkAutoCalcVariables";
            this.chkAutoCalcVariables.Size = new System.Drawing.Size(255, 25);
            this.chkAutoCalcVariables.TabIndex = 38;
            this.chkAutoCalcVariables.Text = "Calculate Variables Automatically";
            this.chkAutoCalcVariables.UseVisualStyleBackColor = false;
            // 
            // lblEndScriptHotKey
            // 
            this.lblEndScriptHotKey.AutoSize = true;
            this.lblEndScriptHotKey.BackColor = System.Drawing.Color.Transparent;
            this.lblEndScriptHotKey.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblEndScriptHotKey.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblEndScriptHotKey.Location = new System.Drawing.Point(6, 223);
            this.lblEndScriptHotKey.Name = "lblEndScriptHotKey";
            this.lblEndScriptHotKey.Size = new System.Drawing.Size(134, 21);
            this.lblEndScriptHotKey.TabIndex = 37;
            this.lblEndScriptHotKey.Text = "End Script Hotkey:";
            // 
            // cbxCancellationKey
            // 
            this.cbxCancellationKey.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbxCancellationKey.FormattingEnabled = true;
            this.cbxCancellationKey.Location = new System.Drawing.Point(142, 223);
            this.cbxCancellationKey.Name = "cbxCancellationKey";
            this.cbxCancellationKey.Size = new System.Drawing.Size(142, 29);
            this.cbxCancellationKey.TabIndex = 36;
            // 
            // chkOverrideInstances
            // 
            this.chkOverrideInstances.AutoSize = true;
            this.chkOverrideInstances.BackColor = System.Drawing.Color.Transparent;
            this.chkOverrideInstances.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkOverrideInstances.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkOverrideInstances.Location = new System.Drawing.Point(10, 145);
            this.chkOverrideInstances.Name = "chkOverrideInstances";
            this.chkOverrideInstances.Size = new System.Drawing.Size(187, 25);
            this.chkOverrideInstances.TabIndex = 35;
            this.chkOverrideInstances.Text = "Override App Instances";
            this.chkOverrideInstances.UseVisualStyleBackColor = false;
            // 
            // lblDelay
            // 
            this.lblDelay.AutoSize = true;
            this.lblDelay.BackColor = System.Drawing.Color.Transparent;
            this.lblDelay.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDelay.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblDelay.Location = new System.Drawing.Point(6, 192);
            this.lblDelay.Name = "lblDelay";
            this.lblDelay.Size = new System.Drawing.Size(349, 21);
            this.lblDelay.TabIndex = 34;
            this.lblDelay.Text = "Default delay between executing commands (ms):";
            // 
            // txtCommandDelay
            // 
            this.txtCommandDelay.Location = new System.Drawing.Point(359, 188);
            this.txtCommandDelay.Name = "txtCommandDelay";
            this.txtCommandDelay.Size = new System.Drawing.Size(77, 29);
            this.txtCommandDelay.TabIndex = 33;
            // 
            // chkTrackMetrics
            // 
            this.chkTrackMetrics.AutoSize = true;
            this.chkTrackMetrics.BackColor = System.Drawing.Color.Transparent;
            this.chkTrackMetrics.Font = new System.Drawing.Font("Segoe UI Semilight", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTrackMetrics.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkTrackMetrics.Location = new System.Drawing.Point(10, 124);
            this.chkTrackMetrics.Name = "chkTrackMetrics";
            this.chkTrackMetrics.Size = new System.Drawing.Size(188, 25);
            this.chkTrackMetrics.TabIndex = 25;
            this.chkTrackMetrics.Text = "Track Execution Metrics";
            this.chkTrackMetrics.UseVisualStyleBackColor = false;
            // 
            // tabServerSettings
            // 
            this.tabServerSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabServerSettings.Controls.Add(this.btnTaskPublish);
            this.tabServerSettings.Controls.Add(this.lblClientGUIDDesc);
            this.tabServerSettings.Controls.Add(this.lblServerSettingsDesc);
            this.tabServerSettings.Controls.Add(this.lblHTTPSDesc);
            this.tabServerSettings.Controls.Add(this.btnGetGUID);
            this.tabServerSettings.Controls.Add(this.lblClientGUID);
            this.tabServerSettings.Controls.Add(this.txtGUID);
            this.tabServerSettings.Controls.Add(this.lblHTTPSServerURL);
            this.tabServerSettings.Controls.Add(this.txtHttpsAddress);
            this.tabServerSettings.Controls.Add(this.chkBypassValidation);
            this.tabServerSettings.Controls.Add(this.btnCloseConnection);
            this.tabServerSettings.Controls.Add(this.lblSocketException);
            this.tabServerSettings.Controls.Add(this.lblServerSettings);
            this.tabServerSettings.Controls.Add(this.chkServerEnabled);
            this.tabServerSettings.Controls.Add(this.txtPublicKey);
            this.tabServerSettings.Controls.Add(this.lblServerURL);
            this.tabServerSettings.Controls.Add(this.lblConnectKey);
            this.tabServerSettings.Controls.Add(this.txtServerURL);
            this.tabServerSettings.Controls.Add(this.btnConnect);
            this.tabServerSettings.Controls.Add(this.chkAutomaticallyConnect);
            this.tabServerSettings.Controls.Add(this.chkRetryOnDisconnect);
            this.tabServerSettings.Controls.Add(this.lblStatus);
            this.tabServerSettings.Location = new System.Drawing.Point(4, 30);
            this.tabServerSettings.Name = "tabServerSettings";
            this.tabServerSettings.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabServerSettings.Size = new System.Drawing.Size(626, 624);
            this.tabServerSettings.TabIndex = 2;
            this.tabServerSettings.Text = "Server";
            // 
            // btnTaskPublish
            // 
            this.btnTaskPublish.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnTaskPublish.Location = new System.Drawing.Point(14, 343);
            this.btnTaskPublish.Name = "btnTaskPublish";
            this.btnTaskPublish.Size = new System.Drawing.Size(147, 33);
            this.btnTaskPublish.TabIndex = 33;
            this.btnTaskPublish.Text = "Publish Task";
            this.btnTaskPublish.UseVisualStyleBackColor = true;
            this.btnTaskPublish.Click += new System.EventHandler(this.btnTaskPublish_Click);
            // 
            // lblClientGUIDDesc
            // 
            this.lblClientGUIDDesc.AutoSize = true;
            this.lblClientGUIDDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblClientGUIDDesc.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientGUIDDesc.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblClientGUIDDesc.Location = new System.Drawing.Point(14, 283);
            this.lblClientGUIDDesc.Name = "lblClientGUIDDesc";
            this.lblClientGUIDDesc.Size = new System.Drawing.Size(488, 20);
            this.lblClientGUIDDesc.TabIndex = 32;
            this.lblClientGUIDDesc.Text = "Indicates the GUID the client will use when connecting to OpenBots Server";
            // 
            // lblServerSettingsDesc
            // 
            this.lblServerSettingsDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblServerSettingsDesc.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerSettingsDesc.ForeColor = System.Drawing.Color.SlateGray;
            this.lblServerSettingsDesc.Location = new System.Drawing.Point(10, 36);
            this.lblServerSettingsDesc.Name = "lblServerSettingsDesc";
            this.lblServerSettingsDesc.Size = new System.Drawing.Size(587, 67);
            this.lblServerSettingsDesc.TabIndex = 30;
            this.lblServerSettingsDesc.Text = resources.GetString("lblServerSettingsDesc.Text");
            // 
            // lblHTTPSDesc
            // 
            this.lblHTTPSDesc.AutoSize = true;
            this.lblHTTPSDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblHTTPSDesc.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHTTPSDesc.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblHTTPSDesc.Location = new System.Drawing.Point(10, 176);
            this.lblHTTPSDesc.Name = "lblHTTPSDesc";
            this.lblHTTPSDesc.Size = new System.Drawing.Size(453, 20);
            this.lblHTTPSDesc.TabIndex = 29;
            this.lblHTTPSDesc.Text = "Enter the location of the OpenBots Server ex. https://localhost:60281";
            // 
            // btnGetGUID
            // 
            this.btnGetGUID.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnGetGUID.Location = new System.Drawing.Point(12, 231);
            this.btnGetGUID.Name = "btnGetGUID";
            this.btnGetGUID.Size = new System.Drawing.Size(147, 33);
            this.btnGetGUID.TabIndex = 28;
            this.btnGetGUID.Text = "Test Connection";
            this.btnGetGUID.UseVisualStyleBackColor = true;
            this.btnGetGUID.Click += new System.EventHandler(this.btnGetGUID_Click);
            // 
            // lblClientGUID
            // 
            this.lblClientGUID.AutoSize = true;
            this.lblClientGUID.BackColor = System.Drawing.Color.Transparent;
            this.lblClientGUID.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblClientGUID.ForeColor = System.Drawing.Color.SlateGray;
            this.lblClientGUID.Location = new System.Drawing.Point(14, 268);
            this.lblClientGUID.Name = "lblClientGUID";
            this.lblClientGUID.Size = new System.Drawing.Size(74, 17);
            this.lblClientGUID.TabIndex = 26;
            this.lblClientGUID.Text = "Client GUID";
            // 
            // txtGUID
            // 
            this.txtGUID.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtGUID.Location = new System.Drawing.Point(15, 308);
            this.txtGUID.Name = "txtGUID";
            this.txtGUID.Size = new System.Drawing.Size(371, 29);
            this.txtGUID.TabIndex = 27;
            // 
            // lblHTTPSServerURL
            // 
            this.lblHTTPSServerURL.AutoSize = true;
            this.lblHTTPSServerURL.BackColor = System.Drawing.Color.Transparent;
            this.lblHTTPSServerURL.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHTTPSServerURL.ForeColor = System.Drawing.Color.SlateGray;
            this.lblHTTPSServerURL.Location = new System.Drawing.Point(11, 154);
            this.lblHTTPSServerURL.Name = "lblHTTPSServerURL";
            this.lblHTTPSServerURL.Size = new System.Drawing.Size(111, 17);
            this.lblHTTPSServerURL.TabIndex = 24;
            this.lblHTTPSServerURL.Text = "HTTPS Server URL";
            // 
            // txtHttpsAddress
            // 
            this.txtHttpsAddress.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtHttpsAddress.Location = new System.Drawing.Point(13, 198);
            this.txtHttpsAddress.Name = "txtHttpsAddress";
            this.txtHttpsAddress.Size = new System.Drawing.Size(371, 29);
            this.txtHttpsAddress.TabIndex = 25;
            // 
            // chkBypassValidation
            // 
            this.chkBypassValidation.AutoSize = true;
            this.chkBypassValidation.BackColor = System.Drawing.Color.Transparent;
            this.chkBypassValidation.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkBypassValidation.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkBypassValidation.Location = new System.Drawing.Point(159, 499);
            this.chkBypassValidation.Name = "chkBypassValidation";
            this.chkBypassValidation.Size = new System.Drawing.Size(138, 19);
            this.chkBypassValidation.TabIndex = 23;
            this.chkBypassValidation.Text = "Bypass SSL Validation";
            this.chkBypassValidation.UseVisualStyleBackColor = false;
            this.chkBypassValidation.Visible = false;
            this.chkBypassValidation.CheckedChanged += new System.EventHandler(this.chkBypassValidation_CheckedChanged);
            // 
            // btnCloseConnection
            // 
            this.btnCloseConnection.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnCloseConnection.Location = new System.Drawing.Point(100, 529);
            this.btnCloseConnection.Name = "btnCloseConnection";
            this.btnCloseConnection.Size = new System.Drawing.Size(75, 33);
            this.btnCloseConnection.TabIndex = 22;
            this.btnCloseConnection.Text = "Close";
            this.btnCloseConnection.UseVisualStyleBackColor = true;
            this.btnCloseConnection.Visible = false;
            this.btnCloseConnection.Click += new System.EventHandler(this.btnCloseConnection_Click);
            // 
            // lblSocketException
            // 
            this.lblSocketException.AutoSize = true;
            this.lblSocketException.BackColor = System.Drawing.Color.Transparent;
            this.lblSocketException.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSocketException.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblSocketException.Location = new System.Drawing.Point(78, 439);
            this.lblSocketException.Name = "lblSocketException";
            this.lblSocketException.Size = new System.Drawing.Size(0, 20);
            this.lblSocketException.TabIndex = 21;
            this.lblSocketException.Visible = false;
            // 
            // lblServerSettings
            // 
            this.lblServerSettings.AutoSize = true;
            this.lblServerSettings.BackColor = System.Drawing.Color.Transparent;
            this.lblServerSettings.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblServerSettings.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblServerSettings.Location = new System.Drawing.Point(7, 6);
            this.lblServerSettings.Name = "lblServerSettings";
            this.lblServerSettings.Size = new System.Drawing.Size(144, 30);
            this.lblServerSettings.TabIndex = 20;
            this.lblServerSettings.Text = "Server Settings";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblStatus.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblStatus.Location = new System.Drawing.Point(78, 405);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(0, 20);
            this.lblStatus.TabIndex = 16;
            this.lblStatus.Visible = false;
            // 
            // tabLocalListener
            // 
            this.tabLocalListener.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabLocalListener.Controls.Add(this.txtWhiteList);
            this.tabLocalListener.Controls.Add(this.chkEnableWhitelist);
            this.tabLocalListener.Controls.Add(this.chkAutoStartListener);
            this.tabLocalListener.Controls.Add(this.lblListeningPort);
            this.tabLocalListener.Controls.Add(this.txtListeningPort);
            this.tabLocalListener.Controls.Add(this.lblListeningStatus);
            this.tabLocalListener.Controls.Add(this.btnStopListening);
            this.tabLocalListener.Controls.Add(this.btnStartListening);
            this.tabLocalListener.Controls.Add(this.chkRequireListenerKey);
            this.tabLocalListener.Controls.Add(this.lblAuthKey);
            this.tabLocalListener.Controls.Add(this.txtAuthListeningKey);
            this.tabLocalListener.Controls.Add(this.chkEnableListening);
            this.tabLocalListener.Controls.Add(this.lblLocalListenerSettingsDesc);
            this.tabLocalListener.Controls.Add(this.lblLocalListenerSettings);
            this.tabLocalListener.Location = new System.Drawing.Point(4, 30);
            this.tabLocalListener.Name = "tabLocalListener";
            this.tabLocalListener.Padding = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.tabLocalListener.Size = new System.Drawing.Size(626, 624);
            this.tabLocalListener.TabIndex = 3;
            this.tabLocalListener.Text = "Local Listener";
            // 
            // txtWhiteList
            // 
            this.txtWhiteList.Location = new System.Drawing.Point(15, 291);
            this.txtWhiteList.Multiline = true;
            this.txtWhiteList.Name = "txtWhiteList";
            this.txtWhiteList.Size = new System.Drawing.Size(371, 93);
            this.txtWhiteList.TabIndex = 48;
            // 
            // chkEnableWhitelist
            // 
            this.chkEnableWhitelist.AutoSize = true;
            this.chkEnableWhitelist.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableWhitelist.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableWhitelist.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkEnableWhitelist.Location = new System.Drawing.Point(14, 261);
            this.chkEnableWhitelist.Name = "chkEnableWhitelist";
            this.chkEnableWhitelist.Size = new System.Drawing.Size(318, 24);
            this.chkEnableWhitelist.TabIndex = 47;
            this.chkEnableWhitelist.Text = "Enable IP Verification (Seperate with comma)";
            this.chkEnableWhitelist.UseVisualStyleBackColor = false;
            // 
            // chkAutoStartListener
            // 
            this.chkAutoStartListener.AutoSize = true;
            this.chkAutoStartListener.BackColor = System.Drawing.Color.Transparent;
            this.chkAutoStartListener.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkAutoStartListener.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkAutoStartListener.Location = new System.Drawing.Point(14, 82);
            this.chkAutoStartListener.Name = "chkAutoStartListener";
            this.chkAutoStartListener.Size = new System.Drawing.Size(193, 24);
            this.chkAutoStartListener.TabIndex = 43;
            this.chkAutoStartListener.Text = "Start Listening on Startup";
            this.chkAutoStartListener.UseVisualStyleBackColor = false;
            // 
            // lblListeningPort
            // 
            this.lblListeningPort.AutoSize = true;
            this.lblListeningPort.BackColor = System.Drawing.Color.Transparent;
            this.lblListeningPort.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblListeningPort.ForeColor = System.Drawing.Color.SlateGray;
            this.lblListeningPort.Location = new System.Drawing.Point(11, 128);
            this.lblListeningPort.Name = "lblListeningPort";
            this.lblListeningPort.Size = new System.Drawing.Size(83, 17);
            this.lblListeningPort.TabIndex = 41;
            this.lblListeningPort.Text = "Listening Port";
            // 
            // txtListeningPort
            // 
            this.txtListeningPort.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtListeningPort.Location = new System.Drawing.Point(14, 148);
            this.txtListeningPort.Name = "txtListeningPort";
            this.txtListeningPort.Size = new System.Drawing.Size(132, 29);
            this.txtListeningPort.TabIndex = 42;
            // 
            // lblListeningStatus
            // 
            this.lblListeningStatus.AutoSize = true;
            this.lblListeningStatus.BackColor = System.Drawing.Color.Transparent;
            this.lblListeningStatus.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblListeningStatus.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblListeningStatus.Location = new System.Drawing.Point(10, 427);
            this.lblListeningStatus.Name = "lblListeningStatus";
            this.lblListeningStatus.Size = new System.Drawing.Size(137, 30);
            this.lblListeningStatus.TabIndex = 40;
            this.lblListeningStatus.Text = "Listening on {}";
            this.lblListeningStatus.Visible = false;
            // 
            // btnStopListening
            // 
            this.btnStopListening.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnStopListening.Location = new System.Drawing.Point(167, 391);
            this.btnStopListening.Name = "btnStopListening";
            this.btnStopListening.Size = new System.Drawing.Size(147, 33);
            this.btnStopListening.TabIndex = 39;
            this.btnStopListening.Text = "Stop Listening";
            this.btnStopListening.UseVisualStyleBackColor = true;
            this.btnStopListening.Click += new System.EventHandler(this.btnStopListening_Click);
            // 
            // btnStartListening
            // 
            this.btnStartListening.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnStartListening.Location = new System.Drawing.Point(14, 391);
            this.btnStartListening.Name = "btnStartListening";
            this.btnStartListening.Size = new System.Drawing.Size(147, 33);
            this.btnStartListening.TabIndex = 38;
            this.btnStartListening.Text = "Start Listening";
            this.btnStartListening.UseVisualStyleBackColor = true;
            this.btnStartListening.Click += new System.EventHandler(this.btnStartListening_Click);
            // 
            // chkRequireListenerKey
            // 
            this.chkRequireListenerKey.AutoSize = true;
            this.chkRequireListenerKey.BackColor = System.Drawing.Color.Transparent;
            this.chkRequireListenerKey.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRequireListenerKey.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkRequireListenerKey.Location = new System.Drawing.Point(14, 183);
            this.chkRequireListenerKey.Name = "chkRequireListenerKey";
            this.chkRequireListenerKey.Size = new System.Drawing.Size(204, 24);
            this.chkRequireListenerKey.TabIndex = 37;
            this.chkRequireListenerKey.Text = "Require Authentication Key";
            this.chkRequireListenerKey.UseVisualStyleBackColor = false;
            // 
            // lblAuthKey
            // 
            this.lblAuthKey.AutoSize = true;
            this.lblAuthKey.BackColor = System.Drawing.Color.Transparent;
            this.lblAuthKey.Font = new System.Drawing.Font("Segoe UI Semilight", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAuthKey.ForeColor = System.Drawing.Color.SlateGray;
            this.lblAuthKey.Location = new System.Drawing.Point(11, 209);
            this.lblAuthKey.Name = "lblAuthKey";
            this.lblAuthKey.Size = new System.Drawing.Size(113, 17);
            this.lblAuthKey.TabIndex = 34;
            this.lblAuthKey.Text = "Authentication Key";
            // 
            // txtAuthListeningKey
            // 
            this.txtAuthListeningKey.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtAuthListeningKey.Location = new System.Drawing.Point(14, 229);
            this.txtAuthListeningKey.Name = "txtAuthListeningKey";
            this.txtAuthListeningKey.Size = new System.Drawing.Size(371, 29);
            this.txtAuthListeningKey.TabIndex = 35;
            // 
            // chkEnableListening
            // 
            this.chkEnableListening.AutoSize = true;
            this.chkEnableListening.BackColor = System.Drawing.Color.Transparent;
            this.chkEnableListening.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkEnableListening.ForeColor = System.Drawing.Color.SteelBlue;
            this.chkEnableListening.Location = new System.Drawing.Point(14, 105);
            this.chkEnableListening.Name = "chkEnableListening";
            this.chkEnableListening.Size = new System.Drawing.Size(180, 24);
            this.chkEnableListening.TabIndex = 33;
            this.chkEnableListening.Text = "Local Listening Enabled";
            this.chkEnableListening.UseVisualStyleBackColor = false;
            // 
            // lblLocalListenerSettingsDesc
            // 
            this.lblLocalListenerSettingsDesc.BackColor = System.Drawing.Color.Transparent;
            this.lblLocalListenerSettingsDesc.Font = new System.Drawing.Font("Segoe UI Semilight", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocalListenerSettingsDesc.ForeColor = System.Drawing.Color.SlateGray;
            this.lblLocalListenerSettingsDesc.Location = new System.Drawing.Point(10, 36);
            this.lblLocalListenerSettingsDesc.Name = "lblLocalListenerSettingsDesc";
            this.lblLocalListenerSettingsDesc.Size = new System.Drawing.Size(587, 67);
            this.lblLocalListenerSettingsDesc.TabIndex = 32;
            this.lblLocalListenerSettingsDesc.Text = "Enable this functionality to allow this computer to accept script execution reque" +
    "sts from other OpenBots or REST-capable clients.";
            // 
            // lblLocalListenerSettings
            // 
            this.lblLocalListenerSettings.AutoSize = true;
            this.lblLocalListenerSettings.BackColor = System.Drawing.Color.Transparent;
            this.lblLocalListenerSettings.Font = new System.Drawing.Font("Segoe UI Light", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocalListenerSettings.ForeColor = System.Drawing.Color.SteelBlue;
            this.lblLocalListenerSettings.Location = new System.Drawing.Point(7, 6);
            this.lblLocalListenerSettings.Name = "lblLocalListenerSettings";
            this.lblLocalListenerSettings.Size = new System.Drawing.Size(268, 30);
            this.lblLocalListenerSettings.TabIndex = 31;
            this.lblLocalListenerSettings.Text = "Local Listener Settings (BETA)";
            // 
            // tlpSettings
            // 
            this.tlpSettings.BackColor = System.Drawing.Color.Transparent;
            this.tlpSettings.ColumnCount = 1;
            this.tlpSettings.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSettings.Controls.Add(this.uiSettingTabs, 0, 1);
            this.tlpSettings.Controls.Add(this.uiBtnOpen, 0, 2);
            this.tlpSettings.Controls.Add(this.pnlSettings, 0, 0);
            this.tlpSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpSettings.Location = new System.Drawing.Point(0, 0);
            this.tlpSettings.Name = "tlpSettings";
            this.tlpSettings.RowCount = 3;
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            this.tlpSettings.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 16F));
            this.tlpSettings.Size = new System.Drawing.Size(640, 776);
            this.tlpSettings.TabIndex = 26;
            // 
            // pnlSettings
            // 
            this.pnlSettings.BackColor = System.Drawing.Color.Transparent;
            this.pnlSettings.Controls.Add(this.lblMainLogo);
            this.pnlSettings.Controls.Add(this.lblManageSettings);
            this.pnlSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlSettings.Location = new System.Drawing.Point(0, 0);
            this.pnlSettings.Margin = new System.Windows.Forms.Padding(0);
            this.pnlSettings.Name = "pnlSettings";
            this.pnlSettings.Size = new System.Drawing.Size(640, 70);
            this.pnlSettings.TabIndex = 26;
            // 
            // tmrGetSocketStatus
            // 
            this.tmrGetSocketStatus.Interval = 250;
            this.tmrGetSocketStatus.Tick += new System.EventHandler(this.tmrGetSocketStatus_Tick);
            // 
            // bgwMetrics
            // 
            this.bgwMetrics.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwMetrics_DoWork);
            this.bgwMetrics.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwMetrics_RunWorkerCompleted);
            // 
            // frmSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 776);
            this.Controls.Add(this.tlpSettings);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmSettings";
            this.Text = "Settings";
            this.Load += new System.EventHandler(this.frmSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOpen)).EndInit();
            this.uiSettingTabs.ResumeLayout(false);
            this.tabAppSettings.ResumeLayout(false);
            this.tabAppSettings.PerformLayout();
            this.tabDebugSettings.ResumeLayout(false);
            this.tabDebugSettings.PerformLayout();
            this.tabServerSettings.ResumeLayout(false);
            this.tabServerSettings.PerformLayout();
            this.tabLocalListener.ResumeLayout(false);
            this.tabLocalListener.PerformLayout();
            this.tlpSettings.ResumeLayout(false);
            this.pnlSettings.ResumeLayout(false);
            this.pnlSettings.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblServerURL;
        private System.Windows.Forms.TextBox txtServerURL;
        private System.Windows.Forms.CheckBox chkAutomaticallyConnect;
        private System.Windows.Forms.CheckBox chkServerEnabled;
        private System.Windows.Forms.Label lblManageSettings;
        private System.Windows.Forms.CheckBox chkRetryOnDisconnect;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.TextBox txtPublicKey;
        private System.Windows.Forms.Label lblConnectKey;
        private System.Windows.Forms.CheckBox chkAutoCloseWindow;
        private System.Windows.Forms.CheckBox chkShowDebug;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnOpen;
        private System.Windows.Forms.CheckBox chkEnableLogging;
        private System.Windows.Forms.Label lblMainLogo;
        private System.Windows.Forms.Label lblAutomationEngine;
        private System.Windows.Forms.Label lblApplicationSettings;
        private System.Windows.Forms.CheckBox chkAntiIdle;
        private System.Windows.Forms.Button btnUpdates;
        private System.Windows.Forms.CheckBox chkAdvancedDebug;
        private CustomControls.CustomUIControls.UITabControl uiSettingTabs;
        private System.Windows.Forms.TabPage tabAppSettings;
        private System.Windows.Forms.TabPage tabDebugSettings;
        private System.Windows.Forms.TabPage tabServerSettings;
        private System.Windows.Forms.Label lblServerSettings;
        private System.Windows.Forms.TableLayoutPanel tlpSettings;
        private System.Windows.Forms.Panel pnlSettings;
        private System.Windows.Forms.Label lblSocketException;
        private System.Windows.Forms.Button btnCloseConnection;
        private System.Windows.Forms.Timer tmrGetSocketStatus;
        private System.Windows.Forms.CheckBox chkBypassValidation;
        private System.Windows.Forms.Label lblRootFolder;
        private System.Windows.Forms.TextBox txtAppFolderPath;
        private System.Windows.Forms.Button btnSelectFolder;
        private System.Windows.Forms.TreeView tvExecutionTimes;
        private System.ComponentModel.BackgroundWorker bgwMetrics;
        private System.Windows.Forms.Label lblScriptExecutionMetrics;
        private System.Windows.Forms.Label lblGettingMetrics;
        private System.Windows.Forms.Button btnClearMetrics;
        private System.Windows.Forms.CheckBox chkTrackMetrics;
        private System.Windows.Forms.CheckBox chkInsertCommandsInline;
        private System.Windows.Forms.Button btnGenerateWikiDocs;
        private System.Windows.Forms.Label lblDelay;
        private System.Windows.Forms.TextBox txtCommandDelay;
        internal System.Windows.Forms.CheckBox chkOverrideInstances;
        private System.Windows.Forms.CheckBox chkSequenceDragDrop;
        private System.Windows.Forms.CheckBox chkMinimizeToTray;
        private System.Windows.Forms.Button btnLaunchAttendedMode;
        private System.Windows.Forms.Button btnSelectAttendedTaskFolder;
        private System.Windows.Forms.Label lblAttendedTasksFolder;
        private System.Windows.Forms.TextBox txtAttendedTaskFolder;
        private System.Windows.Forms.Label lblStartupMode;
        private System.Windows.Forms.ComboBox cboStartUpMode;
        private System.Windows.Forms.Button btnLaunchDisplayManager;
        private System.Windows.Forms.Label lblClientGUID;
        private System.Windows.Forms.TextBox txtGUID;
        private System.Windows.Forms.Label lblHTTPSServerURL;
        private System.Windows.Forms.TextBox txtHttpsAddress;
        private System.Windows.Forms.Button btnGetGUID;
        private System.Windows.Forms.Label lblServerSettingsDesc;
        private System.Windows.Forms.Label lblHTTPSDesc;
        private System.Windows.Forms.Label lblClientGUIDDesc;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.Button btnTaskPublish;
        private System.Windows.Forms.CheckBox chkPreloadCommands;
        private System.Windows.Forms.CheckBox chkSlimActionBar;
        private System.Windows.Forms.ComboBox cbxCancellationKey;
        private System.Windows.Forms.Label lblEndScriptHotKey;
        private System.Windows.Forms.TabPage tabLocalListener;
        private System.Windows.Forms.Label lblLocalListenerSettingsDesc;
        private System.Windows.Forms.Label lblLocalListenerSettings;
        private System.Windows.Forms.Label lblAuthKey;
        private System.Windows.Forms.TextBox txtAuthListeningKey;
        private System.Windows.Forms.CheckBox chkEnableListening;
        private System.Windows.Forms.CheckBox chkRequireListenerKey;
        private System.Windows.Forms.Button btnStopListening;
        private System.Windows.Forms.Button btnStartListening;
        private System.Windows.Forms.Label lblListeningStatus;
        private System.Windows.Forms.CheckBox chkAutoStartListener;
        private System.Windows.Forms.Label lblListeningPort;
        private System.Windows.Forms.TextBox txtListeningPort;
        private System.Windows.Forms.CheckBox chkEnableWhitelist;
        private System.Windows.Forms.TextBox txtWhiteList;
        internal System.Windows.Forms.CheckBox chkAutoCalcVariables;
        private System.Windows.Forms.ComboBox cbxSinkType;
        private System.Windows.Forms.Label lblSinkType;
        private System.Windows.Forms.Label lblLoggingSettings;
        private System.Windows.Forms.TextBox txtLogging1;
        private System.Windows.Forms.Label lblLogging1;
        private System.Windows.Forms.Label lblLogging3;
        private System.Windows.Forms.TextBox txtLogging3;
        private System.Windows.Forms.TextBox txtLogging2;
        private System.Windows.Forms.Label lblLogging2;
        private System.Windows.Forms.TextBox txtLogging4;
        private System.Windows.Forms.Label lblLogging4;
        private System.Windows.Forms.ComboBox cbxMinLogLevel;
        private System.Windows.Forms.Label lblMinLogLevel;
        private System.Windows.Forms.Button btnFileManager;
    }
}