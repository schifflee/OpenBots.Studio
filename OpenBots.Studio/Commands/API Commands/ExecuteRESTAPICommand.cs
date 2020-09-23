using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;

namespace OpenBots.Commands
{

    [Serializable]
    [Group("API Commands")]
    [Description("This command calls a REST API with a specific HTTP method.")]

    public class ExecuteRESTAPICommand : ScriptCommand
    {

        [PropertyDescription("Base URL")]
        [InputSpecification("Provide the base URL of the API.")]
        [SampleUsage("https://example.com || {vMyUrl}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_BaseURL { get; set; }

        [PropertyDescription("Endpoint")]
        [InputSpecification("Define any API endpoint which contains the full URL.")]
        [SampleUsage("/v2/getUser/1 || {vMyUrl}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_APIEndPoint { get; set; }

        [PropertyDescription("Method Type")]
        [PropertyUISelectionOption("GET")]
        [PropertyUISelectionOption("POST")]
        [InputSpecification("Select the necessary method type.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_APIMethodType { get; set; }

        [PropertyDescription("Request Format")]
        [PropertyUISelectionOption("Json")]
        [PropertyUISelectionOption("Xml")]
        [PropertyUISelectionOption("None")]
        [InputSpecification("Select the necessary request format.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_RequestFormat { get; set; }

        [PropertyDescription("Basic REST Parameters")]
        [InputSpecification("Specify default search parameters.")]
        [SampleUsage("")]
        [Remarks("Once you have clicked on a valid window the search parameters will be populated." +
                 " Enable only the ones required to be a match at runtime.")]
        public DataTable v_RESTParameters { get; set; }

        [PropertyDescription("Advanced REST Parameters")]
        [InputSpecification("Specify a list of advanced parameters.")]
        [SampleUsage("")]
        [Remarks("")]
        public DataTable v_AdvancedParameters { get; set; }

        [PropertyDescription("Output Response Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        [JsonIgnore]
        [NonSerialized]
        private DataGridView _RESTParametersGridViewHelper;

        [JsonIgnore]
        [NonSerialized]
        private DataGridView _advancedRESTParametersGridViewHelper;

        public ExecuteRESTAPICommand()
        {
            CommandName = "ExecuteRESTAPICommand";
            SelectionName = "Execute REST API";
            CommandEnabled = true;
            CustomRendering = true;
            v_RequestFormat = "Json";

            v_RESTParameters = new DataTable();
            v_RESTParameters.Columns.Add("Parameter Type");
            v_RESTParameters.Columns.Add("Parameter Name");
            v_RESTParameters.Columns.Add("Parameter Value");
            v_RESTParameters.TableName = DateTime.Now.ToString("RESTParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));

            //advanced parameters
            v_AdvancedParameters = new DataTable();
            v_AdvancedParameters.Columns.Add("Parameter Name");
            v_AdvancedParameters.Columns.Add("Parameter Value");
            v_AdvancedParameters.Columns.Add("Content Type");
            v_AdvancedParameters.Columns.Add("Parameter Type");
            v_AdvancedParameters.TableName = DateTime.Now.ToString("AdvRESTParamTable" + DateTime.Now.ToString("MMddyy.hhmmss"));
        }

        public override void RunCommand(object sender)
        {
            try
            {
                //make REST Request and store into variable
                string restContent;

                //get engine instance
                var engine = (AutomationEngineInstance)sender;

                //get parameters
                var targetURL = v_BaseURL.ConvertUserVariableToString(engine);
                var targetEndpoint = v_APIEndPoint.ConvertUserVariableToString(engine);
                var targetMethod = v_APIMethodType.ConvertUserVariableToString(engine);

                //client
                var client = new RestClient(targetURL);

                //methods
                Method method = (Method)Enum.Parse(typeof(Method), targetMethod);

                //rest request
                var request = new RestRequest(targetEndpoint, method);

                //get parameters
                var apiParameters = v_RESTParameters.AsEnumerable().Where(rw => rw.Field<string>("Parameter Type") == "PARAMETER");

                //get headers
                var apiHeaders = v_RESTParameters.AsEnumerable().Where(rw => rw.Field<string>("Parameter Type") == "HEADER");

                //for each api parameter
                foreach (var param in apiParameters)
                {
                    var paramName = ((string)param["Parameter Name"]).ConvertUserVariableToString(engine);
                    var paramValue = ((string)param["Parameter Value"]).ConvertUserVariableToString(engine);

                    request.AddParameter(paramName, paramValue);
                }

                //for each header
                foreach (var header in apiHeaders)
                {
                    var paramName = ((string)header["Parameter Name"]).ConvertUserVariableToString(engine);
                    var paramValue = ((string)header["Parameter Value"]).ConvertUserVariableToString(engine);

                    request.AddHeader(paramName, paramValue);
                }

                //get json body
                var jsonBody = v_RESTParameters.AsEnumerable().Where(rw => rw.Field<string>("Parameter Type") == "JSON BODY")
                                               .Select(rw => rw.Field<string>("Parameter Value")).FirstOrDefault();

                //add json body
                if (jsonBody != null)
                {
                    var json = jsonBody.ConvertUserVariableToString(engine);
                    request.AddJsonBody(jsonBody);
                }

                //get json body
                var file = v_RESTParameters.AsEnumerable().Where(rw => rw.Field<string>("Parameter Type") == "FILE").FirstOrDefault();

                //get file
                if (file != null)
                {
                    var paramName = ((string)file["Parameter Name"]).ConvertUserVariableToString(engine);
                    var paramValue = ((string)file["Parameter Value"]).ConvertUserVariableToString(engine);
                    var fileData = paramValue.ConvertUserVariableToString(engine);
                    request.AddFile(paramName, fileData);

                }

                //add advanced parameters
                foreach (DataRow rw in v_AdvancedParameters.Rows)
                {
                    var paramName = rw.Field<string>("Parameter Name").ConvertUserVariableToString(engine);
                    var paramValue = rw.Field<string>("Parameter Value").ConvertUserVariableToString(engine);
                    var paramType = rw.Field<string>("Parameter Type").ConvertUserVariableToString(engine);
                    var contentType = rw.Field<string>("Content Type").ConvertUserVariableToString(engine);

                    var param = new Parameter();

                    if (!string.IsNullOrEmpty(contentType))
                        param.ContentType = contentType;

                    if (!string.IsNullOrEmpty(paramType))
                        param.Type = (ParameterType)Enum.Parse(typeof(ParameterType), paramType);


                    param.Name = paramName;
                    param.Value = paramValue;

                    request.Parameters.Add(param);
                }

                var requestFormat = v_RequestFormat.ConvertUserVariableToString(engine);
                if (string.IsNullOrEmpty(requestFormat))
                {
                    requestFormat = "Xml";
                }
                request.RequestFormat = (DataFormat)Enum.Parse(typeof(DataFormat), requestFormat);


                //execute client request
                IRestResponse response = client.Execute(request);
                var content = response.Content;

                //add service response for tracking
                engine.ServiceResponses.Add(response);

                // return response.Content;
                try
                {
                    //try to parse and return json content
                    var result = JsonConvert.DeserializeObject(content);
                    restContent = result.ToString();
                }
                catch (Exception)
                {
                    //content failed to parse simply return it
                    restContent = content;
                }

                restContent.StoreInUserVariable(engine, v_OutputUserVariableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_BaseURL", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_APIEndPoint", this, editor));

            var apiMethodLabel = commandControls.CreateDefaultLabelFor("v_APIMethodType", this);
            var apiMethodDropdown = (ComboBox)commandControls.CreateDropdownFor("v_APIMethodType", this);
            foreach (Method method in (Method[])Enum.GetValues(typeof(Method)))
            {
                apiMethodDropdown.Items.Add(method.ToString());
            }
            RenderedControls.Add(apiMethodLabel);
            RenderedControls.Add(apiMethodDropdown);

            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_RequestFormat", this, editor));
            RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_RESTParameters", this));

            _RESTParametersGridViewHelper = new DataGridView();
            _RESTParametersGridViewHelper.Width = 500;
            _RESTParametersGridViewHelper.Height = 140;
            _RESTParametersGridViewHelper.ColumnHeadersHeight = 30;
            _RESTParametersGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _RESTParametersGridViewHelper.AutoGenerateColumns = false;

            var selectColumn = new DataGridViewComboBoxColumn();
            selectColumn.HeaderText = "Type";
            selectColumn.DataPropertyName = "Parameter Type";
            selectColumn.DataSource = new string[] { "HEADER", "PARAMETER", "JSON BODY", "FILE" };
            _RESTParametersGridViewHelper.Columns.Add(selectColumn);

            var paramNameColumn = new DataGridViewTextBoxColumn();
            paramNameColumn.HeaderText = "Name";
            paramNameColumn.DataPropertyName = "Parameter Name";
            _RESTParametersGridViewHelper.Columns.Add(paramNameColumn);

            var paramValueColumn = new DataGridViewTextBoxColumn();
            paramValueColumn.HeaderText = "Value";
            paramValueColumn.DataPropertyName = "Parameter Value";
            _RESTParametersGridViewHelper.Columns.Add(paramValueColumn);

            _RESTParametersGridViewHelper.DataBindings.Add("DataSource", this, "v_RESTParameters", false, DataSourceUpdateMode.OnPropertyChanged);
            RenderedControls.Add(_RESTParametersGridViewHelper);

            RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_AdvancedParameters", this));

            //advanced parameters
            _advancedRESTParametersGridViewHelper = new DataGridView();
            _advancedRESTParametersGridViewHelper.Width = 500;
            _advancedRESTParametersGridViewHelper.Height = 140;
            _advancedRESTParametersGridViewHelper.ColumnHeadersHeight = 30;
            _advancedRESTParametersGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _advancedRESTParametersGridViewHelper.AutoGenerateColumns = false;

            var advParamNameColumn = new DataGridViewTextBoxColumn();
            advParamNameColumn.HeaderText = "Name";
            advParamNameColumn.DataPropertyName = "Parameter Name";
            _advancedRESTParametersGridViewHelper.Columns.Add(advParamNameColumn);

            var advParamValueColumns = new DataGridViewTextBoxColumn();
            advParamValueColumns.HeaderText = "Value";
            advParamValueColumns.DataPropertyName = "Parameter Value";
            _advancedRESTParametersGridViewHelper.Columns.Add(advParamValueColumns);

            var advParamContentType = new DataGridViewTextBoxColumn();
            advParamContentType.HeaderText = "Content Type";
            advParamContentType.DataPropertyName = "Content Type";
            _advancedRESTParametersGridViewHelper.Columns.Add(advParamContentType);

            var advParamType = new DataGridViewComboBoxColumn();
            advParamType.HeaderText = "Parameter Type";
            advParamType.DataPropertyName = "Parameter Type";
            advParamType.DataSource = new string[] { "Cookie", "GetOrPost", "HttpHeader", "QueryString", "RequestBody", "URLSegment", "QueryStringWithoutEncode" };
            _advancedRESTParametersGridViewHelper.Columns.Add(advParamType);

            _advancedRESTParametersGridViewHelper.DataBindings.Add("DataSource", this, "v_AdvancedParameters", false, DataSourceUpdateMode.OnPropertyChanged);
            RenderedControls.Add(_advancedRESTParametersGridViewHelper);

            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Target URL '{v_BaseURL}{v_APIEndPoint}' - Store Response in '{v_OutputUserVariableName}']";
        }
    }
}

