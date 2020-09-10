using System;
using System.Windows.Forms;
using taskt.Core.UI.Forms;

namespace taskt.UI.Forms.Supplement_Forms
{
    public partial class frmElementSelector : ThemedForm
    {
        public frmElementSelector()
        {
            InitializeComponent();
        }

        private void frmElementSelector_Load(object sender, EventArgs e)
        {
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
    }
}