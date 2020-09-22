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

namespace OpenBots.Commands.DataTable
{
    [Serializable]
    [Group("DataTable Commands")]
    [Description("This command updates a Value in a DataRow at a specified column name/index.")]

    public class UpdateDataRowValueCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("DataRow")]
        [InputSpecification("Enter an existing DataRow to add values to.")]
        [SampleUsage("{vDataRow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataRow { get; set; }

        [XmlAttribute]
        [PropertyDescription("Search Option")]
        [PropertyUISelectionOption("Column Name")]
        [PropertyUISelectionOption("Column Index")]
        [InputSpecification("Select whether the DataRow value should be found by column index or column name.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_Option { get; set; }

        [XmlAttribute]
        [PropertyDescription("Search Value")]
        [InputSpecification("Enter a valid DataRow index or column name.")]
        [SampleUsage("0 || {vIndex} || Column1 || {vColumnName}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataValueIndex { get; set; }

        [XmlAttribute]
        [PropertyDescription("Cell Value")]
        [InputSpecification("Enter the value to write to the DataRow cell.")]
        [SampleUsage("value || {vValue}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DataRowValue { get; set; }

        public UpdateDataRowValueCommand()
        {
            CommandName = "UpdateDataRowValueCommand";
            SelectionName = "Update DataRow Value";
            CommandEnabled = true;
            CustomRendering = true;
            v_Option = "Column Index";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var dataRowValue = v_DataRowValue.ConvertUserVariableToString(engine);

            var dataRowVariable = v_DataRow.ConvertUserVariableToObject(engine);
            DataRow dataRow = (DataRow)dataRowVariable;

            var valueIndex = v_DataValueIndex.ConvertUserVariableToString(engine);

            if (v_Option == "Column Index")
            {
                int index = int.Parse(valueIndex);
                dataRow[index] = dataRowValue;
            }
            else if (v_Option == "Column Name")
            {
                string index = valueIndex;
                dataRow.SetField(index, dataRowValue);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataRow", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_Option", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataValueIndex", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DataRowValue", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Write '{v_DataRowValue}' to Column '{v_DataValueIndex}' in '{v_DataRow}']";
        }       
    }
}