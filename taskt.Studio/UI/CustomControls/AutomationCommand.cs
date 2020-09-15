﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.UI.Forms;

namespace taskt.UI.CustomControls
{
    public class AutomationCommand
    {
        public Type CommandClass { get; set; }
        public string FullName { get; set; }
        public string ShortName { get; set; }
        public string DisplayGroup { get; set; }
        public ScriptCommand Command { get; set; }
        public List<Control> UIControls { get; set; }

        private void RenderUIComponents(frmCommandEditor editorForm)
        {
            if (Command == null)
            {
                throw new InvalidOperationException("Command cannot be null!");
            }

            UIControls = new List<Control>();
            if (Command.CustomRendering)
            {
                var renderedControls = Command.Render(editorForm);

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
                    flpCheckBox.Controls.Add(CommandControls.CreateCheckBoxFor("v_IsPrivate", Command));
                    flpCheckBox.Controls.Add(CommandControls.CreateDefaultLabelFor("v_IsPrivate", Command));
                    UIControls.Add(flpCheckBox);
                }

                //generate comment command if user did not generate it
                var commentControlExists = renderedControls.Any(f => f.Name == "v_Comment");

                if (!commentControlExists)
                {
                    UIControls.Add(CommandControls.CreateDefaultLabelFor("v_Comment", Command));
                    UIControls.Add(CommandControls.CreateDefaultInputFor("v_Comment", Command, 100, 300));
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

        public void Bind(frmCommandEditor editor)
        {
            //preference to preload is false
            //if (UIControls is null)
            //{
            this.RenderUIComponents(editor);
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
