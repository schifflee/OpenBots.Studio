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
using OpenBots.Core.Properties;
using OpenBots.UI.Forms;

namespace OpenBots.Commands
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

            if (engine.ScriptEngineUI == null)
            {
                engine.ReportProgress("HTML UserInput Supported With UI Only");
                MessageBox.Show("HTML UserInput Supported With UI Only", "UserInput Command", 
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //invoke ui for data collection
            var result = ((frmScriptEngine)engine.ScriptEngineUI).Invoke(new Action(() =>
            {
                //sample for temp testing
                var htmlInput = v_InputHTML.ConvertUserVariableToString(engine);

                var variables = engine.ScriptEngineUI.ShowHTMLInput(htmlInput);

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

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InputHTML", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ErrorOnClose", this, editor));
            
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue();
        }
    }
}