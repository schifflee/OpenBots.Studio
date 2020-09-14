namespace taskt.UI.Forms.Supplement_Forms
{
    partial class frmCodeBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCodeBuilder));
            this.tlpBuilder = new System.Windows.Forms.TableLayoutPanel();
            this.pnlTop = new System.Windows.Forms.Panel();
            this.uiBtnSample = new taskt.Core.UI.Controls.UIPictureButton();
            this.uiBtnSave = new taskt.Core.UI.Controls.UIPictureButton();
            this.chkRunAfterCompile = new System.Windows.Forms.CheckBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.uiBtnCompile = new taskt.Core.UI.Controls.UIPictureButton();
            this.lstCompilerResults = new System.Windows.Forms.ListBox();
            this.rtbCode = new System.Windows.Forms.RichTextBox();
            this.tlpBuilder.SuspendLayout();
            this.pnlTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSample)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSave)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCompile)).BeginInit();
            this.SuspendLayout();
            // 
            // tlpBuilder
            // 
            this.tlpBuilder.BackColor = System.Drawing.Color.DimGray;
            this.tlpBuilder.ColumnCount = 1;
            this.tlpBuilder.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45.5F));
            this.tlpBuilder.Controls.Add(this.pnlTop, 0, 0);
            this.tlpBuilder.Controls.Add(this.lstCompilerResults, 0, 2);
            this.tlpBuilder.Controls.Add(this.rtbCode, 0, 1);
            this.tlpBuilder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpBuilder.Location = new System.Drawing.Point(0, 0);
            this.tlpBuilder.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.tlpBuilder.Name = "tlpBuilder";
            this.tlpBuilder.RowCount = 3;
            this.tlpBuilder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16.80672F));
            this.tlpBuilder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 83.19328F));
            this.tlpBuilder.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 114F));
            this.tlpBuilder.Size = new System.Drawing.Size(1103, 555);
            this.tlpBuilder.TabIndex = 0;
            // 
            // pnlTop
            // 
            this.pnlTop.Controls.Add(this.uiBtnSample);
            this.pnlTop.Controls.Add(this.uiBtnSave);
            this.pnlTop.Controls.Add(this.chkRunAfterCompile);
            this.pnlTop.Controls.Add(this.lblHeader);
            this.pnlTop.Controls.Add(this.uiBtnCompile);
            this.pnlTop.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTop.Location = new System.Drawing.Point(4, 4);
            this.pnlTop.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pnlTop.Name = "pnlTop";
            this.pnlTop.Size = new System.Drawing.Size(1095, 66);
            this.pnlTop.TabIndex = 2;
            // 
            // uiBtnSample
            // 
            this.uiBtnSample.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnSample.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnSample.DisplayText = "Sample";
            this.uiBtnSample.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnSample.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.uiBtnSample.Image = global::taskt.Core.Properties.Resources.action_bar_new;
            this.uiBtnSample.IsMouseOver = false;
            this.uiBtnSample.Location = new System.Drawing.Point(290, 4);
            this.uiBtnSample.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.uiBtnSample.Name = "uiBtnSample";
            this.uiBtnSample.Size = new System.Drawing.Size(60, 59);
            this.uiBtnSample.TabIndex = 19;
            this.uiBtnSample.TabStop = false;
            this.uiBtnSample.Text = "Sample";
            this.uiBtnSample.Click += new System.EventHandler(this.uiBtnSample_Click);
            // 
            // uiBtnSave
            // 
            this.uiBtnSave.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnSave.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnSave.DisplayText = "Save";
            this.uiBtnSave.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnSave.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.uiBtnSave.Image = global::taskt.Core.Properties.Resources.action_bar_save;
            this.uiBtnSave.IsMouseOver = false;
            this.uiBtnSave.Location = new System.Drawing.Point(410, 4);
            this.uiBtnSave.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.uiBtnSave.Name = "uiBtnSave";
            this.uiBtnSave.Size = new System.Drawing.Size(60, 59);
            this.uiBtnSave.TabIndex = 18;
            this.uiBtnSave.TabStop = false;
            this.uiBtnSave.Text = "Save";
            this.uiBtnSave.Click += new System.EventHandler(this.uiBtnSave_Click);
            // 
            // chkRunAfterCompile
            // 
            this.chkRunAfterCompile.AutoSize = true;
            this.chkRunAfterCompile.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkRunAfterCompile.ForeColor = System.Drawing.Color.White;
            this.chkRunAfterCompile.Location = new System.Drawing.Point(519, 4);
            this.chkRunAfterCompile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.chkRunAfterCompile.Name = "chkRunAfterCompile";
            this.chkRunAfterCompile.Size = new System.Drawing.Size(288, 27);
            this.chkRunAfterCompile.TabIndex = 16;
            this.chkRunAfterCompile.Text = "Run App after Successful Compile";
            this.chkRunAfterCompile.UseVisualStyleBackColor = true;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblHeader.Location = new System.Drawing.Point(4, 11);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(250, 46);
            this.lblHeader.TabIndex = 15;
            this.lblHeader.Text = "code builder";
            // 
            // uiBtnCompile
            // 
            this.uiBtnCompile.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCompile.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCompile.DisplayText = "Compile";
            this.uiBtnCompile.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCompile.Font = new System.Drawing.Font("Segoe UI", 8F, System.Drawing.FontStyle.Bold);
            this.uiBtnCompile.Image = global::taskt.Core.Properties.Resources.action_bar_run;
            this.uiBtnCompile.IsMouseOver = false;
            this.uiBtnCompile.Location = new System.Drawing.Point(350, 4);
            this.uiBtnCompile.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.uiBtnCompile.Name = "uiBtnCompile";
            this.uiBtnCompile.Size = new System.Drawing.Size(60, 59);
            this.uiBtnCompile.TabIndex = 1;
            this.uiBtnCompile.TabStop = false;
            this.uiBtnCompile.Text = "Compile";
            this.uiBtnCompile.Click += new System.EventHandler(this.utBtnCompile_Click);
            // 
            // lstCompilerResults
            // 
            this.lstCompilerResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstCompilerResults.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstCompilerResults.FormattingEnabled = true;
            this.lstCompilerResults.ItemHeight = 19;
            this.lstCompilerResults.Location = new System.Drawing.Point(4, 444);
            this.lstCompilerResults.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.lstCompilerResults.Name = "lstCompilerResults";
            this.lstCompilerResults.Size = new System.Drawing.Size(1095, 107);
            this.lstCompilerResults.TabIndex = 3;
            // 
            // rtbCode
            // 
            this.rtbCode.Dock = System.Windows.Forms.DockStyle.Fill;
            this.rtbCode.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtbCode.Location = new System.Drawing.Point(4, 78);
            this.rtbCode.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.rtbCode.Name = "rtbCode";
            this.rtbCode.Size = new System.Drawing.Size(1095, 358);
            this.rtbCode.TabIndex = 4;
            this.rtbCode.Text = "";
            this.rtbCode.TextChanged += new System.EventHandler(this.rtbCode_TextChanged);
            // 
            // frmCodeBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1103, 555);
            this.Controls.Add(this.tlpBuilder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "frmCodeBuilder";
            this.Text = "Code Builder";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.frmCodeBuilder_Load);
            this.tlpBuilder.ResumeLayout(false);
            this.pnlTop.ResumeLayout(false);
            this.pnlTop.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSample)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnSave)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCompile)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tlpBuilder;
        private System.Windows.Forms.Panel pnlTop;
        private taskt.Core.UI.Controls.UIPictureButton uiBtnCompile;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.ListBox lstCompilerResults;
        private System.Windows.Forms.CheckBox chkRunAfterCompile;
        private taskt.Core.UI.Controls.UIPictureButton uiBtnSave;
        public System.Windows.Forms.RichTextBox rtbCode;
        private taskt.Core.UI.Controls.UIPictureButton uiBtnSample;
    }
}