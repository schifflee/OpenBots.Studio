using System.Windows.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    partial class frmAddElement
    {
        /// <summary>
        /// Required designer element.
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAddElement));
            this.lblDefineName = new System.Windows.Forms.Label();
            this.lblHeader = new System.Windows.Forms.Label();
            this.txtElementName = new System.Windows.Forms.TextBox();
            this.lblDefineNameDescription = new System.Windows.Forms.Label();
            this.lblDefineDefaultValueDescriptor = new System.Windows.Forms.Label();
            this.lblDefineDefaultValue = new System.Windows.Forms.Label();
            this.uiBtnOk = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.uiBtnCancel = new OpenBots.Core.UI.Controls.UIPictureButton();
            this.lblElementNameError = new System.Windows.Forms.Label();
            this.dgvDefaultValue = new System.Windows.Forms.DataGridView();
            this.enabled = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.parameterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.parameterValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefaultValue)).BeginInit();
            this.SuspendLayout();
            // 
            // lblDefineName
            // 
            this.lblDefineName.AutoSize = true;
            this.lblDefineName.BackColor = System.Drawing.Color.Transparent;
            this.lblDefineName.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefineName.ForeColor = System.Drawing.Color.White;
            this.lblDefineName.Location = new System.Drawing.Point(16, 58);
            this.lblDefineName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefineName.Name = "lblDefineName";
            this.lblDefineName.Size = new System.Drawing.Size(212, 28);
            this.lblDefineName.TabIndex = 15;
            this.lblDefineName.Text = "Define Element Name";
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.BackColor = System.Drawing.Color.Transparent;
            this.lblHeader.Font = new System.Drawing.Font("Segoe UI Semilight", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblHeader.Location = new System.Drawing.Point(8, 4);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(241, 54);
            this.lblHeader.TabIndex = 14;
            this.lblHeader.Text = "add element";
            // 
            // txtElementName
            // 
            this.txtElementName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtElementName.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtElementName.ForeColor = System.Drawing.Color.SteelBlue;
            this.txtElementName.Location = new System.Drawing.Point(21, 167);
            this.txtElementName.Margin = new System.Windows.Forms.Padding(4);
            this.txtElementName.Name = "txtElementName";
            this.txtElementName.Size = new System.Drawing.Size(563, 32);
            this.txtElementName.TabIndex = 16;
            // 
            // lblDefineNameDescription
            // 
            this.lblDefineNameDescription.BackColor = System.Drawing.Color.Transparent;
            this.lblDefineNameDescription.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefineNameDescription.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDefineNameDescription.Location = new System.Drawing.Point(16, 85);
            this.lblDefineNameDescription.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefineNameDescription.Name = "lblDefineNameDescription";
            this.lblDefineNameDescription.Size = new System.Drawing.Size(577, 79);
            this.lblDefineNameDescription.TabIndex = 17;
            this.lblDefineNameDescription.Text = "Define a name for your element, such as <eNumber>.  Remember to enclose the name " +
    "within brackets in order to use the element in commands.";
            // 
            // lblDefineDefaultValueDescriptor
            // 
            this.lblDefineDefaultValueDescriptor.BackColor = System.Drawing.Color.Transparent;
            this.lblDefineDefaultValueDescriptor.Font = new System.Drawing.Font("Segoe UI Light", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefineDefaultValueDescriptor.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lblDefineDefaultValueDescriptor.Location = new System.Drawing.Point(16, 266);
            this.lblDefineDefaultValueDescriptor.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefineDefaultValueDescriptor.Name = "lblDefineDefaultValueDescriptor";
            this.lblDefineDefaultValueDescriptor.Size = new System.Drawing.Size(577, 80);
            this.lblDefineDefaultValueDescriptor.TabIndex = 20;
            this.lblDefineDefaultValueDescriptor.Text = "Define a default value for the element.  The enabled search parameters can be use" +
    "d in web action commands to locate web elements, provided that they contain a va" +
    "lue.";
            // 
            // lblDefineDefaultValue
            // 
            this.lblDefineDefaultValue.AutoSize = true;
            this.lblDefineDefaultValue.BackColor = System.Drawing.Color.Transparent;
            this.lblDefineDefaultValue.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDefineDefaultValue.ForeColor = System.Drawing.Color.White;
            this.lblDefineDefaultValue.Location = new System.Drawing.Point(16, 240);
            this.lblDefineDefaultValue.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblDefineDefaultValue.Name = "lblDefineDefaultValue";
            this.lblDefineDefaultValue.Size = new System.Drawing.Size(280, 28);
            this.lblDefineDefaultValue.TabIndex = 18;
            this.lblDefineDefaultValue.Text = "Define Element Default Value";
            // 
            // uiBtnOk
            // 
            this.uiBtnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uiBtnOk.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnOk.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnOk.DisplayText = "Ok";
            this.uiBtnOk.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnOk.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnOk.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnOk.Image")));
            this.uiBtnOk.IsMouseOver = false;
            this.uiBtnOk.Location = new System.Drawing.Point(20, 640);
            this.uiBtnOk.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnOk.Name = "uiBtnOk";
            this.uiBtnOk.Size = new System.Drawing.Size(60, 60);
            this.uiBtnOk.TabIndex = 21;
            this.uiBtnOk.TabStop = false;
            this.uiBtnOk.Text = "Ok";
            this.uiBtnOk.Click += new System.EventHandler(this.uiBtnOk_Click);
            // 
            // uiBtnCancel
            // 
            this.uiBtnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.uiBtnCancel.BackColor = System.Drawing.Color.Transparent;
            this.uiBtnCancel.Cursor = System.Windows.Forms.Cursors.Arrow;
            this.uiBtnCancel.DisplayText = "Cancel";
            this.uiBtnCancel.DisplayTextBrush = System.Drawing.Color.White;
            this.uiBtnCancel.Font = new System.Drawing.Font("Segoe UI", 8F);
            this.uiBtnCancel.Image = ((System.Drawing.Image)(resources.GetObject("uiBtnCancel.Image")));
            this.uiBtnCancel.IsMouseOver = false;
            this.uiBtnCancel.Location = new System.Drawing.Point(80, 640);
            this.uiBtnCancel.Margin = new System.Windows.Forms.Padding(8, 6, 8, 6);
            this.uiBtnCancel.Name = "uiBtnCancel";
            this.uiBtnCancel.Size = new System.Drawing.Size(60, 60);
            this.uiBtnCancel.TabIndex = 22;
            this.uiBtnCancel.TabStop = false;
            this.uiBtnCancel.Text = "Cancel";
            this.uiBtnCancel.Click += new System.EventHandler(this.uiBtnCancel_Click);
            // 
            // lblElementNameError
            // 
            this.lblElementNameError.BackColor = System.Drawing.Color.Transparent;
            this.lblElementNameError.Font = new System.Drawing.Font("Segoe UI Light", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblElementNameError.ForeColor = System.Drawing.Color.Red;
            this.lblElementNameError.Location = new System.Drawing.Point(16, 204);
            this.lblElementNameError.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblElementNameError.Name = "lblElementNameError";
            this.lblElementNameError.Size = new System.Drawing.Size(567, 36);
            this.lblElementNameError.TabIndex = 26;
            // 
            // dgvDefaultValue
            // 
            this.dgvDefaultValue.AllowUserToResizeRows = false;
            this.dgvDefaultValue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDefaultValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDefaultValue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.enabled,
            this.parameterName,
            this.parameterValue});
            this.dgvDefaultValue.DataBindings.Add(new System.Windows.Forms.Binding("DataSource", this, "ElementValueDT", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dgvDefaultValue.Location = new System.Drawing.Point(21, 345);
            this.dgvDefaultValue.Name = "dgvDefaultValue";
            this.dgvDefaultValue.RowHeadersWidth = 51;
            this.dgvDefaultValue.RowTemplate.Height = 24;
            this.dgvDefaultValue.Size = new System.Drawing.Size(563, 283);
            this.dgvDefaultValue.TabIndex = 28;
            // 
            // enabled
            // 
            this.enabled.DataPropertyName = "Enabled";
            this.enabled.FillWeight = 30F;
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
            this.parameterName.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.parameterName.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // parameterValue
            // 
            this.parameterValue.DataPropertyName = "Parameter Value";
            this.parameterValue.HeaderText = "Parameter Value";
            this.parameterValue.MinimumWidth = 6;
            this.parameterValue.Name = "parameterValue";
            this.parameterValue.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.parameterValue.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // frmAddElement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(609, 716);
            this.Controls.Add(this.dgvDefaultValue);
            this.Controls.Add(this.lblElementNameError);
            this.Controls.Add(this.uiBtnOk);
            this.Controls.Add(this.uiBtnCancel);
            this.Controls.Add(this.lblDefineDefaultValueDescriptor);
            this.Controls.Add(this.lblDefineDefaultValue);
            this.Controls.Add(this.lblDefineNameDescription);
            this.Controls.Add(this.txtElementName);
            this.Controls.Add(this.lblDefineName);
            this.Controls.Add(this.lblHeader);
            this.Icon = taskt.Core.Properties.Resources.openbots_ico;
            this.Margin = new System.Windows.Forms.Padding(5);
            this.MinimumSize = new System.Drawing.Size(627, 630);
            this.Name = "frmAddElement";
            this.Text = "Add Element";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.frmAddElement_Load);
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnOk)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.uiBtnCancel)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDefaultValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblDefineName;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Label lblDefineNameDescription;
        private System.Windows.Forms.Label lblDefineDefaultValueDescriptor;
        private System.Windows.Forms.Label lblDefineDefaultValue;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnOk;
        private OpenBots.Core.UI.Controls.UIPictureButton uiBtnCancel;
        public System.Windows.Forms.TextBox txtElementName;
        private System.Windows.Forms.Label lblElementNameError;
        private System.Windows.Forms.DataGridView dgvDefaultValue;
        private DataGridViewCheckBoxColumn enabled;
        private DataGridViewTextBoxColumn parameterName;
        private DataGridViewTextBoxColumn parameterValue;
    }
}