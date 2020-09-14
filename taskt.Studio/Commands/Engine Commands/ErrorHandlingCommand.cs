using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;
using taskt.Engine;

namespace taskt.Commands
{
    [Serializable]
    [Group("Engine Commands")]
    [Description("This command specifies what to do if an error is encountered during execution.")]
    public class ErrorHandlingCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Error Action")]
        [PropertyUISelectionOption("Stop Processing")]
        [PropertyUISelectionOption("Continue Processing")]
        [InputSpecification("Select the action to take when the bot comes across an error.")]
        [SampleUsage("")]
        [Remarks("**If Command** allows you to specify and test if a line number encountered an error. "+
                 "In order to use that functionality, you must specify **Continue Processing**.")]
        public string v_ErrorHandlingAction { get; set; }

        public ErrorHandlingCommand()
        {
            CommandName = "ErrorHandlingCommand";
            SelectionName = "Error Handling";
            CommandEnabled = true;
            CustomRendering = true;
            v_ErrorHandlingAction = "Stop Processing";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            engine.ErrorHandlingAction = v_ErrorHandlingAction;
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ErrorHandlingAction", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" ['{v_ErrorHandlingAction}']";
        }
    }
}