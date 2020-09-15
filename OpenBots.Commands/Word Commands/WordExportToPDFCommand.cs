using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using Application = Microsoft.Office.Interop.Word.Application;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Word Commands")]
    [Description("This command exports a Word Document to a PDF file.")]

    public class WordExportToPDFCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Word Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyWordInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("PDF Location")]
        [InputSpecification("Enter or Select the path of the folder to export the PDF to.")]
        [SampleUsage(@"C:\temp || {vFolderPath} || {ProjectPath}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_FolderPath { get; set; }

        [XmlAttribute]
        [PropertyDescription("PDF File Name")]
        [InputSpecification("Enter or Select the name of the PDF file.")]
        [SampleUsage("myFile.pdf || {vFilename}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_FileName { get; set; }

        public WordExportToPDFCommand()
        {
            CommandName = "WordExportToPDFCommand";
            SelectionName = "Export To PDF";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultWord";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vFileName = v_FileName.ConvertUserVariableToString(engine);
            var vFolderPath = v_FolderPath.ConvertUserVariableToString(engine);

            //get word app object
            var wordObject = v_InstanceName.GetAppInstance(engine);

            //convert object
            Application wordInstance = (Application)wordObject;
            Document wordDocument = wordInstance.ActiveDocument;
            
            object fileFormat = WdSaveFormat.wdFormatPDF;
            string pdfPath = Path.Combine(vFolderPath, vFileName);
            wordDocument.SaveAs(pdfPath, ref fileFormat, Type.Missing, Type.Missing,
                                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                Type.Missing, Type.Missing, Type.Missing, Type.Missing,
                                Type.Missing, Type.Missing, Type.Missing, Type.Missing);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FolderPath", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FileName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Export to '{v_FolderPath}\\{v_FileName}' - Instance Name '{v_InstanceName}']";
        }
    }
}