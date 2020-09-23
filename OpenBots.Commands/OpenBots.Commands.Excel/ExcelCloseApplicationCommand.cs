using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace OpenBots.Commands.Excel
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command closes an open Excel Workbook and Instance.")]

    public class ExcelCloseApplicationCommand : ScriptCommand
    {
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        public string v_InstanceName { get; set; }
        [PropertyDescription("Save Workbook")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Indicate whether the Workbook should be saved before closing.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_ExcelSaveOnExit { get; set; }

        public ExcelCloseApplicationCommand()
        {
            CommandName = "ExcelCloseApplicationCommand";
            SelectionName = "Close Excel Application";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
            v_ExcelSaveOnExit = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var excelObject = v_InstanceName.GetAppInstance(engine);
            var excelInstance = (Application)excelObject;
            bool saveOnExit;
            if (v_ExcelSaveOnExit == "Yes")
                saveOnExit = true;
            else
                saveOnExit = false;

            //check if workbook exists and save
            if (excelInstance.ActiveWorkbook != null)
            {
                excelInstance.ActiveWorkbook.Close(saveOnExit);
            }

            //close excel
            excelInstance.Quit();
            //remove instance
            v_InstanceName.RemoveAppInstance(engine);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_ExcelSaveOnExit", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Save on Close '{v_ExcelSaveOnExit}' - Instance Name '{v_InstanceName}']";
        }
    }
}