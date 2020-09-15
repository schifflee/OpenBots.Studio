﻿using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;
using Application = Microsoft.Office.Interop.Excel.Application;

namespace taskt.Commands
{
    [Serializable]
    [Group("Excel Commands")]
    [Description("This command gets text from a specific cell in an Excel Worksheet.")]

    public class ExcelGetCellCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Excel Instance Name")]
        [InputSpecification("Enter the unique instance that was specified in the **Create Application** command.")]
        [SampleUsage("MyExcelInstance")]
        [Remarks("Failure to enter the correct instance or failure to first call the **Create Application** command will cause an error.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Cell Location")]
        [InputSpecification("Enter the location of the cell to extract.")]
        [SampleUsage("A1 || {vCellLocation}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_CellLocation { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Cell Value Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public ExcelGetCellCommand()
        {
            CommandName = "ExcelGetCellCommand";
            SelectionName = "Get Cell";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultExcel";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var excelObject = v_InstanceName.GetAppInstance(engine);
            var vTargetAddress = v_CellLocation.ConvertUserVariableToString(engine);
            var excelInstance = (Application)excelObject;
            Worksheet excelSheet = excelInstance.ActiveSheet;

            var cellValue = (string)excelSheet.Range[vTargetAddress].Text;
            cellValue.StoreInUserVariable(engine, v_OutputUserVariableName);          
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_CellLocation", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Get Value From '{v_CellLocation}' - Store Cell Value in '{v_OutputUserVariableName}' - Instance Name '{v_InstanceName}']";
        }
    }
}