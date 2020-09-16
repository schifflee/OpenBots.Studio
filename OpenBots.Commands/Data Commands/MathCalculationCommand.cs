using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Data Commands")]
    [Description("This command performs a math calculation and saves the result in a variable.")]
    public class MathCalculationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Math Expression")]
        [InputSpecification("Specify either text or a variable that contains a valid math expression.")]
        [SampleUsage("(2 + 5) * 3 || ({vNumber1} + {vNumber2}) * {vNumber3}")]
        [Remarks("You can use known numbers or variables.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_MathExpression { get; set; }

        [XmlAttribute]
        [PropertyDescription("Thousand Separator (Optional)")]
        [InputSpecification("Specify the seperator used to identify decimal places.")]
        [SampleUsage(", || . || {vThousandSeparator}")]
        [Remarks("Typically a comma or a decimal point (period), like in 100,000, ',' is a thousand separator.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ThousandSeparator { get; set; }

        [XmlAttribute]
        [PropertyDescription("Decimal Separator (Optional)")]
        [InputSpecification("Specify the seperator used to identify decimal places.")]
        [SampleUsage(". || , || {vDecimalSeparator}")]
        [Remarks("Typically a comma or a decimal point (period), like in 60.99, '.' is a decimal separator.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DecimalSeparator { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Result Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public MathCalculationCommand()
        {
            CommandName = "MathCalculationCommand";
            SelectionName = "Math Calculation";
            CommandEnabled = true;
            CustomRendering = true;

            v_MathExpression = "(2 + 5) * 3";
            v_DecimalSeparator = ".";
            v_ThousandSeparator = ",";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            //get variablized string
            var variableMath = v_MathExpression.ConvertUserVariableToString(engine);

            try
            {
                var decimalSeparator = v_DecimalSeparator.ConvertUserVariableToString(engine);
                var thousandSeparator = v_ThousandSeparator.ConvertUserVariableToString(engine);

                //remove thousandths markers
                variableMath = variableMath.Replace(thousandSeparator, "");

                //check decimal seperator
                if (decimalSeparator != ".")
                {
                    variableMath = variableMath.Replace(decimalSeparator, ".");
                }

                //perform compute
                DataTable dt = new DataTable();
                var result = dt.Compute(variableMath, "");

                //restore decimal seperator
                if (decimalSeparator != ".")
                {
                    result = result.ToString().Replace(".", decimalSeparator);
                }
               
                //store string in variable
                result.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
            }
            catch (Exception ex)
            {
                //throw exception is math calc failed
                throw ex;
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            //create standard group controls
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MathExpression", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ThousandSeparator", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DecimalSeparator", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Compute '{v_MathExpression}' - Store Result in '{v_OutputUserVariableName}']";
        }
    }
}