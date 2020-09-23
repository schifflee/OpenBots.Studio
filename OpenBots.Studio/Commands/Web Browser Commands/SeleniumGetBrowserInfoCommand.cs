using Newtonsoft.Json;
using OpenQA.Selenium;
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

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Web Browser Commands")]
    [Description("This command retrieves information from a Selenium web browser session.")]

    public class SeleniumGetBrowserInfoCommand : ScriptCommand
    {
        [PropertyDescription("Browser Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Browser** command.")]
        [SampleUsage("MyBrowserInstance")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
        public string v_InstanceName { get; set; }
        [PropertyDescription("Info Property")]
        [PropertyUISelectionOption("Window Title")]
        [PropertyUISelectionOption("Window URL")]
        [PropertyUISelectionOption("Current Handle ID")]
        [PropertyUISelectionOption("HTML Page Source")]
        [PropertyUISelectionOption("Handle ID List")]
        [InputSpecification("Indicate which info property to retrieve.")]
        [SampleUsage("")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InfoType { get; set; }
        [PropertyDescription("Output Info Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public SeleniumGetBrowserInfoCommand()
        {
            CommandName = "SeleniumGetBrowserInfoCommand";
            SelectionName = "Get Browser Info";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultBrowser";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var browserObject = v_InstanceName.GetAppInstance(engine);
            var seleniumInstance = (IWebDriver)browserObject;
            var requestedInfo = v_InfoType.ConvertUserVariableToString(engine);
            string info;

            switch (requestedInfo)
            {
                case "Window Title":
                    info = seleniumInstance.Title;
                    break;
                case "Window URL":
                    info = seleniumInstance.Url;
                    break;
                case "Current Handle ID":
                    info = seleniumInstance.CurrentWindowHandle;
                    break;
                case "HTML Page Source":
                    info = seleniumInstance.PageSource;
                    break;
                case "Handle ID List":
                    info = JsonConvert.SerializeObject(seleniumInstance.WindowHandles);
                    break;
                default:
                    throw new NotImplementedException($"{requestedInfo} is not implemented for lookup.");
            }
            //store data
            info.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_InfoType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Get {v_InfoType} - Store Info in '{v_OutputUserVariableName}' - Instance Name '{v_InstanceName}']";
        }
    }
}