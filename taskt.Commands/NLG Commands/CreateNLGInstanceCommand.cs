using SimpleNLG;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("NLG Commands")]
    [Description("This command creates a Natural Language Generation Instance.")]
    public class CreateNLGInstanceCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("NLG Instance Name")]
        [InputSpecification("Enter a unique name that will represent the application instance.")]
        [SampleUsage("MyNLGInstance")]
        [Remarks("This unique name allows you to refer to the instance by name in future commands, " +
                 "ensuring that the commands you specify run against the correct application.")]
        public string v_InstanceName { get; set; }

        public CreateNLGInstanceCommand()
        {
            CommandName = "CreateNLGInstanceCommand";
            SelectionName = "Create NLG Instance";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultNLG";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
  
            Lexicon lexicon = Lexicon.getDefaultLexicon();
            NLGFactory nlgFactory = new NLGFactory(lexicon);
            SPhraseSpec p = nlgFactory.createClause();

            p.AddAppInstance(engine, v_InstanceName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [New Instance Name '{v_InstanceName}']";
        }
    }
}