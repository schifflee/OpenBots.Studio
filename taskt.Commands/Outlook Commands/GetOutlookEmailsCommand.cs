﻿using Microsoft.Office.Interop.Outlook;
using System;
using System.Collections.Generic;
using System.IO;
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
using Application = Microsoft.Office.Interop.Outlook.Application;

namespace taskt.Commands
{
    [Serializable]
    [Group("Outlook Commands")]
    [Description("This command gets selected emails and their attachments from Outlook.")]

    public class GetOutlookEmailsCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Source Mail Folder Name")]
        [InputSpecification("Enter the name of the Outlook mail folder the emails are located in.")]
        [SampleUsage("Inbox || {vFolderName}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_SourceFolder { get; set; }

        [XmlAttribute]
        [PropertyDescription("Filter")]
        [InputSpecification("Enter a valid Outlook filter string.")]
        [SampleUsage("[Subject] = 'Hello' || [Subject] = 'Hello' and [SenderName] = 'Jane Doe' || {vFilter} || None")]
        [Remarks("*Warning* Using 'None' as the Filter will return every email in the selected Mail Folder.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_Filter { get; set; }

        [XmlAttribute]
        [PropertyDescription("Unread Only")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether to retrieve unread email messages only.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_GetUnreadOnly { get; set; }

        [XmlAttribute]
        [PropertyDescription("Mark As Read")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether to mark retrieved emails as read.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_MarkAsRead { get; set; }

        [XmlAttribute]
        [PropertyDescription("Save MailItems and Attachments")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether to save the email attachments to a local directory.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_SaveMessagesAndAttachments { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output MailItem Directory")]   
        [InputSpecification("Enter or Select the path of the directory to store the messages in.")]
        [SampleUsage(@"C:\temp\myfolder || {vFolderPath} || {ProjectPath}\myFolder")]
        [Remarks("This input is optional and will only be used if *Save MailItems and Attachments* is set to **Yes**.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_MessageDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Attachment Directory")]      
        [InputSpecification("Enter or Select the path to the directory to store the attachments in.")]
        [SampleUsage(@"C:\temp\myfolder\attachments || {vFolderPath} || {ProjectPath}\myFolder\attachments")]
        [Remarks("This input is optional and will only be used if *Save MailItems and Attachments* is set to **Yes**.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_AttachmentDirectory { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output MailItem List Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public GetOutlookEmailsCommand()
        {
            CommandName = "GetOutlookEmailsCommand";
            SelectionName = "Get Outlook Emails";
            CommandEnabled = true;
            CustomRendering = true;
            v_SourceFolder = "Inbox";
            v_GetUnreadOnly = "No";
            v_MarkAsRead = "Yes";
            v_SaveMessagesAndAttachments = "No";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            var vFolder = v_SourceFolder.ConvertUserVariableToString(engine);
            var vFilter = v_Filter.ConvertUserVariableToString(engine);
            var vAttachmentDirectory = v_AttachmentDirectory.ConvertUserVariableToString(engine);
            var vMessageDirectory = v_MessageDirectory.ConvertUserVariableToString(engine);

            if (vFolder == "") 
                vFolder = "Inbox";

            Application outlookApp = new Application();
            AddressEntry currentUser = outlookApp.Session.CurrentUser.AddressEntry;
            NameSpace test = outlookApp.GetNamespace("MAPI");

            if (currentUser.Type == "EX")
            {
                MAPIFolder inboxFolder = test.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Parent;
                MAPIFolder userFolder = inboxFolder.Folders[vFolder];
                Items filteredItems = null;

                if (string.IsNullOrEmpty(vFilter.Trim()))
                    throw new NullReferenceException("Outlook Filter not specified");
                else if (vFilter != "None")
                {
                    try
                    {
                        filteredItems = userFolder.Items.Restrict(vFilter);
                    }
                    catch(System.Exception)
                    {
                        throw new InvalidDataException("Outlook Filter is not valid");
                    }
                }                   
                else
                    filteredItems = userFolder.Items;

                List<MailItem> outMail = new List<MailItem>();

                foreach (object _obj in filteredItems)
                {
                    if (_obj is MailItem)
                    { 
                        MailItem tempMail = (MailItem)_obj;
                        if (v_GetUnreadOnly == "Yes")
                        {
                            if (tempMail.UnRead == true)
                            {
                                ProcessEmail(tempMail, vMessageDirectory, vAttachmentDirectory);
                                outMail.Add(tempMail);
                            }
                        }
                        else {
                            ProcessEmail(tempMail, vMessageDirectory, vAttachmentDirectory);
                            outMail.Add(tempMail);
                        }   
                    }
                }
                outMail.StoreInUserVariable(engine, v_OutputUserVariableName);
            }
        }      

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_SourceFolder", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_Filter", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_GetUnreadOnly", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_MarkAsRead", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_SaveMessagesAndAttachments", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_MessageDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_AttachmentDirectory", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [From '{v_SourceFolder}' - Filter by '{v_Filter}' - Store MailItem List in '{v_OutputUserVariableName}']";
        }

        private void ProcessEmail(MailItem mail, string msgDirectory, string attDirectory)
        {
            if (v_MarkAsRead == "Yes")
            {
                mail.UnRead = false;
            }
            if (v_SaveMessagesAndAttachments == "Yes")
            {
                if (Directory.Exists(msgDirectory))
                    mail.SaveAs(Path.Combine(msgDirectory, mail.Subject + ".msg"));
                if (Directory.Exists(attDirectory))
                {
                    foreach (Attachment attachment in mail.Attachments)
                    {
                        attachment.SaveAsFile(Path.Combine(attDirectory, attachment.FileName));
                    }
                }
            }
        }
    }
}