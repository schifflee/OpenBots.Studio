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

namespace OpenBots.Commands.DataTable
{
    [Serializable]
    [Group("DataTable Commands")]
    [Description("This command gets a DataRow Value from a DataTable.")]

    public class GetDataRowValueCommand : ScriptCommand
    {

        [PropertyDescription("DataRow")]
        [InputSpecification("Enter an existing DataRow to get Values from.")]
        [SampleUsage("{vDataRow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataRow { get; set; }

        [PropertyDescription("Search Option")]
        [PropertyUISelectionOption("Column Name")]
        [PropertyUISelectionOption("Column Index")]
        [InputSpecification("Select whether the DataRow value should be found by column index or column name.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_Option { get; set; }

        [PropertyDescription("Search Value")]
        [InputSpecification("Enter a valid DataRow index or column name.")]
        [SampleUsage("0 || {vIndex} || Column1 || {vColumnName}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataValueIndex { get; set; }

        [PropertyDescription("Output Value Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public GetDataRowValueCommand()
        {
            CommandName = "GetDataRowValueCommand";
            SelectionName = "Get DataRow Value";
            CommandEnabled = true;
            CustomRendering = true;
            v_Option = "Column Index";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var dataRowVariable = v_DataRow.ConvertUserVariableToObject(engine);
            DataRow dataRow = (DataRow)dataRowVariable;

            var valueIndex = v_DataValueIndex.ConvertUserVariableToString(engine);
            string value = "";
            if (v_Option == "Column Index")
            {
                int index = int.Parse(valueIndex);
                value = dataRow[index].ToString();

            }
            else if (v_Option == "Column Name")
            {
                string index = valueIndex;
                value = dataRow.Field<string>(index);
            }

            value.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataRow", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Option", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataValueIndex", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Get Value From Column '{v_DataValueIndex}' in '{v_DataRow}' - Store Value in '{v_OutputUserVariableName}']";
        }        
    }
}