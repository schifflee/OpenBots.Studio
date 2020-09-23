//Copyright (c) 2019 Jason Bayldon
//Modifications - Copyright (c) 2020 OpenBots Inc.
//
//Licensed under the Apache License, Version 2.0 (the "License");
//you may not use this file except in compliance with the License.
//You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
//Unless required by applicable law or agreed to in writing, software
//distributed under the License is distributed on an "AS IS" BASIS,
//WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//See the License for the specific language governing permissions and
//limitations under the License.

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using OpenBots.Server;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Engine Commands")]
    [Description("This command retrives data from a local OpenBots Server BotStore.")]
    public class GetBotStoreDataCommand : ScriptCommand
    {

        [PropertyDescription("Key")]
        [InputSpecification("Select or provide the name of the key to retrieve.")]
        [SampleUsage("Hello || {vKey}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Key { get; set; }

        [PropertyDescription("Retrieval Option")]
        [PropertyUISelectionOption("Retrieve Value")]
        [PropertyUISelectionOption("Retrieve Entire Record")]
        [InputSpecification("Indicate whether to retrieve the whole record or just the value.")]
        [SampleUsage("")]
        [Remarks("Depending on the option selected, the whole record with metadata may be retrieved.")]     
        public string v_DataOption { get; set; }

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

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Key", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_DataOption", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Get Data From Key '{v_Key}' in OpenBots Server BotStore - Store Data in '{v_OutputUserVariableName}']";
        }
    }




}







