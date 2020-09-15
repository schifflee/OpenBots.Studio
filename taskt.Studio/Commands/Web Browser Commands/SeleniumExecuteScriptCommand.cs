﻿using OpenQA.Selenium;
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
using taskt.UI.CustomControls;

namespace taskt.Commands
{

    [Serializable]
    [Group("Web Browser Commands")]
    [Description("This command allows you to execute a script in a Selenium web browser session.")]

    public class SeleniumExecuteScriptCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Browser Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Browser** command.")]
        [SampleUsage("MyBrowserInstance")]
        [Remarks("Failure to enter the correct instance name or failure to first call the **Create Browser** command will cause an error.")]
        public string v_InstanceName { get; set; } 

        [XmlAttribute]
        [PropertyDescription("Script Code")]
        [InputSpecification("Enter the script code to execute.")]
        [SampleUsage("arguments[0].click(); || alert('Welcome to OpenBots'); || {vScript}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ScriptCode { get; set; }

        [XmlAttribute]
        [PropertyDescription("Arguments")]
        [InputSpecification("Enter any necessary arguments.")]
        [SampleUsage("button || {vArguments}")]
        [Remarks("This input is optional.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Arguments { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Data Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public SeleniumExecuteScriptCommand()
        {
            CommandName = "SeleniumExecuteScriptCommand";
            SelectionName = "Execute Script";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultBrowser";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var browserObject = v_InstanceName.GetAppInstance(engine);
            var script = v_ScriptCode.ConvertUserVariableToString(engine);
            var args = v_Arguments.ConvertUserVariableToString(engine);
            var seleniumInstance = (IWebDriver)browserObject;
            IJavaScriptExecutor js = (IJavaScriptExecutor)seleniumInstance;

            object result;
            if (string.IsNullOrEmpty(args))
                result = js.ExecuteScript(script);
            else
                result = js.ExecuteScript(script, args);

            //apply result to variable
            if ((result != null) && (!string.IsNullOrEmpty(v_OutputUserVariableName)))
                result.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ScriptCode", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Arguments", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Store Data in '{v_OutputUserVariableName}' - Instance Name '{v_InstanceName}']";
        }
    }
}