using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.UI.Forms;
using OpenBots.Core.UI.Controls;

namespace OpenBots.UI.CustomControls
{
    public class AutomationCommand
    {
        public Type CommandClass { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string DisplayGroup { get; set; }
        public ScriptCommand Command { get; set; }
        public List<Control> UIControls { get; set; }

        private void RenderUIComponents(frmCommandEditor editorForm, ICommandControls commandControls)
        {
            if (Command == null)
            {
                throw new InvalidOperationException("Command cannot be null!");
            }

            UIControls = new List<Control>();
            if (Command.CustomRendering)
            {
                var renderedControls = Command.Render(editorForm, commandControls);

                foreach (var ctrl in renderedControls)
                {
                    UIControls.Add(ctrl);
                }

                //generate Private Checkbox (Control) if user did not add it
                var checkBoxControlExists = renderedControls.Any(f => f.Name == "v_IsPrivate");

                if (!checkBoxControlExists)
                {
                    FlowLayoutPanel flpCheckBox = new FlowLayoutPanel();
                    flpCheckBox.Height = 30;
                    flpCheckBox.FlowDirection = FlowDirection.LeftToRight;
                    flpCheckBox.Controls.Add(commandControls.CreateCheckBoxFor("v_IsPrivate", Command));
                    flpCheckBox.Controls.Add(commandControls.CreateDefaultLabelFor("v_IsPrivate", Command));
                    UIControls.Add(flpCheckBox);
                }

                //generate comment command if user did not generate it
                var commentControlExists = renderedControls.Any(f => f.Name == "v_Comment");

                if (!commentControlExists)
                {
                    UIControls.Add(commandControls.CreateDefaultLabelFor("v_Comment", Command));
                    UIControls.Add(commandControls.CreateDefaultInputFor("v_Comment", Command, 100, 300));
                }
            }
            else
            {
                var label = new Label();
                label.ForeColor = Color.Red;
                label.AutoSize = true;
                label.Font = new Font("Segoe UI", 18, FontStyle.Bold);
                label.Text = "Command not enabled for custom rendering!";
                UIControls.Add(label);
            }
        }

        public void Bind(frmCommandEditor editor, ICommandControls commandControls)
        {
            //preference to preload is false
            //if (UIControls is null)
            //{
            this.RenderUIComponents(editor, commandControls);
            //}

            foreach (var ctrl in UIControls)
            {
                if (ctrl.DataBindings.Count > 0)
                {
                    var newBindingList = new List<Binding>();
                    foreach (Binding binding in ctrl.DataBindings)
                    {
                        newBindingList.Add(
                            new Binding(
                                binding.PropertyName,
                                Command,
                                binding.BindingMemberInfo.BindingField,
                                false,
                                DataSourceUpdateMode.OnPropertyChanged
                                )
                            );
                    }

                    ctrl.DataBindings.Clear();

                    foreach (var newBinding in newBindingList)
                    {
                        ctrl.DataBindings.Add(newBinding);
                    }
                }

                if (ctrl is CommandItemControl)
                {
                    var control = (CommandItemControl)ctrl;
                    switch (control.HelperType)
                    {
                        case UIAdditionalHelperType.ShowVariableHelper:
                            control.DataSource = editor.ScriptVariables;
                            break;
                        case UIAdditionalHelperType.ShowElementHelper:
                            control.DataSource = editor.ScriptElements;
                            break;
                        default:
                            break;
                    }
                }

                //if (ctrl is UIPictureBox)
                //{
                //    var typedControl = (UIPictureBox)InputControl;
                //}

                //Todo: helper for loading variables, move to attribute
                if ((ctrl.Name == "v_userVariableName") && (ctrl is ComboBox))
                {
                    var variableCbo = (ComboBox)ctrl;
                    variableCbo.Items.Clear();
                    foreach (var var in editor.ScriptVariables)
                    {
                        variableCbo.Items.Add(var.VariableName);
                    }
                }
            }
        }
    }
}
