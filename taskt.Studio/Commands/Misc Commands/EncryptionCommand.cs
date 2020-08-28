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
    [Description("This command handles text encryption")]
    [UsesDescription("Use this command when you want to store some data encrypted")]
    [ImplementationDescription("")]
    public class EncryptionCommand : ScriptCommand
    {
        [XmlElement]
        [PropertyDescription("Select Encryption Action")]
        [PropertyUISelectionOption("Encrypt")]
        [PropertyUISelectionOption("Decrypt")]
        [InputSpecification("Select an action to take")]
        [SampleUsage("Select from **Encrypt**, **Decrypt**")]
        [Remarks("")]
        public string v_EncryptionType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Supply the data or variable (ex. {someVariable})")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable or json array value")]
        [SampleUsage("**Test** or **{var}**")]
        [Remarks("")]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyDescription("Provide a Pass Phrase")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [InputSpecification("Select or provide a variable or json array value")]
        [SampleUsage("**Test** or **{var}**")]
        [Remarks("")]
        public string v_PassPhrase { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Encrypted Data Variable")]
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

            //get variablized input
            var variableInput = v_InputValue.ConvertUserVariableToString(engine);
            var passphrase = v_PassPhrase.ConvertUserVariableToString(engine);

            string resultData = "";
            if (v_EncryptionType.ConvertUserVariableToString(engine) == "Encrypt")
            {
                //encrypt data
                resultData = EncryptionServices.EncryptString(variableInput, passphrase);
            }
            else if (v_EncryptionType.ConvertUserVariableToString(engine) == "Decrypt")
            {
                //encrypt data
                resultData = EncryptionServices.DecryptString(variableInput, passphrase);
            }
            else
            {
                throw new NotImplementedException($"Encryption Service Requested '{v_EncryptionType.ConvertUserVariableToString(engine)}' has not been implemented");
            }

            resultData.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_EncryptionType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_PassPhrase", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;

        }



        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_EncryptionType} Data, apply to '{v_OutputUserVariableName}']";
        }
    }
}