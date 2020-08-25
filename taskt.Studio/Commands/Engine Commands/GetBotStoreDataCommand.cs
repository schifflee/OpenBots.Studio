using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.Server;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Engine Commands")]
    [Description("This command retrives data from a local tasktServer BotStore.")]
    public class GetBotStoreDataCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Key")]
        [InputSpecification("Select or provide the name of the key to retrieve.")]
        [SampleUsage("Hello || {vKey}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Key { get; set; }

        [XmlAttribute]
        [PropertyDescription("Retrieval Option")]
        [PropertyUISelectionOption("Retrieve Value")]
        [PropertyUISelectionOption("Retrieve Entire Record")]
        [InputSpecification("Indicate whether to retrieve the whole record or just the value.")]
        [SampleUsage("")]
        [Remarks("Depending on the option selected, the whole record with metadata may be retrieved.")]     
        public string v_DataOption { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Data Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public GetBotStoreDataCommand()
        {
            CommandName = "GetBotStoreDataCommand";
            SelectionName = "Get BotStore Data";
            CommandEnabled = true;
            CustomRendering = true;
            v_DataOption = "Retrieve Value";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var keyName = v_Key.ConvertUserVariableToString(engine);

            BotStoreRequestType requestType;
            if (v_DataOption == "Retrieve Entire Record")
                requestType = BotStoreRequestType.BotStoreModel;
            else
                requestType = BotStoreRequestType.BotStoreValue;

            try
            {
                var result = HttpServerClient.GetData(keyName, requestType);

                if (requestType == BotStoreRequestType.BotStoreValue)
                    result = JsonConvert.DeserializeObject<string>(result);

                result.StoreInUserVariable(engine, v_OutputUserVariableName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Key", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_DataOption", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Get Data From Key '{v_Key}' in tasktServer BotStore - Store Data in '{v_OutputUserVariableName}']";
        }
    }




}







