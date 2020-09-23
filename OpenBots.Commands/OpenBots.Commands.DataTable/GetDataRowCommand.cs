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
using Data = System.Data;

namespace OpenBots.Commands.DataTable
{
    [Serializable]
    [Group("DataTable Commands")]
    [Description("This command gets a DataRow from a DataTable.")]

    public class GetDataRowCommand : ScriptCommand
    {
        [PropertyDescription("DataTable")]
        [InputSpecification("Enter an existing DataTable to get rows from.")]
        [SampleUsage("{vDataTable}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataTable { get; set; }
        [PropertyDescription("DataRow Index")]
        [InputSpecification("Enter a valid DataRow index value.")]
        [SampleUsage("0 || {vIndex}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataRowIndex { get; set; }
        [PropertyDescription("Output DataRow Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public GetDataRowCommand()
        {
            CommandName = "GetDataRowCommand";
            SelectionName = "Get DataRow";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            Data.DataTable dataTable = (Data.DataTable)v_DataTable.ConvertUserVariableToObject(engine);

            var rowIndex = v_DataRowIndex.ConvertUserVariableToString(engine);
            int index = int.Parse(rowIndex);

            DataRow row = dataTable.Rows[index];

            row.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataTable", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataRowIndex", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Get Row '{v_DataRowIndex}' From '{v_DataTable}' - Store DataRow in '{v_OutputUserVariableName}']";
        }        
    }
}