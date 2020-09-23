using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Newtonsoft.Json;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Common;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.User32;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Input Commands")]
    [Description("This command sends advanced keystrokes to a targeted window.")]
    public class SendAdvancedKeystrokesCommand : ScriptCommand
    {

        [PropertyDescription("Window Name")]
        [InputSpecification("Select the name of the window to send advanced keystrokes to.")]
        [SampleUsage("Untitled - Notepad || Current Window || {vWindow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WindowName { get; set; }

        [PropertyDescription("Keystroke Parameters")]
        [InputSpecification("Define the parameters for the keystroke actions.")]
        [SampleUsage("[Enter [Return] | Key Press (Down + Up)]")]
        [Remarks("")]
        public DataTable v_KeyActions { get; set; }

        [PropertyDescription("Return All Keys to 'UP' Position")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]      
        [InputSpecification("Select whether to return all keys to the 'UP' position after execution.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_KeyUpDefault { get; set; }

        [JsonIgnore]
        [NonSerialized]
        private DataGridView _keystrokeGridHelper;

        public SendAdvancedKeystrokesCommand()
        {
            CommandName = "SendAdvancedKeystrokesCommand";
            SelectionName = "Send Advanced Keystrokes";
            CommandEnabled = true;
            CustomRendering = true;

            v_KeyActions = new DataTable();
            v_KeyActions.Columns.Add("Key");
            v_KeyActions.Columns.Add("Action");
            v_KeyActions.TableName = "SendAdvancedKeyStrokesCommand" + DateTime.Now.ToString("MMddyy.hhmmss");

            v_WindowName = "Current Window";
            v_KeyUpDefault = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var variableWindowName = v_WindowName.ConvertUserVariableToString(engine);

            //activate anything except current window
            if (variableWindowName != "Current Window")
            {
                ActivateWindowCommand activateWindow = new ActivateWindowCommand
                {
                    v_WindowName = variableWindowName
                };
                activateWindow.RunCommand(engine);
            }

            //track all keys down
            var keysDown = new List<Keys>();

            //run each selected item
            foreach (DataRow rw in v_KeyActions.Rows)
            {
                //get key name
                var keyName = rw.Field<string>("Key");

                //get key action
                var action = rw.Field<string>("Action");

                //parse OEM key name
                string oemKeyString = keyName.Split('[', ']')[1];

                var oemKeyName = (Keys)Enum.Parse(typeof(Keys), oemKeyString);
           
                //"Key Press (Down + Up)", "Key Down", "Key Up"
                switch (action)
                {
                    case "Key Press (Down + Up)":
                        //simulate press
                        User32Functions.KeyDown(oemKeyName);
                        User32Functions.KeyUp(oemKeyName);
                        
                        //key returned to UP position so remove if we added it to the keys down list
                        if (keysDown.Contains(oemKeyName))
                            keysDown.Remove(oemKeyName);
                        break;
                    case "Key Down":
                        //simulate down
                        User32Functions.KeyDown(oemKeyName);

                        //track via keys down list
                        if (!keysDown.Contains(oemKeyName))
                            keysDown.Add(oemKeyName);
                        break;
                    case "Key Up":
                        //simulate up
                        User32Functions.KeyUp(oemKeyName);

                        //remove from key down
                        if (keysDown.Contains(oemKeyName))
                            keysDown.Remove(oemKeyName);
                        break;
                    default:
                        break;
                }
            }

            //return key to up position if requested
            if (v_KeyUpDefault == "Yes")
            {
                foreach (var key in keysDown)
                    User32Functions.KeyUp(key);
            }       
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultWindowControlGroupFor("v_WindowName", this, editor));

            _keystrokeGridHelper = new DataGridView();
            _keystrokeGridHelper.DataBindings.Add("DataSource", this, "v_KeyActions", false, DataSourceUpdateMode.OnPropertyChanged);
            _keystrokeGridHelper.AllowUserToDeleteRows = true;
            _keystrokeGridHelper.AutoGenerateColumns = false;
            _keystrokeGridHelper.Width = 500;
            _keystrokeGridHelper.Height = 140;

            DataGridViewComboBoxColumn propertyName = new DataGridViewComboBoxColumn();
            propertyName.DataSource = Common.GetAvailableKeys();
            propertyName.HeaderText = "Selected Key";
            propertyName.DataPropertyName = "Key";
            _keystrokeGridHelper.Columns.Add(propertyName);

            DataGridViewComboBoxColumn propertyValue = new DataGridViewComboBoxColumn();
            propertyValue.DataSource = new List<string> { "Key Press (Down + Up)", "Key Down", "Key Up" };
            propertyValue.HeaderText = "Selected Action";
            propertyValue.DataPropertyName = "Action";
            _keystrokeGridHelper.Columns.Add(propertyValue);

            _keystrokeGridHelper.ColumnHeadersHeight = 30;
            _keystrokeGridHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _keystrokeGridHelper.AllowUserToAddRows = true;
            _keystrokeGridHelper.AllowUserToDeleteRows = true;

            RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_KeyActions", this));
            RenderedControls.Add(_keystrokeGridHelper);

            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_KeyUpDefault", this, editor));

            return RenderedControls;
        }
     
        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Window '{v_WindowName}']";
        }
    }
}