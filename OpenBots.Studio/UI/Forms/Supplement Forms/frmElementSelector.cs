using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenBots.Core.UI.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmElementSelector : ThemedForm
    {
        private List<string> _elementList;
        private string _txtCommandWatermark = "Type Here to Search";

        public frmElementSelector()
        {
            InitializeComponent();
        }

        private void frmElementSelector_Load(object sender, EventArgs e)
        {
            _elementList = new List<string>();
            _elementList.AddRange(lstElements.Items.Cast<string>().ToList());
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            if (lstElements.SelectedItem == null)
            {
                MessageBox.Show("There are no item(s) selected! Select an item and Ok or select Cancel");
                return;
            }
            DialogResult = DialogResult.OK;
        }

        private void uiBtnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void lstElements_DoubleClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void txtElementSearch_TextChanged(object sender, EventArgs e)
        {
            string search = txtElementSearch.Text;

            if (search == _txtCommandWatermark)
                return;

            if (string.IsNullOrEmpty(search))
            {
                lstElements.Items.Clear();
                lstElements.Items.AddRange(_elementList.ToArray());
            }

            var items = (from a in _elementList
                         where a.ToLower().Contains(search.ToLower())
                         select a).ToArray();

            lstElements.Items.Clear();
            lstElements.Items.AddRange(items);
        }

        private void txtElementSearch_Enter(object sender, EventArgs e)
        {
            if (txtElementSearch.Text == _txtCommandWatermark)
            {
                txtElementSearch.Text = "";
                txtElementSearch.ForeColor = Color.Black;
            }
        }

        private void txtElementSearch_Leave(object sender, EventArgs e)
        {
            if (txtElementSearch.Text == "")
            {
                txtElementSearch.Text = _txtCommandWatermark;
                txtElementSearch.ForeColor = Color.LightGray;
            }
        }
    }
}