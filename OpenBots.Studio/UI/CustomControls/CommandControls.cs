using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using OpenBots.Commands;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Common;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Properties;
using OpenBots.Core.Settings;
using OpenBots.Core.UI.Controls;
using OpenBots.Core.Utilities.CommandUtilities;
using OpenBots.UI.CustomControls.CustomUIControls;
using OpenBots.UI.Forms;
using OpenBots.UI.Forms.Supplement_Forms;
using Group = OpenBots.Core.Attributes.ClassAttributes.Group;

namespace OpenBots.UI.CustomControls
{
    public class CommandControls : ICommandControls
    {
        private frmCommandEditor _currentEditor;

        public CommandControls()
        {

        }
        public CommandControls(frmCommandEditor editor)
        {
            _currentEditor = editor;
        }

        public List<Control> CreateDefaultInputGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor, int height = 30, int width = 300)
        {
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var input = CreateDefaultInputFor(parameterName, parent, height, width);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { input }, (frmCommandEditor)editor);

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(input);

            return controlList;
        }

        public List<Control> CreateDefaultPasswordInputGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor)
        {
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var passwordInput = CreateDefaultInputFor(parameterName, parent);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { passwordInput }, (frmCommandEditor)editor);

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(passwordInput);

            ((TextBox)passwordInput).PasswordChar = '*';

            return controlList;
        }

        public List<Control> CreateDefaultOutputGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor)
        {
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var variableNameControl = AddVariableNames(CreateStandardComboboxFor(parameterName, parent), editor);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { variableNameControl }, editor);

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(variableNameControl);
            return controlList;
        }

        public List<Control> CreateDefaultDropdownGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor)
        {
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var input = CreateDropdownFor(parameterName, parent);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { input }, (frmCommandEditor)editor);

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(input);

            return controlList;
        }

        public List<Control> CreateDataGridViewGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor)
        {
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var gridview = CreateDataGridView(parent, parameterName);
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { gridview }, (frmCommandEditor)editor);

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(gridview);

            return controlList;
        }

        public List<Control> CreateDefaultWindowControlGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor)
        {
            var controlList = new List<Control>();
            var label = CreateDefaultLabelFor(parameterName, parent);
            var windowNameControl = AddWindowNames(CreateStandardComboboxFor(parameterName, parent));
            var helpers = CreateUIHelpersFor(parameterName, parent, new Control[] { windowNameControl }, (frmCommandEditor)editor);

            controlList.Add(label);
            controlList.AddRange(helpers);
            controlList.Add(windowNameControl);

            return controlList;
        }

        public Control CreateDefaultLabelFor(string parameterName, ScriptCommand parent)
        {
            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();

            var propertyAttributesAssigned = variableProperties.GetCustomAttributes(typeof(PropertyDescription), true);

            Label inputLabel = new Label();
            if (propertyAttributesAssigned.Length > 0)
            {
                var attribute = (PropertyDescription)propertyAttributesAssigned[0];
                inputLabel.Text = attribute.Description;
            }
            else
            {
                inputLabel.Text = parameterName;
            }

            inputLabel.AutoSize = true;
            inputLabel.Font = new Font("Segoe UI Light", 12);
            inputLabel.ForeColor = Color.White;
            inputLabel.Name = "lbl_" + parameterName;
            CreateDefaultToolTipFor(parameterName, parent, inputLabel);
            return inputLabel;
        }

        public void CreateDefaultToolTipFor(string parameterName, ScriptCommand parent, Control label)
        {
            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();
            var inputSpecificationAttributesAssigned = variableProperties.GetCustomAttributes(typeof(InputSpecification), true);
            var sampleUsageAttributesAssigned = variableProperties.GetCustomAttributes(typeof(SampleUsage), true);
            var remarksAttributesAssigned = variableProperties.GetCustomAttributes(typeof(Remarks), true);

            string toolTipText = "";
            if (inputSpecificationAttributesAssigned.Length > 0)
            {
                var attribute = (InputSpecification)inputSpecificationAttributesAssigned[0];
                toolTipText = attribute.Specification;
            }
            if (sampleUsageAttributesAssigned.Length > 0)
            {
                var attribute = (SampleUsage)sampleUsageAttributesAssigned[0];
                if (attribute.Usage.Length > 0)
                    toolTipText += "\nSample: " + attribute.Usage;
            }
            if (remarksAttributesAssigned.Length > 0)
            {
                var attribute = (Remarks)remarksAttributesAssigned[0];
                if (attribute.Remark.Length > 0)
                    toolTipText += "\n" + attribute.Remark;
            }

            ToolTip inputToolTip = new ToolTip();
            inputToolTip.ToolTipIcon = ToolTipIcon.Info;
            inputToolTip.IsBalloon = true;
            inputToolTip.ShowAlways = true;
            inputToolTip.ToolTipTitle = label.Text;
            inputToolTip.AutoPopDelay = 15000;
            inputToolTip.SetToolTip(label, toolTipText);
        }

        public Control CreateDefaultInputFor(string parameterName, ScriptCommand parent, int height = 30, int width = 300)
        {
            var inputBox = new TextBox();
            inputBox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            inputBox.DataBindings.Add("Text", parent, parameterName, false, DataSourceUpdateMode.OnPropertyChanged);
            inputBox.Height = height;
            inputBox.Width = width;

            if (height > 30)
            {
                inputBox.Multiline = true;
            }

            inputBox.Name = parameterName;
            inputBox.KeyDown += InputBox_KeyDown;

            if (parameterName == "v_Comment")
                inputBox.Margin = new Padding(0, 0, 0, 20);
            return inputBox;
        }

        private void InputBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Modifiers == Keys.Shift && e.KeyCode == Keys.Enter)
                return;
            else if (e.KeyCode == Keys.Enter)
                _currentEditor.uiBtnAdd_Click(null, null);
        }

        public CheckBox CreateCheckBoxFor(string parameterName, ScriptCommand parent)
        {
            var checkBox = new CheckBox();
            checkBox.DataBindings.Add("Checked", parent, parameterName, false, DataSourceUpdateMode.OnPropertyChanged);
            checkBox.Name = parameterName;
            checkBox.AutoSize = true;
            checkBox.Size = new Size(20, 20);
            checkBox.UseVisualStyleBackColor = true;
            checkBox.Margin = new Padding(0, 8, 0, 0);
            return checkBox;
        }

        public Control CreateDropdownFor(string parameterName, ScriptCommand parent)
        {
            var dropdownBox = new ComboBox();
            dropdownBox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            dropdownBox.DataBindings.Add("Text", parent, parameterName, false, DataSourceUpdateMode.OnPropertyChanged);
            dropdownBox.Height = 30;
            dropdownBox.Width = 300;
            dropdownBox.Name = parameterName;

            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();
            var propertyAttributesAssigned = variableProperties.GetCustomAttributes(typeof(PropertyUISelectionOption), true);

            foreach (PropertyUISelectionOption option in propertyAttributesAssigned)
            {
                dropdownBox.Items.Add(option.UIOption);
            }

            dropdownBox.Click += DropdownBox_Click;
            dropdownBox.KeyDown += DropdownBox_KeyDown;
            dropdownBox.KeyPress += DropdownBox_KeyPress;

            return dropdownBox;
        }

        private void DropdownBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar != (char)Keys.Return)
                e.Handled = true;
        }

        private void DropdownBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode != Keys.Down && e.KeyCode != Keys.Up && e.KeyCode != Keys.Enter)
                e.Handled = true;
        }

        private void DropdownBox_Click(object sender, EventArgs e)
        {
            ComboBox clickedDropdownBox = (ComboBox)sender;
            clickedDropdownBox.DroppedDown = true;
        }

        public ComboBox CreateStandardComboboxFor(string parameterName, ScriptCommand parent)
        {
            var standardComboBox = new ComboBox();
            standardComboBox.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            standardComboBox.DataBindings.Add("Text", parent, parameterName, false, DataSourceUpdateMode.OnPropertyChanged);
            standardComboBox.Height = 30;
            standardComboBox.Width = 300;
            standardComboBox.Name = parameterName;
            standardComboBox.Click += StandardComboBox_Click;

            return standardComboBox;
        }

        private void StandardComboBox_Click(object sender, EventArgs e)
        {
            ComboBox clickedStandardComboBox = (ComboBox)sender;
            clickedStandardComboBox.DroppedDown = true;
        }

        public List<Control> CreateUIHelpersFor(string parameterName, ScriptCommand parent, Control[] targetControls,
            IfrmCommandEditor editor)
        {
            var variableProperties = parent.GetType().GetProperties().Where(f => f.Name == parameterName).FirstOrDefault();
            var propertyUIHelpers = variableProperties.GetCustomAttributes(typeof(PropertyUIHelper), true);
            var controlList = new List<Control>();

            if (propertyUIHelpers.Count() == 0)
            {
                return controlList;
            }

            foreach (PropertyUIHelper attrib in propertyUIHelpers)
            {
                CommandItemControl helperControl = new CommandItemControl();
                helperControl.Padding = new Padding(10, 0, 0, 0);
                helperControl.ForeColor = Color.AliceBlue;
                helperControl.Font = new Font("Segoe UI Semilight", 10);
                helperControl.Name = parameterName + "_helper";
                helperControl.Tag = targetControls.FirstOrDefault();
                helperControl.HelperType = attrib.AdditionalHelper;

                switch (attrib.AdditionalHelper)
                {
                    case UIAdditionalHelperType.ShowVariableHelper:
                        //show variable selector
                        helperControl.CommandImage = Resources.command_parse;
                        helperControl.CommandDisplay = "Insert Variable";
                        helperControl.Click += (sender, e) => ShowVariableSelector(sender, e);
                        break;

                    case UIAdditionalHelperType.ShowElementHelper:
                        //show element selector
                        helperControl.CommandImage = Resources.command_element;
                        helperControl.CommandDisplay = "Insert Element";
                        helperControl.Click += (sender, e) => ShowElementSelector(sender, e);
                        break;

                    case UIAdditionalHelperType.ShowFileSelectionHelper:
                        //show file selector
                        helperControl.CommandImage = Resources.command_files;
                        helperControl.CommandDisplay = "Select a File";
                        helperControl.Click += (sender, e) => ShowFileSelector(sender, e, (frmCommandEditor)editor);
                        break;

                    case UIAdditionalHelperType.ShowFolderSelectionHelper:
                        //show file selector
                        helperControl.CommandImage = Resources.command_folders;
                        helperControl.CommandDisplay = "Select a Folder";
                        helperControl.Click += (sender, e) => ShowFolderSelector(sender, e, (frmCommandEditor)editor);
                        break;

                    case UIAdditionalHelperType.ShowImageCaptureHelper:
                        //show file selector
                        helperControl.CommandImage = Resources.command_camera;
                        helperControl.CommandDisplay = "Capture Reference Image";
                        helperControl.Click += (sender, e) => ShowImageCapture(sender, e);

                        CommandItemControl testRun = new CommandItemControl();
                        testRun.Padding = new Padding(10, 0, 0, 0);
                        testRun.ForeColor = Color.AliceBlue;

                        testRun.CommandImage = Resources.command_camera;
                        testRun.CommandDisplay = "Run Image Recognition Test";
                        testRun.ForeColor = Color.AliceBlue;
                        testRun.Font = new Font("Segoe UI Semilight", 10);
                        testRun.Tag = targetControls.FirstOrDefault();
                        testRun.Click += (sender, e) => RunImageCapture(sender, e);
                        controlList.Add(testRun);
                        break;

                    case UIAdditionalHelperType.ShowCodeBuilder:
                        //show variable selector
                        helperControl.CommandImage = Resources.command_script;
                        helperControl.CommandDisplay = "Code Builder";
                        helperControl.Click += (sender, e) => ShowCodeBuilder(sender, e, (frmCommandEditor)editor);
                        break;

                    case UIAdditionalHelperType.ShowMouseCaptureHelper:
                        helperControl.CommandImage = Resources.command_input;
                        helperControl.CommandDisplay = "Capture Mouse Position";
                        helperControl.ForeColor = Color.AliceBlue;
                        helperControl.Click += (sender, e) => ShowMouseCaptureForm(sender, e, (frmCommandEditor)editor);
                        break;
                    case UIAdditionalHelperType.ShowElementRecorder:
                        //show variable selector
                        helperControl.CommandImage = Resources.command_camera;
                        helperControl.CommandDisplay = "Element Recorder";
                        helperControl.Click += (sender, e) => ShowElementRecorder(sender, e, (frmCommandEditor)editor);
                        break;
                    case UIAdditionalHelperType.GenerateDLLParameters:
                        //show variable selector
                        helperControl.CommandImage = Resources.command_run_code;
                        helperControl.CommandDisplay = "Generate Parameters";
                        helperControl.Click += (sender, e) => GenerateDLLParameters(sender, e);
                        break;
                    case UIAdditionalHelperType.ShowDLLExplorer:
                        //show variable selector
                        helperControl.CommandImage = Resources.command_run_code;
                        helperControl.CommandDisplay = "Launch DLL Explorer";
                        helperControl.Click += (sender, e) => ShowDLLExplorer(sender, e);
                        break;
                    case UIAdditionalHelperType.AddInputParameter:
                        //show variable selector
                        helperControl.CommandImage = Resources.command_run_code;
                        helperControl.CommandDisplay = "Add Input Parameter";
                        helperControl.Click += (sender, e) => AddInputParameter(sender, e, (frmCommandEditor)editor);
                        break;
                    case UIAdditionalHelperType.ShowHTMLBuilder:
                        helperControl.CommandImage = Resources.command_web;
                        helperControl.CommandDisplay = "Launch HTML Builder";
                        helperControl.Click += (sender, e) => ShowHTMLBuilder(sender, e, (frmCommandEditor)editor);
                        break;
                    case UIAdditionalHelperType.ShowIfBuilder:
                        //show variable selector
                        helperControl.CommandImage = Resources.command_begin_if;
                        helperControl.CommandDisplay = "Add New If Statement";
                        break;
                    case UIAdditionalHelperType.ShowLoopBuilder:
                        //show variable selector
                        helperControl.CommandImage = Resources.command_startloop;
                        helperControl.CommandDisplay = "Add New Loop Statement";
                        break;
                    case UIAdditionalHelperType.ShowEncryptionHelper:
                        //show variable selector
                        helperControl.CommandImage = Resources.command_password;
                        helperControl.CommandDisplay = "Encrypt Text";
                        helperControl.Click += (sender, e) => EncryptText(sender, e, (frmCommandEditor)editor);
                        break;

                        //default:
                        //    MessageBox.Show("Command Helper does not exist for: " + attrib.additionalHelper.ToString());
                        //    break;
                }

                controlList.Add(helperControl);
            }

            return controlList;
        }

        public DataGridView CreateDataGridView(object sourceCommand, string dataSourceName)
        {
            var gridView = new DataGridView();
            gridView.AllowUserToAddRows = true;
            gridView.AllowUserToDeleteRows = true;
            gridView.Size = new Size(400, 250);
            gridView.ColumnHeadersHeight = 30;
            gridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            gridView.DataBindings.Add("DataSource", sourceCommand, dataSourceName, false, DataSourceUpdateMode.OnPropertyChanged);
            gridView.AllowUserToResizeRows = false;
            return gridView;
        }

        private void ShowCodeBuilder(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            //get textbox text
            CommandItemControl commandItem = (CommandItemControl)sender;
            TextBox targetTextbox = (TextBox)commandItem.Tag;

            frmCodeBuilder codeBuilder = new frmCodeBuilder(targetTextbox.Text);

            if (codeBuilder.ShowDialog() == DialogResult.OK)
            {
                targetTextbox.Text = codeBuilder.rtbCode.Text;
            }
        }

        private void ShowMouseCaptureForm(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            frmShowCursorPosition frmShowCursorPos = new frmShowCursorPosition();

            //if user made a successful selection
            if (frmShowCursorPos.ShowDialog() == DialogResult.OK)
            {
                //add selected variables to associated control text
                ((frmCommandEditor)editor).flw_InputVariables.Controls["v_XMousePosition"].Text = frmShowCursorPos.XPosition.ToString();
                ((frmCommandEditor)editor).flw_InputVariables.Controls["v_YMousePosition"].Text = frmShowCursorPos.YPosition.ToString();
            }
        }

        public void ShowVariableSelector(object sender, EventArgs e)
        {
            //create variable selector form
            frmVariableSelector newVariableSelector = new frmVariableSelector();

            //get copy of user variables and append system variables, then load to combobox
            var variableList = _currentEditor.ScriptVariables.Select(f => f.VariableName).ToList();
            variableList.AddRange(Common.GenerateSystemVariables().Select(f => f.VariableName));
            newVariableSelector.lstVariables.Items.AddRange(variableList.ToArray());

            //if user pressed "OK"
            if (newVariableSelector.ShowDialog() == DialogResult.OK)
            {
                //ensure that a variable was actually selected
                if (newVariableSelector.lstVariables.SelectedItem == null)
                {
                    //return out as nothing was selected
                    MessageBox.Show("There were no variables selected!");
                    return;
                }

                //grab the referenced input assigned to the 'insert variable' button instance
                CommandItemControl inputBox = (CommandItemControl)sender;
                //currently variable insertion is only available for simply textboxes

                //load settings
                var settings = new ApplicationSettings().GetOrCreateApplicationSettings();

                if (inputBox.Tag is TextBox)
                {
                    TextBox targetTextbox = (TextBox)inputBox.Tag;
                    //concat variable name with brackets [vVariable] as engine searches for the same
                    targetTextbox.Text = targetTextbox.Text + string.Concat("{",
                        newVariableSelector.lstVariables.SelectedItem.ToString(), "}");
                }
                else if (inputBox.Tag is ComboBox)
                {
                    ComboBox targetCombobox = (ComboBox)inputBox.Tag;
                    //concat variable name with brackets [vVariable] as engine searches for the same
                    targetCombobox.Text = targetCombobox.Text + string.Concat("{",
                        newVariableSelector.lstVariables.SelectedItem.ToString(), "}");
                }
                else if (inputBox.Tag is DataGridView)
                {
                    DataGridView targetDGV = (DataGridView)inputBox.Tag;

                    if (targetDGV.SelectedCells.Count == 0)
                    {
                        MessageBox.Show("Please make sure you have selected an action and selected a cell before attempting" +
                            " to insert a variable!", "No Cell Found", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    if (targetDGV.SelectedCells[0].ColumnIndex == 0)
                    {
                        MessageBox.Show("Invalid Cell Selected!", "Invalid Cell Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    targetDGV.SelectedCells[0].Value = targetDGV.SelectedCells[0].Value +
                        string.Concat("{", newVariableSelector.lstVariables.SelectedItem.ToString(), "}");
                }


            }
        }

        public void ShowElementSelector(object sender, EventArgs e)
        {
            //create element selector form
            frmElementSelector newElementSelector = new frmElementSelector();

            //get copy of user element and append system elements, then load to combobox
            var elementList = _currentEditor.ScriptElements.Select(f => f.ElementName).ToList();

            newElementSelector.lstElements.Items.AddRange(elementList.ToArray());

            //if user pressed "OK"
            if (newElementSelector.ShowDialog() == DialogResult.OK)
            {
                //ensure that a element was actually selected
                if (newElementSelector.lstElements.SelectedItem == null)
                {
                    //return out as nothing was selected
                    MessageBox.Show("There were no elements selected!");
                    return;
                }

                //grab the referenced input assigned to the 'insert element' button instance
                CommandItemControl inputBox = (CommandItemControl)sender;

                if (inputBox.Tag is DataGridView)
                {
                    DataGridView targetDGV = (DataGridView)inputBox.Tag;

                    targetDGV.DataSource = _currentEditor.ScriptElements
                        .Where(x => x.ElementName == newElementSelector.lstElements.SelectedItem.ToString().Replace("<", "").Replace(">", ""))
                        .FirstOrDefault().ElementValue;
                }
            }
        }
        private void ShowFileSelector(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                CommandItemControl inputBox = (CommandItemControl)sender;
                //currently variable insertion is only available for simply textboxes
                TextBox targetTextbox = (TextBox)inputBox.Tag;
                //concat variable name with brackets [vVariable] as engine searches for the same
                targetTextbox.Text = ofd.FileName;
            }
        }

        private void ShowFolderSelector(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                CommandItemControl inputBox = (CommandItemControl)sender;
                TextBox targetTextBox = (TextBox)inputBox.Tag;
                targetTextBox.Text = fbd.SelectedPath;
            }
        }

        private void ShowImageCapture(object sender, EventArgs e)
        {
            ApplicationSettings settings = new ApplicationSettings().GetOrCreateApplicationSettings();
            var minimizePreference = settings.ClientSettings.MinimizeToTray;

            if (minimizePreference)
            {
                settings.ClientSettings.MinimizeToTray = false;
                settings.Save(settings);
            }

            HideAllForms();

            var userAcceptance = MessageBox.Show("The image capture process will now begin and display a screenshot of the" +
                " current desktop in a custom full-screen window.  You may stop the capture process at any time by pressing" +
                " the 'ESC' key, or selecting 'Close' at the top left. Simply create the image by clicking once to start" +
                " the rectangle and clicking again to finish. The image will be cropped to the boundary within the red rectangle." +
                " Shall we proceed?", "Image Capture", MessageBoxButtons.YesNo);

            if (userAcceptance == DialogResult.Yes)
            {
                frmImageCapture imageCaptureForm = new frmImageCapture();

                if (imageCaptureForm.ShowDialog() == DialogResult.OK)
                {
                    CommandItemControl inputBox = (CommandItemControl)sender;
                    UIPictureBox targetPictureBox = (UIPictureBox)inputBox.Tag;
                    targetPictureBox.Image = imageCaptureForm.UserSelectedBitmap;
                    var convertedImage = Common.ImageToBase64(imageCaptureForm.UserSelectedBitmap);
                    targetPictureBox.EncodedImage = convertedImage;
                    imageCaptureForm.Close();
                }
            }

            ShowAllForms();

            if (minimizePreference)
            {
                settings.ClientSettings.MinimizeToTray = true;
                settings.Save(settings);
            }
        }

        private void RunImageCapture(object sender, EventArgs e)
        {
            //get input control
            CommandItemControl inputBox = (CommandItemControl)sender;
            UIPictureBox targetPictureBox = (UIPictureBox)inputBox.Tag;
            string imageSource = targetPictureBox.EncodedImage;

            if (string.IsNullOrEmpty(imageSource))
            {
                MessageBox.Show("Please capture an image before attempting to test!");
                return;
            }

            //hide all
            HideAllForms();

            try
            {
                //run image recognition
                SurfaceAutomationCommand surfaceAutomationCommand = new SurfaceAutomationCommand();
                surfaceAutomationCommand.v_ImageCapture = imageSource;
                surfaceAutomationCommand.TestMode = true;
                surfaceAutomationCommand.RunCommand(null);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.ToString());
            }
            //show all forms
            ShowAllForms();
        }

        private void ShowElementRecorder(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            //get command reference
            UIAutomationCommand cmd = (UIAutomationCommand)((frmCommandEditor)editor).SelectedCommand;

            //create recorder
            frmThickAppElementRecorder newElementRecorder = new frmThickAppElementRecorder();
            newElementRecorder.SearchParameters = cmd.v_UIASearchParameters;

            //show form
            newElementRecorder.ShowDialog();

            ComboBox txtWindowName = (ComboBox)((frmCommandEditor)editor).flw_InputVariables.Controls["v_WindowName"];
            txtWindowName.Text = newElementRecorder.cboWindowTitle.Text;

            ((frmCommandEditor)editor).WindowState = FormWindowState.Normal;
            ((frmCommandEditor)editor).BringToFront();
        }

        private void GenerateDLLParameters(object sender, EventArgs e)
        {
            ExecuteDLLCommand cmd = (ExecuteDLLCommand)_currentEditor.SelectedCommand;

            var filePath = _currentEditor.flw_InputVariables.Controls["v_FilePath"].Text;
            var className = _currentEditor.flw_InputVariables.Controls["v_ClassName"].Text;
            var methodName = _currentEditor.flw_InputVariables.Controls["v_MethodName"].Text;
            DataGridView parameterBox = (DataGridView)_currentEditor.flw_InputVariables.Controls["v_MethodParameters"];

            //clear all rows
            cmd.v_MethodParameters.Rows.Clear();

            //Load Assembly
            try
            {
                Assembly requiredAssembly = Assembly.LoadFrom(filePath);

                //get type
                Type t = requiredAssembly.GetType(className);

                //verify type was found
                if (t == null)
                {
                    MessageBox.Show("The class '" + className + "' was not found in assembly loaded at '" + filePath + "'",
                        "Class Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //get method
                MethodInfo m = t.GetMethod(methodName);

                //verify method found
                if (m == null)
                {
                    MessageBox.Show("The method '" + methodName + "' was not found in assembly loaded at '" + filePath + "'",
                        "Method Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    return;
                }

                //get parameters
                var reqdParams = m.GetParameters();

                if (reqdParams.Length > 0)
                {
                    cmd.v_MethodParameters.Rows.Clear();
                    foreach (var param in reqdParams)
                    {
                        cmd.v_MethodParameters.Rows.Add(param.Name, "");
                    }
                }
                else
                {
                    MessageBox.Show("There are no parameters required for this method!", "No Parameters Required",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("There was an error generating the parameters: " + ex.ToString());
            }
        }

        private void ShowDLLExplorer(object sender, EventArgs e)
        {
            //create form
            frmDLLExplorer dllExplorer = new frmDLLExplorer();

            //show dialog
            if (dllExplorer.ShowDialog() == DialogResult.OK)
            {
                //user accepted the selections
                //declare command
                ExecuteDLLCommand cmd = (ExecuteDLLCommand)_currentEditor.SelectedCommand;

                //add file name
                if (!string.IsNullOrEmpty(dllExplorer.FileName))
                {
                    _currentEditor.flw_InputVariables.Controls["v_FilePath"].Text = dllExplorer.FileName;
                }

                //add class name
                if (dllExplorer.lstClasses.SelectedItem != null)
                {
                    _currentEditor.flw_InputVariables.Controls["v_ClassName"].Text = dllExplorer.lstClasses.SelectedItem.ToString();
                }

                //add method name
                if (dllExplorer.lstMethods.SelectedItem != null)
                {
                    _currentEditor.flw_InputVariables.Controls["v_MethodName"].Text = dllExplorer.lstMethods.SelectedItem.ToString();
                }

                cmd.v_MethodParameters.Rows.Clear();

                //add parameters
                if ((dllExplorer.lstParameters.Items.Count > 0) &&
                    (dllExplorer.lstParameters.Items[0].ToString() != "This method requires no parameters!"))
                {
                    foreach (var param in dllExplorer.SelectedParameters)
                    {
                        cmd.v_MethodParameters.Rows.Add(param, "");
                    }
                }
            }
        }

        private void AddInputParameter(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            DataGridView inputControl = (DataGridView)_currentEditor.flw_InputVariables.Controls["v_UserInputConfig"];
            var inputTable = (DataTable)inputControl.DataSource;
            var newRow = inputTable.NewRow();
            newRow["Size"] = "500,100";
            inputTable.Rows.Add(newRow);
        }

        private void ShowHTMLBuilder(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            var htmlForm = new frmHTMLBuilder();

            TextBox inputControl = (TextBox)((frmCommandEditor)editor).flw_InputVariables.Controls["v_InputHTML"];
            htmlForm.rtbHTML.Text = ((frmCommandEditor)editor).flw_InputVariables.Controls["v_InputHTML"].Text;

            if (htmlForm.ShowDialog() == DialogResult.OK)
            {
                inputControl.Text = htmlForm.rtbHTML.Text;
            }
        }

        private void EncryptText(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            CommandItemControl inputBox = (CommandItemControl)sender;
            TextBox targetTextbox = (TextBox)inputBox.Tag;

            if (string.IsNullOrEmpty(targetTextbox.Text))
                return;

            var encrypted = EncryptionServices.EncryptString(targetTextbox.Text, "OPENBOTS");
            targetTextbox.Text = encrypted;

            ComboBox comboBoxControl = (ComboBox)((frmCommandEditor)editor).flw_InputVariables.Controls["v_EncryptionOption"];
            comboBoxControl.Text = "Encrypted";
        }

        public void ShowAllForms()
        {
            foreach (Form form in Application.OpenForms)
                ShowForm(form);

            Thread.Sleep(1000);
        }

        public delegate void ShowFormDelegate(Form form);
        public void ShowForm(Form form)
        {
            if (form.InvokeRequired)
            {
                var d = new ShowFormDelegate(ShowForm);
                form.Invoke(d, new object[] { form });
            }
            else
                form.WindowState = FormWindowState.Normal;
        }

        public void HideAllForms()
        {
            foreach (Form form in Application.OpenForms)
                HideForm(form);

            Thread.Sleep(1000);
        }

        public delegate void HideFormDelegate(Form form);
        public void HideForm(Form form)
        {
            if (form.InvokeRequired)
            {
                var d = new HideFormDelegate(HideForm);
                form.Invoke(d, new object[] { form });
            }
            else
                form.WindowState = FormWindowState.Minimized;
        }

        public ComboBox AddWindowNames(ComboBox cbo)
        {
            if (cbo == null)
                return null;

            cbo.Items.Clear();
            cbo.Items.Add("Current Window");

            Process[] processlist = Process.GetProcesses();

            //pull the main window title for each
            foreach (Process process in processlist)
            {
                if (!String.IsNullOrEmpty(process.MainWindowTitle))
                {
                    //add to the control list of available windows
                    cbo.Items.Add(process.MainWindowTitle);
                }
            }

            return cbo;
        }

        public ComboBox AddVariableNames(ComboBox cbo, IfrmCommandEditor editor)
        {
            if (cbo == null)
                return null;

            if (editor != null)
            {
                cbo.Items.Clear();

                foreach (var variable in ((frmCommandEditor)editor).ScriptVariables)
                    cbo.Items.Add("{" + variable.VariableName + "}");
            }
            return cbo;
        }

        public ComboBox AddElementNames(ComboBox cbo, frmCommandEditor editor)
        {
            if (cbo == null)
                return null;

            if (editor != null)
            {
                cbo.Items.Clear();

                foreach (var element in editor.ScriptElements)
                    cbo.Items.Add("<" + element.ElementName + ">");
            }
            return cbo;
        }
    }
}
