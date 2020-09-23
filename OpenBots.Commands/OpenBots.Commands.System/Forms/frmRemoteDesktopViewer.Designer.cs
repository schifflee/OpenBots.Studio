using GSystem = global::System;

namespace OpenBots.Commands.System.Forms
{
    partial class frmRemoteDesktopViewer
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
            GSystem.ComponentModel.ComponentResourceManager resources = new GSystem.ComponentModel.ComponentResourceManager(typeof(frmRemoteDesktopViewer));
            this.pnlCover = new GSystem.Windows.Forms.Panel();
            this.lblLogo = new GSystem.Windows.Forms.Label();
            this.tmrLoginFailure = new GSystem.Windows.Forms.Timer(this.components);
            this.axRDP = new AxMSTSCLib.AxMsRdpClient6NotSafeForScripting();
            this.pnlCover.SuspendLayout();
            ((GSystem.ComponentModel.ISupportInitialize)(this.axRDP)).BeginInit();
            this.SuspendLayout();
            // 
            // pnlCover
            // 
            this.pnlCover.BackColor = GSystem.Drawing.Color.DimGray;
            this.pnlCover.Controls.Add(this.lblLogo);
            this.pnlCover.Location = new GSystem.Drawing.Point(12, 12);
            this.pnlCover.Name = "pnlCover";
            this.pnlCover.Size = new GSystem.Drawing.Size(200, 100);
            this.pnlCover.TabIndex = 1;
            this.pnlCover.MouseDoubleClick += new GSystem.Windows.Forms.MouseEventHandler(this.pnlCover_MouseDoubleClick);
            // 
            // lblLogo
            // 
            this.lblLogo.Anchor = ((GSystem.Windows.Forms.AnchorStyles)((((GSystem.Windows.Forms.AnchorStyles.Top | GSystem.Windows.Forms.AnchorStyles.Bottom)
            | GSystem.Windows.Forms.AnchorStyles.Left)
            | GSystem.Windows.Forms.AnchorStyles.Right)));
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new GSystem.Drawing.Font("Segoe UI", 48F, GSystem.Drawing.FontStyle.Regular, GSystem.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLogo.ForeColor = GSystem.Drawing.Color.White;
            this.lblLogo.Location = new GSystem.Drawing.Point(743, 377);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new GSystem.Drawing.Size(361, 86);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "vm window";
            // 
            // tmrLoginFailure
            // 
            this.tmrLoginFailure.Interval = 15000;
            this.tmrLoginFailure.Tick += new GSystem.EventHandler(this.tmrLoginFailure_Tick);
            // 
            // axRDP
            // 
            this.axRDP.Dock = GSystem.Windows.Forms.DockStyle.Fill;
            this.axRDP.Enabled = true;
            this.axRDP.Location = new GSystem.Drawing.Point(0, 0);
            this.axRDP.Name = "axRDP";
            this.axRDP.OcxState = ((GSystem.Windows.Forms.AxHost.State)(resources.GetObject("axRDP.OcxState")));
            this.axRDP.Size = new GSystem.Drawing.Size(1083, 716);
            this.axRDP.TabIndex = 2;
            // 
            // frmRemoteDesktopViewer
            // 
            this.AutoScaleDimensions = new GSystem.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = GSystem.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new GSystem.Drawing.Size(1083, 716);
            this.Controls.Add(this.pnlCover);
            this.Controls.Add(this.axRDP);
            this.Icon = ((GSystem.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmRemoteDesktopViewer";
            this.StartPosition = GSystem.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Remote Desktop";
            this.Load += new GSystem.EventHandler(this.frmRemoteDesktopViewer_Load);
            this.pnlCover.ResumeLayout(false);
            this.pnlCover.PerformLayout();
            ((GSystem.ComponentModel.ISupportInitialize)(this.axRDP)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private GSystem.Windows.Forms.Panel pnlCover;
        private GSystem.Windows.Forms.Label lblLogo;
        private GSystem.Windows.Forms.Timer tmrLoginFailure;
        private AxMSTSCLib.AxMsRdpClient6NotSafeForScripting axRDP;
    }
}