using System;
using System.Collections.Generic;
using System.Diagnostics;
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
    [Group("Engine Commands")]
    [Description("This command measures time elapsed during the execution of the process.")]
    public class StopwatchCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Stopwatch Instance Name")]
        [InputSpecification("Enter a unique name that will represent the application instance.")]
        [SampleUsage("MyStopwatchInstance")]
        [Remarks("This unique name allows you to refer to the instance by name in future commands, " +
                 "ensuring that the commands you specify run against the correct application.")]
        public string v_InstanceName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Stopwatch Action")]
        [PropertyUISelectionOption("Start Stopwatch")]
        [PropertyUISelectionOption("Stop Stopwatch")]
        [PropertyUISelectionOption("Restart Stopwatch")]
        [PropertyUISelectionOption("Reset Stopwatch")]
        [PropertyUISelectionOption("Measure Stopwatch")]
        [InputSpecification("Select the appropriate stopwatch action.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_StopwatchAction { get; set; }

        [XmlAttribute]
        [PropertyDescription("String Format")]
        [InputSpecification("Specify a DateTime string format if required.")]
        [SampleUsage("MM/dd/yy || hh:mm || {vFormat}")]
        [Remarks("This input is optional.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ToStringFormat { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Elapsed Time Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        [XmlIgnore]
        [NonSerialized]
        public List<Control> MeasureControls;

        public StopwatchCommand()
        {
            CommandName = "StopwatchCommand";
            SelectionName = "Stopwatch";
            CommandEnabled = true;
            CustomRendering = true;
            v_InstanceName = "DefaultStopwatch";
            v_StopwatchAction = "Start Stopwatch";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var format = v_ToStringFormat.ConvertUserVariableToString(engine);
            
            Stopwatch stopwatch;
            switch (v_StopwatchAction)
            {
                case "Start Stopwatch":
                    //start a new stopwatch
                    stopwatch = new Stopwatch();
                    stopwatch.AddAppInstance(engine, v_InstanceName);
                    stopwatch.Start();
                    break;
                case "Stop Stopwatch":
                    //stop existing stopwatch
                    stopwatch = (Stopwatch)engine.AppInstances[v_InstanceName];
                    stopwatch.Stop();
                    break;
                case "Restart Stopwatch":
                    //restart which sets to 0 and automatically starts
                    stopwatch = (Stopwatch)engine.AppInstances[v_InstanceName];
                    stopwatch.Restart();
                    break;
                case "Reset Stopwatch":
                    //reset which sets to 0
                    stopwatch = (Stopwatch)engine.AppInstances[v_InstanceName];
                    stopwatch.Reset();
                    break;
                case "Measure Stopwatch":
                    //check elapsed which gives measure
                    stopwatch = (Stopwatch)engine.AppInstances[v_InstanceName];
                    string elapsedTime;
                    if (string.IsNullOrEmpty(format.Trim()))
                        elapsedTime = stopwatch.Elapsed.ToString();
                    else
                        elapsedTime = stopwatch.Elapsed.ToString(format);

                    elapsedTime.StoreInUserVariable(engine, v_OutputUserVariableName);
                    break;
                default:
                    throw new NotImplementedException("Stopwatch Action '" + v_StopwatchAction + "' not implemented");
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_InstanceName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_StopwatchAction", this, editor));
            ((ComboBox)RenderedControls[3]).SelectedIndexChanged += StopWatchComboBox_SelectedValueChanged;

            MeasureControls = new List<Control>();
            MeasureControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ToStringFormat", this, editor));
            MeasureControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            foreach (var ctrl in MeasureControls)
                ctrl.Visible = false;

            RenderedControls.AddRange(MeasureControls);
          
            return RenderedControls;
        }

        private void StopWatchComboBox_SelectedValueChanged(object sender, EventArgs e)
        {
            if (((ComboBox)RenderedControls[3]).Text == "Measure Stopwatch")
            {
                foreach (var ctrl in MeasureControls)
                    ctrl.Visible = true;
            }
            else 
            {
                foreach (var ctrl in MeasureControls)
                    ctrl.Visible = false;
            }
        }

        public override string GetDisplayValue()
        {
            if (v_StopwatchAction == "Measure Stopwatch")
                return base.GetDisplayValue() + $" [{v_StopwatchAction} - Store Elapsed Time in '{v_OutputUserVariableName}' - Instance Name '{v_InstanceName}']";
            else
                return base.GetDisplayValue() + $" [{v_StopwatchAction} - Instance Name '{v_InstanceName}']";
        }
    }
}