using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.UI.Forms;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Misc Commands")]
    [Description("This command displays a message to the user.")]
    public class ShowMessageCommand : ScriptCommand
    {

        [PropertyDescription("Message")]      
        [InputSpecification("Specify any text or variable value that should be displayed on screen.")]
        [SampleUsage("Hello World || {vMyText} || Hello {vName}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Message { get; set; }

        [PropertyDescription("Close After X (Seconds)")]
        [InputSpecification("Specify how many seconds to display the message on screen. After the specified time," + 
                            "\nthe message box will be automatically closed and script will resume execution.")]
        [SampleUsage("0 || 5 || {vSeconds})")]
        [Remarks("Set value to 0 to remain open indefinitely.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_AutoCloseAfter { get; set; }

        public ShowMessageCommand()
        {
            CommandName = "MessageBoxCommand";
            SelectionName = "Show Message";
            CommandEnabled = true;          
            CustomRendering = true;
            v_AutoCloseAfter = "0";
        }

        public override void RunCommand(object sender)
        {
            var engine = (Engine.AutomationEngineInstance)sender;

            int closeAfter = int.Parse(v_AutoCloseAfter.ConvertUserVariableToString(engine));

            dynamic variableMessage = v_Message.ConvertUserVariableToString(engine);

            if (variableMessage == v_Message && variableMessage.StartsWith("{") && variableMessage.EndsWith("}"))
                variableMessage = v_Message.ConvertUserVariableToObject(engine);

            string type = "";
            if (variableMessage != null)
                type = variableMessage.GetType().FullName;

            switch (type)
            {
                case "System.String":
                    variableMessage = variableMessage.Replace("\\n", Environment.NewLine);
                    break;
                case "System.Security.SecureString":
                    variableMessage = type + Environment.NewLine + "*Secure String*";
                    break;
                case "System.Data.DataTable":
                    variableMessage = type + Environment.NewLine + CurrentScriptBuilder.ConvertDataTableToString(variableMessage);
                    break;
                case "System.Data.DataRow":
                    variableMessage = type + Environment.NewLine + CurrentScriptBuilder.ConvertDataRowToString(variableMessage);
                    break;
                case "System.__ComObject":
                    variableMessage = type + Environment.NewLine + CurrentScriptBuilder.ConvertMailItemToString(variableMessage);
                    break;
                case "MimeKit.MimeMessage":
                    variableMessage = type + Environment.NewLine + CurrentScriptBuilder.ConvertMimeMessageToString(variableMessage);
                    break;
                case "OpenQA.Selenium.Remote.RemoteWebElement":
                    variableMessage = type + Environment.NewLine + CurrentScriptBuilder.ConvertIWebElementToString(variableMessage);
                    break;
                case "System.Drawing.Bitmap":
                    variableMessage = type + Environment.NewLine + CurrentScriptBuilder.ConvertBitmapToString(variableMessage);
                    break;
                case string a when a.Contains("System.Collections.Generic.List`1[[System.String"):
                case string b when b.Contains("System.Collections.Generic.List`1[[System.Data.DataTable"):
                case string c when c.Contains("System.Collections.Generic.List`1[[Microsoft.Office.Interop.Outlook.MailItem"):
                case string d when d.Contains("System.Collections.Generic.List`1[[MimeKit.MimeMessage"):
                case string e when e.Contains("System.Collections.Generic.List`1[[OpenQA.Selenium.IWebElement"):
                    variableMessage = type + Environment.NewLine + CurrentScriptBuilder.ConvertListToString(variableMessage);
                    break;
                case "":
                    variableMessage = "null";    
                    break;
                default:
                    variableMessage = v_Message + Environment.NewLine + "*Variable Type Not Yet Supported*";
                    break;
            }

            if (engine.ScriptEngineUI == null)
            {
                engine.ReportProgress("Complex Messagebox Supported With UI Only");
                MessageBox.Show(variableMessage, "Message Box Command", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            //automatically close messageboxes for server requests
            if (engine.ServerExecution && closeAfter <= 0)
                closeAfter = 10;

            var result = ((frmScriptEngine)engine.ScriptEngineUI).Invoke(new Action(() =>
                {
                    engine.ScriptEngineUI.ShowMessage(variableMessage, "MessageBox Command", DialogType.OkOnly, closeAfter);
                }
            ));

        }
        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Message", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_AutoCloseAfter", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" ['{v_Message}']";
        }
    }
}
