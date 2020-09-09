using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using taskt.UI.Forms;

namespace taskt.UI.Supplement_Forms
{
    public partial class frmHTMLElementRecorderSettings : UIForm
    {
        public DataTable ParameterSettingsDT { get; set; }

        private int _indexOfItemUnderMouseToDrag = -1;
        private int _indexOfItemUnderMouseToDrop = -1;
        private Rectangle _dragBoxFromMouseDown = Rectangle.Empty;
        public frmHTMLElementRecorderSettings()
        {
            InitializeComponent();
            ParameterSettingsDT = new DataTable();
            ParameterSettingsDT.Columns.Add("Enabled");
            ParameterSettingsDT.Columns.Add("Parameter Name");
            ParameterSettingsDT.TableName = DateTime.Now.ToString("ParameterSettingsDT" + DateTime.Now.ToString("MMddyy.hhmmss"));

            ParameterSettingsDT.Rows.Add(true, "XPath");
            ParameterSettingsDT.Rows.Add(false, "ID");
            ParameterSettingsDT.Rows.Add(false, "Name");
            ParameterSettingsDT.Rows.Add(false, "Tag Name");
            ParameterSettingsDT.Rows.Add(false, "Class Name");
            ParameterSettingsDT.Rows.Add(false, "Link Text");
            ParameterSettingsDT.Rows.Add(false, "CSS Selector");
        }

        private void btnOkay_Click(object sender, EventArgs e)
        {
            dgvParameterSettings.EndEdit();
            DialogResult = DialogResult.OK;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void dgvParameterSettings_MouseDown(object sender, MouseEventArgs e)
        {
            var hitTest = dgvParameterSettings.HitTest(e.X, e.Y);
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

        private void dgvParameterSettings_MouseUp(object sender, MouseEventArgs e)
        {
            _dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void dgvParameterSettings_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;
            if (_dragBoxFromMouseDown == Rectangle.Empty || _dragBoxFromMouseDown.Contains(e.X, e.Y))
                return;
            if (_indexOfItemUnderMouseToDrag < 0)
                return;

            var row = dgvParameterSettings.Rows[_indexOfItemUnderMouseToDrag];
            dgvParameterSettings.DoDragDrop(row, DragDropEffects.All);

            //Clear
            dgvParameterSettings.ClearSelection();
            //Set
            if (_indexOfItemUnderMouseToDrop > -1)
                dgvParameterSettings.Rows[_indexOfItemUnderMouseToDrop].Selected = true;
        }

        private void dgvParameterSettings_DragOver(object sender, DragEventArgs e)
        {
            Point p = dgvParameterSettings.PointToClient(new Point(e.X, e.Y));
            var hitTest = dgvParameterSettings.HitTest(p.X, p.Y);
            if (hitTest.Type != DataGridViewHitTestType.Cell || hitTest.RowIndex == _indexOfItemUnderMouseToDrag)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            e.Effect = DragDropEffects.Move;
        }

        private void dgvParameterSettings_DragDrop(object sender, DragEventArgs e)
        {
            Point p = dgvParameterSettings.PointToClient(new Point(e.X, e.Y));
            var hitTest = dgvParameterSettings.HitTest(p.X, p.Y);
            if (hitTest.Type != DataGridViewHitTestType.Cell || hitTest.RowIndex == _indexOfItemUnderMouseToDrag + 1)
                return;

            _indexOfItemUnderMouseToDrop = hitTest.RowIndex;

            var tempRow = ParameterSettingsDT.NewRow();
            tempRow.ItemArray = ParameterSettingsDT.Rows[_indexOfItemUnderMouseToDrag].ItemArray;
            ParameterSettingsDT.Rows.RemoveAt(_indexOfItemUnderMouseToDrag);

            if (_indexOfItemUnderMouseToDrag < _indexOfItemUnderMouseToDrop)
                _indexOfItemUnderMouseToDrop--;

            ParameterSettingsDT.Rows.InsertAt(tempRow, _indexOfItemUnderMouseToDrop);
        }
    }
}











