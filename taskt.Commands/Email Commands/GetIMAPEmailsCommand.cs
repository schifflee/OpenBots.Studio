using MailKit;
using MailKit.Net.Imap;
using MailKit.Search;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
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

namespace taskt.Commands
{
    [Serializable]
    [Group("Email Commands")]
    [Description("This command gets selected emails and their attachments using IMAP protocol.")]

    public class GetIMAPEmailsCommand : ScriptCommand
    {
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
        [PropertyDescription("Source Mail Folder Name")]
        [InputSpecification("Enter the name of the mail folder the emails are located in.")]
        [SampleUsage("Inbox || {vFolderName}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_IMAPSourceFolder { get; set; }

        [XmlAttribute]
        [PropertyDescription("Filter")]
        [InputSpecification("Enter a valid filter string.")]
        [SampleUsage("Hello World || myRobot@company.com || {vFilter} || None")]
        [Remarks("*Warning* Using 'None' as the Filter will return every email in the selected Mail Folder.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_IMAPFilter { get; set; }

        [XmlAttribute]
        [PropertyDescription("Unread Only")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether to retrieve unread email messages only.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_IMAPGetUnreadOnly { get; set; }

        [XmlAttribute]
        [PropertyDescription("Mark As Read")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether to mark retrieved emails as read.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_IMAPMarkAsRead { get; set; }

        [XmlAttribute]
        [PropertyDescription("Save MimeMessages and Attachments")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether to save the email attachments to a local directory.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_IMAPSaveMessagesAndAttachments { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output MimeMessage Directory")]
        [InputSpecification("Enter or Select the path of the directory to store the messages in.")]
        [SampleUsage(@"C:\temp\myfolder || {vFolderPath} || {ProjectPath}\myFolder")]
        [Remarks("This input is optional and will only be used if *Save MimeMessages and Attachments* is set to **Yes**.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_IMAPMessageDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Attachment Directory")]
        [InputSpecification("Enter or Select the path to the directory to store the attachments in.")]
        [SampleUsage(@"C:\temp\myfolder\attachments || {vFolderPath} || {ProjectPath}\myFolder\attachments")]
        [Remarks("This input is optional and will only be used if *Save MimeMessages and Attachments* is set to **Yes**.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_IMAPAttachmentDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output MimeMessage List Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public GetIMAPEmailsCommand()
        {
            CommandName = "GetIMAPEmailsCommand";
            SelectionName = "Get IMAP Emails";
            CommandEnabled = true;
            CustomRendering = true;
            v_IMAPSourceFolder = "INBOX";
            v_IMAPGetUnreadOnly = "No";
            v_IMAPMarkAsRead = "Yes";
            v_IMAPSaveMessagesAndAttachments = "No";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            string vIMAPHost = v_IMAPHost.ConvertUserVariableToString(engine);
            string vIMAPPort = v_IMAPPort.ConvertUserVariableToString(engine);
            string vIMAPUserName = v_IMAPUserName.ConvertUserVariableToString(engine);
            string vIMAPPassword = v_IMAPPassword.ConvertUserVariableToString(engine);
            string vIMAPSourceFolder = v_IMAPSourceFolder.ConvertUserVariableToString(engine);
            string vIMAPFilter = v_IMAPFilter.ConvertUserVariableToString(engine);
            string vIMAPMessageDirectory = v_IMAPMessageDirectory.ConvertUserVariableToString(engine);
            string vIMAPAttachmentDirectory = v_IMAPAttachmentDirectory.ConvertUserVariableToString(engine);

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

                    IMailFolder toplevel = client.GetFolder(client.PersonalNamespaces[0]);
                    IMailFolder foundFolder = FindFolder(toplevel, vIMAPSourceFolder);

                    if (foundFolder != null)
                        foundFolder.Open(FolderAccess.ReadWrite, cancel.Token);
                    else
                        throw new Exception("Source Folder not found");

                    SearchQuery query;
                    if (vIMAPFilter.ToLower() == "none")
                        query = SearchQuery.All;
                    else if (!string.IsNullOrEmpty(vIMAPFilter.Trim()))
                    {
                        query = SearchQuery.MessageContains(vIMAPFilter)
                            .Or(SearchQuery.SubjectContains(vIMAPFilter))
                            .Or(SearchQuery.FromContains(vIMAPFilter))
                            .Or(SearchQuery.BccContains(vIMAPFilter))
                            .Or(SearchQuery.BodyContains(vIMAPFilter))
                            .Or(SearchQuery.CcContains(vIMAPFilter))
                            .Or(SearchQuery.ToContains(vIMAPFilter));
                    }                   
                    else 
                        throw new NullReferenceException("Filter not specified");

                    if (v_IMAPGetUnreadOnly == "Yes")
                        query = query.And(SearchQuery.NotSeen);

                    var filteredItems = foundFolder.Search(query, cancel.Token);

                    List<MimeMessage> outMail = new List<MimeMessage>();

                    foreach (UniqueId uid in filteredItems)
                    {
                        if (v_IMAPMarkAsRead == "Yes")
                            foundFolder.AddFlags(uid, MessageFlags.Seen, true);

                        MimeMessage message = foundFolder.GetMessage(uid, cancel.Token);

                        if (v_IMAPSaveMessagesAndAttachments == "Yes")                       
                            ProcessEmail(message, vIMAPMessageDirectory, vIMAPAttachmentDirectory);

                        message.MessageId = $"{vIMAPSourceFolder}#{uid}";
                        outMail.Add(message);

                    }
                    outMail.StoreInUserVariable(engine, v_OutputUserVariableName);

                    client.Disconnect(true, cancel.Token);
                    client.ServerCertificateValidationCallback = null;
                }
            }           
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPHost", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPPort", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPUserName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultPasswordInputGroupFor("v_IMAPPassword", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPSourceFolder", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPFilter", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_IMAPGetUnreadOnly", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_IMAPMarkAsRead", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_IMAPSaveMessagesAndAttachments", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPMessageDirectory", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_IMAPAttachmentDirectory", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_IMAPSourceFolder}' - Filter by '{v_IMAPFilter}' - Store MimeMessage List in '{v_OutputUserVariableName}']";
        }

        public static IMailFolder FindFolder(IMailFolder toplevel, string name)
        {
            var subfolders = toplevel.GetSubfolders().ToList();

            foreach (var subfolder in subfolders)
            {
                if (subfolder.Name == name)
                    return subfolder;
            }

            foreach (var subfolder in subfolders)
            {
                var folder = FindFolder(subfolder, name);

                if (folder != null)
                    return folder;
            }

            return null;
        }

        private void ProcessEmail(MimeMessage message, string msgDirectory, string attDirectory)
        {
            if (Directory.Exists(msgDirectory))
                message.WriteTo(Path.Combine(msgDirectory, message.Subject + ".eml"));

            if (Directory.Exists(attDirectory))
            {
                foreach (var attachment in message.Attachments)
                {
                    if (attachment is MessagePart)
                    {
                        var fileName = attachment.ContentDisposition?.FileName;
                        var rfc822 = (MessagePart)attachment;

                        if (string.IsNullOrEmpty(fileName))
                            fileName = "attached-message.eml";

                        using (var stream = File.Create(Path.Combine(attDirectory, fileName)))
                            rfc822.Message.WriteTo(stream);
                    }
                    else
                    {
                        var part = (MimePart)attachment;
                        var fileName = part.FileName;

                        using (var stream = File.Create(Path.Combine(attDirectory, fileName)))
                            part.Content.DecodeTo(stream);
                    }
                }
            }           
        }
    }
}
