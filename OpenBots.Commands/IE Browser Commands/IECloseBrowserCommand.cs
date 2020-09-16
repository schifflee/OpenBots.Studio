using SHDocVw;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("IE Browser Commands")]
    [Description("This command closes the associated IE Web Browser.")]
    public class IECloseBrowserCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("IE Browser Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **IE Create Browser** command.")]
        [SampleUsage("MyIEBrowserInstance")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **IE Create Browser** command will cause an error.")]
        public string v_InstanceName { get; set; }

        public IECloseBrowserCommand()
        {
            CommandName = "IECloseBrowserCommand";
            SelectionName = "Close IE Browser";
            CommandEnabled = true;
            v_InstanceName = "DefaultIEBrowser";
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            var browserObject = v_InstanceName.GetAppInstance(engine);

            var browserInstance = (InternetExplorer)browserObject;
            browserInstance.Quit();

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