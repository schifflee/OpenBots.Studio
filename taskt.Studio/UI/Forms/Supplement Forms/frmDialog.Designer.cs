namespace taskt.UI.Forms.Supplement_Forms
{
    partial class frmDialog
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
            taskt.Core.Utilities.FormsUtilities.Theme theme1 = new taskt.Core.Utilities.FormsUtilities.Theme();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDialog));
            this.autoCloseTimer = new System.Windows.Forms.Timer(this.components);
            this.lblAutoClose = new System.Windows.Forms.Label();
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.txtMessage = new System.Windows.Forms.RichTextBox();
            this.pnlControlContainer = new taskt.UI.CustomControls.CustomUIControls.UIPanel();
            this.uiBtnOk = new taskt.Core.UI.Controls.UIPictureButton();
            this.uiBtnCancel = new taskt.Core.UI.Controls.UIPictureButton();
            this.tlpMain.SuspendLayout();
            this.pnlControlContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // autoCloseTimer
            // 
            this.autoCloseTimer.Tick += new System.EventHandler(this.autoCloseTimer_Tick);
            // 
            // lblAutoClose
            // 
            this.lblAutoClose.AutoSize = true;
            this.lblAutoClose.BackColor = System.Drawing.Color.Transparent;
            this.lblAutoClose.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblAutoClose.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAutoClose.ForeColor = System.Drawing.Color.DarkOrange;
            this.lblAutoClose.Location = new System.Drawing.Point(4, 0);
            this.lblAutoClose.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblAutoClose.Name = "lblAutoClose";
            this.lblAutoClose.Size = new System.Drawing.Size(883, 28);
            this.lblAutoClose.TabIndex = 19;
            this.lblAutoClose.Text = "auto close label";
            this.lblAutoClose.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblAutoClose.Visible = false;
            // 
            // tlpMain
            // 
            this.tlpMain.BackColor = System.Drawing.Color.White;
            this.tlpMain.ColumnCount = 1;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.Controls.Add(this.pnlControlContainer, 0, 2);
            this.tlpMain.Controls.Add(this.lblAutoClose, 0, 0);
            this.tlpMain.Controls.Add(this.txtMessage, 0, 1);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(4);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 3;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tlpMain.Size = new System.Drawing.Size(891, 459);
            this.tlpMain.TabIndex = 20;
            // 
            // txtMessage
            // 
            this.txtMessage.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMessage.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtMessage.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMessage.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtMessage.Location = new System.Drawing.Point(4, 32);
            this.txtMessage.Margin = new System.Windows.Forms.Padding(4);
            this.txtMessage.Name = "txtMessage";
            this.txtMessage.Size = new System.Drawing.Size(883, 347);
            this.txtMessage.TabIndex = 20;
            this.txtMessage.Text = "";
            this.txtMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtMessage_KeyDown);
            // 
            // pnlControlContainer
            // 
            this.pnlControlContainer.BackColor = System.Drawing.Color.SteelBlue;
            this.pnlControlContainer.Controls.Add(this.uiBtnOk);
            this.pnlControlContainer.Controls.Add(this.uiBtnCancel);
            this.pnlControlContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlControlContainer.Location = new System.Drawing.Point(0, 383);
            this.pnlControlContainer.Margin = new System.Windows.Forms.Padding(0);
            this.pnlControlContainer.Name = "pnlControlContainer";
            this.pnlControlContainer.Size = new System.Drawing.Size(891, 76);
            this.pnlControlContainer.TabIndex = 19;
            theme1.BgGradientEndColor = System.Drawing.Color.FromArgb(((int)(((byte)(43)))), ((int)(((byte)(50)))), ((int)(((byte)(178)))));
            theme1.BgGradientStartColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(136)))), ((int)(((byte)(204)))));
            this.pnlControlContainer.Theme = theme1;
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
            this.uiBtnOk.Location = new System.Drawing.Point(10, 8);
            this.uiBtnOk.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnOk.Name = "uiBtnOk";
            this.uiBtnOk.Size = new System.Drawing.Size(60, 60);
            this.uiBtnOk.TabIndex = 16;
            this.uiBtnOk.TabStop = false;
            this.uiBtnOk.Text = "Ok";
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
            this.uiBtnCancel.Location = new System.Drawing.Point(70, 8);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(60, 60);
            this.uiBtnCancel.TabIndex = 17;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // frmDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.SteelBlue;
            this.ClientSize = new System.Drawing.Size(891, 459);
            this.Controls.Add(this.tlpMain);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmDialog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Dialog";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmDialog_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmDialog_KeyDown);
            this.tlpMain.ResumeLayout(false);
            this.tlpMain.PerformLayout();
            this.pnlControlContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private taskt.Core.UI.Controls.UIPictureButton uiBtnOk;
        private taskt.Core.UI.Controls.UIPictureButton uiBtnCancel;
        private System.Windows.Forms.Timer autoCloseTimer;
        private System.Windows.Forms.Label lblAutoClose;
        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private taskt.UI.CustomControls.CustomUIControls.UIPanel pnlControlContainer;
        private System.Windows.Forms.RichTextBox txtMessage;
    }
}