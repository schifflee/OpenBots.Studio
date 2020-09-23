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
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;
using Group = OpenBots.Core.Attributes.ClassAttributes.Group;

namespace OpenBots.Commands.Excel
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command writes a DataRow to an Excel Worksheet starting from a specific cell address.")]

    public class ExcelWriteRowCommand : ScriptCommand
    {

        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [PropertyDescription("Row")]
        [InputSpecification("Enter the text value that will be set in the selected row (Can be a DataRow).")]
        [SampleUsage("Hello,World || {vData1},{vData2} || {vDataRow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_RowToSet { get; set; }

        [PropertyDescription("Cell Location")]
        [InputSpecification("Enter the location of the cell to write the row to.")]
        [SampleUsage("A1 || {vCellLocation}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_CellLocation { get; set; }

        public ExcelWriteRowCommand()
        {
            CommandName = "ExcelWriteRowCommand";
            SelectionName = "Write Row";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
            v_CellLocation = "A1";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vRow = v_RowToSet.ConvertUserVariableToObject(engine);
            var vTargetAddress = v_CellLocation.ConvertUserVariableToString(engine);
            var excelObject = v_InstanceName.GetAppInstance(engine);
            var excelInstance = (Application)excelObject;
            var excelSheet = (Worksheet)excelInstance.ActiveSheet;
            
            if (string.IsNullOrEmpty(vTargetAddress)) 
                throw new ArgumentNullException("columnName");

            var numberOfRow = int.Parse(Regex.Match(vTargetAddress, @"\d+").Value);
            vTargetAddress = Regex.Replace(vTargetAddress, @"[\d-]", string.Empty);
            vTargetAddress = vTargetAddress.ToUpperInvariant();

            int sum = 0;
            for (int i = 0; i < vTargetAddress.Length; i++)
            {
                sum *= 26;
                sum += (vTargetAddress[i] - 'A' + 1);
            }

            //Write row
            DataRow row;
            if (vRow != null && vRow is DataRow)
            {
                row = (DataRow)vRow;

                string cellValue;
                for (int j = 0; j < row.ItemArray.Length; j++)
                {
                    if (row.ItemArray[j] == null)
                        cellValue = string.Empty;
                    else
                        cellValue = row.ItemArray[j].ToString();

                    excelSheet.Cells[numberOfRow, j + sum] = cellValue;
                }
            }
            else
            {
                string vRowString = v_RowToSet.ConvertUserVariableToString(engine);
                var splittext = vRowString.Split(',');

                string cellValue;
                for (int j = 0; j < splittext.Length; j++)
                {
                    cellValue = splittext[j];
                    if (cellValue == "null")
                    {
                        cellValue = string.Empty;
                    }
                    excelSheet.Cells[numberOfRow, j + sum] = cellValue;
                }
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_RowToSet", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CellLocation", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Write '{v_RowToSet}' to Row '{v_CellLocation}' - Instance Name '{v_InstanceName}']";
        }       
    }
}