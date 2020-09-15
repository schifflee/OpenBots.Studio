﻿using System;
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

namespace taskt.Commands
{
    [Serializable]
    [Group("Data Commands")]
    [Description("This command returns the count of all words in a string.")]
    public class GetWordCountCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Text Data")]
        [InputSpecification("Provide a variable or text value.")]
        [SampleUsage("Hello World || {vStringVariable}")]
        [Remarks("Providing data of a type other than a 'String' will result in an error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_InputValue { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Count Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public GetWordCountCommand()
        {
            CommandName = "GetWordCountCommand";
            SelectionName = "Get Word Count";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get engine
            var engine = (AutomationEngineInstance)sender;

            //get input value
            var stringRequiringCount = v_InputValue.ConvertUserVariableToString(engine);

            //count number of words
            var wordCount = stringRequiringCount.Split(new string[] {" "}, StringSplitOptions.RemoveEmptyEntries).Length;

            //store word count into variable
            wordCount.ToString().StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create standard group controls
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InputValue", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From Text '{v_InputValue}' - Store Count in '{v_OutputUserVariableName}']";
        }
    }
}