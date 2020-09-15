using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
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
    [Description("This command appends a row after the last row of an Excel Worksheet.")]

    public class ExcelAppendRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Row")]
        [InputSpecification("Enter the text value that will be set in the appended row (Can be a DataRow).")]
        [SampleUsage("Hello,World || {vData1},{vData2} || {vDataRow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_RowToSet { get; set; }

        public ExcelAppendRowCommand()
        {
            CommandName = "ExcelAppendRowCommand";
            SelectionName = "Append Row";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vRow = v_RowToSet.ConvertUserVariableToObject(engine);
            var excelObject = v_InstanceName.GetAppInstance(engine);
            var excelInstance = (Application)excelObject;
            Worksheet excelSheet = excelInstance.ActiveSheet;

            int lastUsedRow;
            int i = 1;
            try
            {
                lastUsedRow = excelSheet.Cells.Find("*", Missing.Value, Missing.Value, Missing.Value, XlSearchOrder.xlByRows, 
                                                    XlSearchDirection.xlPrevious, false, Missing.Value, Missing.Value).Row;
            }
            catch(Exception)
            {
                lastUsedRow = 0;
            }

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

                    excelSheet.Cells[lastUsedRow + 1, i] = cellValue;
                    i++;
                }
            }
            else
            {
                string vRowString = v_RowToSet.ConvertUserVariableToString(engine);
                var splittext = vRowString.Split(',');
                string cellValue;
                foreach (var item in splittext)
                {
                    cellValue = item;
                    if (cellValue == "null")
                    {
                        cellValue = string.Empty;
                    }
                    excelSheet.Cells[lastUsedRow + 1, i] = cellValue;
                    i++;
                }
            }          
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_RowToSet", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Append '{v_RowToSet}' - Instance Name '{v_InstanceName}']";
        }        
    }
}
