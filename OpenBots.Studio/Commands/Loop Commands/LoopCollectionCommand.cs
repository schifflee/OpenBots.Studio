using Microsoft.Office.Interop.Outlook;
using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Script;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Loop Commands")]
    [Description("This command iterates over a collection to let user perform actions on the collection items.")]
    public class LoopCollectionCommand : ScriptCommand
    {
        [PropertyDescription("Input Collection")]
        [InputSpecification("Provide a collection variable.")]
        [SampleUsage("{vMyCollection}")]
        [Remarks("If the collection is a DataTable then the output item will be a DataRow and its column value can be accessed using the " +
            "dot operator like {vDataRow.ColumnName}.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_LoopParameter { get; set; }

        [PropertyDescription("Output Collection Item Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public LoopCollectionCommand()
        {
            CommandName = "LoopCollectionCommand";
            SelectionName = "Loop Collection";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender, ScriptAction parentCommand)
        {
            LoopCollectionCommand loopCommand = (LoopCollectionCommand)parentCommand.ScriptCommand;
            var engine = (AutomationEngineInstance)sender;

            int loopTimes;
            var complexVariable = v_LoopParameter.ConvertUserVariableToObject(engine);           

            //if still null then throw exception
            if (complexVariable == null)
            {
                throw new System.Exception("Complex Variable '" + v_LoopParameter + 
                    "' not found. Ensure the variable exists before attempting to modify it.");
            }

            dynamic listToLoop;
            if (complexVariable is List<string>)
            {
                listToLoop = (List<string>)complexVariable;
            }
            else if (complexVariable is List<IWebElement>)
            {
                listToLoop = (List<IWebElement>)complexVariable;
            }
            else if (complexVariable is DataTable)
            {
                listToLoop = ((DataTable)complexVariable).Rows;
            }
            else if (complexVariable is List<MailItem>)
            {
                listToLoop = (List<MailItem>)complexVariable;
            }
            else if (complexVariable is List<MimeMessage>)
            {
                listToLoop = (List<MimeMessage>)complexVariable;
            }
            else if ((complexVariable.ToString().StartsWith("[")) && 
                (complexVariable.ToString().EndsWith("]")) && 
                (complexVariable.ToString().Contains(",")))
            {
                //automatically handle if user has given a json array
                JArray jsonArray = JsonConvert.DeserializeObject(complexVariable.ToString()) as JArray;

               var itemList = new List<string>();
                foreach (var item in jsonArray)
                {
                    var value = (JValue)item;
                    itemList.Add(value.ToString());
                }

                itemList.StoreInUserVariable(engine, v_LoopParameter);
                listToLoop = itemList;
            }
            else
                throw new System.Exception("Complex Variable List Type<T> Not Supported");

            loopTimes = listToLoop.Count;

            for (int i = 0; i < loopTimes; i++)
            {
                engine.ReportProgress("Starting Loop Number " + (i + 1) + "/" + loopTimes + " From Line " + loopCommand.LineNumber);
                
                ((object)listToLoop[i]).StoreInUserVariable(engine, v_OutputUserVariableName);

                foreach (var cmd in parentCommand.AdditionalScriptCommands)
                {
                    if (engine.IsCancellationPending)
                        return;

                    engine.ExecuteCommand(cmd);

                    if (engine.CurrentLoopCancelled)
                    {
                        engine.ReportProgress("Exiting Loop From Line " + loopCommand.LineNumber);
                        engine.CurrentLoopCancelled = false;
                        return;
                    }

                    if (engine.CurrentLoopContinuing)
                    {
                        engine.ReportProgress("Continuing Next Loop From Line " + loopCommand.LineNumber);
                        engine.CurrentLoopContinuing = false;
                        break;
                    }
                }

                engine.ReportProgress("Finished Loop From Line " + loopCommand.LineNumber);
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_LoopParameter", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));
            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return $"Loop Collection '{v_LoopParameter}' - Store Collection Item in '{v_OutputUserVariableName}'";
        }
    }
}