using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using Application = Microsoft.Office.Interop.Excel.Application;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command exports a Excel Worksheet to a PDF file.")]

    public class ExcelExportToPDFCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance || {vExcelInstance}")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
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

        [XmlAttribute]
        [PropertyDescription("AutoFit Cells")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Indicate whether to autofit cell sizes to fit their contents.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_AutoFitCells { get; set; }

        [XmlAttribute]
        [PropertyDescription("Display Gridlines")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Indicate whether to display Worksheet gridlines.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_DisplayGridlines { get; set; }

        public ExcelExportToPDFCommand()
        {
            CommandName = "ExcelExportToPDFCommand";
            SelectionName = "Export To PDF";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
            v_AutoFitCells = "Yes";
            v_DisplayGridlines = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vFileName = v_FileName.ConvertUserVariableToString(engine);
            var vFolderPath = v_FolderPath.ConvertUserVariableToString(engine);

            //get excel app object
            var excelObject = v_InstanceName.GetAppInstance(engine);

            //convert object
            Application excelInstance = (Application)excelObject;
            Worksheet excelWorksheet = excelInstance.ActiveSheet;

            var fileFormat = XlFixedFormatType.xlTypePDF;
            string pdfPath = Path.Combine(vFolderPath, vFileName);
            
            if (v_AutoFitCells == "Yes")
            {
                excelWorksheet.Columns.AutoFit();
                excelWorksheet.Rows.AutoFit();
            }

            if (v_DisplayGridlines == "Yes")
            {
                Range last = excelWorksheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
                Range range = excelWorksheet.Range["A1", last];
                range.Borders[XlBordersIndex.xlEdgeLeft].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeRight].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeTop].LineStyle = XlLineStyle.xlContinuous;
                range.Borders[XlBordersIndex.xlEdgeBottom].LineStyle = XlLineStyle.xlContinuous;
                range.Borders.Color = Color.Black;
            }
                
            excelWorksheet.ExportAsFixedFormat(fileFormat, pdfPath);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FolderPath", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FileName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_AutoFitCells", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_DisplayGridlines", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Export to '{v_FolderPath}\\{v_FileName}' - Instance Name '{v_InstanceName}']";
        }
    }
}