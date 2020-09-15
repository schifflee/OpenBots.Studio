﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Error Handling Commands")]
    [Description("This command retrieves the most recent error in the engine and stores it in the defined variable.")]
    public class GetExceptionMessageCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Output Exception Message Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public GetExceptionMessageCommand()
        {
            CommandName = "GetExceptionMessageCommand";
            SelectionName = "Get Exception Message";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var error = engine.ErrorsOccured.OrderByDescending(x => x.LineNumber).FirstOrDefault();
            string errorMessage = string.Empty;
            if (error != null)
                errorMessage = $"Source: {error.SourceFile}, Line: {error.LineNumber}, " +
                    $"Exception Type: {error.ErrorType}, Exception Message: {error.ErrorMessage}";
            errorMessage.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Store Exception Message in '{v_OutputUserVariableName}']";
        }
    }
}