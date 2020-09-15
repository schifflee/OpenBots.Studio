﻿namespace taskt.UI.Forms.Supplement_Forms
{
    partial class frmEngineContextViewer
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmEngineContextViewer));
            this.tvContext = new System.Windows.Forms.TreeView();
            this.tlpContextViewer = new System.Windows.Forms.TableLayoutPanel();
            this.lblMainLogo = new System.Windows.Forms.Label();
            this.pnlDialogResult = new System.Windows.Forms.Panel();
            this.uiBtnOk = new taskt.UI.CustomControls.CustomUIControls.UIPictureButton();
            this.tlpContextViewer.SuspendLayout();
            this.pnlDialogResult.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).BeginInit();
            this.SuspendLayout();
            // 
            // tvContext
            // 
            this.tvContext.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tvContext.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tvContext.Location = new System.Drawing.Point(4, 66);
            this.tvContext.Margin = new System.Windows.Forms.Padding(4);
            this.tvContext.Name = "tvContext";
            this.tvContext.Size = new System.Drawing.Size(1064, 429);
            this.tvContext.TabIndex = 0;
            // 
            // tlpContextViewer
            // 
            this.tlpContextViewer.BackColor = System.Drawing.Color.Transparent;
            this.tlpContextViewer.ColumnCount = 1;
            this.tlpContextViewer.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpContextViewer.Controls.Add(this.lblMainLogo, 0, 0);
            this.tlpContextViewer.Controls.Add(this.tvContext, 0, 1);
            this.tlpContextViewer.Controls.Add(this.pnlDialogResult, 0, 2);
            this.tlpContextViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpContextViewer.Location = new System.Drawing.Point(0, 0);
            this.tlpContextViewer.Margin = new System.Windows.Forms.Padding(4);
            this.tlpContextViewer.Name = "tlpContextViewer";
            this.tlpContextViewer.RowCount = 3;
            this.tlpContextViewer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 62F));
            this.tlpContextViewer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpContextViewer.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 68F));
            this.tlpContextViewer.Size = new System.Drawing.Size(1072, 567);
            this.tlpContextViewer.TabIndex = 1;
            // 
            // lblMainLogo
            // 
            this.lblMainLogo.AutoSize = true;
            this.lblMainLogo.BackColor = System.Drawing.Color.Transparent;
            this.lblMainLogo.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblMainLogo.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblMainLogo.Location = new System.Drawing.Point(4, 0);
            this.lblMainLogo.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMainLogo.Name = "lblMainLogo";
            this.lblMainLogo.Size = new System.Drawing.Size(403, 54);
            this.lblMainLogo.TabIndex = 5;
            this.lblMainLogo.Text = "engine context viewer";
            // 
            // pnlDialogResult
            // 
            this.pnlDialogResult.Controls.Add(this.uiBtnOk);
            this.pnlDialogResult.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlDialogResult.Location = new System.Drawing.Point(4, 503);
            this.pnlDialogResult.Margin = new System.Windows.Forms.Padding(4);
            this.pnlDialogResult.Name = "pnlDialogResult";
            this.pnlDialogResult.Size = new System.Drawing.Size(1064, 60);
            this.pnlDialogResult.TabIndex = 1;
            // 
            // uiBtnOk
            // 
            this.uiBtnOk.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOk.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOk.DisplayText = "Ok";
            this.uiBtnOk.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOk.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOk.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOk.Image")));
            this.uiBtnOk.IsMouseOver = false;
            this.uiBtnOk.Location = new System.Drawing.Point(10, 0);
            this.uiBtnOk.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnOk.Name = "uiBtnOk";
            this.uiBtnOk.Size = new System.Drawing.Size(60, 60);
            this.uiBtnOk.TabIndex = 18;
            this.uiBtnOk.TabStop = false;
            this.uiBtnOk.Text = "Ok";
            this.uiBtnOk.Click += new System.EventHandler(this.UiBtnOk_Click);
            // 
            // frmEngineContextViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1072, 567);
            this.Controls.Add(this.tlpContextViewer);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "frmEngineContextViewer";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "engine context viewer";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.FrmEngineContextViewer_Load);
            this.tlpContextViewer.ResumeLayout(false);
            this.tlpContextViewer.PerformLayout();
            this.pnlDialogResult.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView tvContext;
        private System.Windows.Forms.TableLayoutPanel tlpContextViewer;
        private System.Windows.Forms.Panel pnlDialogResult;
        private CustomControls.CustomUIControls.UIPictureButton uiBtnOk;
        private System.Windows.Forms.Label lblMainLogo;
    }
}