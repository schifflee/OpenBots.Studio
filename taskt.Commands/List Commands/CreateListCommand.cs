using Microsoft.Office.Interop.Outlook;
using MimeKit;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
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
using Exception = System.Exception;

namespace taskt.Commands
{
    [Serializable]
    [Group("List Commands")]
    [Description("This command creates a new List variable.")]
    public class CreateListCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("List Type")]
        [PropertyUISelectionOption("String")]
        [PropertyUISelectionOption("DataTable")]
        [PropertyUISelectionOption("MailItem (Outlook)")]
        [PropertyUISelectionOption("MimeMessage (IMAP/SMTP)")]
        [PropertyUISelectionOption("IWebElement")]
        [InputSpecification("Specify the data type of the List to be created.")]
        [SampleUsage("")]
        [Remarks("")]
        public string v_ListType { get; set; }

        [XmlAttribute]
        [PropertyDescription("List Item(s)")]
        [InputSpecification("Enter the item(s) to write to the List.")]
        [SampleUsage("Hello || {vItem} || Hello,World || {vItem1},{vItem2}")]
        [Remarks("List item can only be a String, DataTable, MailItem or IWebElement.\n" + 
                 "Multiple items should be delimited by a comma(,). This input is optional.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ListItems { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output List Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public CreateListCommand()
        {
            CommandName = "CreateListCommand";
            SelectionName = "Create List";
            CommandEnabled = true;
            CustomRendering = true;
            v_ListType = "String";
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (AutomationEngineInstance)sender;
            dynamic vNewList = null;
            string[] splitListItems = null;

            if (!string.IsNullOrEmpty(v_ListItems.Trim()))
            {
                splitListItems = v_ListItems.Split(',');
            }

            switch (v_ListType)
            {
                case "String":
                    vNewList = new List<string>();
                    if (splitListItems != null)
                    {
                        foreach (string item in splitListItems)
                            ((List<string>)vNewList).Add(item.Trim().ConvertUserVariableToString(engine));
                    }                   
                    break;
                case "DataTable":
                    vNewList = new List<DataTable>();
                    if (splitListItems != null)
                    {                       
                        foreach (string item in splitListItems)
                        {
                            DataTable dataTable;
                            var dataTableVariable = item.Trim().ConvertUserVariableToObject(engine);
                            if (dataTableVariable != null && dataTableVariable is DataTable)
                                dataTable = (DataTable)dataTableVariable;
                            else
                                throw new Exception("Invalid List Item type, please provide valid List Item type.");
                            ((List<DataTable>)vNewList).Add(dataTable);
                        }                           
                    }
                    break;
                case "MailItem (Outlook)":
                    vNewList = new List<MailItem>();
                    if (splitListItems != null)
                    {
                        foreach (string item in splitListItems)
                        {
                            MailItem mailItem;
                            var mailItemVariable = item.Trim().ConvertUserVariableToObject(engine);
                            if (mailItemVariable != null && mailItemVariable is MailItem)
                                mailItem = (MailItem)mailItemVariable;
                            else
                                throw new Exception("Invalid List Item type, please provide valid List Item type.");
                            ((List<MailItem>)vNewList).Add(mailItem);
                        }
                    }
                    break;
                case "MimeMessage (IMAP/SMTP)":
                    vNewList = new List<MimeMessage>();
                    if (splitListItems != null)
                    {
                        foreach (string item in splitListItems)
                        {
                            MimeMessage mimeMessage;
                            var mimeMessageVariable = item.Trim().ConvertUserVariableToObject(engine);
                            if (mimeMessageVariable != null && mimeMessageVariable is MimeMessage)
                                mimeMessage = (MimeMessage)mimeMessageVariable;
                            else
                                throw new Exception("Invalid List Item type, please provide valid List Item type.");
                            ((List<MimeMessage>)vNewList).Add(mimeMessage);
                        }
                    }
                    break;
                case "IWebElement":
                    vNewList = new List<IWebElement>();
                    if (splitListItems != null)
                    {
                        foreach (string item in splitListItems)
                        {
                            IWebElement webElement;
                            var webElementVariable = item.Trim().ConvertUserVariableToObject(engine);
                            if (webElementVariable != null && webElementVariable is IWebElement)
                                webElement = (IWebElement)webElementVariable;
                            else
                                throw new Exception("Invalid List Item type, please provide valid List Item type.");
                            ((List<IWebElement>)vNewList).Add(webElement);
                        }
                    }
                    break;
            }

            ((object)vNewList).StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            RenderedControls.AddRange(CommandControls.CreateDefaultDropdownGroupFor("v_ListType", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_ListItems", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Create New List<{v_ListType}> With Item(s) '{v_ListItems}' - Store List<{v_ListType}> in '{v_OutputUserVariableName}']";
        }
    }
}