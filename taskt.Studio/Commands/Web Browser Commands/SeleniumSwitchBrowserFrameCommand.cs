using OpenQA.Selenium;
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
    [Group("Web Browser Commands")]
    [Description("This command switches between browser frames provided a valid search parameter.")]
   
    public class SeleniumSwitchBrowserFrameCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Browser Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Browser** command.")]
        [SampleUsage("MyBrowserInstance")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Frame Search Type")]
        [PropertyUISelectionOption("Index")]
        [PropertyUISelectionOption("Name or ID")]
        [PropertyUISelectionOption("Parent Frame")]
        [PropertyUISelectionOption("Default Content")]
        [PropertyUISelectionOption("Alert")]
        [InputSpecification("Select an option which best fits the search type you would like to use.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_SelectionType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Frame Search Parameter")]
        [InputSpecification("Provide the parameter to match (ex. Index, Name or ID).")]
        [SampleUsage("1 || name || {vSearchData}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_FrameParameter { get; set; }

        public SeleniumSwitchBrowserFrameCommand()
        {
            CommandName = "SeleniumSwitchBrowserFrameCommand";
            SelectionName = "Switch Browser Frame";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultBrowser";
            v_SelectionType = "Index";
            v_FrameParameter = "0";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var browserObject = v_InstanceName.GetAppInstance(engine);
            var seleniumInstance = (IWebDriver)browserObject;
            var frameIndex = v_FrameParameter.ConvertUserVariableToString(engine);

            switch (v_SelectionType)
            {
                case "Index":
                    var intFrameIndex = int.Parse(frameIndex);
                    seleniumInstance.SwitchTo().Frame(intFrameIndex);
                    break;
                case "Name or ID":
                    seleniumInstance.SwitchTo().Frame(frameIndex);
                    break;
                case "Parent Frame":
                    seleniumInstance.SwitchTo().ParentFrame();
                    break;
                case "Default Content":
                    seleniumInstance.SwitchTo().DefaultContent();
                    break;
                case "Alert":
                    seleniumInstance.SwitchTo().Alert();
                    break;
                default:
                    throw new NotImplementedException($"Logic to Select Frame '{v_SelectionType}' Not Implemented");
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_SelectionType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FrameParameter", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [To {v_SelectionType} '{v_FrameParameter}' - Instance Name '{v_InstanceName}']";
        }
    }
}