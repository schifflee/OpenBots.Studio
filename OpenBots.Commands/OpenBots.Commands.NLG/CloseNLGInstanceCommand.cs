using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OpenBots.Commands.NLG
{
    [Serializable]
    [Group("NLG Commands")]
    [Description("This command closes a Natural Language Generation Instance.")]
    public class CloseNLGInstanceCommand : ScriptCommand
    {
        [PropertyDescription("NLG Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create NLG Instance** command.")]
        [SampleUsage("MyNLGInstance")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **Create NLG Instance** command will cause an error.")]
        public string v_InstanceName { get; set; }

        public CloseNLGInstanceCommand()
        {
            CommandName = "CloseNLGInstanceCommand";
            SelectionName = "Close NLG Instance";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultNLG";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            v_InstanceName.RemoveAppInstance(engine);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Instance Name '{v_InstanceName}']";
        }
    }
}