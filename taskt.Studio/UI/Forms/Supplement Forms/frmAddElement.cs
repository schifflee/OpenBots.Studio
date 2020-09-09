using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
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

        private int _indexOfItemUnderMouseToDrag = -1;
        private int _indexOfItemUnderMouseToDrop = -1;
        private Rectangle _dragBoxFromMouseDown = Rectangle.Empty;

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

            dgvDefaultValue.EndEdit();
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void dgvDefaultValue_MouseDown(object sender, MouseEventArgs e)
        {
            var hitTest = dgvDefaultValue.HitTest(e.X, e.Y);
            if (hitTest.Type != DataGridViewHitTestType.Cell)
                return;

            _indexOfItemUnderMouseToDrag = hitTest.RowIndex;
            if (_indexOfItemUnderMouseToDrag > -1)
            {
                Size dragSize = SystemInformation.DragSize;
                _dragBoxFromMouseDown = new Rectangle(new Point(e.X - (dragSize.Width / 2), e.Y - (dragSize.Height / 2)), dragSize);
            }
            else
                _dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void dgvDefaultValue_MouseUp(object sender, MouseEventArgs e)
        {
            _dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void dgvDefaultValue_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;
            if (_dragBoxFromMouseDown == Rectangle.Empty || _dragBoxFromMouseDown.Contains(e.X, e.Y))
                return;
            if (_indexOfItemUnderMouseToDrag < 0)
                return;            

            var row = dgvDefaultValue.Rows[_indexOfItemUnderMouseToDrag];
            dgvDefaultValue.DoDragDrop(row, DragDropEffects.All);

            //Clear
            dgvDefaultValue.ClearSelection();
            //Set
            if (_indexOfItemUnderMouseToDrop > -1)
                dgvDefaultValue.Rows[_indexOfItemUnderMouseToDrop].Selected = true;
        }

        private void dgvDefaultValue_DragOver(object sender, DragEventArgs e)
        {
            Point p = dgvDefaultValue.PointToClient(new Point(e.X, e.Y));
            var hitTest = dgvDefaultValue.HitTest(p.X, p.Y);
            if (hitTest.Type != DataGridViewHitTestType.Cell || hitTest.RowIndex == _indexOfItemUnderMouseToDrag)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            e.Effect = DragDropEffects.Move;
        }

        private void dgvDefaultValue_DragDrop(object sender, DragEventArgs e)
        {
            Point p = dgvDefaultValue.PointToClient(new Point(e.X, e.Y));
            var hitTest = dgvDefaultValue.HitTest(p.X, p.Y);
            if (hitTest.Type != DataGridViewHitTestType.Cell || hitTest.RowIndex == _indexOfItemUnderMouseToDrag + 1)
                return;

            _indexOfItemUnderMouseToDrop = hitTest.RowIndex;

            var tempRow = ElementValueDT.NewRow();
            tempRow.ItemArray = ElementValueDT.Rows[_indexOfItemUnderMouseToDrag].ItemArray;
            ElementValueDT.Rows.RemoveAt(_indexOfItemUnderMouseToDrag);

            if (_indexOfItemUnderMouseToDrag < _indexOfItemUnderMouseToDrop)
                _indexOfItemUnderMouseToDrop--;

            ElementValueDT.Rows.InsertAt(tempRow, _indexOfItemUnderMouseToDrop);
        }
    }
}
