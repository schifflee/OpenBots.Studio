namespace OpenBots.UI.Forms.Supplement_Forms
{
    partial class frmHTMLBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHTMLBuilder));
            this.tlpMain = new System.Windows.Forms.TableLayoutPanel();
            this.webBrowserHTML = new System.Windows.Forms.WebBrowser();
            this.flwAcceptIcons = new System.Windows.Forms.FlowLayoutPanel();
            this.uiBtnOK = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnCancel = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.rtbHTML = new System.Windows.Forms.RichTextBox();
            this.tlpMain.SuspendLayout();
            this.flwAcceptIcons.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpMain
            // 
            this.tlpMain.BackColor = System.Drawing.Color.White;
            this.tlpMain.ColumnCount = 2;
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tlpMain.Controls.Add(this.webBrowserHTML, 1, 0);
            this.tlpMain.Controls.Add(this.flwAcceptIcons, 0, 1);
            this.tlpMain.Controls.Add(this.rtbHTML, 0, 0);
            this.tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpMain.Location = new System.Drawing.Point(0, 0);
            this.tlpMain.Margin = new System.Windows.Forms.Padding(4);
            this.tlpMain.Name = "tlpMain";
            this.tlpMain.RowCount = 2;
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tlpMain.Size = new System.Drawing.Size(1405, 858);
            this.tlpMain.TabIndex = 0;
            // 
            // webBrowserHTML
            // 
            this.webBrowserHTML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webBrowserHTML.Location = new System.Drawing.Point(706, 4);
            this.webBrowserHTML.Margin = new System.Windows.Forms.Padding(4);
            this.webBrowserHTML.MinimumSize = new System.Drawing.Size(27, 25);
            this.webBrowserHTML.Name = "webBrowserHTML";
            this.webBrowserHTML.Size = new System.Drawing.Size(695, 776);
            this.webBrowserHTML.TabIndex = 0;
            // 
            // flwAcceptIcons
            // 
            this.tlpMain.SetColumnSpan(this.flwAcceptIcons, 2);
            this.flwAcceptIcons.Controls.Add(this.uiBtnOK);
            this.flwAcceptIcons.Controls.Add(this.uiBtnCancel);
            this.flwAcceptIcons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flwAcceptIcons.Location = new System.Drawing.Point(4, 788);
            this.flwAcceptIcons.Margin = new System.Windows.Forms.Padding(4);
            this.flwAcceptIcons.Name = "flwAcceptIcons";
            this.flwAcceptIcons.Size = new System.Drawing.Size(1397, 66);
            this.flwAcceptIcons.TabIndex = 2;
            // 
            // uiBtnOK
            // 
            this.uiBtnOK.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOK.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOK.DisplayText = "Save";
            this.uiBtnOK.DisplayTextBrush = System.Drawing.Color.SteelBlue;
            this.uiBtnOK.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOK.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOK.Image")));
            this.uiBtnOK.IsMouseOver = false;
            this.uiBtnOK.Location = new System.Drawing.Point(4, 4);
            this.uiBtnOK.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnOK.Name = "uiBtnOK";
            this.uiBtnOK.Size = new System.Drawing.Size(60, 60);
            this.uiBtnOK.TabIndex = 18;
            this.uiBtnOK.TabStop = false;
            this.uiBtnOK.Text = "Save";
            this.uiBtnOK.Click += new System.EventHandler(this.uiBtnOK_Click);
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Close";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.SteelBlue;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnCancel.Image")));
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(72, 4);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(4);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(60, 60);
            this.uiBtnCancel.TabIndex = 19;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Close";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // rtbHTML
            // 
            this.rtbHTML.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.rtbHTML.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbHTML.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbHTML.Location = new System.Drawing.Point(4, 4);
            this.rtbHTML.Margin = new System.Windows.Forms.Padding(4);
            this.rtbHTML.Name = "rtbHTML";
            this.rtbHTML.Size = new System.Drawing.Size(694, 776);
            this.rtbHTML.TabIndex = 3;
            this.rtbHTML.Text = "";
            this.rtbHTML.TextChanged += new System.EventHandler(this.rtbHTML_TextChanged);
            // 
            // frmHTMLBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1405, 858);
            this.Controls.Add(this.tlpMain);
            this.Icon = OpenBots.Core.Properties.Resources.OpenBots_ico;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmHTMLBuilder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "HTML Builder";
            this.tlpMain.ResumeLayout(false);
            this.flwAcceptIcons.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOK)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpMain;
        private System.Windows.Forms.WebBrowser webBrowserHTML;
        private System.Windows.Forms.FlowLayoutPanel flwAcceptIcons;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnOK;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnCancel;
        public System.Windows.Forms.RichTextBox rtbHTML;
    }
}