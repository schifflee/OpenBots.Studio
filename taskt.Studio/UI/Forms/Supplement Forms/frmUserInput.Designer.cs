namespace taskt.UI.Forms.Supplement_Forms
{
    partial class frmUserInput
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserInput));
            this.flwInputControls = new System.Windows.Forms.FlowLayoutPanel();
            this.lblHeader = new System.Windows.Forms.Label();
            this.lblDirections = new System.Windows.Forms.Label();
            this.tlpInputs = new System.Windows.Forms.TableLayoutPanel();
            this.pnlBottom = new System.Windows.Forms.Panel();
            this.uiBtnOk = new taskt.Core.UI.Controls.UIPictureButton();
            this.uiBtnCancel = new taskt.Core.UI.Controls.UIPictureButton();
            this.tlpInputs.SuspendLayout();
            this.pnlBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            this.SuspendLayout();
            // 
            // flwInputControls
            // 
            this.flwInputControls.AutoScroll = true;
            this.flwInputControls.BackColor = System.Drawing.Color.WhiteSmoke;
            this.flwInputControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flwInputControls.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            this.flwInputControls.Location = new System.Drawing.Point(4, 68);
            this.flwInputControls.Margin = new System.Windows.Forms.Padding(4);
            this.flwInputControls.Name = "flwInputControls";
            this.flwInputControls.Size = new System.Drawing.Size(704, 478);
            this.flwInputControls.TabIndex = 0;
            this.flwInputControls.WrapContents = false;
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(4, 0);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(704, 37);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "Please Provide Input";
            // 
            // lblDirections
            // 
            this.lblDirections.AutoSize = true;
            this.lblDirections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDirections.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDirections.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDirections.Location = new System.Drawing.Point(4, 37);
            this.lblDirections.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDirections.Name = "lblDirections";
            this.lblDirections.Size = new System.Drawing.Size(704, 27);
            this.lblDirections.TabIndex = 1;
            this.lblDirections.Text = "Directions:";
            // 
            // tlpInputs
            // 
            this.tlpInputs.BackColor = System.Drawing.Color.DimGray;
            this.tlpInputs.ColumnCount = 1;
            this.tlpInputs.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpInputs.Controls.Add(this.lblDirections, 0, 1);
            this.tlpInputs.Controls.Add(this.flwInputControls, 0, 2);
            this.tlpInputs.Controls.Add(this.lblHeader, 0, 0);
            this.tlpInputs.Controls.Add(this.pnlBottom, 0, 3);
            this.tlpInputs.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tlpInputs.Location = new System.Drawing.Point(0, 0);
            this.tlpInputs.Margin = new System.Windows.Forms.Padding(4);
            this.tlpInputs.Name = "tlpInputs";
            this.tlpInputs.RowCount = 4;
            this.tlpInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 37F));
            this.tlpInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tlpInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tlpInputs.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 76F));
            this.tlpInputs.Size = new System.Drawing.Size(712, 626);
            this.tlpInputs.TabIndex = 1;
            // 
            // pnlBottom
            // 
            this.pnlBottom.BackColor = System.Drawing.Color.DimGray;
            this.pnlBottom.Controls.Add(this.uiBtnOk);
            this.pnlBottom.Controls.Add(this.uiBtnCancel);
            this.pnlBottom.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlBottom.Location = new System.Drawing.Point(4, 554);
            this.pnlBottom.Margin = new System.Windows.Forms.Padding(4);
            this.pnlBottom.Name = "pnlBottom";
            this.pnlBottom.Size = new System.Drawing.Size(704, 68);
            this.pnlBottom.TabIndex = 2;
            // 
            // uiBtnOk
            // 
            this.uiBtnOk.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOk.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOk.DisplayText = "Accept";
            this.uiBtnOk.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOk.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOk.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOk.Image")));
            this.uiBtnOk.IsMouseOver = false;
            this.uiBtnOk.Location = new System.Drawing.Point(9, 2);
            this.uiBtnOk.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnOk.Name = "uiBtnOk";
            this.uiBtnOk.Size = new System.Drawing.Size(60, 60);
            this.uiBtnOk.TabIndex = 20;
            this.uiBtnOk.TabStop = false;
            this.uiBtnOk.Text = "Accept";
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
            this.uiBtnCancel.Location = new System.Drawing.Point(70, 2);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(60, 60);
            this.uiBtnCancel.TabIndex = 21;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // frmUserInput
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundChangeIndex = 153;
            this.ClientSize = new System.Drawing.Size(712, 626);
            this.Controls.Add(this.tlpInputs);
            this.Icon = taskt.Core.Properties.Resources.openbots_ico;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "frmUserInput";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User Input";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmUserInput_Load);
            this.tlpInputs.ResumeLayout(false);
            this.tlpInputs.PerformLayout();
            this.pnlBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.FlowLayoutPanel flwInputControls;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblDirections;
        private System.Windows.Forms.TableLayoutPanel tlpInputs;
        private System.Windows.Forms.Panel pnlBottom;
        private taskt.Core.UI.Controls.UIPictureButton uiBtnOk;
        private taskt.Core.UI.Controls.UIPictureButton uiBtnCancel;
    }
}