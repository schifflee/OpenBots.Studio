using System;
using System.Collections.Generic;
using System.Security;
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
    [Group("Secure Data Commands")]
    [Description("This command adds text as a SecureString into a variable.")]
    public class NewSecureStringCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Input Text")]
        [InputSpecification("Enter the text for the variable.")]
        [SampleUsage("Some Text || {vText}")]
        [Remarks("You can use variables in input if you encase them within braces {vText}. You can also perform basic math operations.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Input { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output SecureString Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public NewSecureStringCommand()
        {
            CommandName = "NewSecureStringCommand";
            SelectionName = "New SecureString";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            SecureString secureStringValue = v_Input.ConvertUserVariableToString(engine).GetSecureString();

            secureStringValue.StoreInUserVariable(engine, v_OutputUserVariableName);           
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultPasswordInputGroupFor("v_Input", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Convert '{v_Input}' - Store SecureString in '{v_OutputUserVariableName}']";
        }
    }
}
