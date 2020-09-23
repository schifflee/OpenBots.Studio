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
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;

namespace OpenBots.Commands.Excel
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command reads an Excel Config Worksheet and stores it in a Dictionary.")]

    public class LoadDictionaryCommand : ScriptCommand
    {
        [PropertyDescription("Workbook File Path")]
        [InputSpecification("Enter or Select the path to the Workbook file.")]
        [SampleUsage(@"C:\temp\myfile.xlsx || {vFilePath} || {ProjectPath}\myfile.xlsx")]
        [Remarks("This command does not require Excel to be opened. A snapshot will be taken of the workbook as it exists at the time this command runs.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_FilePath { get; set; }
        [PropertyDescription("Worksheet")]
        [InputSpecification("Indicate the Worksheet to be retrieved.")]
        [SampleUsage("Sheet1 || {vSheet}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SheetName { get; set; }
        [PropertyDescription("Key Column Name")]
        [InputSpecification("Enter the name of the column to be loaded as Dictionary Keys.")]
        [SampleUsage("Name || {vKeyColumn}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_KeyColumn { get; set; }
        [PropertyDescription("Value Column Name")]
        [InputSpecification("Enter the name of the column to be loaded as Dictionary Values.")]
        [SampleUsage("Value || {vValueColumn}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ValueColumn { get; set; }
        [PropertyDescription("Output Dictionary Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public LoadDictionaryCommand()
        {
            CommandName = "LoadDictionaryCommand";
            SelectionName = "Load Dictionary";
            CommandEnabled = true;
            CustomRendering = true;
            v_KeyColumn = "Name";
            v_ValueColumn = "Value";
        }
        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vInstance = DateTime.Now.ToString();
            var vFilePath = v_FilePath.ConvertUserVariableToString(engine);
            var vSheetName = v_SheetName.ConvertUserVariableToString(engine);
            var vKeyColumn = v_KeyColumn.ConvertUserVariableToString(engine);
            var vValueColumn = v_ValueColumn.ConvertUserVariableToString(engine);

            var newExcelSession = new Application{ Visible = false };
            newExcelSession.AddAppInstance(engine, vInstance);

            var excelObject = vInstance.GetAppInstance(engine);
            var excelInstance = (Application)excelObject;

            var excelWorkbook = newExcelSession.Workbooks.Open(vFilePath);
            var excelSheet = excelWorkbook.Sheets[vSheetName];
 
            Range last = excelSheet.Cells.SpecialCells(XlCellType.xlCellTypeLastCell, Type.Missing);
            Range cellValue = excelSheet.Range["A1", last];
            
            int rw = cellValue.Rows.Count;
            int cl = 2;
            int rCnt;
            int cCnt;

            DataTable DT = new DataTable();

            for (rCnt = 2; rCnt <= rw; rCnt++)
            {
                DataRow newRow = DT.NewRow();
                for (cCnt = 1; cCnt <= cl; cCnt++)
                {
                    if (((cellValue.Cells[rCnt, cCnt] as Range).Value2) != null)
                    {
                        if (!DT.Columns.Contains(cCnt.ToString()))
                        {
                            DT.Columns.Add(cCnt.ToString());
                        }
                        newRow[cCnt.ToString()] = ((cellValue.Cells[rCnt, cCnt] as Range).Value2).ToString();
                    }
                }
                DT.Rows.Add(newRow);
            }

            string cKeyName = ((cellValue.Cells[1, 1] as Range).Value2).ToString();
            DT.Columns[0].ColumnName = cKeyName;
            string cValueName = ((cellValue.Cells[1, 2] as Range).Value2).ToString();
            DT.Columns[1].ColumnName = cValueName;

            var dictlist = DT.AsEnumerable().Select(x => new
            {
                keys = (string)x[vKeyColumn],
                values = (string)x[vValueColumn]
            }).ToList();

            Dictionary<string, string> outputDictionary = new Dictionary<string, string>();
            foreach (var dict in dictlist)
            {
                outputDictionary.Add(dict.keys, dict.values);
            }

            //close excel
            excelInstance.Quit();

            //remove instance
            vInstance.RemoveAppInstance(engine);

            outputDictionary.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_FilePath", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SheetName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_KeyColumn", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ValueColumn", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Load Keys '{v_KeyColumn}' and Values '{v_ValueColumn}' From '{v_FilePath}' - Store Dictionary in '{v_OutputUserVariableName}']";
        }
    }
}