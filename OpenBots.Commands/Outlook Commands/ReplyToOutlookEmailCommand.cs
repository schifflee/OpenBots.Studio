using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Xml.Serialization;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Outlook Commands")]
    [Description("This command replies to a selected email in Outlook.")]

    public class ReplyToOutlookEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("MailItem")]
        [InputSpecification("Enter the MailItem to reply to.")]
        [SampleUsage("{vMailItem}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_MailItem { get; set; }

        [XmlAttribute]
        [PropertyDescription("Mail Operation")]
        [PropertyUISelectionOption("Reply")]
        [PropertyUISelectionOption("Reply All")]
        [InputSpecification("Specify whether you intend to reply or reply all.")]
        [SampleUsage("")]
        [Remarks("Replying will reply to only the original sender. Reply all will reply to everyone in the recipient list.")]
        public string v_OperationType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Email Body")]
        [InputSpecification("Enter text to be used as the email body.")]
        [SampleUsage("Dear John, ... || {vBody}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Body { get; set; }

        [XmlAttribute]
        [PropertyDescription("Email Body Type")]
        [PropertyUISelectionOption("Plain")]
        [PropertyUISelectionOption("HTML")]
        [InputSpecification("Select the email body format.")]
        [Remarks("")]
        public string v_BodyType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Attachment File Path(s)")]
        [InputSpecification("Enter the file path(s) of the file(s) to attach.")]
        [SampleUsage(@"C:\temp\myFile.xlsx || {vFile} || C:\temp\myFile1.xlsx;C:\temp\myFile2.xlsx || {vFile1};{vFile2} || {vFiles}")]
        [Remarks("This input is optional. Multiple attachments should be delimited by a semicolon (;).")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_Attachments { get; set; }

        public ReplyToOutlookEmailCommand()
        {
            CommandName = "ReplyToOutlookEmailCommand";
            SelectionName = "Reply To Outlook Email";
            CommandEnabled = true;
            CustomRendering = true;
            v_OperationType = "Reply";
            v_BodyType = "Plain";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            MailItem vMailItem = (MailItem)v_MailItem.ConvertUserVariableToObject(engine);
            var vBody = v_Body.ConvertUserVariableToString(engine);
            var vAttachment = v_Attachments.ConvertUserVariableToString(engine);
           
            if (v_OperationType == "Reply")
            {
                MailItem newMail = vMailItem.Reply();
                Reply(newMail, vBody, vAttachment);
            }
            else if(v_OperationType == "Reply All")
            {
                MailItem newMail = vMailItem.ReplyAll();
                Reply(newMail, vBody, vAttachment);
            }                           
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MailItem", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_OperationType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Body", this, editor, 100, 300));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_BodyType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_Attachments", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [MailItem '{v_MailItem}']";
        }

        private void Reply(MailItem mail, string body, string attPath)
        {
            if (v_BodyType == "HTML")
                mail.HTMLBody = body;
            else 
                mail.Body = body;

            if (!string.IsNullOrEmpty(attPath))
            {
                var splitAttachments = attPath.Split(';');

                foreach (var attachment in splitAttachments)
                    mail.Attachments.Add(attachment);
            }
            mail.Send();
        }
    }
}
