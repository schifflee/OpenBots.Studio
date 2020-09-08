using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using taskt.Core.Script;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmAddElement : ThemedForm
    {
        public List<ScriptElement> ScriptElements { get; set; }
        public DataTable ElementValueDT { get; set; }
        private bool _isEditMode;
        private string _editingVariableName;

        public frmAddElement()
        {
            InitializeComponent();
            ElementValueDT = new DataTable();
            ElementValueDT.Columns.Add("Enabled");
            ElementValueDT.Columns.Add("Parameter Name");
            ElementValueDT.Columns.Add("Parameter Value");
            ElementValueDT.TableName = DateTime.Now.ToString("ElementValueDT" + DateTime.Now.ToString("MMddyy.hhmmss"));

            ElementValueDT.Rows.Add(true, "XPath", "");
            ElementValueDT.Rows.Add(false, "ID", "");
            ElementValueDT.Rows.Add(false, "Name", "");
            ElementValueDT.Rows.Add(false, "Tag Name", "");
            ElementValueDT.Rows.Add(false, "Class Name", "");
            ElementValueDT.Rows.Add(false, "Link Text", "");
            ElementValueDT.Rows.Add(false, "CSS Selector", "");
        }

        public frmAddElement(string elementName, DataTable elementValueDT)
        {
            InitializeComponent();

            Text = "edit element";
            lblHeader.Text = "edit element";
            txtElementName.Text = elementName;
            ElementValueDT = elementValueDT;

            _isEditMode = true;
            _editingVariableName = elementName.Replace("<", "").Replace(">", "");          
        }

        private void frmAddElement_Load(object sender, EventArgs e)
        {
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            txtElementName.Text = txtElementName.Text.Trim();
            if (txtElementName.Text == string.Empty)
            {
                lblElementNameError.Text = "Element Name not provided"; 
                return;
            }

            string newElementName = txtElementName.Text.Replace("<", "").Replace(">", "");
            var existingElement = ScriptElements.Where(var => var.ElementName == newElementName).FirstOrDefault();
            if (existingElement != null)                
            {
                if (!_isEditMode || existingElement.ElementName != _editingVariableName)
                {
                    lblElementNameError.Text = "An Element with this name already exists";
                    return;
                }               
            }

            if (!txtElementName.Text.StartsWith("<") || !txtElementName.Text.EndsWith(">"))
            {
                lblElementNameError.Text = "Element markers '<' and '>' must be included";
                return;
            }

            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }
    }
}
