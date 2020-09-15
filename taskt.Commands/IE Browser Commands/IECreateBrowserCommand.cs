﻿using SHDocVw;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.App;
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
    [Group("IE Browser Commands")]
    [Description("This command creates a new IE Web Browser Session.")]
    public class IECreateBrowserCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("IE Browser Instance Name")]
        [InputSpecification("Enter a unique name that will represent the application instance.")]
        [SampleUsage("MyIEBrowserInstance")]
        [Remarks("This unique name allows you to refer to the instance by name in future commands, " +
                 "ensuring that the commands you specify run against the correct application.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("URL")]
        [InputSpecification("Enter a Web URL to navigate to.")]
        [SampleUsage("https://example.com/ || {vURL}")]
        [Remarks("This input is optional.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_URL { get; set; }

        [XmlAttribute]
        [PropertyDescription("Instance Tracking (after task ends)")]
        [PropertyUISelectionOption("Forget Instance")]
        [PropertyUISelectionOption("Keep Instance Alive")]
        [InputSpecification("Specify if taskt should remember this instance name after the script has finished executing.")]
        [SampleUsage("")]
        [Remarks("Calling the **Close Browser** command or closing the application will end the instance.")]
        public string v_InstanceTracking { get; set; }

        public IECreateBrowserCommand()
        {
            CommandName = "IECreateBrowserCommand";
            SelectionName = "Create IE Browser";
            v_InstanceName = "DefaultIEBrowser";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceTracking = "Forget Instance";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var webURL = v_URL.ConvertUserVariableToString(engine);

            InternetExplorer newBrowserSession = new InternetExplorer();

            if (!string.IsNullOrEmpty(webURL.Trim()))
            {
                try
                {
                    newBrowserSession.Navigate(webURL);
                    WaitForReadyState(newBrowserSession);
                    newBrowserSession.Visible = true;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
                
            //add app instance
            newBrowserSession.AddAppInstance(engine, v_InstanceName);

            //handle app instance tracking
            if (v_InstanceTracking == "Keep Instance Alive")
                GlobalAppInstances.AddInstance(v_InstanceName, newBrowserSession);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_URL", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_InstanceTracking", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [New Instance Name '{v_InstanceName}']";
        }

        public static void WaitForReadyState(InternetExplorer ieInstance)
        {
            try
            {
                DateTime waitExpires = DateTime.Now.AddSeconds(15);

                do
                {
                    Thread.Sleep(500);
                }

                while ((ieInstance.ReadyState != tagREADYSTATE.READYSTATE_COMPLETE) && (waitExpires > DateTime.Now));
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
    }
}