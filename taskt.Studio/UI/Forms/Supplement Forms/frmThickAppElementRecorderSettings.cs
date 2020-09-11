using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using taskt.UI.Forms;

namespace taskt.UI.Supplement_Forms
{
    public partial class frmThickAppElementRecorderSettings : UIForm
    {
        public DataTable ParameterSettingsDT { get; set; }

        private int _indexOfItemUnderMouseToDrag = -1;
        private int _indexOfItemUnderMouseToDrop = -1;
        private Rectangle _dragBoxFromMouseDown = Rectangle.Empty;

        public frmThickAppElementRecorderSettings()
        {
            InitializeComponent();
            ParameterSettingsDT = new DataTable();
            ParameterSettingsDT.Columns.Add("Enabled");
            ParameterSettingsDT.Columns.Add("Parameter Name");
            ParameterSettingsDT.TableName = DateTime.Now.ToString("ParameterSettingsDT" + DateTime.Now.ToString("MMddyy.hhmmss"));

            ParameterSettingsDT.Rows.Add(false, "AcceleratorKey");
            ParameterSettingsDT.Rows.Add(false, "AccessKey");
            ParameterSettingsDT.Rows.Add(true, "AutomationId");
            ParameterSettingsDT.Rows.Add(false, "ClassName");
            ParameterSettingsDT.Rows.Add(false, "FrameworkId");
            ParameterSettingsDT.Rows.Add(false, "HasKeyboardFocus");
            ParameterSettingsDT.Rows.Add(false, "HelpText");
            ParameterSettingsDT.Rows.Add(false, "IsContentElement");
            ParameterSettingsDT.Rows.Add(false, "IsControlElement");
            ParameterSettingsDT.Rows.Add(false, "IsEnabled");
            ParameterSettingsDT.Rows.Add(false, "IsKeyboardFocusable");
            ParameterSettingsDT.Rows.Add(false, "IsOffscreen");
            ParameterSettingsDT.Rows.Add(false, "IsPassword");
            ParameterSettingsDT.Rows.Add(false, "IsRequiredForForm");
            ParameterSettingsDT.Rows.Add(false, "ItemStatus");
            ParameterSettingsDT.Rows.Add(false, "ItemType");
            ParameterSettingsDT.Rows.Add(false, "LocalizedControlType");
            ParameterSettingsDT.Rows.Add(false, "Name");
            ParameterSettingsDT.Rows.Add(false, "NativeWindowHandle");
            ParameterSettingsDT.Rows.Add(false, "ProcessID");
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











