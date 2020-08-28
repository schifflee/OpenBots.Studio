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
using taskt.Properties;
using taskt.UI.CustomControls;
using taskt.UI.Forms;

namespace taskt.Commands
{
    [Serializable]
    [Group("Input Commands")]
    [Description("This command provides the user with an HTML form to input and store a collection of data.")]
    public class HTMLInputCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("HTML")]
        [InputSpecification("Define the form to be displayed using the HTML Builder.")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowHTMLBuilder)]
        public string v_InputHTML { get; set; }

        [XmlAttribute]
        [PropertyDescription("Error On Close")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify if an exception should be thrown on any result other than 'OK'.")]
        [SampleUsage("")]
        [Remarks("")]      
        public string v_ErrorOnClose { get; set; }

        public HTMLInputCommand()
        {
            CommandName = "HTMLInputCommand";
            SelectionName = "Prompt for HTML Input";
            CommandEnabled = true;
            CustomRendering = true;
            v_InputHTML = Resources.HTMLInputSample;
            v_ErrorOnClose = "No";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            if (engine.TasktEngineUI == null)
            {
                engine.ReportProgress("HTML UserInput Supported With UI Only");
                MessageBox.Show("HTML UserInput Supported With UI Only", "UserInput Command", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //invoke ui for data collection
            var result = ((frmScriptEngine)engine.TasktEngineUI).Invoke(new Action(() =>
            {
                //sample for temp testing
                var htmlInput = v_InputHTML.ConvertUserVariableToString(engine);

                var variables = engine.TasktEngineUI.ShowHTMLInput(htmlInput);

                //if user selected Ok then process variables
                //null result means user cancelled/closed
                if (variables != null)
                {
                    //store each one into context
                    foreach (var variable in variables)
                        variable.VariableValue.ToString().StoreInUserVariable(engine, variable.VariableName);
                }
                else if (v_ErrorOnClose == "Yes")
                {
                    throw new Exception("Input Form was closed by the user");
                }
            }
            ));
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputHTML", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ErrorOnClose", this, editor));
            
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}