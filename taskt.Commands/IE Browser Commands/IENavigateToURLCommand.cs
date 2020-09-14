using SHDocVw;
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

namespace taskt.Commands
{
    [Serializable]
    [Group("IE Browser Commands")]
    [Description("This command navigates an existing IE web browser session to a given URL or web resource.")]
    public class IENavigateToURLCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("IE Browser Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **IE Create Browser** command.")]
        [SampleUsage("MyIEBrowserInstance")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **IE Create Browser** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Navigate to URL")]
        [InputSpecification("Enter the destination URL that you want the IE instance to navigate to.")]
        [SampleUsage("https://example.com/ || {vURL}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_URL { get; set; }

        public IENavigateToURLCommand()
        {
            CommandName = "IENavigateToURLCommand";
            SelectionName = "IE Navigate to URL";
            v_InstanceName = "DefaultIEBrowser";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            var browserObject = v_InstanceName.GetAppInstance(engine);
            var browserInstance = (InternetExplorer)browserObject;

            browserInstance.Navigate(v_URL.ConvertUserVariableToString(engine));
            IECreateBrowserCommand.WaitForReadyState(browserInstance);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_URL", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Navigate to '{v_URL}' - Instance Name '{v_InstanceName}']";
        }
    }
}