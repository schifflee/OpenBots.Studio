using System;
using System.Data;
using System.Windows.Forms;
using taskt.UI.Forms;

namespace taskt.UI.Supplement_Forms
{
    public partial class frmHTMLElementRecorderSettings : UIForm
    {
        public DataTable ParameterSettingsDT { get; set; }

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
    }
}











