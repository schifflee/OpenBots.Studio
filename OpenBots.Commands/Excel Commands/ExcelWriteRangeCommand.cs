using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
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
using DataTable = System.Data.DataTable;
using Group = OpenBots.Core.Attributes.ClassAttributes.Group;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command writes a DataTable to an Excel Worksheet starting from a specific cell address.")]

    public class ExcelWriteRangeCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("DataTable")]
        [InputSpecification("Enter the DataTable to write to the Worksheet.")]
        [SampleUsage("{vDataTable}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataTableToSet { get; set; }

        [XmlAttribute]
        [PropertyDescription("Cell Location")]
        [InputSpecification("Enter the location of the cell to set the DataTable at.")]
        [SampleUsage("A1 || {vCellLocation}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_CellLocation { get; set; }

        [XmlAttribute]
        [PropertyDescription("Add Headers")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("When selected, the column headers from the specified DataTable are also written.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_AddHeaders { get; set; }

        public ExcelWriteRangeCommand()
        {
            CommandName = "ExcelWriteRangeCommand";
            SelectionName = "Write Range";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
            v_AddHeaders = "Yes";
            v_CellLocation = "A1";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vTargetAddress = v_CellLocation.ConvertUserVariableToString(engine);
            var excelObject = v_InstanceName.GetAppInstance(engine);

            var excelInstance = (Application)excelObject;
            var excelSheet = (Worksheet)excelInstance.ActiveSheet;

            DataTable Dt = (DataTable)v_DataTableToSet.ConvertUserVariableToObject(engine);
            if (string.IsNullOrEmpty(vTargetAddress) || vTargetAddress.Contains(":")) 
                throw new Exception("Cell Location is invalid or empty");
          
            var numberOfRow = Regex.Match(vTargetAddress, @"\d+").Value;
            vTargetAddress = Regex.Replace(vTargetAddress, @"[\d-]", string.Empty);
            vTargetAddress = vTargetAddress.ToUpperInvariant();

            int sum = 0;

            for (int i = 0; i < vTargetAddress.Length; i++)
            {   
                sum *= 26;
                sum += (vTargetAddress[i] - 'A' + 1);
            }

            if (v_AddHeaders == "Yes")
            {
                //Write column names
                string columnName;
                for (int j = 0; j < Dt.Columns.Count; j++)
                {
                    if (Dt.Columns[j].ColumnName == "null")                    
                        columnName = string.Empty;                  
                    else                    
                        columnName = Dt.Columns[j].ColumnName;
                    
                    excelSheet.Cells[int.Parse(numberOfRow), j + sum] = columnName;
                }

                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    for (int j = 0; j < Dt.Columns.Count; j++)
                    {
                        if (Dt.Rows[i][j].ToString() == "null")
                        {
                            Dt.Rows[i][j] = string.Empty;
                        }
                        excelSheet.Cells[i + int.Parse(numberOfRow) + 1, j + sum] = Dt.Rows[i][j].ToString();
                    }
                }
            }
            else { 
                for (int i = 0; i < Dt.Rows.Count; i++)
                {
                    for (int j = 0; j < Dt.Columns.Count; j++)
                    {
                        if (Dt.Rows[i][j].ToString() == "null")
                        {
                            Dt.Rows[i][j] = string.Empty;
                        }
                        excelSheet.Cells[i + int.Parse(numberOfRow), j + sum] = Dt.Rows[i][j].ToString();
                    }
                }
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataTableToSet", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_CellLocation", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_AddHeaders", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Write '{v_DataTableToSet}' to Cell '{v_CellLocation}' - Instance Name '{v_InstanceName}']";
        }        
    }
}