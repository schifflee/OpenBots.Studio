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
    [Description("This command appends text to a Word Document.")]

    public class WordAppendTextCommand : ScriptCommand
    {
        [PropertyDescription("Word Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyWordInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        public string v_InstanceName { get; set; }
        [PropertyDescription("Text")]
        [InputSpecification("Enter the text to append to the Document.")]
        [SampleUsage("Hello World || {vText}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_TextToSet { get; set; }
        [PropertyDescription("Font Name")]
        [PropertyUISelectionOption("Arial")]
        [PropertyUISelectionOption("Calibri")]
        [PropertyUISelectionOption("Helvetica")]
        [PropertyUISelectionOption("Times New Roman")]
        [PropertyUISelectionOption("Verdana")]
        [InputSpecification("Select or provide a valid font name.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_FontName { get; set; }
        [PropertyDescription("Font Size")]
        [PropertyUISelectionOption("10")]
        [PropertyUISelectionOption("11")]
        [PropertyUISelectionOption("12")]
        [PropertyUISelectionOption("14")]
        [PropertyUISelectionOption("16")]
        [PropertyUISelectionOption("18")]
        [PropertyUISelectionOption("20")]
        [InputSpecification("Select or provide a valid font size.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_FontSize { get; set; }
        [PropertyDescription("Bold")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the text font should be bold.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_FontBold { get; set; }
        [PropertyDescription("Italic")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the text font should be italic.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_FontItalic { get; set; }
        [PropertyDescription("Underline")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether the text font should be underlined.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_FontUnderline { get; set; }

        public WordAppendTextCommand()
        {
            CommandName = "WordAppendTextCommand";
            SelectionName = "Append Text";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultWord";
            v_FontName = "Calibri";
            v_FontSize = "11";
            v_FontBold = "No";
            v_FontItalic = "No";
            v_FontUnderline = "No";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vText = v_TextToSet.ConvertUserVariableToString(engine);
            var wordObject = v_InstanceName.GetAppInstance(engine);

            Application wordInstance = (Application)wordObject;
            Document wordDocument = wordInstance.ActiveDocument;

            Paragraph paragraph = wordDocument.Content.Paragraphs.Add();
            paragraph.Range.Text = vText;
            paragraph.Range.Font.Name = v_FontName;
            paragraph.Range.Font.Size = float.Parse(v_FontSize);

            if (v_FontBold == "Yes")
                paragraph.Range.Font.Bold = 1;
            else 
                paragraph.Range.Font.Bold = 0;

            if (v_FontItalic == "Yes")
                paragraph.Range.Font.Italic = 1;
            else 
                paragraph.Range.Font.Italic = 0;

            if (v_FontUnderline == "Yes")
                paragraph.Range.Font.Underline = WdUnderline.wdUnderlineSingle;
            else 
                paragraph.Range.Font.Underline = WdUnderline.wdUnderlineNone;

            paragraph.Range.InsertParagraphAfter();
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_FontName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_FontSize", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_FontBold", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_FontItalic", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_FontUnderline", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Append '{v_TextToSet}' - Instance Name '{v_InstanceName}']";
        }
    }
}