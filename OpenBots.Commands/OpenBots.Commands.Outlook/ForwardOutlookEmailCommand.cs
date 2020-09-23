using Microsoft.Office.Interop.Outlook;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenBots.Commands.Outlook
{
    [Serializable]
    [Group("Outlook Commands")]
    [Description("This command forwards a selected email in Outlook.")]

    public class ForwardOutlookEmailCommand : ScriptCommand
    {

        [PropertyDescription("MailItem")]
        [InputSpecification("Enter the MailItem to forward.")]
        [SampleUsage("{vMailItem}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_MailItem { get; set; }

        [PropertyDescription("Recipient(s)")]
        [InputSpecification("Enter the email address(es) of the recipient(s).")]
        [SampleUsage("test@test.com || {vEmail} || test@test.com;test2@test.com || {vEmail1};{vEmail2} || {vEmails}")]
        [Remarks("Multiple recipient email addresses should be delimited by a semicolon (;).")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Recipients { get; set; }

        public ForwardOutlookEmailCommand()
        {
            CommandName = "ForwardOutlookEmailCommand";
            SelectionName = "Forward Outlook Email";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            MailItem vMailItem = (MailItem)v_MailItem.ConvertUserVariableToObject(engine);
  
            var vRecipients = v_Recipients.ConvertUserVariableToString(engine);
            var splitRecipients = vRecipients.Split(';');

            MailItem newMail = vMailItem.Forward();

            foreach (var recipient in splitRecipients)
                newMail.Recipients.Add(recipient.ToString().Trim());

            newMail.Recipients.ResolveAll();
            newMail.Send();         
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MailItem", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Recipients", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [MailItem '{v_MailItem}' - Forward to '{v_Recipients}']";
        }
    }
}
