using Microsoft.Office.Interop.Excel;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;

namespace OpenBots.Commands.Excel
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command gets the range from an Excel Worksheet and stores it in a DataTable.")]

    public class ExcelGetRangeCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Range")]
        [InputSpecification("Enter the location of the range to extract.")]
        [SampleUsage("A1:B10 || A1: || {vRange} || {vStart}:{vEnd} || {vStart}:")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Range { get; set; }   

        [XmlAttribute]
        [PropertyDescription("Add Headers")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("When selected, the column headers from the specified spreadsheet range are also extracted.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_AddHeaders { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Range Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public ExcelGetRangeCommand()
        {
            CommandName = "ExcelGetRangeCommand";
            SelectionName = "Get Range";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
            v_AddHeaders = "Yes";
            v_Range = "A1:";
        }

        public override void RunCommand(object sender)
        {         
            var engine = (AutomationEngineInstance)sender;
            var excelObject = v_InstanceName.GetAppInstance(engine);
            var vRange = v_Range.ConvertUserVariableToString(engine);
            var excelInstance = (Application)excelObject;

            Worksheet excelSheet = excelInstance.ActiveSheet;
            //Extract a range of cells
            var splitRange = vRange.Split(':');
            Range cellRange;

            try
            {
                Range last = excelSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
                if (splitRange[1] == "")
                    cellRange = excelSheet.Range[splitRange[0], last];
                else
                    cellRange = excelSheet.Range[splitRange[0], splitRange[1]];
            }
            //Attempt to extract a single cell
            catch (Exception)
            {
                throw new Exception("Selected range is invalid");
            }              

            int rw = cellRange.Rows.Count;
            int cl = cellRange.Columns.Count;
            int rCnt;
            int cCnt;
            string cName;
            DataTable DT = new DataTable();

            for (rCnt = 2; rCnt <= rw; rCnt++)
            {
                DataRow newRow = DT.NewRow();
                for (cCnt = 1; cCnt <= cl; cCnt++)
                {
                    if (!DT.Columns.Contains(cCnt.ToString()))
                        DT.Columns.Add(cCnt.ToString());

                    if (((cellRange.Cells[rCnt, cCnt] as Range).Value2) == null)
                        newRow[cCnt.ToString()] = "";
                    else
                        newRow[cCnt.ToString()] = (cellRange.Cells[rCnt, cCnt] as Range).Value2.ToString();
                }
                DT.Rows.Add(newRow);
            }

            if (v_AddHeaders == "Yes")
            {
                //Set column names
                for (cCnt = 1; cCnt <= cl; cCnt++)
                {
                    cName = ((cellRange.Cells[1, cCnt] as Range).Value2).ToString();
                    DT.Columns[cCnt - 1].ColumnName = cName;
                }
            }

            DT.StoreInUserVariable(engine, v_OutputUserVariableName);           
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Range", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_AddHeaders", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Get Range '{v_Range}' - Store DataTable in '{v_OutputUserVariableName}' - Instance Name '{v_InstanceName}']";
        }
    }
}