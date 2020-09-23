using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Error Handling Commands")]
    [Description("This command defines a catch block whose commands will execute if an exception is thrown from the " +
                 "associated try.")]
    public class CatchCommand : ScriptCommand
    {

        [PropertyDescription("Exception Type")]
        [PropertyUISelectionOption("AccessViolationException")]
        [PropertyUISelectionOption("ArgumentException")]
        [PropertyUISelectionOption("ArgumentNullException")]
        [PropertyUISelectionOption("ArgumentOutOfRangeException")]
        [PropertyUISelectionOption("DivideByZeroException")]
        [PropertyUISelectionOption("Exception")]
        [PropertyUISelectionOption("FileNotFoundException")]
        [PropertyUISelectionOption("FormatException")]
        [PropertyUISelectionOption("IndexOutOfRangeException")]
        [PropertyUISelectionOption("InvalidDataException")]
        [PropertyUISelectionOption("InvalidOperationException")]
        [PropertyUISelectionOption("KeyNotFoundException")]
        [PropertyUISelectionOption("NotSupportedException")]
        [PropertyUISelectionOption("NullReferenceException")]
        [PropertyUISelectionOption("OverflowException")]
        [PropertyUISelectionOption("TimeoutException")]
        [InputSpecification("Select the appropriate exception type.")]
        [SampleUsage("")]
        [Remarks("This command will be executed if the type of the exception that occurred in the try block matches the selected exception type.")]
        public string v_ExceptionType { get; set; }

        public string ErrorMessage { get; set; }
        public string StackTrace { get; set; }

        public CatchCommand()
        {
            CommandName = "CatchCommand";
            SelectionName = "Catch";
            CommandEnabled = true;
            CustomRendering = true;
            v_ExceptionType = "Exception";
        }

        public override void RunCommand(object sender)
        {
            //no execution required, used as a marker by the Automation Engine
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ExceptionType", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" ({v_ExceptionType})";
        }
    }
}
