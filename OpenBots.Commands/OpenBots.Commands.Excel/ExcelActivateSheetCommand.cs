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
using System.Windows.Forms;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace OpenBots.Commands.Excel
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command activates a specific Worksheet in an Excel Workbook.")]

    public class ExcelActivateSheetCommand : ScriptCommand
    {

        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [PropertyDescription("Worksheet")]
        [InputSpecification("Specify the Worksheet within the Workbook to activate.")]
        [SampleUsage("Sheet1 || {vSheet}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SheetName { get; set; }

        public ExcelActivateSheetCommand()
        {
            CommandName = "ExcelActivateSheetCommand";
            SelectionName = "Activate Sheet";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            string vSheetToDelete = v_SheetName.ConvertUserVariableToString(engine);

            var excelObject = v_InstanceName.GetAppInstance(engine);
            var excelInstance = (Application)excelObject;      
            var workSheet = excelInstance.Sheets[vSheetToDelete] as Worksheet;
            workSheet.Select(); 
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SheetName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Sheet '{v_SheetName}' - Instance Name '{v_InstanceName}']";
        }
    }
}