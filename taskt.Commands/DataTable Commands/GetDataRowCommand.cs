using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;

namespace taskt.Commands
{
    [Serializable]
    [Group("DataTable Commands")]
    [Description("This command gets a DataRow from a DataTable.")]

    public class GetDataRowCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("DataTable")]
        [InputSpecification("Enter an existing DataTable to get rows from.")]
        [SampleUsage("{vDataTable}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("DataRow Index")]
        [InputSpecification("Enter a valid DataRow index value.")]
        [SampleUsage("0 || {vIndex}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataRowIndex { get; set; }

        [XmlAttribute]
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
            DataTable dataTable = (DataTable)v_DataTable.ConvertUserVariableToObject(engine);

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