﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.User32;
using taskt.Core.Utilities.CommandUtilities;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.Properties;
using taskt.UI.CustomControls;
using taskt.UI.Forms.Supplement_Forms;

namespace taskt.Commands
{
    [Serializable]
    [Group("Input Commands")]
    [Description("This Command automates an element in a targeted window.")]
    public class UIAutomationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Window Name")]
        [InputSpecification("Select the name of the window to automate.")]
        [SampleUsage("Untitled - Notepad || Current Window || {vWindow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Element Action")]
        [PropertyUISelectionOption("Click Element")]
        [PropertyUISelectionOption("Set Text")]
        [PropertyUISelectionOption("Set Secure Text")]
        [PropertyUISelectionOption("Get Text")]
        [PropertyUISelectionOption("Clear Element")]
        [PropertyUISelectionOption("Get Value From Element")]
        [PropertyUISelectionOption("Check If Element Exists")]
        [PropertyUISelectionOption("Wait For Element To Exist")]
        [InputSpecification("Select the appropriate corresponding action to take once the element has been located.")]
        [SampleUsage("")]
        [Remarks("Selecting this field changes the parameters required in the following step.")]
        public string v_AutomationType { get; set; }

        [PropertyDescription("Element Search Parameter")]
        [InputSpecification("Use the Element Recorder to generate a listing of potential search parameters.")]
        [SampleUsage("AutomationId || Name")]
        [Remarks("Once you have clicked on a valid window the search parameters will be populated. Select a single parameter to find the element.")]
        public DataTable v_UIASearchParameters { get; set; }

        [PropertyDescription("Action Parameters")]
        [InputSpecification("Action Parameters will be determined based on the action settings selected.")]
        [SampleUsage("data || {vData} || *Variable Name*: {vNewVariable}")]
        [Remarks("Action Parameters range from adding offset coordinates to specifying a variable to apply element text to.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_UIAActionParameters { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private ComboBox _automationTypeControl;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _searchParametersGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _actionParametersGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> _actionParametersControls;

        public UIAutomationCommand()
        {
            CommandName = "UIAutomationCommand";
            SelectionName = "UI Automation";
            CommandEnabled = true;
            CustomRendering = true;

            //set up search parameter table
            v_UIASearchParameters = new DataTable();
            v_UIASearchParameters.Columns.Add("Enabled");
            v_UIASearchParameters.Columns.Add("Parameter Name");
            v_UIASearchParameters.Columns.Add("Parameter Value");
            v_UIASearchParameters.TableName = DateTime.Now.ToString("UIASearchParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

            v_UIAActionParameters = new DataTable();
            v_UIAActionParameters.Columns.Add("Parameter Name");
            v_UIAActionParameters.Columns.Add("Parameter Value");
            v_UIAActionParameters.TableName = DateTime.Now.ToString("UIAActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

            v_WindowName = "Current Window";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            //create variable window name
            var variableWindowName = v_WindowName.ConvertUserVariableToString(engine);
            if (variableWindowName == "Current Window")
                variableWindowName = User32Functions.GetActiveWindowTitle();

            dynamic requiredHandle = null;
            if (v_AutomationType != "Wait For Element To Exist")
                requiredHandle =  SearchForGUIElement(sender, variableWindowName);

            switch (v_AutomationType)
            {
                //determine element click type
                case "Click Element":
                    //if handle was not found
                    if (requiredHandle == null)
                        throw new Exception("Element was not found in window '" + variableWindowName + "'");
                    //create search params
                    var clickType = (from rw in v_UIAActionParameters.AsEnumerable()
                                     where rw.Field<string>("Parameter Name") == "Click Type"
                                     select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    //get x adjust
                    var xAdjust = (from rw in v_UIAActionParameters.AsEnumerable()
                                   where rw.Field<string>("Parameter Name") == "X Adjustment"
                                   select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    //get y adjust
                    var yAdjust = (from rw in v_UIAActionParameters.AsEnumerable()
                                   where rw.Field<string>("Parameter Name") == "Y Adjustment"
                                   select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    //convert potential variable
                    var xAdjustVariable = xAdjust.ConvertUserVariableToString(engine);
                    var yAdjustVariable = yAdjust.ConvertUserVariableToString(engine);

                    //parse to int
                    var xAdjustInt = int.Parse(xAdjustVariable);
                    var yAdjustInt = int.Parse(yAdjustVariable);

                    //get clickable point
                    var newPoint = requiredHandle.GetClickablePoint();

                    //send mousemove command
                    var newMouseMove = new SendMouseMoveCommand
                    {
                        v_XMousePosition = (newPoint.X + xAdjustInt).ToString(),
                        v_YMousePosition = (newPoint.Y + yAdjustInt).ToString(),
                        v_MouseClick = clickType
                    };

                    //run commands
                    newMouseMove.RunCommand(sender);
                    break;
                case "Set Text":
                    string textToSet = (from rw in v_UIAActionParameters.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Text To Set"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault();


                    string clearElement = (from rw in v_UIAActionParameters.AsEnumerable()
                                           where rw.Field<string>("Parameter Name") == "Clear Element Before Setting Text"
                                           select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    string encryptedData = (from rw in v_UIAActionParameters.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Encrypted Text"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault();
                    if (clearElement == null)
                        clearElement = "No";

                    if (encryptedData == "Encrypted")
                        textToSet = EncryptionServices.DecryptString(textToSet, "OPENBOTS");

                    textToSet = textToSet.ConvertUserVariableToString(engine);

                    if (requiredHandle.Current.IsEnabled && requiredHandle.Current.IsKeyboardFocusable)
                    {
                        object valuePattern = null;
                        if (!requiredHandle.TryGetCurrentPattern(ValuePattern.Pattern, out valuePattern))
                        {
                            //The control does not support ValuePattern Using keyboard input
                            // Set focus for input functionality and begin.
                            requiredHandle.SetFocus();

                            // Pause before sending keyboard input.
                            Thread.Sleep(100);

                            if (clearElement.ToLower() == "yes")
                            {
                                // Delete existing content in the control and insert new content.
                                SendKeys.SendWait("^{HOME}");   // Move to start of control
                                SendKeys.SendWait("^+{END}");   // Select everything
                                SendKeys.SendWait("{DEL}");     // Delete selection
                            }
                            SendKeys.SendWait(textToSet);
                        }
                        else
                        {
                            if (clearElement.ToLower() == "no")
                            {
                                string currentText;
                                object tPattern = null;
                                if (requiredHandle.TryGetCurrentPattern(TextPattern.Pattern, out tPattern))
                                {
                                    var textPattern = (TextPattern)tPattern;
                                    currentText = textPattern.DocumentRange.GetText(-1).TrimEnd('\r').ToString(); // often there is an extra '\r' hanging off the end.
                                }
                                else
                                    currentText = requiredHandle.Current.Name.ToString();

                                textToSet = currentText + textToSet;
                            }
                            requiredHandle.SetFocus();
                            ((ValuePattern)valuePattern).SetValue(textToSet);
                        }
                    }
                    break;
                case "Set Secure Text":
                    string secureString = (from rw in v_UIAActionParameters.AsEnumerable()
                                           where rw.Field<string>("Parameter Name") == "Secure String Variable"
                                           select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    string _clearElement = (from rw in v_UIAActionParameters.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Clear Element Before Setting Text"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    var secureStrVariable = secureString.ConvertUserVariableToObject(engine);

                    if (secureStrVariable is SecureString)
                        secureString = ((SecureString)secureStrVariable).ConvertSecureStringToString();
                    else
                        throw new ArgumentException("Provided Argument is not a 'Secure String'");

                    if (_clearElement == null)
                        _clearElement = "No";

                    if (requiredHandle.Current.IsEnabled && requiredHandle.Current.IsKeyboardFocusable)
                    {
                        object valuePattern = null;
                        if (!requiredHandle.TryGetCurrentPattern(ValuePattern.Pattern, out valuePattern))
                        {
                            //The control does not support ValuePattern Using keyboard input
                            // Set focus for input functionality and begin.
                            requiredHandle.SetFocus();

                            // Pause before sending keyboard input.
                            Thread.Sleep(100);

                            if (_clearElement.ToLower() == "yes")
                            {
                                // Delete existing content in the control and insert new content.
                                SendKeys.SendWait("^{HOME}");   // Move to start of control
                                SendKeys.SendWait("^+{END}");   // Select everything
                                SendKeys.SendWait("{DEL}");     // Delete selection
                            }
                            SendKeys.SendWait(secureString);
                        }
                        else
                        {
                            if (_clearElement.ToLower() == "no")
                            {
                                string currentText;
                                object tPattern = null;
                                if (requiredHandle.TryGetCurrentPattern(TextPattern.Pattern, out tPattern))
                                {
                                    var textPattern = (TextPattern)tPattern;
                                    currentText = textPattern.DocumentRange.GetText(-1).TrimEnd('\r').ToString(); // often there is an extra '\r' hanging off the end.
                                }
                                else
                                    currentText = requiredHandle.Current.Name.ToString();

                                secureString = currentText + secureString;
                            }
                            requiredHandle.SetFocus();
                            ((ValuePattern)valuePattern).SetValue(secureString);
                        }
                    }
                    break;
                case "Clear Element":
                    if (requiredHandle.Current.IsEnabled && requiredHandle.Current.IsKeyboardFocusable)
                    {
                        object valuePattern = null;
                        if (!requiredHandle.TryGetCurrentPattern(ValuePattern.Pattern, out valuePattern))
                        {
                            //The control does not support ValuePattern Using keyboard input
                            // Set focus for input functionality and begin.
                            requiredHandle.SetFocus();

                            // Pause before sending keyboard input.
                            Thread.Sleep(100);

                            // Delete existing content in the control and insert new content.
                            SendKeys.SendWait("^{HOME}");   // Move to start of control
                            SendKeys.SendWait("^+{END}");   // Select everything
                            SendKeys.SendWait("{DEL}");     // Delete selection
                        }
                        else
                        {
                            requiredHandle.SetFocus();
                            ((ValuePattern)valuePattern).SetValue("");
                        }
                    }
                    break;
                case "Get Text":
                //if element exists type
                case "Check If Element Exists":
                    //Variable Name
                    var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
                                           where rw.Field<string>("Parameter Name") == "Variable Name"
                                           select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    //declare search result
                    string searchResult = "";
                    if (v_AutomationType == "Get Text")
                    {
                        //string currentText;
                        object tPattern = null;
                        if (requiredHandle.TryGetCurrentPattern(TextPattern.Pattern, out tPattern))
                        {
                            var textPattern = (TextPattern)tPattern;
                            searchResult = textPattern.DocumentRange.GetText(-1).TrimEnd('\r').ToString(); // often there is an extra '\r' hanging off the end.
                        }
                        else
                            searchResult = requiredHandle.Current.Name.ToString();
                    }

                    else if (v_AutomationType == "Check If Element Exists")
                    {
                        //determine search result
                        if (requiredHandle == null)
                            searchResult = "False";
                        else
                            searchResult = "True";
                    }
                    //store data
                    searchResult.StoreInUserVariable(engine, applyToVariable);
                    break;
                case "Wait For Element To Exist":
                    var timeoutText = (from rw in v_UIAActionParameters.AsEnumerable()
                                       where rw.Field<string>("Parameter Name") == "Timeout (Seconds)"
                                       select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    timeoutText = timeoutText.ConvertUserVariableToString(engine);
                    int timeOut = Convert.ToInt32(timeoutText);

                    var timeToEnd = DateTime.Now.AddSeconds(timeOut);
                    while (timeToEnd >= DateTime.Now)
                    {
                        try
                        {
                            requiredHandle = SearchForGUIElement(sender, variableWindowName);
                            break;
                        }
                        catch (Exception)
                        {
                            engine.ReportProgress("Element Not Yet Found... " + (timeToEnd - DateTime.Now).Seconds + "s remain");
                            Thread.Sleep(1000);
                        }
                    }
                    break;

                case "Get Value From Element":
                    if (requiredHandle == null)
                        throw new Exception("Element was not found in window '" + variableWindowName + "'");
                    //get value from property
                    var propertyName = (from rw in v_UIAActionParameters.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Get Value From"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    //Variable Name
                    var applyToVariable2 = (from rw in v_UIAActionParameters.AsEnumerable()
                                           where rw.Field<string>("Parameter Name") == "Variable Name"
                                           select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    //get required value
                    var requiredValue = requiredHandle.Current.GetType().GetRuntimeProperty(propertyName)?.GetValue(requiredHandle.Current).ToString();

                    //store into variable
                    ((object)requiredValue).StoreInUserVariable(engine, applyToVariable2);
                    break;
                default:
                    throw new NotImplementedException("Automation type '" + v_AutomationType + "' not supported.");
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create search param grid
            _searchParametersGridViewHelper = new DataGridView();
            _searchParametersGridViewHelper.Width = 500;
            _searchParametersGridViewHelper.Height = 140;
            _searchParametersGridViewHelper.DataBindings.Add("DataSource", this, "v_UIASearchParameters", false, DataSourceUpdateMode.OnPropertyChanged);

            DataGridViewCheckBoxColumn enabled = new DataGridViewCheckBoxColumn();
            enabled.HeaderText = "Enabled";
            enabled.DataPropertyName = "Enabled";
            _searchParametersGridViewHelper.Columns.Add(enabled);

            DataGridViewTextBoxColumn propertyName = new DataGridViewTextBoxColumn();
            propertyName.HeaderText = "Parameter Name";
            propertyName.DataPropertyName = "Parameter Name";
            _searchParametersGridViewHelper.Columns.Add(propertyName);

            DataGridViewTextBoxColumn propertyValue = new DataGridViewTextBoxColumn();
            propertyValue.HeaderText = "Parameter Value";
            propertyValue.DataPropertyName = "Parameter Value";
            _searchParametersGridViewHelper.Columns.Add(propertyValue);
            _searchParametersGridViewHelper.ColumnHeadersHeight = 30;
            _searchParametersGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _searchParametersGridViewHelper.AllowUserToAddRows = false;
            _searchParametersGridViewHelper.AllowUserToDeleteRows = false;

            //create actions
            _actionParametersGridViewHelper = new DataGridView();
            _actionParametersGridViewHelper.Size = new Size(400, 150);
            _actionParametersGridViewHelper.ColumnHeadersHeight = 30;
            _actionParametersGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _actionParametersGridViewHelper.DataBindings.Add("DataSource", this, "v_UIAActionParameters", false, DataSourceUpdateMode.OnPropertyChanged);
            _actionParametersGridViewHelper.AllowUserToAddRows = false;
            _actionParametersGridViewHelper.AllowUserToDeleteRows = false;
            _actionParametersGridViewHelper.AllowUserToResizeRows = false;
            _actionParametersGridViewHelper.MouseEnter += ActionParametersGridViewHelper_MouseEnter;

            propertyName = new DataGridViewTextBoxColumn();
            propertyName.HeaderText = "Parameter Name";
            propertyName.DataPropertyName = "Parameter Name";
            _actionParametersGridViewHelper.Columns.Add(propertyName);

            propertyValue = new DataGridViewTextBoxColumn();
            propertyValue.HeaderText = "Parameter Value";
            propertyValue.DataPropertyName = "Parameter Value";
            _actionParametersGridViewHelper.Columns.Add(propertyValue);

            _actionParametersGridViewHelper.ColumnHeadersHeight = 30;
            _actionParametersGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _actionParametersGridViewHelper.AllowUserToAddRows = false;
            _actionParametersGridViewHelper.AllowUserToDeleteRows = false;

            //create helper control
            CommandItemControl helperControl = new CommandItemControl();
            helperControl.Padding = new Padding(10, 0, 0, 0);
            helperControl.ForeColor = Color.AliceBlue;
            helperControl.Font = new Font("Segoe UI Semilight", 10);         
            helperControl.CommandImage = Resources.command_camera;
            helperControl.CommandDisplay = "Element Recorder";
            helperControl.Click += ShowRecorder;

            //window name
            RenderedControls.AddRange(CommandControls.CreateDefaultWindowControlGroupFor("v_WindowName", this, editor));

            //automation type
            var automationTypeGroup = CommandControls.CreateDefaultDropdownGroupFor("v_AutomationType", this, editor);
            _automationTypeControl = (ComboBox)automationTypeGroup.Where(f => f is ComboBox).FirstOrDefault();
            _automationTypeControl.SelectionChangeCommitted += UIAType_SelectionChangeCommitted;
            RenderedControls.AddRange(automationTypeGroup);

            //create search parameters   
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_UIASearchParameters", this));
            RenderedControls.Add(helperControl);
            RenderedControls.Add(_searchParametersGridViewHelper);

            //create action parameters
            _actionParametersControls = new List<Control>();
            _actionParametersControls.Add(CommandControls.CreateDefaultLabelFor("v_UIAActionParameters", this));
            _actionParametersControls.Add(_actionParametersGridViewHelper);
            RenderedControls.AddRange(_actionParametersControls);

            return RenderedControls;
        }
        
        public override string GetDisplayValue()
        {
            var applyToVariable = (from rw in v_UIAActionParameters.AsEnumerable()
                                   where rw.Field<string>("Parameter Name") == "Variable Name"
                                   select rw.Field<string>("Parameter Value")).FirstOrDefault();

            switch (v_AutomationType)
            {
                case "Click Element":
                    //create search params
                    var clickType = (from rw in v_UIAActionParameters.AsEnumerable()
                                     where rw.Field<string>("Parameter Name") == "Click Type"
                                     select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    return base.GetDisplayValue() + $" [{clickType} Element in Window '{v_WindowName}']";

                case "Set Text":
                case "Set Secure Text":
                    var textToSet = (from rw in v_UIAActionParameters.AsEnumerable()
                                     where rw.Field<string>("Parameter Name") == "Text To Set"
                                     select rw.Field<string>("Parameter Value")).FirstOrDefault();
                    return base.GetDisplayValue() + $" [{v_AutomationType} '{textToSet}' in Element in Window '{v_WindowName}']";
                  
                case "Get Text":
                case "Check If Element Exists":          
                    return base.GetDisplayValue() + $" ['{v_AutomationType}' in Window '{v_WindowName}' - Store Result in '{applyToVariable}']";
               
                case "Get Value From Element":          
                    //get value from property
                    var propertyName = (from rw in v_UIAActionParameters.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Get Value From"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    return base.GetDisplayValue() + $" [Get Value From Element '{propertyName}' in Window '{v_WindowName}' - Store Value in '{applyToVariable}']";

                default:
                    return base.GetDisplayValue() + $" [{v_AutomationType} in Window '{v_WindowName}']";
            }
        }

        private void ActionParametersGridViewHelper_MouseEnter(object sender, EventArgs e)
        {
            UIAType_SelectionChangeCommitted(null, null);
        }

        public PropertyCondition CreatePropertyCondition(string propertyName, object propertyValue)
        {
            switch (propertyName)
            {
                case "AcceleratorKey":
                    return new PropertyCondition(AutomationElement.AcceleratorKeyProperty, propertyValue);
                case "AccessKey":
                    return new PropertyCondition(AutomationElement.AccessKeyProperty, propertyValue);
                case "AutomationId":
                    return new PropertyCondition(AutomationElement.AutomationIdProperty, propertyValue);
                case "ClassName":
                    return new PropertyCondition(AutomationElement.ClassNameProperty, propertyValue);
                case "FrameworkId":
                    return new PropertyCondition(AutomationElement.FrameworkIdProperty, propertyValue);
                case "HasKeyboardFocus":
                    return new PropertyCondition(AutomationElement.HasKeyboardFocusProperty, propertyValue);
                case "HelpText":
                    return new PropertyCondition(AutomationElement.HelpTextProperty, propertyValue);
                case "IsContentElement":
                    return new PropertyCondition(AutomationElement.IsContentElementProperty, propertyValue);
                case "IsControlElement":
                    return new PropertyCondition(AutomationElement.IsControlElementProperty, propertyValue);
                case "IsEnabled":
                    return new PropertyCondition(AutomationElement.IsEnabledProperty, propertyValue);
                case "IsKeyboardFocusable":
                    return new PropertyCondition(AutomationElement.IsKeyboardFocusableProperty, propertyValue);
                case "IsOffscreen":
                    return new PropertyCondition(AutomationElement.IsOffscreenProperty, propertyValue);
                case "IsPassword":
                    return new PropertyCondition(AutomationElement.IsPasswordProperty, propertyValue);
                case "IsRequiredForForm":
                    return new PropertyCondition(AutomationElement.IsRequiredForFormProperty, propertyValue);
                case "ItemStatus":
                    return new PropertyCondition(AutomationElement.ItemStatusProperty, propertyValue);
                case "ItemType":
                    return new PropertyCondition(AutomationElement.ItemTypeProperty, propertyValue);
                case "LocalizedControlType":
                    return new PropertyCondition(AutomationElement.LocalizedControlTypeProperty, propertyValue);
                case "Name":
                    return new PropertyCondition(AutomationElement.NameProperty, propertyValue);
                case "NativeWindowHandle":
                    return new PropertyCondition(AutomationElement.NativeWindowHandleProperty, propertyValue);
                case "ProcessID":
                    return new PropertyCondition(AutomationElement.ProcessIdProperty, propertyValue);
                default:
                    throw new NotImplementedException("Property Type '" + propertyName + "' not implemented");
            }
        }

        public AutomationElement SearchForGUIElement(object sender, string variableWindowName)
        {
            var engine = (AutomationEngineInstance)sender;
            //create search params
            var searchParams = from rw in v_UIASearchParameters.AsEnumerable()
                               where rw.Field<string>("Enabled") == "True"
                               select rw;

            //create and populate condition list
            var conditionList = new List<Condition>();
            foreach (var param in searchParams)
            {
                var parameterName = (string)param["Parameter Name"];
                var parameterValue = (string)param["Parameter Value"];

                parameterValue = parameterValue.ConvertUserVariableToString(engine);

                PropertyCondition propCondition;
                if (bool.TryParse(parameterValue, out bool bValue))
                    propCondition = CreatePropertyCondition(parameterName, bValue);
                else
                    propCondition = CreatePropertyCondition(parameterName, parameterValue);

                conditionList.Add(propCondition);
            }

            //concatenate or take first condition
            Condition searchConditions;
            if (conditionList.Count > 1)
                searchConditions = new AndCondition(conditionList.ToArray());
            else
                searchConditions = conditionList[0];

            //find window
            var windowElement = AutomationElement.RootElement.FindFirst(TreeScope.Children, new PropertyCondition(AutomationElement.NameProperty, variableWindowName));

            //if window was not found
            if (windowElement == null)
                throw new Exception("Window named '" + variableWindowName + "' was not found!");

            //find required handle based on specified conditions
            var element = windowElement.FindFirst(TreeScope.Descendants, searchConditions);
            return element;
        }

        public void ShowRecorder(object sender, EventArgs e)
        {
            //get command reference
            //create recorder
            frmThickAppElementRecorder newElementRecorder = new frmThickAppElementRecorder();
            newElementRecorder.cboWindowTitle.Text = RenderedControls[2].Text;
            newElementRecorder.SearchParameters = v_UIASearchParameters;

            //show form
            newElementRecorder.ShowDialog();

            //window name combobox
            RenderedControls[2].Text = newElementRecorder.cboWindowTitle.Text;

            v_UIASearchParameters.Rows.Clear();

            foreach (DataRow rw in newElementRecorder.SearchParameters.Rows)
                v_UIASearchParameters.ImportRow(rw);

            _searchParametersGridViewHelper.DataSource = v_UIASearchParameters;
            _searchParametersGridViewHelper.Refresh();
        }

        public void UIAType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            UIAutomationCommand cmd = this;
            DataTable actionParameters = cmd.v_UIAActionParameters;

            if (sender != null)
                actionParameters.Rows.Clear();

            switch (_automationTypeControl.SelectedItem)
            {
                case "Click Element":
                    foreach (var ctrl in _actionParametersControls)
                        ctrl.Show();

                    var mouseClickBox = new DataGridViewComboBoxCell();
                    mouseClickBox.Items.Add("Left Click");
                    mouseClickBox.Items.Add("Middle Click");
                    mouseClickBox.Items.Add("Right Click");
                    mouseClickBox.Items.Add("Left Down");
                    mouseClickBox.Items.Add("Middle Down");
                    mouseClickBox.Items.Add("Right Down");
                    mouseClickBox.Items.Add("Left Up");
                    mouseClickBox.Items.Add("Middle Up");
                    mouseClickBox.Items.Add("Right Up");
                    mouseClickBox.Items.Add("Double Left Click");

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Click Type", "");
                        actionParameters.Rows.Add("X Adjustment", 0);
                        actionParameters.Rows.Add("Y Adjustment", 0);
                    }

                    if (_actionParametersGridViewHelper.Rows.Count > 0)
                        _actionParametersGridViewHelper.Rows[0].Cells[1] = mouseClickBox;
                    break;
                case "Set Text":
                    foreach (var ctrl in _actionParametersControls)
                        ctrl.Show();

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Text To Set");
                        actionParameters.Rows.Add("Clear Element Before Setting Text");
                        actionParameters.Rows.Add("Encrypted Text");
                        actionParameters.Rows.Add("Optional - Click to Encrypt 'Text To Set'");

                        DataGridViewComboBoxCell encryptedBox = new DataGridViewComboBoxCell();
                        encryptedBox.Items.Add("Not Encrypted");
                        encryptedBox.Items.Add("Encrypted");
                        _actionParametersGridViewHelper.Rows[2].Cells[1] = encryptedBox;
                        _actionParametersGridViewHelper.Rows[2].Cells[1].Value = "Not Encrypted";

                        var buttonCell = new DataGridViewButtonCell();
                        _actionParametersGridViewHelper.Rows[3].Cells[1] = buttonCell;
                        _actionParametersGridViewHelper.Rows[3].Cells[1].Value = "Encrypt Text";
                        _actionParametersGridViewHelper.CellContentClick += ElementsGridViewHelper_CellContentClick;
                    }

                    DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("Yes");
                    comparisonComboBox.Items.Add("No");

                    //assign cell as a combobox
                    if (sender != null)
                        _actionParametersGridViewHelper.Rows[1].Cells[1].Value = "No";

                    if (_actionParametersGridViewHelper.Rows.Count > 1)
                        _actionParametersGridViewHelper.Rows[1].Cells[1] = comparisonComboBox;

                    break;

                case "Set Secure Text":
                    foreach (var ctrl in _actionParametersControls)
                        ctrl.Show();

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Secure String Variable");
                        actionParameters.Rows.Add("Clear Element Before Setting Text");
                    }

                    DataGridViewComboBoxCell _comparisonComboBox = new DataGridViewComboBoxCell();
                    _comparisonComboBox.Items.Add("Yes");
                    _comparisonComboBox.Items.Add("No");

                    //assign cell as a combobox
                    if (sender != null)
                        _actionParametersGridViewHelper.Rows[1].Cells[1].Value = "No";

                    if (_actionParametersGridViewHelper.Rows.Count > 1)
                        _actionParametersGridViewHelper.Rows[1].Cells[1] = _comparisonComboBox;
                    break;
                case "Get Text":
                case "Check If Element Exists":
                    foreach (var ctrl in _actionParametersControls)
                        ctrl.Show();

                    if (sender != null)
                        actionParameters.Rows.Add("Variable Name", "");
                    break;
                case "Clear Element":
                    foreach (var ctrl in _actionParametersControls)
                        ctrl.Hide();

                    break;
                case "Wait For Element To Exist":
                    foreach (var ctrl in _actionParametersControls)
                        ctrl.Show();

                    if (sender != null)
                        actionParameters.Rows.Add("Timeout (Seconds)");
                    break;
                case "Get Value From Element":
                    foreach (var ctrl in _actionParametersControls)
                        ctrl.Show();

                    var parameterName = new DataGridViewComboBoxCell();
                    parameterName.Items.Add("AcceleratorKey");
                    parameterName.Items.Add("AccessKey");
                    parameterName.Items.Add("AutomationId");
                    parameterName.Items.Add("ClassName");
                    parameterName.Items.Add("FrameworkId");
                    parameterName.Items.Add("HasKeyboardFocus");
                    parameterName.Items.Add("HelpText");
                    parameterName.Items.Add("IsContentElement");
                    parameterName.Items.Add("IsControlElement");
                    parameterName.Items.Add("IsEnabled");
                    parameterName.Items.Add("IsKeyboardFocusable");
                    parameterName.Items.Add("IsOffscreen");
                    parameterName.Items.Add("IsPassword");
                    parameterName.Items.Add("IsRequiredForForm");
                    parameterName.Items.Add("ItemStatus");
                    parameterName.Items.Add("ItemType");
                    parameterName.Items.Add("LocalizedControlType");
                    parameterName.Items.Add("Name");
                    parameterName.Items.Add("NativeWindowHandle");
                    parameterName.Items.Add("ProcessID");

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Get Value From", "");
                        actionParameters.Rows.Add("Variable Name", "");
                        _actionParametersGridViewHelper.Refresh();
                        try
                        {
                            _actionParametersGridViewHelper.Rows[0].Cells[1] = parameterName;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Unable to select first row, second cell to apply '" + parameterName + "': " + ex.ToString());
                        }
                    }
                    break;
                default:
                    break;
            }
            _actionParametersGridViewHelper.DataSource = v_UIAActionParameters;
        }

        private void ElementsGridViewHelper_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var targetCell = _actionParametersGridViewHelper.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (targetCell is DataGridViewButtonCell && targetCell.Value.ToString() == "Encrypt Text")
            {
                var targetElement = _actionParametersGridViewHelper.Rows[0].Cells[1];

                if (string.IsNullOrEmpty(targetElement.Value.ToString()))
                    return;

                var warning = MessageBox.Show($"Warning! Text should only be encrypted one time and is not reversible in the builder. " + 
                                              "Would you like to proceed and convert '{targetElement.Value.ToString()}' to an encrypted value?", 
                                              "Encryption Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (warning == DialogResult.Yes)
                {
                    targetElement.Value = EncryptionServices.EncryptString(targetElement.Value.ToString(), "OPENBOTS");
                    _actionParametersGridViewHelper.Rows[2].Cells[1].Value = "Encrypted";
                }
            }
        }
    }
}