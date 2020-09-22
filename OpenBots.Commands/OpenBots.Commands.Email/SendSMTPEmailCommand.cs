using MailKit.Net.Smtp;
using MimeKit;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Authentication;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OpenBots.Commands.Email
{
    [Serializable]
    [Group("Email Commands")]
    [Description("This command sends an email with optional attachment(s) using SMTP protocol.")]

    public class SendSMTPEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Host")]
        [InputSpecification("Define the host/service name that the script should use.")]
        [SampleUsage("smtp.gmail.com || {vHost}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPHost { get; set; }

        [XmlAttribute]
        [PropertyDescription("Port")]
        [InputSpecification("Define the port number that should be used when contacting the SMTP service.")]
        [SampleUsage("465 || {vPort}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPPort { get; set; }

        [XmlAttribute]
        [PropertyDescription("Username")]
        [InputSpecification("Define the username to use when contacting the SMTP service.")]
        [SampleUsage("myRobot || {vUsername}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPUserName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Password")]
        [InputSpecification("Define the password to use when contacting the SMTP service.")]
        [SampleUsage("password || {vPassword}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPPassword { get; set; }

        [XmlAttribute]
        [PropertyDescription("Recipient(s)")]
        [InputSpecification("Enter the email address(es) of the recipient(s).")]
        [SampleUsage("test@test.com || test@test.com;test2@test.com || {vEmail} || {vEmail1};{vEmail2} || {vEmails}")]
        [Remarks("Multiple recipient email addresses should be delimited by a semicolon (;).")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPRecipients { get; set; }

        [XmlAttribute]
        [PropertyDescription("Email Subject")]
        [InputSpecification("Enter the subject of the email.")]
        [SampleUsage("Hello || {vSubject}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPSubject { get; set; }

        [XmlAttribute]
        [PropertyDescription("Email Body")]
        [InputSpecification("Enter text to be used as the email body.")]
        [SampleUsage("Dear John, ... || {vBody}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SMTPBody { get; set; }

        [XmlAttribute]
        [PropertyDescription("Attachment File Path(s)")]
        [InputSpecification("Enter the file path(s) of the file(s) to attach.")]
        [SampleUsage(@"C:\temp\myFile.xlsx || {vFile} || C:\temp\myFile1.xlsx;C:\temp\myFile2.xlsx || {vFile1};{vFile2} || {vFiles}")]
        [Remarks("This input is optional. Multiple attachments should be delimited by a semicolon (;).")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFileSelectionHelper)]
        public string v_SMTPAttachments { get; set; }

        public SendSMTPEmailCommand()
        {
            CommandName = "SendSMTPEmailCommand";
            SelectionName = "Send SMTP Email";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            string vSMTPHost = v_SMTPHost.ConvertUserVariableToString(engine);
            string vSMTPPort = v_SMTPPort.ConvertUserVariableToString(engine);
            string vSMTPUserName = v_SMTPUserName.ConvertUserVariableToString(engine);
            string vSMTPPassword = v_SMTPPassword.ConvertUserVariableToString(engine);
            string vSMTPRecipients = v_SMTPRecipients.ConvertUserVariableToString(engine);
            string vSMTPSubject = v_SMTPSubject.ConvertUserVariableToString(engine);
            string vSMTPBody = v_SMTPBody.ConvertUserVariableToString(engine);
            string vSMTPAttachments = v_SMTPAttachments.ConvertUserVariableToString(engine);

            using (var client = new SmtpClient())
            {
                client.ServerCertificateValidationCallback = (sndr, certificate, chain, sslPolicyErrors) => true;
                client.SslProtocols = SslProtocols.None;

                using (var cancel = new CancellationTokenSource())
                {
                    try
                    {
                        client.Connect(vSMTPHost, int.Parse(vSMTPPort), true, cancel.Token); //SSL
                    }
                    catch (Exception)
                    {
                        client.Connect(vSMTPHost, int.Parse(vSMTPPort)); //TLS
                    }

                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(vSMTPUserName, vSMTPPassword, cancel.Token);

                    //construct a new message
                    var message = new MimeMessage();
                    message.From.Add(MailboxAddress.Parse(vSMTPUserName));

                    var splitRecipients = vSMTPRecipients.Split(';');
                    foreach (var vSMTPToEmail in splitRecipients)
                        message.To.Add(MailboxAddress.Parse(vSMTPToEmail));

                    message.Subject = vSMTPSubject;

                    //create a body
                    var builder = new BodyBuilder();
                    builder.TextBody = vSMTPBody;

                    if (!string.IsNullOrEmpty(vSMTPAttachments))
                    {
                        var splitAttachments = vSMTPAttachments.Split(';');
                        foreach (var vSMTPattachment in splitAttachments)
                        {
                            using (MemoryStream memoryStream = new MemoryStream(File.ReadAllBytes(vSMTPattachment)))
                                builder.Attachments.Add(vSMTPattachment, memoryStream.ToArray());
                        }
                    }
                    message.Body = builder.ToMessageBody();

                    client.Send(message);
                    client.ServerCertificateValidationCallback = null;
                }
            }   
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPHost", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPPort", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPUserName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultPasswordInputGroupFor("v_SMTPPassword", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPRecipients", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPSubject", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPBody", this, editor, 100, 300));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_SMTPAttachments", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [To '{v_SMTPRecipients}' - Subject '{v_SMTPSubject}']";
        }
    }
}
