namespace taskt.UI.Supplement_Forms
{
    partial class frmThickAppElementRecorderSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmThickAppElementRecorderSettings));
            this.btnCancel = new taskt.Core.UI.Controls.UIPictureButton();
            this.btnOkay = new taskt.Core.UI.Controls.UIPictureButton();
            this.lblScreenRecorder = new System.Windows.Forms.Label();
            this.lblSearchParameterOrder = new System.Windows.Forms.Label();
            this.dgvParameterSettings = new System.Windows.Forms.DataGridView();
            this.enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.parameterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOkay)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParameterSettings)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.Transparent;
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnCancel.DisplayText = "Cancel";
            this.btnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.btnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnCancel.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.Image")));
            this.btnCancel.IsMouseOver = false;
            this.btnCancel.Location = new System.Drawing.Point(60, 276);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(45, 49);
            this.btnCancel.TabIndex = 32;
            this.btnCancel.TabStop = false;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOkay
            // 
            this.btnOkay.BackColor = System.Drawing.Color.Transparent;
            this.btnOkay.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.btnOkay.DisplayText = "OK";
            this.btnOkay.DisplayTextBrush = System.Drawing.Color.White;
            this.btnOkay.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.btnOkay.Image = ((System.Drawing.Image)(resources.GetObject("btnOkay.Image")));
            this.btnOkay.IsMouseOver = false;
            this.btnOkay.Location = new System.Drawing.Point(15, 276);
            this.btnOkay.Margin = new System.Windows.Forms.Padding(6, 5, 6, 5);
            this.btnOkay.Name = "btnOkay";
            this.btnOkay.Size = new System.Drawing.Size(45, 49);
            this.btnOkay.TabIndex = 31;
            this.btnOkay.TabStop = false;
            this.btnOkay.Text = "OK";
            this.btnOkay.Click += new System.EventHandler(this.btnOkay_Click);
            // 
            // lblScreenRecorder
            // 
            this.lblScreenRecorder.AutoSize = true;
            this.lblScreenRecorder.BackColor = System.Drawing.Color.Transparent;
            this.lblScreenRecorder.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScreenRecorder.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblScreenRecorder.Location = new System.Drawing.Point(9, 7);
            this.lblScreenRecorder.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScreenRecorder.Name = "lblScreenRecorder";
            this.lblScreenRecorder.Size = new System.Drawing.Size(427, 37);
            this.lblScreenRecorder.TabIndex = 35;
            this.lblScreenRecorder.Text = "sequence recorder settings";
            // 
            // lblSearchParameterOrder
            // 
            this.lblSearchParameterOrder.BackColor = System.Drawing.Color.Transparent;
            this.lblSearchParameterOrder.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSearchParameterOrder.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblSearchParameterOrder.Location = new System.Drawing.Point(11, 49);
            this.lblSearchParameterOrder.Name = "lblSearchParameterOrder";
            this.lblSearchParameterOrder.Size = new System.Drawing.Size(362, 24);
            this.lblSearchParameterOrder.TabIndex = 38;
            this.lblSearchParameterOrder.Text = "Search Parameter Order";
            // 
            // dgvParameterSettings
            // 
            this.dgvParameterSettings.AllowUserToAddRows = false;
            this.dgvParameterSettings.AllowUserToDeleteRows = false;
            this.dgvParameterSettings.AllowUserToResizeColumns = false;
            this.dgvParameterSettings.AllowUserToResizeRows = false;
            this.dgvParameterSettings.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvParameterSettings.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvParameterSettings.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.enabled,
            this.parameterName});
            this.dgvParameterSettings.DataBindings.Add(new System.Windows.Forms.Binding("DataSource", this, "ParameterSettingsDT", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dgvParameterSettings.Location = new System.Drawing.Point(15, 73);
            this.dgvParameterSettings.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.dgvParameterSettings.Name = "dgvParameterSettings";
            this.dgvParameterSettings.RowHeadersWidth = 51;
            this.dgvParameterSettings.RowTemplate.Height = 24;
            this.dgvParameterSettings.Size = new System.Drawing.Size(414, 196);
            this.dgvParameterSettings.TabIndex = 39;
            // 
            // enabled
            // 
            this.enabled.DataPropertyName = "Enabled";
            this.enabled.FillWeight = 15F;
            this.enabled.HeaderText = "Enabled";
            this.enabled.MinimumWidth = 6;
            this.enabled.Name = "enabled";
            // 
            // parameterName
            // 
            this.parameterName.DataPropertyName = "Parameter Name";
            this.parameterName.FillWeight = 50F;
            this.parameterName.HeaderText = "Parameter Name";
            this.parameterName.MinimumWidth = 6;
            this.parameterName.Name = "parameterName";
            this.parameterName.ReadOnly = true;
            this.parameterName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.parameterName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // frmThickAppElementRecorderSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundChangeIndex = 1000;
            this.ClientSize = new System.Drawing.Size(442, 339);
            this.Controls.Add(this.dgvParameterSettings);
            this.Controls.Add(this.lblSearchParameterOrder);
            this.Controls.Add(this.lblScreenRecorder);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOkay);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmThickAppElementRecorderSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sequence Recorder Settings";
            this.TopMost = true;
            ((System.ComponentModel.ISupportInitialize)(this.btnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnOkay)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvParameterSettings)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private taskt.Core.UI.Controls.UIPictureButton btnCancel;
        private taskt.Core.UI.Controls.UIPictureButton btnOkay;
        private System.Windows.Forms.Label lblScreenRecorder;
        private System.Windows.Forms.Label lblSearchParameterOrder;
        private System.Windows.Forms.DataGridView dgvParameterSettings;
        private System.Windows.Forms.DataGridViewCheckBoxColumn enabled;
        private System.Windows.Forms.DataGridViewTextBoxColumn parameterName;
    }
}