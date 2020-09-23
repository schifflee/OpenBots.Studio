using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using SimpleNLG;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenBots.Commands.NLG
{
    [Serializable]
    [Group("NLG Commands")]
    [Description("This command produces a Natural Language Generation phrase.")]
    public class GenerateNLGPhraseCommand : ScriptCommand
    {
        [PropertyDescription("NLG Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create NLG Instance** command.")]
        [SampleUsage("MyNLGInstance")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **Create NLG Instance** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [PropertyDescription("Output Phrase Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public GenerateNLGPhraseCommand()
        {
            CommandName = "GenerateNLGPhraseCommand";
            SelectionName = "Generate NLG Phrase";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultNLG";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var p = (SPhraseSpec)v_InstanceName.GetAppInstance(engine);

            Lexicon lexicon = Lexicon.getDefaultLexicon();
            Realiser realiser = new Realiser(lexicon);

            string phraseOutput = realiser.realiseSentence(p);
            phraseOutput.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Store Phrase in '{v_OutputUserVariableName}' - Instance Name '{v_InstanceName}']";
        }
    }
}