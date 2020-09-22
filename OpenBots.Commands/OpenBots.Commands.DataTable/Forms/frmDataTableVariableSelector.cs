using System;
using System.Windows.Forms;
using OpenBots.Core.UI.Forms;

namespace OpenBots.Commands.DataTable.Forms
{
    public partial class frmDataTableVariableSelector : ThemedForm
    {
        public frmDataTableVariableSelector()
        {
            InitializeComponent();
        }

        private void frmDataTableVariableSelector_Load(object sender, EventArgs e)
        {
        }

        private void uiBtnOk_Click(object sender, EventArgs e)
        {
            if (lstVariables.SelectedItem == null)
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

        private void lstVariables_DoubleClick(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }
    }
}