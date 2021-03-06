﻿using MSHTML;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Engine;
using SHDocVw;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OpenBots.Commands.IEBrowser
{
    [Serializable]
    [Group("IE Browser Commands")]
    [Description("This command finds and attaches to an existing IE Web Browser session.")]
    public class IEFindBrowserCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("IE Browser Instance Name")]
        [InputSpecification("Enter a unique name that will represent the application instance.")]
        [SampleUsage("MyIEBrowserInstance")]
        [Remarks("This unique name allows you to refer to the instance by name in future commands, " +
                 "ensuring that the commands you specify run against the correct application.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Browser Name (Title)")]
        [InputSpecification("Select the Name (Title) of the IE Browser Instance to get attached to.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_IEBrowserName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        private ComboBox _ieBrowerNameDropdown;

        public IEFindBrowserCommand()
        {
            CommandName = "IEFindBrowserCommand";
            SelectionName = "Find IE Browser";
            v_InstanceName = "DefaultIEBrowser";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            bool browserFound = false;
            var shellWindows = new ShellWindows();
            foreach (IWebBrowser2 shellWindow in shellWindows)
            {
                if ((shellWindow.Document is HTMLDocument) && (v_IEBrowserName==null || shellWindow.Document.Title == v_IEBrowserName))
                {
                    shellWindow.Application.AddAppInstance(engine, v_InstanceName);
                    browserFound = true;
                    break;
                }
            }

            //try partial match
            if (!browserFound)
            {
                foreach (IWebBrowser2 shellWindow in shellWindows)
                {
                    if ((shellWindow.Document is HTMLDocument) && 
                        ((shellWindow.Document.Title.Contains(v_IEBrowserName) || 
                        shellWindow.Document.Url.Contains(v_IEBrowserName))))
                    {
                        shellWindow.Application.AddAppInstance(engine, v_InstanceName);
                        browserFound = true;
                        break;
                    }
                }
            }

            if (!browserFound)
            {
                throw new Exception("Browser was not found!");
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

            _ieBrowerNameDropdown = (ComboBox)commandControls.CreateDropdownFor("v_IEBrowserName", this);
            var shellWindows = new ShellWindows();
            foreach (IWebBrowser2 shellWindow in shellWindows)
            {
                if (shellWindow.Document is HTMLDocument)
                    _ieBrowerNameDropdown.Items.Add(shellWindow.Document.Title);
            }
            RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_IEBrowserName", this));
            RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_IEBrowserName", this, new Control[] { _ieBrowerNameDropdown }, editor));
            RenderedControls.Add(_ieBrowerNameDropdown);

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Having Title '{v_IEBrowserName}' - Instance Name '{v_InstanceName}']";
        }
    }

}