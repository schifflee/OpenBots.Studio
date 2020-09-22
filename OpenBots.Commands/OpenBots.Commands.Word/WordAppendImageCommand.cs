using Microsoft.Office.Interop.Word;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using Application = Microsoft.Office.Interop.Word.Application;
using Group = OpenBots.Core.Attributes.ClassAttributes.Group;

namespace OpenBots.Commands.Word
{
    [Serializable]
    [Group("Word Commands")]
    [Description("This command appends an image to a Word Document.")]

    public class WordAppendImageCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Word Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyWordInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Image File Path")]    
        [InputSpecification("Enter the file path of the image to append to the Document.")]
        [SampleUsage(@"C:\temp\myImage.png || {vImageFilePath} || {ProjectPath}\myImage.png")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_ImagePath { get; set; }

        public WordAppendImageCommand()
        {
            CommandName = "WordAppendImageCommand";
            SelectionName = "Append Image";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultWord";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vImagePath = v_ImagePath.ConvertUserVariableToString(engine);
            var wordObject = v_InstanceName.GetAppInstance(engine);

            Application wordInstance = (Application)wordObject;
            Document wordDocument = wordInstance.ActiveDocument;

            //Appends image after text/images
            object collapseEnd = WdCollapseDirection.wdCollapseEnd;
            Range imageRange = wordDocument.Content;
            imageRange.Collapse(ref collapseEnd);
            imageRange.InlineShapes.AddPicture(vImagePath, Type.Missing, Type.Missing, imageRange);

            Paragraph paragraph = wordDocument.Content.Paragraphs.Add();
            paragraph.Format.SpaceAfter = 10f;
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ImagePath", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Append '{v_ImagePath}' - Instance Name '{v_InstanceName}']";
        }
    }
}