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
using System.Xml.Serialization;
using Application = Microsoft.Office.Interop.Outlook.Application;

namespace OpenBots.Commands.Outlook
{
    [Serializable]
    [Group("Outlook Commands")]
    [Description("This command moves or copies a selected email in Outlook.")]

    public class MoveCopyOutlookEmailCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("MailItem")]
        [InputSpecification("Enter the MailItem to move or copy.")]
        [SampleUsage("{vMailItem}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_MailItem { get; set; }

        [XmlAttribute]
        [PropertyDescription("Destination Mail Folder Name")]
        [InputSpecification("Enter the name of the Outlook mail folder the emails are being moved/copied to.")]
        [SampleUsage("New Folder || {vFolderName}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_DestinationFolder { get; set; }

        [XmlAttribute]
        [PropertyDescription("Mail Operation")]
        [PropertyUISelectionOption("Move MailItem")]
        [PropertyUISelectionOption("Copy MailItem")]
        [InputSpecification("Specify whether to move or copy the selected emails.")]
        [SampleUsage("")]
        [Remarks("Moving will remove the emails from the original folder while copying will not.")]
        public string v_OperationType { get; set; }

        [XmlAttribute]
        [PropertyDescription("Unread Only")]
        [PropertyUISelectionOption("Yes")]
        [PropertyUISelectionOption("No")]
        [InputSpecification("Specify whether to move/copy unread email messages only.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_MoveCopyUnreadOnly { get; set; }

        public MoveCopyOutlookEmailCommand()
        {
            CommandName = "MoveCopyOutlookEmailCommand";
            SelectionName = "Move/Copy Outlook Email";
            CommandEnabled = true;
            CustomRendering = true;
            v_OperationType = "Move MailItem";
            v_MoveCopyUnreadOnly = "Yes";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            MailItem vMailItem = (MailItem)v_MailItem.ConvertUserVariableToObject(engine);
            var vDestinationFolder = v_DestinationFolder.ConvertUserVariableToString(engine);
            
            Application outlookApp = new Application();
            AddressEntry currentUser = outlookApp.Session.CurrentUser.AddressEntry;
            NameSpace test = outlookApp.GetNamespace("MAPI");

            if (currentUser.Type == "EX")
            {
                MAPIFolder inboxFolder = (MAPIFolder)test.GetDefaultFolder(OlDefaultFolders.olFolderInbox).Parent;
                MAPIFolder destinationFolder = inboxFolder.Folders[vDestinationFolder];

                if(v_OperationType == "Move MailItem")
                {
                    if (v_MoveCopyUnreadOnly == "Yes")
                    {
                        if (vMailItem.UnRead == true)
                            vMailItem.Move(destinationFolder);
                    }
                    else
                    {
                        vMailItem.Move(destinationFolder);
                    }
                }
                else if (v_OperationType == "Copy MailItem")
                {
                    MailItem copyMail = null;
                    if (v_MoveCopyUnreadOnly == "Yes")
                    {
                        if (vMailItem.UnRead == true)
                        {
                            copyMail = (MailItem)vMailItem.Copy();
                            copyMail.Move(destinationFolder);
                        }
                    }
                    else
                    {
                        copyMail = (MailItem)vMailItem.Copy();
                        copyMail.Move(destinationFolder);
                    }                       
                }               
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MailItem", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_DestinationFolder", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_OperationType", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultDropdownGroupFor("v_MoveCopyUnreadOnly", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_OperationType} '{v_MailItem}' to '{v_DestinationFolder}']";
        }
    }
}