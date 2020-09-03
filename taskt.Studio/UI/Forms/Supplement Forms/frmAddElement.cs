using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using taskt.Core.Script;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmAddElement : ThemedForm
    {
        public Dictionary<ScriptElementType, object> ElementValueDict { get; set; }
        public List<ScriptElement> ScriptElements { get; set; }
        private bool _isEditMode;
        private string _editingVariableName;

        public frmAddElement()
        {
            InitializeComponent();
            cbxElementType.DataSource = Enum.GetValues(typeof(ScriptElementType)).Descriptions(); 
        }

        public frmAddElement(string elementName, ScriptElementType elementType, string elementValue)
        {
            InitializeComponent();
            cbxElementType.DataSource = Enum.GetValues(typeof(ScriptElementType)).Descriptions();
            Text = "edit element";
            lblHeader.Text = "edit element";
            txtElementName.Text = elementName;
            cbxElementType.SelectedIndex = cbxElementType.Items.IndexOf(elementType.Description());

            cbxDefaultValue.Visible = false;
            txtDefaultValue.Visible = true;
            txtDefaultValue.Text = elementValue;

            _isEditMode = true;
            _editingVariableName = elementName.Replace("<", "").Replace(">", "");
        }

        private void frmAddElement_Load(object sender, EventArgs e)
        {
            cbxElementType_SelectedIndexChanged(null, null);
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

        private void cbxElementType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ElementValueDict != null)
            {
                ComboBox typeBox = cbxElementType;
                ScriptElementType selectedType = (ScriptElementType)Enum.Parse(typeof(ScriptElementType), 
                                                  typeBox.SelectedItem.ToString().Replace(" ",""));
                string elementValue;
                cbxDefaultValue.Items.Clear();

                if (selectedType == ScriptElementType.CSSSelector)
                {
                    txtDefaultValue.Visible = false;
                    cbxDefaultValue.Visible = true;
                    
                    List<string> cssSelectors = (List<string>)ElementValueDict[selectedType];
                    cbxDefaultValue.Items.AddRange(cssSelectors.ToArray());

                    elementValue = cssSelectors.FirstOrDefault();
                    cbxDefaultValue.Text = elementValue;
                }
                else
                {
                    cbxDefaultValue.Visible = false;
                    txtDefaultValue.Visible = true;
                    elementValue = ElementValueDict[selectedType] == null ? "" : 
                                   ElementValueDict[selectedType].ToString();
                    txtDefaultValue.Text = elementValue;
                }                   
            }
        }

        private void cbxDefaultValue_MouseClick(object sender, MouseEventArgs e)
        {
            ComboBox clickedComboBox = (ComboBox)sender;
            clickedComboBox.DroppedDown = true;
        }
    }
}
