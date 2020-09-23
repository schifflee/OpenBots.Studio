using OpenQA.Selenium;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
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
    [Group("Web Browser Commands")]
    [Description("This command allows you to navigate a Selenium web browser session to a given URL or resource.")]

    public class SeleniumNavigateToURLCommand : ScriptCommand
    {

        [PropertyDescription("Browser Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Browser** command.")]
        [SampleUsage("MyBrowserInstance")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [PropertyDescription("URL")]
        [InputSpecification("Enter the URL that you want the selenium instance to navigate to.")]
        [SampleUsage("https://mycompany.com/orders || {vURL}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_URL { get; set; }

        public SeleniumNavigateToURLCommand()
        {
            CommandName = "SeleniumNavigateToURLCommand";
            SelectionName = "Navigate to URL";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultBrowser";
            v_URL = "https://";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var browserObject = v_InstanceName.GetAppInstance(engine);
            var vURL = v_URL.ConvertUserVariableToString(engine);
            var seleniumInstance = (IWebDriver)browserObject;

            try
            {
                seleniumInstance.Navigate().GoToUrl(vURL);
            }
            catch (Exception ex)
            {
                if (!vURL.StartsWith("https://"))
                    seleniumInstance.Navigate().GoToUrl("https://" + vURL);
                else
                    throw ex;
            }           
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
            return base.GetDisplayValue() + $" [URL '{v_URL}' - Instance Name '{v_InstanceName}']";
        }

        private void WaitForReadyState(InternetExplorer ieInstance)
        {
            DateTime waitExpires = DateTime.Now.AddSeconds(15);
            do
            {
                Thread.Sleep(500);
            }
            while ((ieInstance.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE) && (waitExpires > DateTime.Now));
        }
    }
}