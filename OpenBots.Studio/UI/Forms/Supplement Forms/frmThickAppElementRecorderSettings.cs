using System;
using System.Data;
using System.Windows.Forms;
using OpenBots.UI.Forms;

namespace OpenBots.UI.Supplement_Forms
{
    public partial class frmThickAppElementRecorderSettings : UIForm
    {
        public DataTable ParameterSettingsDT { get; set; }


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
    }
}