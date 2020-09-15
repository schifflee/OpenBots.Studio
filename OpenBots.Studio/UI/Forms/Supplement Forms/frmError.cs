using System;
using System.Windows.Forms;
using OpenBots.Core.UI.Forms;

namespace OpenBots.UI.Forms.Supplement_Forms
{
    public partial class frmError : ThemedForm
    {
        public frmError(string errorMessage)
        {
            InitializeComponent();
            lblErrorMessage.Text = errorMessage;
        }

        private void uiBtnIgnore_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
        }

        private void uiBtnContinue_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Abort;
        }

        private void uiBtnBreak_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
        }

        private void uiBtnCopyError_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lblErrorMessage.Text);
        }       
    }
}
