using HtmlAgilityPack;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommandUtilities;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.Properties;
using taskt.UI.CustomControls;
using taskt.UI.Forms.Supplement_Forms;
using Group = taskt.Core.Attributes.ClassAttributes.Group;
using HtmlDocument = HtmlAgilityPack.HtmlDocument;

namespace taskt.Commands
{
    [Serializable]
    [Group("Web Browser Commands")]
    [Description("This command performs an element action in a Selenium web browser session.")]

    public class SeleniumElementActionCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Browser Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Browser** command.")]
        [SampleUsage("MyBrowserInstance")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [PropertyDescription("Element Search Parameter")]
        [InputSpecification("Use the Element Recorder to generate a listing of potential search parameters." + 
            "Select the specific search type(s) that you want to use to isolate the element on the web page.")]
        [SampleUsage("{vSearchParameter}" +
                     "\n\tXPath : //*[@id=\"features\"]/div[2]/div/h2" +
                     "\n\tID: 1" +
                     "\n\tName: myName" +
                     "\n\tTag Name: h1" +
                     "\n\tClass Name: myClass" +
                     "\n\tCSS Selector: [attribute=value]" +
                     "\n\tLink Text: https://www.mylink.com/"
                    )]
        [Remarks("If multiple parameters are enabled, they will be used in the order that they are listed until an element is found."+
                 "Drag and drop rows to reorder the search parameters.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowElementHelper)] 
        public DataTable v_SeleniumSearchParameters { get; set; }

        [XmlElement]
        [PropertyDescription("Element Search Option")]
        [PropertyUISelectionOption("Find Element")]
        [PropertyUISelectionOption("Find Elements")]
        [InputSpecification("Indicate whether to search for a single or multiple elements.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_SeleniumSearchOption { get; set; }

        [XmlElement]
        [PropertyDescription("Element Action")]
        [PropertyUISelectionOption("Invoke Click")]
        [PropertyUISelectionOption("Left Click")]
        [PropertyUISelectionOption("Right Click")]
        [PropertyUISelectionOption("Middle Click")]
        [PropertyUISelectionOption("Double Left Click")]
        [PropertyUISelectionOption("Set Text")]
        [PropertyUISelectionOption("Set Secure Text")]
        [PropertyUISelectionOption("Get Text")]
        [PropertyUISelectionOption("Get Table")]
        [PropertyUISelectionOption("Get Count")]
        [PropertyUISelectionOption("Get Options")]
        [PropertyUISelectionOption("Get Attribute")]
        [PropertyUISelectionOption("Get Matching Element(s)")]
        [PropertyUISelectionOption("Clear Element")]
        [PropertyUISelectionOption("Wait For Element To Exist")]
        [PropertyUISelectionOption("Switch to frame")]
        [PropertyUISelectionOption("Select Option")]
        [InputSpecification("Select the appropriate corresponding action to take once the element has been located.")]
        [SampleUsage("")]
        [Remarks("Selecting this field changes the parameters required in the following step.")]
        public string v_SeleniumElementAction { get; set; }

        [XmlElement]
        [PropertyDescription("Action Parameters")]
        [InputSpecification("Action Parameters will be determined based on the action settings selected.")]
        [SampleUsage("data || {vData} || *Variable Name*: {vNewVariable}")]
        [Remarks("Action Parameters range from adding offset coordinates to specifying a variable to apply element text to.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_WebActionParameterTable { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _elementsGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private ComboBox _elementActionDropdown;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> _elementParameterControls;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _searchParametersGridViewHelper;

        [NonSerialized]
        private int _indexOfItemUnderMouseToDrag = -1;
        [NonSerialized]
        private int _indexOfItemUnderMouseToDrop = -1;
        [NonSerialized]
        private Rectangle _dragBoxFromMouseDown = Rectangle.Empty;

        public SeleniumElementActionCommand()
        {
            CommandName = "SeleniumElementActionCommand";
            SelectionName = "Element Action";
            v_InstanceName = "DefaultBrowser";
            CommandEnabled = true;
            CustomRendering = true;
            v_SeleniumSearchOption = "Find Element";

            v_WebActionParameterTable = new DataTable
            {
                TableName = "WebActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            };
            v_WebActionParameterTable.Columns.Add("Parameter Name");
            v_WebActionParameterTable.Columns.Add("Parameter Value");

            //set up search parameter table
            v_SeleniumSearchParameters = new DataTable();
            v_SeleniumSearchParameters.Columns.Add("Enabled");
            v_SeleniumSearchParameters.Columns.Add("Parameter Name");
            v_SeleniumSearchParameters.Columns.Add("Parameter Value");
            v_SeleniumSearchParameters.TableName = DateTime.Now.ToString("v_SeleniumSearchParameters" + DateTime.Now.ToString("MMddyy.hhmmss"));

            //create search param grid
            _searchParametersGridViewHelper = new DataGridView();
            _searchParametersGridViewHelper.Width = 400;
            _searchParametersGridViewHelper.Height = 250;
            _searchParametersGridViewHelper.DataBindings.Add("DataSource", this, "v_SeleniumSearchParameters", false, DataSourceUpdateMode.OnPropertyChanged);

            DataGridViewCheckBoxColumn enabled = new DataGridViewCheckBoxColumn();
            enabled.HeaderText = "Enabled";
            enabled.DataPropertyName = "Enabled";
            enabled.FillWeight = 30;
            _searchParametersGridViewHelper.Columns.Add(enabled);

            DataGridViewTextBoxColumn propertyName = new DataGridViewTextBoxColumn();
            propertyName.HeaderText = "Parameter Name";
            propertyName.DataPropertyName = "Parameter Name";
            propertyName.FillWeight = 40;
            _searchParametersGridViewHelper.Columns.Add(propertyName);

            DataGridViewTextBoxColumn propertyValue = new DataGridViewTextBoxColumn();
            propertyValue.HeaderText = "Parameter Value";
            propertyValue.DataPropertyName = "Parameter Value";
            _searchParametersGridViewHelper.Columns.Add(propertyValue);
            _searchParametersGridViewHelper.ColumnHeadersHeight = 30;
            _searchParametersGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _searchParametersGridViewHelper.AllowUserToAddRows = true;
            _searchParametersGridViewHelper.AllowUserToDeleteRows = true;
            _searchParametersGridViewHelper.AllowUserToResizeRows = false;
            _searchParametersGridViewHelper.DragDrop += new DragEventHandler(SearchParametersGridViewHelper_DragDrop);
            _searchParametersGridViewHelper.DragOver += new DragEventHandler(SearchParametersGridViewHelper_DragOver);
            _searchParametersGridViewHelper.MouseDown += new MouseEventHandler(SearchParametersGridViewHelper_MouseDown);
            _searchParametersGridViewHelper.MouseMove += new MouseEventHandler(SearchParametersGridViewHelper_MouseMove);
            _searchParametersGridViewHelper.MouseUp += new MouseEventHandler(SearchParametersGridViewHelper_MouseUp);

            _elementsGridViewHelper = new DataGridView();
            _elementsGridViewHelper.AllowUserToAddRows = true;
            _elementsGridViewHelper.AllowUserToDeleteRows = true;
            _elementsGridViewHelper.Size = new Size(400, 150);
            _elementsGridViewHelper.ColumnHeadersHeight = 30;
            _elementsGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _elementsGridViewHelper.DataBindings.Add("DataSource", this, "v_WebActionParameterTable", false, DataSourceUpdateMode.OnPropertyChanged);
            _elementsGridViewHelper.AllowUserToAddRows = false;
            _elementsGridViewHelper.AllowUserToDeleteRows = false;
            _elementsGridViewHelper.AllowUserToResizeRows = false;
        }      

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            var seleniumSearchParam = (from rw in v_SeleniumSearchParameters.AsEnumerable()
                                       where rw.Field<string>("Enabled") == "True"
                                       select rw.Field<string>("Parameter Value")).ToList();

            var seleniumSearchType = (from rw in v_SeleniumSearchParameters.AsEnumerable()
                                      where rw.Field<string>("Enabled") == "True"
                                      select rw.Field<string>("Parameter Name")).ToList();

            var browserObject = v_InstanceName.GetAppInstance(engine);
            var seleniumInstance = (IWebDriver)browserObject;
            dynamic element = null;
            Exception elementSearchException = new Exception("Element not found");

            if (v_SeleniumElementAction == "Wait For Element To Exist")
            {
                var timeoutText = (from rw in v_WebActionParameterTable.AsEnumerable()
                                   where rw.Field<string>("Parameter Name") == "Timeout (Seconds)"
                                   select rw.Field<string>("Parameter Value")).FirstOrDefault();

                timeoutText = timeoutText.ConvertUserVariableToString(engine);
                int timeOut = Convert.ToInt32(timeoutText);
                var timeToEnd = DateTime.Now.AddSeconds(timeOut);

                while (timeToEnd >= DateTime.Now)
                {
                    try
                    {
                        for(int i = 0; i < seleniumSearchParam.Count; i++)
                        {
                            try
                            {
                                element = FindElement(engine, seleniumInstance, seleniumSearchType[i], seleniumSearchParam[i]);
                                break;
                            }
                            catch (Exception ex)
                            {
                                elementSearchException = ex;
                                //try next search parameter
                            }
                        }
                        if (element == null)
                            throw elementSearchException;
                    }
                    catch (Exception)
                    {
                        engine.ReportProgress("Element Not Yet Found... " + (timeToEnd - DateTime.Now).Seconds + "s remain");
                        Thread.Sleep(1000);
                    }
                }              
            }
            else if (seleniumSearchParam.Count > 0)
            {
                for (int i = 0; i < seleniumSearchParam.Count; i++)
                {
                    try
                    {
                        element = FindElement(engine, seleniumInstance, seleniumSearchType[i], seleniumSearchParam[i]);
                        break;
                    }
                    catch (Exception ex)
                    {
                        elementSearchException = ex;
                        //try next search parameter
                    }
                }                                           
            }

            if (element == null)
                throw elementSearchException;

            switch (v_SeleniumElementAction)
            {
                case "Invoke Click":
                    int seleniumWindowHeightY = seleniumInstance.Manage().Window.Size.Height;
                    int elementPositionY = element.Location.Y;
                    if (elementPositionY > seleniumWindowHeightY)
                    {
                        string scroll = string.Format("window.scroll(0, {0})", elementPositionY);
                        IJavaScriptExecutor js = browserObject as IJavaScriptExecutor;
                        js.ExecuteScript(scroll);
                    }
                    element.Click();
                    break;

                case "Left Click":
                case "Right Click":
                case "Middle Click":
                case "Double Left Click":
                    int userXAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                       where rw.Field<string>("Parameter Name") == "X Adjustment"
                                                       select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertUserVariableToString(engine));

                    int userYAdjust = Convert.ToInt32((from rw in v_WebActionParameterTable.AsEnumerable()
                                                       where rw.Field<string>("Parameter Name") == "Y Adjustment"
                                                       select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertUserVariableToString(engine));

                    var elementLocation = element.Location;
                    SendMouseMoveCommand newMouseMove = new SendMouseMoveCommand();
                    var seleniumWindowPosition = seleniumInstance.Manage().Window.Position;
                    newMouseMove.v_XMousePosition = (seleniumWindowPosition.X + elementLocation.X + 30 + userXAdjust).ToString(); // added 30 for offset
                    newMouseMove.v_YMousePosition = (seleniumWindowPosition.Y + elementLocation.Y + 130 + userYAdjust).ToString(); //added 130 for offset
                    newMouseMove.v_MouseClick = v_SeleniumElementAction;
                    newMouseMove.RunCommand(sender);
                    break;

                case "Set Text":
                    string textToSet = (from rw in v_WebActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Text To Set"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertUserVariableToString(engine);


                    string clearElement = (from rw in v_WebActionParameterTable.AsEnumerable()
                                           where rw.Field<string>("Parameter Name") == "Clear Element Before Setting Text"
                                           select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    string encryptedData = (from rw in v_WebActionParameterTable.AsEnumerable()
                                           where rw.Field<string>("Parameter Name") == "Encrypted Text"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    if (clearElement == null)
                        clearElement = "No";

                    if (clearElement.ToLower() == "yes")
                        element.Clear();

                    if (encryptedData == "Encrypted")
                        textToSet = EncryptionServices.DecryptString(textToSet, "OPENBOTS");

                    string[] potentialKeyPresses = textToSet.Split('{', '}');

                    Type seleniumKeys = typeof(OpenQA.Selenium.Keys);
                    FieldInfo[] fields = seleniumKeys.GetFields(BindingFlags.Static | BindingFlags.Public);

                    //check if chunked string contains a key press command like {ENTER}
                    foreach (string chunkedString in potentialKeyPresses)
                    {
                        if (chunkedString == "")
                            continue;

                        if (fields.Any(f => f.Name == chunkedString))
                        {
                            string keyPress = (string)fields.Where(f => f.Name == chunkedString).FirstOrDefault().GetValue(null);
                            element.SendKeys(keyPress);
                        }
                        else
                        {
                            var convertedChunk = chunkedString.ConvertUserVariableToString(engine);
                            element.SendKeys(convertedChunk);
                        }
                    }
                    break;

                case "Set Secure Text":
                    var secureString = (from rw in v_WebActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Secure String Variable"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    string _clearElement = (from rw in v_WebActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Clear Element Before Setting Text"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    var secureStrVariable = secureString.ConvertUserVariableToObject(engine);

                    if (secureStrVariable is SecureString)
                        secureString = ((SecureString)secureStrVariable).ConvertSecureStringToString();
                    else
                        throw new ArgumentException("Provided Argument is not a 'Secure String'");

                    if (_clearElement == null)
                        _clearElement = "No";

                    if (_clearElement.ToLower() == "yes")
                        element.Clear();

                    string[] _potentialKeyPresses = secureString.Split('{', '}');

                    Type _seleniumKeys = typeof(OpenQA.Selenium.Keys);
                    FieldInfo[] _fields = _seleniumKeys.GetFields(BindingFlags.Static | BindingFlags.Public);

                    //check if chunked string contains a key press command like {ENTER}
                    foreach (string chunkedString in _potentialKeyPresses)
                    {
                        if (chunkedString == "")
                            continue;

                        if (_fields.Any(f => f.Name == chunkedString))
                        {
                            string keyPress = (string)_fields.Where(f => f.Name == chunkedString).FirstOrDefault().GetValue(null);
                            element.SendKeys(keyPress);
                        }
                        else
                        {
                            var convertedChunk = chunkedString.ConvertUserVariableToString(engine);
                            element.SendKeys(convertedChunk);
                        }
                    }
                    break;

                case "Get Options":
                    string applyToVarName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                           where rw.Field<string>("Parameter Name") == "Variable Name"
                                           select rw.Field<string>("Parameter Value")).FirstOrDefault();


                    string attribName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Attribute Name"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertUserVariableToString(engine);

                    var optionsItems = new List<string>();
                    var ele = (IWebElement)element;
                    var select = new SelectElement(ele);
                    var options = select.Options;

                    foreach (var option in options)
                    {
                        var optionValue = option.GetAttribute(attribName);
                        optionsItems.Add(optionValue);
                    }

                    optionsItems.StoreInUserVariable(engine, applyToVarName);
                   
                    break;

                case "Select Option":
                    string selectionType = (from rw in v_WebActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Selection Type"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    string selectionParam = (from rw in v_WebActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Selection Parameter"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertUserVariableToString(engine);

                    seleniumInstance.SwitchTo().ActiveElement();

                    var el = (IWebElement)element;
                    var selectionElement = new SelectElement(el);

                    switch (selectionType)
                    {
                        case "Select By Index":
                            selectionElement.SelectByIndex(int.Parse(selectionParam));
                            break;
                        case "Select By Text":
                            selectionElement.SelectByText(selectionParam);
                            break;
                        case "Select By Value":
                            selectionElement.SelectByValue(selectionParam);
                            break;
                        case "Deselect By Index":
                            selectionElement.DeselectByIndex(int.Parse(selectionParam));
                            break;
                        case "Deselect By Text":
                            selectionElement.DeselectByText(selectionParam);
                            break;
                        case "Deselect By Value":
                            selectionElement.DeselectByValue(selectionParam);
                            break;
                        case "Deselect All":
                            selectionElement.DeselectAll();
                            break;
                        default:
                            throw new NotImplementedException();
                    }
                    break;

                case "Get Text":
                case "Get Attribute":
                case "Get Count":
                    string VariableName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                           where rw.Field<string>("Parameter Name") == "Variable Name"
                                           select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    string attributeName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Attribute Name"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertUserVariableToString(engine);

                    string elementValue;
                    if (v_SeleniumElementAction == "Get Text")
                        elementValue = element.Text;
                    else if (v_SeleniumElementAction == "Get Count")
                    {
                        elementValue = "1";
                        if (element is ReadOnlyCollection<IWebElement>)
                            elementValue = ((ReadOnlyCollection<IWebElement>)element).Count().ToString();
                    }
                    else
                        elementValue = element.GetAttribute(attributeName);

                    elementValue.StoreInUserVariable(engine, VariableName);
                    break;

                case "Get Matching Element(s)":
                    var variableName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                        where rw.Field<string>("Parameter Name") == "Variable Name"
                                        select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    if (!(element is IWebElement))
                    {
                        //create element list
                        List<IWebElement> elementList = new List<IWebElement>();
                        foreach (IWebElement item in element)
                        {
                            elementList.Add(item);
                        }
                        elementList.StoreInUserVariable(engine, variableName);
                    }
                    else
                        ((IWebElement)element).StoreInUserVariable(engine, variableName);                    
                    break;

                case "Get Table":
                    var DTVariableName = (from rw in v_WebActionParameterTable.AsEnumerable()
                                          where rw.Field<string>("Parameter Name") == "Variable Name"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault();

                    // Get HTML (Source) of the Element
                    string tableHTML = element.GetAttribute("innerHTML").ToString();
                    HtmlDocument doc = new HtmlDocument();

                    //Load Source (String) as HTML Document
                    doc.LoadHtml(tableHTML);

                    //Get Header Tags
                    var headers = doc.DocumentNode.SelectNodes("//tr/th");
                    DataTable DT = new DataTable();

                    //If headers found
                    if (headers != null && headers.Count != 0)
                    {
                        // add columns from th (headers)
                        foreach (HtmlNode header in headers)
                            DT.Columns.Add(Regex.Replace(header.InnerText, @"\t|\n|\r", "").Trim()); 
                    }
                    else
                    {
                        var columnsCount = doc.DocumentNode.SelectSingleNode("//tr[1]").ChildNodes.Where(node=>node.Name=="td").Count();
                        DT.Columns.AddRange((Enumerable.Range(1, columnsCount).Select(dc => new DataColumn())).ToArray());
                    }

                    // select rows with td elements and load each row (containing <td> tags) into DataTable
                    foreach (var row in doc.DocumentNode.SelectNodes("//tr[td]"))
                        DT.Rows.Add(row.SelectNodes("td").Select(td => Regex.Replace(td.InnerText, @"\t|\n|\r", "").Trim()).ToArray());

                    DT.StoreInUserVariable(engine, DTVariableName);
                    break;

                case "Clear Element":
                    element.Clear();
                    break;

                case "Switch to Frame":
                    if (seleniumSearchParam.Count == 0)
                        seleniumInstance.SwitchTo().DefaultContent();
                    else
                        seleniumInstance.SwitchTo().Frame(element);
                    break;
                case "Wait For Element To Exist":
                    return;
                default:
                    throw new Exception("Element Action was not found");
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create helper control
            CommandItemControl helperControl = new CommandItemControl();
            helperControl.Padding = new Padding(10, 0, 0, 0);
            helperControl.ForeColor = Color.AliceBlue;
            helperControl.Font = new Font("Segoe UI Semilight", 10);
            helperControl.CommandImage = Resources.command_camera;
            helperControl.CommandDisplay = "Element Recorder";
            helperControl.Click += new EventHandler((s, e) => ShowRecorder(s, e, editor));

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

            _searchParametersGridViewHelper.AllowDrop = true;
            if (v_SeleniumSearchParameters.Rows.Count == 0)
            {
                v_SeleniumSearchParameters.Rows.Add(true, "XPath", "");
                v_SeleniumSearchParameters.Rows.Add(false, "ID", "");
                v_SeleniumSearchParameters.Rows.Add(false, "Name", "");
                v_SeleniumSearchParameters.Rows.Add(false, "Tag Name", "");
                v_SeleniumSearchParameters.Rows.Add(false, "Class Name", "");
                v_SeleniumSearchParameters.Rows.Add(false, "Link Text", "");
                v_SeleniumSearchParameters.Rows.Add(false, "CSS Selector", "");
            }
            
            //create search parameters   
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_SeleniumSearchParameters", this));
            RenderedControls.Add(helperControl);
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_SeleniumSearchParameters", this, new Control[] { _searchParametersGridViewHelper }, editor));
            RenderedControls.Add(_searchParametersGridViewHelper);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SeleniumSearchOption", this, editor));

            _elementActionDropdown = (ComboBox)CommandControls.CreateDropdownFor("v_SeleniumElementAction", this);
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_SeleniumElementAction", this));
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_SeleniumElementAction", this, new Control[] { _elementActionDropdown }, editor));
            _elementActionDropdown.SelectionChangeCommitted += SeleniumAction_SelectionChangeCommitted;
            RenderedControls.Add(_elementActionDropdown);

            _elementParameterControls = new List<Control>();
            _elementParameterControls.Add(CommandControls.CreateDefaultLabelFor("v_WebActionParameterTable", this));
            _elementParameterControls.AddRange(CommandControls.CreateUIHelpersFor("v_WebActionParameterTable", this, new Control[] { _elementsGridViewHelper }, editor));
            _elementParameterControls.Add(_elementsGridViewHelper);
            RenderedControls.AddRange(_elementParameterControls);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            string searchParameterName = (from rw in v_SeleniumSearchParameters.AsEnumerable()
                                          where rw.Field<string>("Enabled") == "True"
                                          select rw.Field<string>("Parameter Name")).FirstOrDefault();

            string searchParameterValue = (from rw in v_SeleniumSearchParameters.AsEnumerable()
                                          where rw.Field<string>("Enabled") == "True"
                                          select rw.Field<string>("Parameter Value")).FirstOrDefault();


            return base.GetDisplayValue() + $" [{v_SeleniumElementAction} - {v_SeleniumSearchOption} by {searchParameterName}" + 
                                            $" '{searchParameterValue}' - Instance Name '{v_InstanceName}']";
        }

        public bool ElementExists(object sender, string searchType, string elementName)
        {
            //get engine reference
            var engine = (AutomationEngineInstance)sender;
            var seleniumSearchParam = elementName.ConvertUserVariableToString(engine);

            //get stored app object
            var browserObject = v_InstanceName.GetAppInstance(engine);

            //get selenium instance driver
            var seleniumInstance = (ChromeDriver)browserObject;

            try
            {
                //search for element
                var element = FindElement(engine, seleniumInstance, searchType, seleniumSearchParam);

                //element exists
                return true;
            }
            catch (Exception)
            {
                //element does not exist
                return false;
            }
        }

        private object FindElement(AutomationEngineInstance engine, IWebDriver seleniumInstance, string searchType, string searchParameter)
        {
            searchParameter = searchParameter.ConvertUserVariableToString(engine);
            object element;

            switch (searchType)
            {
                case string a when a.ToLower().Contains("xpath"):
                    if (v_SeleniumSearchOption == "Find Element")
                        element = seleniumInstance.FindElement(By.XPath(searchParameter));
                    else
                        element = seleniumInstance.FindElements(By.XPath(searchParameter));
                    break;

                case string a when a.ToLower().Contains("id"):
                    if (v_SeleniumSearchOption == "Find Element")
                        element = seleniumInstance.FindElement(By.Id(searchParameter));
                    else
                        element = seleniumInstance.FindElements(By.Id(searchParameter));
                    break;
       
                case string a when a.ToLower().Contains("tag name"):
                    if (v_SeleniumSearchOption == "Find Element")
                        element = seleniumInstance.FindElement(By.TagName(searchParameter));
                    else
                        element = seleniumInstance.FindElements(By.TagName(searchParameter));
                    break;

                case string a when a.ToLower().Contains("class name"):
                    if (v_SeleniumSearchOption == "Find Element")
                        element = seleniumInstance.FindElement(By.ClassName(searchParameter));
                    else
                        element = seleniumInstance.FindElements(By.ClassName(searchParameter));
                    break;

                case string a when a.ToLower().Contains("name"):
                    if (v_SeleniumSearchOption == "Find Element")
                        element = seleniumInstance.FindElement(By.Name(searchParameter));
                    else
                        element = seleniumInstance.FindElements(By.Name(searchParameter));
                    break;

                case string a when a.ToLower().Contains("css selector"):
                    if (v_SeleniumSearchOption == "Find Element")
                        element = seleniumInstance.FindElement(By.CssSelector(searchParameter));
                    else
                        element = seleniumInstance.FindElements(By.CssSelector(searchParameter));
                    break;

                case string a when a.ToLower().Contains("link text"):
                    if (v_SeleniumSearchOption == "Find Element")
                        element = seleniumInstance.FindElement(By.LinkText(searchParameter));
                    else
                        element = seleniumInstance.FindElements(By.LinkText(searchParameter));
                    break;

                default:
                    throw new Exception("Element Search Type was not found: " + searchType);
            }
            return element;
        }

        public void ShowRecorder(object sender, EventArgs e, IfrmCommandEditor editor)
        {
            //create recorder
            frmHTMLElementRecorder newElementRecorder = new frmHTMLElementRecorder(editor.HTMLElementRecorderURL);
            newElementRecorder.ScriptElements = editor.ScriptElements;

            //show form
            newElementRecorder.ShowDialog();

            editor.HTMLElementRecorderURL = newElementRecorder.StartURL;
            editor.ScriptElements = newElementRecorder.ScriptElements;

            try
            {
                if (newElementRecorder.SearchParameters != null)
                {
                    v_SeleniumSearchParameters.Rows.Clear();

                    foreach (DataRow rw in newElementRecorder.SearchParameters.Rows)
                        v_SeleniumSearchParameters.ImportRow(rw);

                    _searchParametersGridViewHelper.DataSource = v_SeleniumSearchParameters;
                    _searchParametersGridViewHelper.Refresh();
                }               
            }
            catch (Exception)
            {
                //Search parameter not found
            }
        }

        public void SeleniumAction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SeleniumElementActionCommand cmd = this;
            DataTable actionParameters = cmd.v_WebActionParameterTable;

            if (sender != null)
                actionParameters.Rows.Clear();

            switch (_elementActionDropdown.SelectedItem)
            {
                case "Invoke Click":
                case "Clear Element":
                    foreach (var ctrl in _elementParameterControls)
                        ctrl.Hide();
                    break;

                case "Left Click":
                case "Middle Click":
                case "Right Click":
                case "Double Left Click":
                    foreach (var ctrl in _elementParameterControls)
                        ctrl.Show();

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("X Adjustment", 0);
                        actionParameters.Rows.Add("Y Adjustment", 0);
                    }
                    break;

                case "Set Text":
                    foreach (var ctrl in _elementParameterControls)
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
                        _elementsGridViewHelper.Rows[2].Cells[1] = encryptedBox;
                        _elementsGridViewHelper.Rows[2].Cells[1].Value = "Not Encrypted";

                        var buttonCell = new DataGridViewButtonCell();
                        _elementsGridViewHelper.Rows[3].Cells[1] = buttonCell;
                        _elementsGridViewHelper.Rows[3].Cells[1].Value = "Encrypt Text";
                        _elementsGridViewHelper.CellContentClick += ElementsGridViewHelper_CellContentClick;
                    }

                    DataGridViewComboBoxCell comparisonComboBox = new DataGridViewComboBoxCell();
                    comparisonComboBox.Items.Add("Yes");
                    comparisonComboBox.Items.Add("No");

                    //assign cell as a combobox
                    if (sender != null)
                        _elementsGridViewHelper.Rows[1].Cells[1].Value = "No";

                    _elementsGridViewHelper.Rows[1].Cells[1] = comparisonComboBox;
                    break;

                case "Set Secure Text":
                    foreach (var ctrl in _elementParameterControls)
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
                        _elementsGridViewHelper.Rows[1].Cells[1].Value = "No";

                    _elementsGridViewHelper.Rows[1].Cells[1] = _comparisonComboBox; 
                    break;

                case "Get Text":
                case "Get Matching Element(s)":
                case "Get Table":
                case "Get Count":
                    foreach (var ctrl in _elementParameterControls)
                        ctrl.Show();

                    if (sender != null)
                        actionParameters.Rows.Add("Variable Name");
                    break;

                case "Get Attribute":
                    foreach (var ctrl in _elementParameterControls)
                        ctrl.Show();

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Attribute Name");
                        actionParameters.Rows.Add("Variable Name");
                    }
                    break;

                case "Get Options":
                    actionParameters.Rows.Add("Attribute Name");
                    actionParameters.Rows.Add("Variable Name");
                    break;

                case "Select Option":
                    actionParameters.Rows.Add("Selection Type");
                    actionParameters.Rows.Add("Selection Parameter");

                    DataGridViewComboBoxCell selectionTypeBox = new DataGridViewComboBoxCell();
                    selectionTypeBox.Items.Add("Select By Index");
                    selectionTypeBox.Items.Add("Select By Text");
                    selectionTypeBox.Items.Add("Select By Value");
                    selectionTypeBox.Items.Add("Deselect By Index");
                    selectionTypeBox.Items.Add("Deselect By Text");
                    selectionTypeBox.Items.Add("Deselect By Value");
                    selectionTypeBox.Items.Add("Deselect All");

                    //assign cell as a combobox
                    if (sender != null)
                        _elementsGridViewHelper.Rows[0].Cells[1].Value = "Select By Text";

                    _elementsGridViewHelper.Rows[0].Cells[1] = selectionTypeBox;
                    break;

                case "Wait For Element To Exist":
                    foreach (var ctrl in _elementParameterControls)
                        ctrl.Show();
       
                    if (sender != null)
                        actionParameters.Rows.Add("Timeout (Seconds)");
                    break;

                case "Switch to frame":
                    foreach (var ctrl in _elementParameterControls)
                        ctrl.Hide();
                    break;

                default:
                    break;
            }
            _elementsGridViewHelper.DataSource = v_WebActionParameterTable;
        }

        private void ElementsGridViewHelper_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var targetCell = _elementsGridViewHelper.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (targetCell is DataGridViewButtonCell && targetCell.Value.ToString() == "Encrypt Text")
            {
                var targetElement = _elementsGridViewHelper.Rows[0].Cells[1];

                if (string.IsNullOrEmpty(targetElement.Value.ToString()))
                    return;

                var warning = MessageBox.Show($"Warning! Text should only be encrypted one time and is not reversible in the builder. " +
                                               "Would you like to proceed and convert '{targetElement.Value.ToString()}' to an encrypted value?", 
                                               "Encryption Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (warning == DialogResult.Yes)
                {
                    targetElement.Value = EncryptionServices.EncryptString(targetElement.Value.ToString(), "OPENBOTS");
                    _elementsGridViewHelper.Rows[2].Cells[1].Value = "Encrypted";
                }
            }
        }

        #region drag/drop events
        private void SearchParametersGridViewHelper_MouseDown(object sender, MouseEventArgs e)
        {
            var hitTest = _searchParametersGridViewHelper.HitTest(e.X, e.Y);
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

        private void SearchParametersGridViewHelper_MouseUp(object sender, MouseEventArgs e)
        {
            _dragBoxFromMouseDown = Rectangle.Empty;
        }

        private void SearchParametersGridViewHelper_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) != MouseButtons.Left)
                return;
            if (_dragBoxFromMouseDown == Rectangle.Empty || _dragBoxFromMouseDown.Contains(e.X, e.Y))
                return;
            if (_indexOfItemUnderMouseToDrag < 0)
                return;          

            var row = _searchParametersGridViewHelper.Rows[_indexOfItemUnderMouseToDrag];
            _searchParametersGridViewHelper.DoDragDrop(row, DragDropEffects.All);

            //Clear
            _searchParametersGridViewHelper.ClearSelection();
            //Set
            if (_indexOfItemUnderMouseToDrop > -1)
                _searchParametersGridViewHelper.Rows[_indexOfItemUnderMouseToDrop].Selected = true;
        }

        private void SearchParametersGridViewHelper_DragOver(object sender, DragEventArgs e)
        {
            Point p = _searchParametersGridViewHelper.PointToClient(new Point(e.X, e.Y));
            var hitTest = _searchParametersGridViewHelper.HitTest(p.X, p.Y);
            if (hitTest.Type != DataGridViewHitTestType.Cell || hitTest.RowIndex == _indexOfItemUnderMouseToDrag)
            {
                e.Effect = DragDropEffects.None;
                return;
            }
            e.Effect = DragDropEffects.Move;
        }

        private void SearchParametersGridViewHelper_DragDrop(object sender, DragEventArgs e)
        {
            Point p = _searchParametersGridViewHelper.PointToClient(new Point(e.X, e.Y));
            var hitTest = _searchParametersGridViewHelper.HitTest(p.X, p.Y);
            if (hitTest.Type != DataGridViewHitTestType.Cell || hitTest.RowIndex == _indexOfItemUnderMouseToDrag + 1)
                return;

            _indexOfItemUnderMouseToDrop = hitTest.RowIndex;

            var tempRow = v_SeleniumSearchParameters.NewRow();
            tempRow.ItemArray = v_SeleniumSearchParameters.Rows[_indexOfItemUnderMouseToDrag].ItemArray;
            v_SeleniumSearchParameters.Rows.RemoveAt(_indexOfItemUnderMouseToDrag);

            if (_indexOfItemUnderMouseToDrag < _indexOfItemUnderMouseToDrop)
                _indexOfItemUnderMouseToDrop--;

            v_SeleniumSearchParameters.Rows.InsertAt(tempRow, _indexOfItemUnderMouseToDrop);
        }
        #endregion
    }
}