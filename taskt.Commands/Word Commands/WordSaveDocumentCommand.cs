﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;
using Application = Microsoft.Office.Interop.Word.Application;

namespace taskt.Commands
{
    [Serializable]
    [Group("Word Commands")]
    [Description("This command saves a Word Document.")]

    public class WordSaveDocumentCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Word Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyWordInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        public string v_InstanceName { get; set; }

        public WordSaveDocumentCommand()
        {
            CommandName = "WordSaveDocumentCommand";
            SelectionName = "Save Document";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultWord";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            //get word app object
            var wordObject = v_InstanceName.GetAppInstance(engine);

            //convert object
            Application wordInstance = (Application)wordObject;

            //save
            wordInstance.ActiveDocument.Save();
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Instance Name '{v_InstanceName}']";
        }
    }
}