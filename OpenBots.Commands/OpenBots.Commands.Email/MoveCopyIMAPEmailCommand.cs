using MailKit;
using MailKit.Net.Imap;
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
using System.Linq;
using System.Security.Authentication;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace OpenBots.Commands.Email
{
    [Serializable]
    [Group("Email Commands")]
    [Description("This command moves or copies a selected email using IMAP protocol.")]

    public class MoveCopyIMAPEmailCommand : ScriptCommand
    {
        [PropertyDescription("MimeMessage")]
        [InputSpecification("Enter the MimeMessage to move or copy.")]
        [SampleUsage("{vMimeMessage}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_IMAPMimeMessage { get; set; }
        [PropertyDescription("Host")]
        [InputSpecification("Define the host/service name that the script should use.")]
        [SampleUsage("imap.gmail.com || {vHost}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_IMAPHost { get; set; }
        [PropertyDescription("Port")]
        [InputSpecification("Define the port number that should be used when contacting the IMAP service.")]
        [SampleUsage("993 || {vPort}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_IMAPPort { get; set; }
        [PropertyDescription("Username")]
        [InputSpecification("Define the username to use when contacting the IMAP service.")]
        [SampleUsage("myRobot || {vUsername}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_IMAPUserName { get; set; }
        [PropertyDescription("Password")]
        [InputSpecification("Define the password to use when contacting the IMAP service.")]
        [SampleUsage("password || {vPassword}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_IMAPPassword { get; set; }
        [PropertyDescription("Destination Mail Folder Name")]
        [InputSpecification("Enter the name of the mail folder the emails are being moved/copied to.")]
        [SampleUsage("New Folder || {vFolderName}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_IMAPDestinationFolder { get; set; }
        [PropertyDescription("Mail Operation")]
        [PropertyUISelectionOption("Move MimeMessage")]
        [PropertyUISelectionOption("Copy MimeMessage")]
        [InputSpecification("Specify whether to move or copy the selected emails.")]
        [SampleUsage("")]
        [Remarks("Moving will remove the emails from the original folder while copying will not.")]
        public string v_IMAPOperationType { get; set; }
        [PropertyDescription("Unread Only")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether to move/copy unread email messages only.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_IMAPMoveCopyUnreadOnly { get; set; }

        public MoveCopyIMAPEmailCommand()
        {
            CommandName = "MoveCopyIMAPEmailCommand";
            SelectionName = "Move/Copy IMAP Email";
            CommandEnabled = true;
            CustomRendering = true;
            v_IMAPOperationType = "Move MimeMessage";
            v_IMAPMoveCopyUnreadOnly = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            MimeMessage vMimeMessage = (MimeMessage)v_IMAPMimeMessage.ConvertUserVariableToObject(engine);
            string vIMAPHost = v_IMAPHost.ConvertUserVariableToString(engine);
            string vIMAPPort = v_IMAPPort.ConvertUserVariableToString(engine);
            string vIMAPUserName = v_IMAPUserName.ConvertUserVariableToString(engine);
            string vIMAPPassword = v_IMAPPassword.ConvertUserVariableToString(engine);
            var vIMAPDestinationFolder = v_IMAPDestinationFolder.ConvertUserVariableToString(engine);

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
                    IMailFolder foundDestinationFolder = GetIMAPEmailsCommand.FindFolder(toplevel, vIMAPDestinationFolder);

                    if (foundSourceFolder != null)
                        foundSourceFolder.Open(FolderAccess.ReadWrite, cancel.Token);
                    else
                        throw new Exception("Source Folder not found");

                    if (foundDestinationFolder == null)
                        throw new Exception("Destination Folder not found");

                    var messageSummary = foundSourceFolder.Fetch(new[] { messageId }, MessageSummaryItems.Flags);

                    if (v_IMAPOperationType == "Move MimeMessage")
                    {
                        if (v_IMAPMoveCopyUnreadOnly == "Yes")
                        {
                            if (!messageSummary[0].Flags.Value.HasFlag(MessageFlags.Seen))
                                foundSourceFolder.MoveTo(messageId, foundDestinationFolder, cancel.Token);
                        }
                        else
                            foundSourceFolder.MoveTo(messageId, foundDestinationFolder, cancel.Token);
                    }
                    else if (v_IMAPOperationType == "Copy MimeMessage")
                    {
                        if (v_IMAPMoveCopyUnreadOnly == "Yes")
                        {
                            if (!messageSummary[0].Flags.Value.HasFlag(MessageFlags.Seen))
                                foundSourceFolder.CopyTo(messageId, foundDestinationFolder, cancel.Token);
                        }
                        else
                            foundSourceFolder.CopyTo(messageId, foundDestinationFolder, cancel.Token);
                    }

                    client.Disconnect(true, cancel.Token);
                    client.ServerCertificateValidationCallback = null;
                }
            } 
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPMimeMessage", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPHost", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPPort", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPUserName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultPasswordInputGroupFor("v_IMAPPassword", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPDestinationFolder", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_IMAPOperationType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_IMAPMoveCopyUnreadOnly", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_IMAPOperationType} '{v_IMAPMimeMessage}' to '{v_IMAPDestinationFolder}']";
        }
    }
}