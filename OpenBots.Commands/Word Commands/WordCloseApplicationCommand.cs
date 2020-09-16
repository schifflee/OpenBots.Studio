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
using Application = Microsoft.Office.Interop.Word.Application;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Word Commands")]
    [Description("This command closes an open Word Document and Instance.")]

    public class WordCloseApplicationCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Word Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyWordInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Save Document")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Indicate whether the Document should be saved before closing.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_WordSaveOnExit { get; set; }

        public WordCloseApplicationCommand()
        {
            CommandName = "WordCloseApplicationCommand";
            SelectionName = "Close Word Application";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultWord";
            v_WordSaveOnExit = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var wordObject = v_InstanceName.GetAppInstance(engine);
            Application wordInstance = (Application)wordObject;
            bool saveOnExit;
            if (v_WordSaveOnExit == "Yes")
                saveOnExit = true;
            else
                saveOnExit = false;

            //check if document exists and save
            if (wordInstance.Documents.Count >= 1)
                wordInstance.ActiveDocument.Close(saveOnExit);

            //close word
            wordInstance.Quit();

            //remove instance
            v_InstanceName.RemoveAppInstance(engine);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_WordSaveOnExit", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Save on Close '{v_WordSaveOnExit}' - Instance Name '{v_InstanceName}']";
        }
    }
}