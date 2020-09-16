using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
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
    [Description("This command uploads data to a local OpenBots Server BotStore.")]

    public class UploadBotStoreDataCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Key")]
        [InputSpecification("Select or provide the name of the key to create.")]
        [SampleUsage("Hello || {vKey}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Key { get; set; }

        [XmlAttribute]
        [PropertyDescription("Value")]
        [InputSpecification("Select or provide a value for the key.")]
        [SampleUsage("World || {vValue}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Value { get; set; }

        public UploadBotStoreDataCommand()
        {
            CommandName = "UploadBotStoreDataCommand";
            SelectionName = "Upload BotStore Data";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var keyName = v_Key.ConvertUserVariableToString(engine);
            var keyValue = v_Value.ConvertUserVariableToString(engine);
            
            try
            {
                var result = HttpServerClient.UploadData(keyName, keyValue);
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
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Value", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Upload Key/Value '({v_Key},{v_Value})' to OpenBotsServer BotStore]";
        }
    }
}