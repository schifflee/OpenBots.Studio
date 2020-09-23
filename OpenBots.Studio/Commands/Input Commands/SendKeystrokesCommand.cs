using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.User32;
using OpenBots.Core.Utilities.CommandUtilities;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Input Commands")]
    [Description("This command sends keystrokes to a targeted window.")]
    public class SendKeystrokesCommand : ScriptCommand
    {

        [PropertyDescription("Window Name")]
        [InputSpecification("Select the name of the window to send keystrokes to.")]
        [SampleUsage("Untitled - Notepad || Current Window || {vWindow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WindowName { get; set; }

        [PropertyDescription("Text to Send")]
        [InputSpecification("Enter the text to be sent to the specified window.")]
        [SampleUsage("Hello, World! || {vText}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowEncryptionHelper)]
        public string v_TextToSend { get; set; }

        [PropertyDescription("Text Encrypted")]
        [PropertyUISelectionOption("Not Encrypted")]
        [PropertyUISelectionOption("Encrypted")]
        [InputSpecification("Indicate whether the text in *Text to Send* is encrypted.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_EncryptionOption { get; set; }

        public SendKeystrokesCommand()
        {
            CommandName = "SendKeystrokesCommand";
            SelectionName = "Send Keystrokes";
            CommandEnabled = true;
            CustomRendering = true;

            v_WindowName = "Current Window";
            v_EncryptionOption = "Not Encrypted";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var variableWindowName = v_WindowName.ConvertUserVariableToString(engine);

            if (variableWindowName != "Current Window")
            {
                ActivateWindowCommand activateWindow = new ActivateWindowCommand
                {
                    v_WindowName = variableWindowName
                };
                activateWindow.RunCommand(engine);
            }

            string textToSend = v_TextToSend.ConvertUserVariableToString(engine);

            if (v_EncryptionOption == "Encrypted")
                textToSend = EncryptionServices.DecryptString(textToSend, "OPENBOTS");

            if (textToSend == "{WIN_KEY}")
            {
                User32Functions.KeyDown(Keys.LWin);
                User32Functions.KeyUp(Keys.LWin);
            }
            else if (textToSend.Contains("{WIN_KEY+"))
            {
                User32Functions.KeyDown(Keys.LWin);
                var remainingText = textToSend.Replace("{WIN_KEY+", "").Replace("}","");

                foreach (var c in remainingText)
                {
                    Keys key = (Keys)Enum.Parse(typeof(Keys), c.ToString());
                    User32Functions.KeyDown(key);
                }
                User32Functions.KeyUp(Keys.LWin);

                foreach (var c in remainingText)
                {
                    Keys key = (Keys)Enum.Parse(typeof(Keys), c.ToString());
                    User32Functions.KeyUp(key);
                }
            }
            else
                SendKeys.SendWait(textToSend);

            Thread.Sleep(500);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultWindowControlGroupFor("v_WindowName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_TextToSend", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_EncryptionOption", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Text '{v_TextToSend}' - Window '{v_WindowName}']";
        }     
    }
}