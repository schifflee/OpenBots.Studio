using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using System.Threading;
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
    [Group("Email Commands")]
    [Description("This command deletes a selected email using IMAP protocol.")]

    public class DeleteIMAPEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("MimeMessage")]
        [InputSpecification("Enter the MimeMessage to delete.")]
        [SampleUsage("{vMimeMessage}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_IMAPMimeMessage { get; set; }

        [XmlAttribute]
        [PropertyDescription("Host")]
        [InputSpecification("Define the host/service name that the script should use.")]
        [SampleUsage("imap.gmail.com || {vHost}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_IMAPHost { get; set; }

        [XmlAttribute]
        [PropertyDescription("Port")]
        [InputSpecification("Define the port number that should be used when contacting the IMAP service.")]
        [SampleUsage("993 || {vPort}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_IMAPPort { get; set; }

        [XmlAttribute]
        [PropertyDescription("Username")]
        [InputSpecification("Define the username to use when contacting the IMAP service.")]
        [SampleUsage("myRobot || {vUsername}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_IMAPUserName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Password")]
        [InputSpecification("Define the password to use when contacting the IMAP service.")]
        [SampleUsage("password || {vPassword}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_IMAPPassword { get; set; }

        [XmlAttribute]
        [PropertyDescription("Delete Read Emails Only")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether to delete read email messages only.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_IMAPDeleteReadOnly { get; set; }

        public DeleteIMAPEmailCommand()
        {
            CommandName = "DeleteIMAPEmailCommand";
            SelectionName = "Delete IMAP Email";
            CommandEnabled = true;
            CustomRendering = true;
            v_IMAPDeleteReadOnly = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            MimeMessage vMimeMessage = (MimeMessage)v_IMAPMimeMessage.ConvertUserVariableToObject(engine);
            string vIMAPHost = v_IMAPHost.ConvertUserVariableToString(engine);
            string vIMAPPort = v_IMAPPort.ConvertUserVariableToString(engine);
            string vIMAPUserName = v_IMAPUserName.ConvertUserVariableToString(engine);
            string vIMAPPassword = v_IMAPPassword.ConvertUserVariableToString(engine);

            using (var client = new ImapClient())
            {
                client.ServerCertificateValidationCallback = (sndr, certificate, chain, sslPolicyErrors) => true;
                client.SslProtocols = SslProtocols.None;

                using (var cancel = new CancellationTokenSource())
                {
                    try
                    {
                        client.Connect(v_IMAPHost, int.Parse(v_IMAPPort), true, cancel.Token); //SSL
                    }
                    catch (Exception)
                    {
                        client.Connect(v_IMAPHost, int.Parse(v_IMAPPort)); //TLS
                    }

                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(vIMAPUserName, vIMAPPassword, cancel.Token);

                    var splitId = vMimeMessage.MessageId.Split('#').ToList();
                    UniqueId messageId = UniqueId.Parse(splitId.Last());
                    splitId.RemoveAt(splitId.Count - 1);
                    string messageFolder = string.Join("", splitId);

                    IMailFolder toplevel = client.GetFolder(client.PersonalNamespaces[0]);
                    IMailFolder foundSourceFolder = GetIMAPEmailsCommand.FindFolder(toplevel, messageFolder);

                    if (foundSourceFolder != null)
                        foundSourceFolder.Open(FolderAccess.ReadWrite, cancel.Token);
                    else
                        throw new Exception("Source Folder not found");

                    var messageSummary = foundSourceFolder.Fetch(new[] { messageId }, MessageSummaryItems.Flags);

                    if (v_IMAPDeleteReadOnly == "Yes")
                    {
                        if (messageSummary[0].Flags.Value.HasFlag(MessageFlags.Seen))
                            foundSourceFolder.AddFlags(messageId, MessageFlags.Deleted, true, cancel.Token);
                    }
                    else
                        foundSourceFolder.AddFlags(messageId, MessageFlags.Deleted, true, cancel.Token);

                    client.Disconnect(true, cancel.Token);
                    client.ServerCertificateValidationCallback = null;
                }
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_IMAPMimeMessage", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_IMAPHost", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_IMAPPort", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_IMAPUserName", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultPasswordInputGroupFor("v_IMAPPassword", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_IMAPDeleteReadOnly", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [MimeMessage '{v_IMAPMimeMessage}']";
        }
    }
}