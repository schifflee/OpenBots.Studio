using Microsoft.Office.Interop.Outlook;
using MimeKit;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using Exception = System.Exception;

namespace OpenBots.Commands.List
{
    [Serializable]
    [Group("List Commands")]
    [Description("This command adds an item to an existing List variable.")]
    public class AddListItemCommand : ScriptCommand
    {

        [PropertyDescription("List")]
        [InputSpecification("Provide a List variable.")]
        [SampleUsage("{vList}")]
        [Remarks("Any type of variable other than List will cause error.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ListName { get; set; }

        [PropertyDescription("List Item")]
        [InputSpecification("Enter the item to add to the List.")]
        [SampleUsage("Hello || {vItem}")]
        [Remarks("List item can only be a String, DataTable, MailItem or IWebElement.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_ListItem { get; set; }

        public AddListItemCommand()
        {
            CommandName = "AddListItemCommand";
            SelectionName = "Add List Item";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            //get sending instance
            var engine = (AutomationEngineInstance)sender;

            var vListVariable = v_ListName.ConvertUserVariableToObject(engine);

            if (vListVariable != null)
            {
                if (vListVariable is List<string>)
                {
                    ((List<string>)vListVariable).Add(v_ListItem.Trim().ConvertUserVariableToString(engine));
                }
                else if (vListVariable is List<DataTable>)
                {
                    DataTable dataTable;
                    var dataTableVariable = v_ListItem.Trim().ConvertUserVariableToObject(engine);
                    if (dataTableVariable != null && dataTableVariable is DataTable)
                        dataTable = (DataTable)dataTableVariable;
                    else
                        throw new Exception("Invalid List Item type, please provide valid List Item type.");
                    ((List<DataTable>)vListVariable).Add(dataTable);
                }
                else if (vListVariable is List<MailItem>)
                {
                    MailItem mailItem;
                    var mailItemVariable = v_ListItem.Trim().ConvertUserVariableToObject(engine);
                    if (mailItemVariable != null && mailItemVariable is MailItem)
                        mailItem = (MailItem)mailItemVariable;
                    else
                        throw new Exception("Invalid List Item type, please provide valid List Item type.");
                    ((List<MailItem>)vListVariable).Add(mailItem);
                }
                else if (vListVariable is List<MimeMessage>)
                {
                    MimeMessage mimeMessage;
                    var mimeMessageVariable = v_ListItem.Trim().ConvertUserVariableToObject(engine);
                    if (mimeMessageVariable != null && mimeMessageVariable is MimeMessage)
                        mimeMessage = (MimeMessage)mimeMessageVariable;
                    else
                        throw new Exception("Invalid List Item type, please provide valid List Item type.");
                    ((List<MimeMessage>)vListVariable).Add(mimeMessage);
                }
                else if (vListVariable is List<IWebElement>)
                {
                    IWebElement webElement;
                    var webElementVariable = v_ListItem.Trim().ConvertUserVariableToObject(engine);
                    if (webElementVariable != null && webElementVariable is IWebElement)
                        webElement = (IWebElement)webElementVariable;
                    else
                        throw new Exception("Invalid List Item type, please provide valid List Item type.");
                    ((List<IWebElement>)vListVariable).Add(webElement);
                }
                else
                {
                    throw new Exception("Complex Variable List Type<T> Not Supported");
                }
            }
            else
            {
                throw new Exception("Attempted to add data to a variable, but the variable was not found. Enclose variables within braces, ex. {vVariable}");
            }
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ListName", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_ListItem", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Add Item '{v_ListItem}' to List '{v_ListName}']";
        }
    }
}
