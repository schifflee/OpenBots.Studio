using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.User32;
using taskt.Core.Utilities.CommandUtilities;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Input Commands")]
    [Description("This command sends keystrokes to a targeted window.")]
    public class SendKeystrokesCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Window Name")]
        [InputSpecification("Select the name of the window to send keystrokes to.")]
        [SampleUsage("Untitled - Notepad || Current Window || {vWindow}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_WindowName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Text to Send")]
        [InputSpecification("Enter the text to be sent to the specified window.")]
        [SampleUsage("Hello, World! || {vText}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowEncryptionHelper)]
        public string v_TextToSend { get; set; }

        [XmlAttribute]
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

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultWindowControlGroupFor("v_WindowName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_TextToSend", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_EncryptionOption", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Text '{v_TextToSend}' - Window '{v_WindowName}']";
        }     
    }
}