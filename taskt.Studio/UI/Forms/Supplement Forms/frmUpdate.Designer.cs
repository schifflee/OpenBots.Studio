﻿namespace taskt.UI.Forms.Supplement_Forms
{
    partial class frmUpdate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpdate));
            this.lblXPosition = new System.Windows.Forms.Label();
            this.lblLocal = new System.Windows.Forms.Label();
            this.lblRemote = new System.Windows.Forms.Label();
            this.uiBtnOk = new taskt.UI.CustomControls.CustomUIControls.UIPictureButton();
            this.uiBtnCancel = new taskt.UI.CustomControls.CustomUIControls.UIPictureButton();
            this.lblText = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // lblXPosition
            // 
            this.lblXPosition.AutoSize = true;
            this.lblXPosition.BackColor = System.Drawing.Color.Transparent;
            this.lblXPosition.Font = new System.Drawing.Font("Segoe UI", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblXPosition.ForeColor = System.Drawing.Color.White;
            this.lblXPosition.Location = new System.Drawing.Point(11, 1);
            this.lblXPosition.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblXPosition.Name = "lblXPosition";
            this.lblXPosition.Size = new System.Drawing.Size(255, 54);
            this.lblXPosition.TabIndex = 2;
            this.lblXPosition.Text = "Great News!";
            // 
            // lblLocal
            // 
            this.lblLocal.AutoSize = true;
            this.lblLocal.BackColor = System.Drawing.Color.Transparent;
            this.lblLocal.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLocal.ForeColor = System.Drawing.Color.White;
            this.lblLocal.Location = new System.Drawing.Point(16, 135);
            this.lblLocal.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblLocal.Name = "lblLocal";
            this.lblLocal.Size = new System.Drawing.Size(142, 32);
            this.lblLocal.TabIndex = 3;
            this.lblLocal.Text = "localVersion";
            // 
            // lblRemote
            // 
            this.lblRemote.AutoSize = true;
            this.lblRemote.BackColor = System.Drawing.Color.Transparent;
            this.lblRemote.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRemote.ForeColor = System.Drawing.Color.White;
            this.lblRemote.Location = new System.Drawing.Point(16, 172);
            this.lblRemote.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblRemote.Name = "lblRemote";
            this.lblRemote.Size = new System.Drawing.Size(170, 32);
            this.lblRemote.TabIndex = 4;
            this.lblRemote.Text = "remoteVersion";
            // 
            // uiBtnOk
            // 
            this.uiBtnOk.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOk.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOk.DisplayText = "Update";
            this.uiBtnOk.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOk.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOk.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOk.Image")));
            this.uiBtnOk.IsMouseOver = false;
            this.uiBtnOk.Location = new System.Drawing.Point(10, 222);
            this.uiBtnOk.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnOk.Name = "uiBtnOk";
            this.uiBtnOk.Size = new System.Drawing.Size(60, 60);
            this.uiBtnOk.TabIndex = 18;
            this.uiBtnOk.TabStop = false;
            this.uiBtnOk.Text = "Update";
            this.uiBtnOk.Click += new System.EventHandler(this.uiBtnOk_Click);
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Cancel";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnCancel.Image")));
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(70, 222);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(60, 60);
            this.uiBtnCancel.TabIndex = 19;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // lblText
            // 
            this.lblText.BackColor = System.Drawing.Color.Transparent;
            this.lblText.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblText.ForeColor = System.Drawing.Color.White;
            this.lblText.Location = new System.Drawing.Point(16, 54);
            this.lblText.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(495, 80);
            this.lblText.TabIndex = 20;
            this.lblText.Text = "We found a signed version that is newer than the one you are running.  Select \'Up" +
    "date\' to get the latest version of taskt.";
            // 
            // frmUpdate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.BackgroundChangeIndex = 175;
            this.ClientSize = new System.Drawing.Size(527, 288);
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.uiBtnOk);
            this.Controls.Add(this.uiBtnCancel);
            this.Controls.Add(this.lblRemote);
            this.Controls.Add(this.lblLocal);
            this.Controls.Add(this.lblXPosition);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmUpdate";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "An Update is Available!";
            this.Load += new System.EventHandler(this.Update_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblXPosition;
        private System.Windows.Forms.Label lblLocal;
        private System.Windows.Forms.Label lblRemote;
        private CustomControls.CustomUIControls.UIPictureButton uiBtnOk;
        private CustomControls.CustomUIControls.UIPictureButton uiBtnCancel;
        private System.Windows.Forms.Label lblText;
    }
}