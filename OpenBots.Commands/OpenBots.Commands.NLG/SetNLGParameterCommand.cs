using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using SimpleNLG;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OpenBots.Commands.NLG
{
    [Serializable]
    [Group("NLG Commands")]
    [Description("This command defines a Natural Language Generation parameter.")]
    public class SetNLGParameterCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("NLG Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create NLG Instance** command.")]
        [SampleUsage("MyNLGInstance")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **Create NLG Instance** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("NLG Parameter Type")]
        [PropertyUISelectionOption("Set Subject")]
        [PropertyUISelectionOption("Set Verb")]
        [PropertyUISelectionOption("Set Object")]
        [PropertyUISelectionOption("Add Complement")]
        [PropertyUISelectionOption("Add Modifier")]
        [PropertyUISelectionOption("Add Pre-Modifier")]
        [PropertyUISelectionOption("Add Front Modifier")]
        [PropertyUISelectionOption("Add Post Modifier")]
        [InputSpecification("Select the appropriate Natural Language Generation Parameter.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_ParameterType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Input Value")]
        [InputSpecification("Enter the value that should be associated with the parameter")]
        [SampleUsage("Hello || {vValue}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Parameter { get; set; }

        public SetNLGParameterCommand()
        {
            CommandName = "SetNLGParameterCommand";
            SelectionName = "Set NLG Parameter";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultNLG";
            v_ParameterType = "Set Subject";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var p = (SPhraseSpec)v_InstanceName.GetAppInstance(engine);

            var userInput = v_Parameter.ConvertUserVariableToString(engine);

            switch (v_ParameterType)
            {
                case "Set Subject":
                    p.setSubject(userInput);
                    break;
                case "Set Object":
                    p.setObject(userInput);
                    break;
                case "Set Verb":
                    p.setVerb(userInput);
                    break;
                case "Add Complement":
                    p.addComplement(userInput);
                    break;
                case "Add Modifier":
                    p.addModifier(userInput);             
                    break;
                case "Add Front Modifier":
                    p.addFrontModifier(userInput);
                    break;
                case "Add Post Modifier":
                    p.addPostModifier(userInput);
                    break;
                case "Add Pre-Modifier":
                    p.addPreModifier(userInput);
                    break;
                default:
                    break;
            }

            //remove existing associations if override app instances is not enabled
            v_InstanceName.RemoveAppInstance(engine);

            //add to app instance to track
            p.AddAppInstance(engine, v_InstanceName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ParameterType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Parameter", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_ParameterType} '{v_Parameter}' - Instance Name '{v_InstanceName}']";
        }
    }
}