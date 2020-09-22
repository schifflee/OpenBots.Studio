using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OpenBots.Commands.Data
{
    [Serializable]
    [Group("Data Commands")]
    [Description("This command performs a specific operation on a date and saves the result in a variable.")]
    public class DateCalculationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Date")]
        [InputSpecification("Specify either text or a variable that contains the date.")]
        [SampleUsage("1/1/2000 || {DateTime.Now}")]
        [Remarks("You can use known text or variables.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputDate { get; set; }

        [XmlAttribute]
        [PropertyDescription("Calculation Method")]
        [PropertyUISelectionOption("Add Second(s)")]
        [PropertyUISelectionOption("Add Minute(s)")]
        [PropertyUISelectionOption("Add Hour(s)")]
        [PropertyUISelectionOption("Add Day(s)")]
        [PropertyUISelectionOption("Add Month(s)")]
        [PropertyUISelectionOption("Add Year(s)")]
        [PropertyUISelectionOption("Subtract Second(s)")]
        [PropertyUISelectionOption("Subtract Minute(s)")]
        [PropertyUISelectionOption("Subtract Hour(s)")]
        [PropertyUISelectionOption("Subtract Day(s)")]
        [PropertyUISelectionOption("Subtract Month(s)")]
        [PropertyUISelectionOption("Subtract Year(s)")]
        [PropertyUISelectionOption("Get Next Day")]
        [PropertyUISelectionOption("Get Next Month")]
        [PropertyUISelectionOption("Get Next Year")]
        [PropertyUISelectionOption("Get Previous Day")]
        [PropertyUISelectionOption("Get Previous Month")]
        [PropertyUISelectionOption("Get Previous Year")]
        [InputSpecification("Select the date operation.")]
        [SampleUsage("")]
        [Remarks("The selected operation will be applied to the input date value and result will be stored in the output variable.")]
        public string v_CalculationMethod { get; set; }

        [XmlAttribute]
        [PropertyDescription("Increment Value")]
        [InputSpecification("Specify how many units to increment by.")]
        [SampleUsage("15 || {vIncrement}")]
        [Remarks("You can use negative numbers which will do the opposite, ex. Subtract Days and an increment of -5 will Add Days.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Increment { get; set; }

        [XmlAttribute]
        [PropertyDescription("Date Format (Optional)")]
        [InputSpecification("Specify the output date format.")]
        [SampleUsage("MM/dd/yy hh:mm:ss || MM/dd/yyyy || {vDateFormat}")]
        [Remarks("You can specify either a valid DateTime, Date or Time Format; an invalid format will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ToStringFormat { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Date Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public DateCalculationCommand()
        {
            CommandName = "DateCalculationCommand";
            SelectionName = "Date Calculation";
            CommandEnabled = true;
            CustomRendering = true;

            v_InputDate = "{DateTime.Now}";
            v_ToStringFormat = "MM/dd/yyyy hh:mm:ss";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            //get variablized string
            var variableDateTime = v_InputDate.ConvertUserVariableToString(engine);
            var formatting = v_ToStringFormat.ConvertUserVariableToString(engine);
            var variableIncrement = v_Increment.ConvertUserVariableToString(engine);

            //convert to date time
            DateTime requiredDateTime;
            if (!DateTime.TryParse(variableDateTime, out requiredDateTime))
                throw new InvalidDataException("Date was unable to be parsed - " + variableDateTime);

            //get increment value
            double requiredInterval;

            //convert to double
            if (!double.TryParse(variableIncrement, out requiredInterval))
                throw new InvalidDataException("Date was unable to be parsed - " + variableIncrement);

            dynamic dateTimeValue;

            //perform operation
            switch (v_CalculationMethod)
            {
                case "Add Second(s)":
                    dateTimeValue = requiredDateTime.AddSeconds(requiredInterval);
                    break;
                case "Add Minute(s)":
                    dateTimeValue = requiredDateTime.AddMinutes(requiredInterval);
                    break;
                case "Add Hour(s)":
                    dateTimeValue = requiredDateTime.AddHours(requiredInterval);
                    break;
                case "Add Day(s)":
                    dateTimeValue = requiredDateTime.AddDays(requiredInterval);
                    break;
                case "Add Month(s)":
                    dateTimeValue = requiredDateTime.AddMonths((int)requiredInterval);
                    break;
                case "Add Year(s)":
                    dateTimeValue = requiredDateTime.AddYears((int)requiredInterval);
                    break;
                case "Subtract Second(s)":
                    dateTimeValue = requiredDateTime.AddSeconds((requiredInterval * -1));
                    break;
                case "Subtract Minute(s)":
                    dateTimeValue = requiredDateTime.AddMinutes((requiredInterval * -1));
                    break;
                case "Subtract Hour(s)":
                    dateTimeValue = requiredDateTime.AddHours(requiredInterval * -1);
                    break;
                case "Subtract Day(s)":
                    dateTimeValue = requiredDateTime.AddDays(requiredInterval * -1);
                    break;
                case "Subtract Month(s)":
                    dateTimeValue = requiredDateTime.AddMonths((int)requiredInterval * -1);
                    break;
                case "Subtract Year(s)":
                    dateTimeValue = requiredDateTime.AddYears((int)requiredInterval * -1);
                    break;
                case "Get Next Day":
                    dateTimeValue = requiredDateTime.AddDays(requiredInterval).Day;
                    break;
                case "Get Next Month":
                    dateTimeValue = requiredDateTime.AddMonths((int)requiredInterval).Month;
                    break;
                case "Get Next Year":
                    dateTimeValue = requiredDateTime.AddYears((int)requiredInterval).Year;
                    break;
                case "Get Previous Day":
                    dateTimeValue = requiredDateTime.AddDays(requiredInterval * -1).Day;
                    break;
                case "Get Previous Month":
                    dateTimeValue = requiredDateTime.AddMonths((int)requiredInterval * -1).Month;
                    break;
                case "Get Previous Year":
                    dateTimeValue = requiredDateTime.AddYears((int)requiredInterval * -1).Year;
                    break;
                default:
                    dateTimeValue = "";
                    break;
            }

            string stringDateFormatted;

            //handle if formatter is required
            if (!string.IsNullOrEmpty(formatting.Trim()))
                stringDateFormatted = ((DateTime)dateTimeValue).ToString(formatting);
            else
                stringDateFormatted = ((object)dateTimeValue).ToString();

            //store string (Result) in variable
            stringDateFormatted.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //create standard group controls
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputDate", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_CalculationMethod", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Increment", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ToStringFormat", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            //determine operand and interval
            var operand = v_CalculationMethod.Split(' ').First();
            var interval = v_CalculationMethod.Split(' ').Last();

            //additional language handling based on selection made
            string operandLanguage;
            if (operand == "Add")
                operandLanguage = "to";
            else
                operandLanguage = "from";

            if (operand == "Get")
                operand = v_CalculationMethod.Replace(interval, "").TrimEnd();

            //return value
            return base.GetDisplayValue() + $" [{operand} '{v_Increment}' {interval} {operandLanguage} '{v_InputDate}' - Store Date in '{v_OutputUserVariableName}']";
        }
    }
}