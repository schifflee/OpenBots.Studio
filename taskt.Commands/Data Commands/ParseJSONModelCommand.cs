﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Data Commands")]
    [Description("This command runs a number of queries on a JSON object and saves the results in the specified list variables.")]
    public class ParseJSONModelCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("JSON Object")]
        [InputSpecification("Provide a variable or JSON object value.")]
        [SampleUsage("{\"rect\":{\"length\":10, \"width\":5}} || {vJsonObject}")]
        [Remarks("Providing data of a type other than a 'JSON Object' will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_JsonObject { get; set; }

        [XmlElement]
        [PropertyDescription("Parameters")]
        [InputSpecification("Specify JSON Selector(s) (JPath) and Output Variable(s).")]
        [SampleUsage("[$.rect.length | vOutputList] || [{Selector} | {vOutputList}]")]
        [Remarks("'$.rect.length' is a JSON Selector to query on an inputted JSON Object and store its results in {vOutputList}.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_ParseObjects { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _parseObjectsGridViewHelper;

        public ParseJSONModelCommand()
        {
            CommandName = "ParseJSONModelCommand";
            SelectionName = "Parse JSON Model";
            CommandEnabled = true;
            CustomRendering = true;

            v_ParseObjects = new DataTable();
            v_ParseObjects.Columns.Add("Json Selector");
            v_ParseObjects.Columns.Add("Output Variable");
            v_ParseObjects.TableName = $"ParseJsonObjectsTable{DateTime.Now.ToString("MMddyyhhmmss")}";

            _parseObjectsGridViewHelper = new DataGridView();
            _parseObjectsGridViewHelper.AllowUserToAddRows = true;
            _parseObjectsGridViewHelper.AllowUserToDeleteRows = true;
            _parseObjectsGridViewHelper.Size = new Size(400, 250);
            _parseObjectsGridViewHelper.ColumnHeadersHeight = 30;
            _parseObjectsGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _parseObjectsGridViewHelper.DataBindings.Add("DataSource", this, "v_ParseObjects", false, DataSourceUpdateMode.OnPropertyChanged);
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            
            //get variablized input
            var variableInput = v_JsonObject.ConvertUserVariableToString(engine);

            foreach (DataRow rw in v_ParseObjects.Rows)
            {
                var jsonSelector = rw.Field<string>("Json Selector").ConvertUserVariableToString(engine);
                var targetVariableName = rw.Field<string>("Output Variable").ConvertUserVariableToString(engine);

                //create objects
                JObject o;
                IEnumerable<JToken> searchResults;
                List<string> resultList = new List<string>();

                //parse json
                try
                {
                    o = JObject.Parse(variableInput);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error Occured Parsing Tokens: " + ex.ToString());
                }

                //select results
                try
                {
                    searchResults = o.SelectTokens(jsonSelector);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error Occured Selecting Tokens: " + ex.ToString());
                }

                //add results to result list since list<string> is supported
                foreach (var result in searchResults)
                {
                    resultList.Add(result.ToString());
                }

                resultList.StoreInUserVariable(engine, targetVariableName);               
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_JsonObject", this, editor));

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_ParseObjects", this));
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_ParseObjects", this, new[] { _parseObjectsGridViewHelper }, editor));
            RenderedControls.Add(_parseObjectsGridViewHelper);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return $"{base.GetDisplayValue()} [Select {v_ParseObjects.Rows.Count} Item(s) From JSON]";
        }
    }
}