using GSystem = global::System;

namespace OpenBots.Commands.System.Forms
{
    partial class frmDisplayManager
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private GSystem.ComponentModel.IContainer components = null;

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
            this.components = new GSystem.ComponentModel.Container();
            GSystem.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new GSystem.Windows.Forms.DataGridViewCellStyle();
            GSystem.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new GSystem.Windows.Forms.DataGridViewCellStyle();
            GSystem.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new GSystem.Windows.Forms.DataGridViewCellStyle();
            GSystem.ComponentModel.ComponentResourceManager resources = new GSystem.ComponentModel.ComponentResourceManager(typeof(frmDisplayManager));
            this.lstEventLogs = new GSystem.Windows.Forms.ListBox();
            this.dgvMachines = new GSystem.Windows.Forms.DataGridView();
            this.btnStart = new GSystem.Windows.Forms.Button();
            this.btnStop = new GSystem.Windows.Forms.Button();
            this.btnAddMachine = new GSystem.Windows.Forms.Button();
            this.tmrCheck = new GSystem.Windows.Forms.Timer(this.components);
            this.tlpDisplayManager = new GSystem.Windows.Forms.TableLayoutPanel();
            this.pnlBottom = new GSystem.Windows.Forms.Panel();
            this.lblResolution = new GSystem.Windows.Forms.Label();
            this.txtHeight = new GSystem.Windows.Forms.TextBox();
            this.txtWidth = new GSystem.Windows.Forms.TextBox();
            this.chkStartMinimized = new GSystem.Windows.Forms.CheckBox();
            this.chkHideScreen = new GSystem.Windows.Forms.CheckBox();
            this.pnlTop = new GSystem.Windows.Forms.Panel();
            this.lblScheduledScripts = new GSystem.Windows.Forms.Label();
            this.lblHeader = new GSystem.Windows.Forms.Label();
            ((GSystem.ComponentModel.ISupportInitialize)(this.dgvMachines)).BeginInit();
            this.tlpDisplayManager.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            this.pnlTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // lstEventLogs
            // 
            this.tlpDisplayManager.SetColumnSpan(this.lstEventLogs, 2);
            this.lstEventLogs.Dock = GSystem.Windows.Forms.DockStyle.Fill;
            this.lstEventLogs.Font = new GSystem.Drawing.Font("Consolas", 9.75F, GSystem.Drawing.FontStyle.Regular, GSystem.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstEventLogs.FormattingEnabled = true;
            this.lstEventLogs.ItemHeight = 15;
            this.lstEventLogs.Location = new GSystem.Drawing.Point(3, 411);
            this.lstEventLogs.Name = "lstEventLogs";
            this.lstEventLogs.Size = new GSystem.Drawing.Size(1011, 240);
            this.lstEventLogs.TabIndex = 0;
            // 
            // dgvMachines
            // 
            this.dgvMachines.AutoSizeColumnsMode = GSystem.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dataGridViewCellStyle1.Alignment = GSystem.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = GSystem.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new GSystem.Drawing.Font("Segoe UI", 8.25F, GSystem.Drawing.FontStyle.Regular, GSystem.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = GSystem.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = GSystem.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = GSystem.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = GSystem.Windows.Forms.DataGridViewTriState.True;
            this.dgvMachines.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvMachines.ColumnHeadersHeightSizeMode = GSystem.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.tlpDisplayManager.SetColumnSpan(this.dgvMachines, 2);
            dataGridViewCellStyle2.Alignment = GSystem.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = GSystem.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new GSystem.Drawing.Font("Segoe UI", 8.25F, GSystem.Drawing.FontStyle.Regular, GSystem.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = GSystem.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = GSystem.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = GSystem.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = GSystem.Windows.Forms.DataGridViewTriState.False;
            this.dgvMachines.DefaultCellStyle = dataGridViewCellStyle2;
            this.dgvMachines.Dock = GSystem.Windows.Forms.DockStyle.Fill;
            this.dgvMachines.Location = new GSystem.Drawing.Point(3, 75);
            this.dgvMachines.Name = "dgvMachines";
            dataGridViewCellStyle3.Alignment = GSystem.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = GSystem.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new GSystem.Drawing.Font("Segoe UI", 8.25F, GSystem.Drawing.FontStyle.Regular, GSystem.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = GSystem.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = GSystem.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = GSystem.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = GSystem.Windows.Forms.DataGridViewTriState.True;
            this.dgvMachines.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.dgvMachines.Size = new GSystem.Drawing.Size(1011, 290);
            this.dgvMachines.TabIndex = 1;
            // 
            // btnStart
            // 
            this.btnStart.Font = new GSystem.Drawing.Font("Segoe UI", 9.75F, GSystem.Drawing.FontStyle.Regular, GSystem.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStart.Location = new GSystem.Drawing.Point(10, 3);
            this.btnStart.Name = "btnStart";
            this.btnStart.Size = new GSystem.Drawing.Size(75, 27);
            this.btnStart.TabIndex = 2;
            this.btnStart.Text = "Start";
            this.btnStart.UseVisualStyleBackColor = true;
            this.btnStart.Click += new GSystem.EventHandler(this.btnStart_Click);
            // 
            // btnStop
            // 
            this.btnStop.Font = new GSystem.Drawing.Font("Segoe UI", 9.75F, GSystem.Drawing.FontStyle.Regular, GSystem.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnStop.Location = new GSystem.Drawing.Point(90, 3);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new GSystem.Drawing.Size(75, 27);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new GSystem.EventHandler(this.btnStop_Click);
            // 
            // btnAddMachine
            // 
            this.btnAddMachine.Font = new GSystem.Drawing.Font("Segoe UI", 9.75F, GSystem.Drawing.FontStyle.Regular, GSystem.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnAddMachine.Location = new GSystem.Drawing.Point(240, 12);
            this.btnAddMachine.Name = "btnAddMachine";
            this.btnAddMachine.Size = new GSystem.Drawing.Size(126, 27);
            this.btnAddMachine.TabIndex = 4;
            this.btnAddMachine.Text = "Add Machine";
            this.btnAddMachine.UseVisualStyleBackColor = true;
            this.btnAddMachine.Click += new GSystem.EventHandler(this.btnAddMachine_Click);
            // 
            // tmrCheck
            // 
            this.tmrCheck.Interval = 5000;
            this.tmrCheck.Tick += new GSystem.EventHandler(this.tmrCheck_Tick);
            // 
            // tlpDisplayManager
            // 
            this.tlpDisplayManager.BackColor = GSystem.Drawing.Color.DimGray;
            this.tlpDisplayManager.ColumnCount = 2;
            this.tlpDisplayManager.ColumnStyles.Add(new GSystem.Windows.Forms.ColumnStyle(GSystem.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDisplayManager.ColumnStyles.Add(new GSystem.Windows.Forms.ColumnStyle(GSystem.Windows.Forms.SizeType.Percent, 50F));
            this.tlpDisplayManager.Controls.Add(this.pnlBottom, 0, 2);
            this.tlpDisplayManager.Controls.Add(this.dgvMachines, 0, 1);
            this.tlpDisplayManager.Controls.Add(this.lstEventLogs, 0, 3);
            this.tlpDisplayManager.Controls.Add(this.pnlTop, 0, 0);
            this.tlpDisplayManager.Dock = GSystem.Windows.Forms.DockStyle.Fill;
            this.tlpDisplayManager.Location = new GSystem.Drawing.Point(0, 0);
            this.tlpDisplayManager.Name = "tlpDisplayManager";
            this.tlpDisplayManager.RowCount = 4;
            this.tlpDisplayManager.RowStyles.Add(new GSystem.Windows.Forms.RowStyle(GSystem.Windows.Forms.SizeType.Percent, 19.52862F));
            this.tlpDisplayManager.RowStyles.Add(new GSystem.Windows.Forms.RowStyle(GSystem.Windows.Forms.SizeType.Percent, 80.47138F));
            this.tlpDisplayManager.RowStyles.Add(new GSystem.Windows.Forms.RowStyle(GSystem.Windows.Forms.SizeType.Absolute, 40F));
            this.tlpDisplayManager.RowStyles.Add(new GSystem.Windows.Forms.RowStyle(GSystem.Windows.Forms.SizeType.Absolute, 245F));
            this.tlpDisplayManager.Size = new GSystem.Drawing.Size(1017, 654);
            this.tlpDisplayManager.TabIndex = 5;
            // 
            // pnlBottom
            // 
            this.tlpDisplayManager.SetColumnSpan(this.pnlBottom, 2);
            this.pnlBottom.Controls.Add(this.lblResolution);
            this.pnlBottom.Controls.Add(this.txtHeight);
            this.pnlBottom.Controls.Add(this.txtWidth);
            this.pnlBottom.Controls.Add(this.chkStartMinimized);
            this.pnlBottom.Controls.Add(this.chkHideScreen);
            this.pnlBottom.Controls.Add(this.btnStart);
            this.pnlBottom.Controls.Add(this.btnStop);
            this.pnlBottom.Dock = GSystem.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new GSystem.Drawing.Point(3, 371);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new GSystem.Drawing.Size(1011, 34);
            this.pnlBottom.TabIndex = 6;
            // 
            // lblResolution
            // 
            this.lblResolution.AutoSize = true;
            this.lblResolution.Font = new GSystem.Drawing.Font("Segoe UI", 9F, GSystem.Drawing.FontStyle.Regular, GSystem.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResolution.ForeColor = GSystem.Drawing.Color.White;
            this.lblResolution.Location = new GSystem.Drawing.Point(427, 9);
            this.lblResolution.Name = "lblResolution";
            this.lblResolution.Size = new GSystem.Drawing.Size(123, 15);
            this.lblResolution.TabIndex = 9;
            this.lblResolution.Text = "Desktop Window Size:";
            // 
            // txtHeight
            // 
            this.txtHeight.Font = new GSystem.Drawing.Font("Segoe UI", 8.25F, GSystem.Drawing.FontStyle.Regular, GSystem.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHeight.Location = new GSystem.Drawing.Point(590, 6);
            this.txtHeight.Name = "txtHeight";
            this.txtHeight.Size = new GSystem.Drawing.Size(32, 22);
            this.txtHeight.TabIndex = 8;
            this.txtHeight.Text = "1080";
            // 
            // txtWidth
            // 
            this.txtWidth.Font = new GSystem.Drawing.Font("Segoe UI", 8.25F, GSystem.Drawing.FontStyle.Regular, GSystem.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtWidth.Location = new GSystem.Drawing.Point(552, 6);
            this.txtWidth.Name = "txtWidth";
            this.txtWidth.Size = new GSystem.Drawing.Size(32, 22);
            this.txtWidth.TabIndex = 7;
            this.txtWidth.Text = "1920";
            // 
            // chkStartMinimized
            // 
            this.chkStartMinimized.AutoSize = true;
            this.chkStartMinimized.Font = new GSystem.Drawing.Font("Segoe UI", 9.75F, GSystem.Drawing.FontStyle.Regular, GSystem.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkStartMinimized.ForeColor = GSystem.Drawing.Color.White;
            this.chkStartMinimized.Location = new GSystem.Drawing.Point(274, 6);
            this.chkStartMinimized.Name = "chkStartMinimized";
            this.chkStartMinimized.Size = new GSystem.Drawing.Size(118, 21);
            this.chkStartMinimized.TabIndex = 6;
            this.chkStartMinimized.Text = "Start Minimized";
            this.chkStartMinimized.UseVisualStyleBackColor = true;
            // 
            // chkHideScreen
            // 
            this.chkHideScreen.AutoSize = true;
            this.chkHideScreen.Font = new GSystem.Drawing.Font("Segoe UI", 9.75F, GSystem.Drawing.FontStyle.Regular, GSystem.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHideScreen.ForeColor = GSystem.Drawing.Color.White;
            this.chkHideScreen.Location = new GSystem.Drawing.Point(171, 6);
            this.chkHideScreen.Name = "chkHideScreen";
            this.chkHideScreen.Size = new GSystem.Drawing.Size(97, 21);
            this.chkHideScreen.TabIndex = 5;
            this.chkHideScreen.Text = "Hide Screen";
            this.chkHideScreen.UseVisualStyleBackColor = true;
            // 
            // pnlTop
            // 
            this.pnlTop.BackColor = GSystem.Drawing.Color.Transparent;
            this.tlpDisplayManager.SetColumnSpan(this.pnlTop, 2);
            this.pnlTop.Controls.Add(this.lblScheduledScripts);
            this.pnlTop.Controls.Add(this.lblHeader);
            this.pnlTop.Controls.Add(this.btnAddMachine);
            this.pnlTop.Dock = GSystem.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new GSystem.Drawing.Point(0, 0);
            this.pnlTop.Margin = new GSystem.Windows.Forms.Padding(0);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new GSystem.Drawing.Size(1017, 72);
            this.pnlTop.TabIndex = 7;
            // 
            // lblScheduledScripts
            // 
            this.lblScheduledScripts.AutoSize = true;
            this.lblScheduledScripts.BackColor = GSystem.Drawing.Color.Transparent;
            this.lblScheduledScripts.Font = new GSystem.Drawing.Font("Segoe UI", 12F, GSystem.Drawing.FontStyle.Regular, GSystem.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScheduledScripts.ForeColor = GSystem.Drawing.Color.White;
            this.lblScheduledScripts.Location = new GSystem.Drawing.Point(6, 44);
            this.lblScheduledScripts.Name = "lblScheduledScripts";
            this.lblScheduledScripts.Size = new GSystem.Drawing.Size(598, 21);
            this.lblScheduledScripts.TabIndex = 7;
            this.lblScheduledScripts.Text = "Automatically invoke remote desktop connections which keep virtual desktops activ" +
    "e.";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new GSystem.Drawing.Font("Segoe UI", 21.75F, GSystem.Drawing.FontStyle.Regular, GSystem.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = GSystem.Drawing.Color.White;
            this.lblHeader.Location = new GSystem.Drawing.Point(3, 4);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new GSystem.Drawing.Size(231, 40);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Display Manager";
            // 
            // frmDisplayManager
            // 
            this.AutoScaleDimensions = new GSystem.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = GSystem.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundChangeIndex = 3;
            this.ClientSize = new GSystem.Drawing.Size(1017, 654);
            this.Controls.Add(this.tlpDisplayManager);
            this.Icon = OpenBots.Core.Properties.Resources.OpenBots_ico;
            this.Name = "frmDisplayManager";
            this.Text = "Display Manager for VMs";
            this.StartPosition = GSystem.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new GSystem.EventHandler(this.frmDisplayManager_Load);
            ((GSystem.ComponentModel.ISupportInitialize)(this.dgvMachines)).EndInit();
            this.tlpDisplayManager.ResumeLayout(false);
            this.pnlBottom.ResumeLayout(false);
            this.pnlBottom.PerformLayout();
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private GSystem.Windows.Forms.ListBox lstEventLogs;
        private GSystem.Windows.Forms.DataGridView dgvMachines;
        private GSystem.Windows.Forms.Button btnStart;
        private GSystem.Windows.Forms.Button btnStop;
        private GSystem.Windows.Forms.Button btnAddMachine;
        private GSystem.Windows.Forms.Timer tmrCheck;
        private GSystem.Windows.Forms.TableLayoutPanel tlpDisplayManager;
        private GSystem.Windows.Forms.Panel pnlBottom;
        private GSystem.Windows.Forms.CheckBox chkHideScreen;
        private GSystem.Windows.Forms.CheckBox chkStartMinimized;
        private GSystem.Windows.Forms.Panel pnlTop;
        private GSystem.Windows.Forms.Label lblHeader;
        private GSystem.Windows.Forms.Label lblScheduledScripts;
        private GSystem.Windows.Forms.Label lblResolution;
        private GSystem.Windows.Forms.TextBox txtHeight;
        private GSystem.Windows.Forms.TextBox txtWidth;
    }
}