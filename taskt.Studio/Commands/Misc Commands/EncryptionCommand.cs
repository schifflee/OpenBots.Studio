using System;
using System.Collections.Generic;
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
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Misc Commands")]
    [Description("This command encrypts or decrypts text.")]
    public class EncryptionCommand : ScriptCommand
    {
        [XmlElement]
        [PropertyDescription("Encryption Action")]
        [PropertyUISelectionOption("Encrypt")]
        [PropertyUISelectionOption("Decrypt")]
        [InputSpecification("Select the appropriate action to take.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_EncryptionType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text")]
        [InputSpecification("Select or provide the text to encrypt/decrypt.")]
        [SampleUsage("Hello || {vText}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyDescription("Pass Phrase")]
        [InputSpecification("Select or provide a pass phrase for encryption/decryption.")]
        [SampleUsage("TASKT || {vPassPhrase}")]
        [Remarks("If decrypting, provide the pass phrase used to encypt the original text.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_PassPhrase { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Result Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public EncryptionCommand()
        {
            CommandName = "EncryptionCommand";
            SelectionName = "Encryption Command";
            CommandEnabled = true;
            CustomRendering = true;
            v_EncryptionType = "Encrypt";
            v_PassPhrase = "TASKT";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            var variableInput = v_InputValue.ConvertUserVariableToString(engine);
            var passphrase = v_PassPhrase.ConvertUserVariableToString(engine);

            string resultData = "";
            if (v_EncryptionType == "Encrypt")
                resultData = EncryptionServices.EncryptString(variableInput, passphrase);
            else if (v_EncryptionType == "Decrypt")
                resultData = EncryptionServices.DecryptString(variableInput, passphrase);

            resultData.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_EncryptionType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_PassPhrase", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_EncryptionType} '{v_InputValue}' - Store Result in '{v_OutputUserVariableName}']";
        }
    }
}