using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
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
    [Description("This command appends a cell to the first column after the last row in an Excel Worksheet.")]

    public class ExcelAppendCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Cell Value")]
        [InputSpecification("Enter the text value that will be set in the appended cell.")]
        [SampleUsage("Hello World || {vText}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_TextToSet { get; set; }

        public ExcelAppendCellCommand()
        {
            CommandName = "ExcelAppendCellCommand";
            SelectionName = "Append Cell";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var excelObject = v_InstanceName.GetAppInstance(engine);
            var excelInstance = (Application)excelObject;
            Worksheet excelSheet = excelInstance.ActiveSheet;
  
            var lastUsedRow = excelSheet.Cells.Find("*", Missing.Value, Missing.Value, Missing.Value, XlSearchOrder.xlByRows, 
                                                    XlSearchDirection.xlPrevious, false, Missing.Value, Missing.Value).Row;
            var targetAddress = "A" + (lastUsedRow + 1);
            var vTargetText = v_TextToSet.ConvertUserVariableToString(engine);
            excelSheet.Range[targetAddress].Value = vTargetText;
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TextToSet", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Append '{v_TextToSet}' - Instance Name '{v_InstanceName}']";
        }
    }
}