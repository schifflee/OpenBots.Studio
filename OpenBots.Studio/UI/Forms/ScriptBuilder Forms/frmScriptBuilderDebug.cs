using Microsoft.Office.Interop.Outlook;
using MimeKit;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Script;
using OpenBots.UI.Forms.Supplement_Forms;

namespace OpenBots.UI.Forms.ScriptBuilder_Forms
{
    public partial class frmScriptBuilder : Form, IfrmScriptBuilder
    {
        public delegate void CreateDebugTabDelegate();
        private void CreateDebugTab()
        {
            if (InvokeRequired)
            {
                var d = new CreateDebugTabDelegate(CreateDebugTab);
                Invoke(d, new object[] { });
            }
            else
            {
                TabPage debugTab = uiPaneTabs.TabPages.Cast<TabPage>().Where(t => t.Name == "DebugVariables")
                                                                              .FirstOrDefault();

                if (debugTab == null)
                {
                    debugTab = new TabPage();
                    debugTab.Name = "DebugVariables";
                    debugTab.Text = "Variables";
                    uiPaneTabs.TabPages.Add(debugTab);
                    uiPaneTabs.SelectedTab = debugTab;
                }
                LoadDebugTab(debugTab);
            }          
        }

        public delegate void LoadDebugTabDelegate(TabPage debugTab);
        private void LoadDebugTab(TabPage debugTab)
        {
            if (InvokeRequired)
            {
                var d = new LoadDebugTabDelegate(LoadDebugTab);
                Invoke(d, new object[] { debugTab });
            }
            else
            {
                DataTable variableValues = new DataTable();
                variableValues.Columns.Add("Name");
                variableValues.Columns.Add("Type");
                variableValues.Columns.Add("Value");
                variableValues.TableName = "VariableValuesDataTable" + DateTime.Now.ToString("MMddyyhhmmss");

                DataGridView variablesGridViewHelper = new DataGridView();
                variablesGridViewHelper.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                variablesGridViewHelper.Dock = DockStyle.Fill;
                variablesGridViewHelper.ColumnHeadersHeight = 30;
                variablesGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
                variablesGridViewHelper.AllowUserToAddRows = false;
                variablesGridViewHelper.AllowUserToDeleteRows = false;

                if (debugTab.Controls.Count != 0)
                    debugTab.Controls.RemoveAt(0);
                debugTab.Controls.Add(variablesGridViewHelper);

                List<ScriptVariable> engineVariables = ((frmScriptEngine)CurrentEngine).EngineInstance.VariableList;
                foreach (var variable in engineVariables)
                {
                    DataRow[] foundVariables = variableValues.Select("Name = '" + variable.VariableName + "'");
                    if (foundVariables.Length == 0)
                    {
                        string type = "";
                        if (variable.VariableValue != null)
                            type = variable.VariableValue.GetType().FullName;

                        switch (type)
                        {
                            case "System.String":
                                variableValues.Rows.Add(variable.VariableName, type,
                                    variable.VariableValue);
                                break;
                            case "System.Security.SecureString":
                                variableValues.Rows.Add(variable.VariableName, type,
                                    "*Secure String*");
                                break;
                            case "System.Data.DataTable":
                                variableValues.Rows.Add(variable.VariableName, type, 
                                    ConvertDataTableToString((DataTable)variable.VariableValue));
                                break;
                            case "System.Data.DataRow":
                                variableValues.Rows.Add(variable.VariableName, type,
                                    ConvertDataRowToString((DataRow)variable.VariableValue));
                                break;
                            case "System.__ComObject":
                                variableValues.Rows.Add(variable.VariableName, "Microsoft.Office.Interop.Outlook.MailItem",
                                    ConvertMailItemToString((MailItem)variable.VariableValue));
                                break;
                            case "MimeKit.MimeMessage":
                                variableValues.Rows.Add(variable.VariableName, type,
                                    ConvertMimeMessageToString((MimeMessage)variable.VariableValue));
                                break;
                            case "OpenQA.Selenium.Remote.RemoteWebElement":
                                variableValues.Rows.Add(variable.VariableName, "OpenQA.Selenium.IWebElement",
                                    ConvertIWebElementToString((IWebElement)variable.VariableValue));
                                break;
                            case "System.Drawing.Bitmap":
                                variableValues.Rows.Add(variable.VariableName, type,
                                    ConvertBitmapToString((Bitmap)variable.VariableValue));
                                break;
                            case string a when a.Contains("System.Collections.Generic.List`1[[System.String"):
                            case string b when b.Contains("System.Collections.Generic.List`1[[System.Data.DataTable"):
                            case string c when c.Contains("System.Collections.Generic.List`1[[Microsoft.Office.Interop.Outlook.MailItem"):
                            case string d when d.Contains("System.Collections.Generic.List`1[[MimeKit.MimeMessage"):
                            case string e when e.Contains("System.Collections.Generic.List`1[[OpenQA.Selenium.IWebElement"):
                                variableValues.Rows.Add(variable.VariableName, type,
                                    ConvertListToString(variable.VariableValue));
                                break;
                            case string a when a.Contains("System.Collections.Generic.Dictionary`2[[System.String") && a.Contains("],[System.String"):
                                variableValues.Rows.Add(variable.VariableName, type,
                                   ConvertDictionaryToString(variable.VariableValue));
                                break;
                            case "":
                                variableValues.Rows.Add(variable.VariableName, "null", "null");
                                break;
                            default:
                                variableValues.Rows.Add(variable.VariableName, type, 
                                    "*Type Not Yet Supported*");
                                break;
                        }                       
                    }
                }
                variablesGridViewHelper.DataSource = variableValues;
                uiPaneTabs.SelectedTab = debugTab;
            }           
        }

        public delegate void RemoveDebugTabDelegate();
        public void RemoveDebugTab()
        {
            if (InvokeRequired)
            {
                var d = new RemoveDebugTabDelegate(RemoveDebugTab);
                Invoke(d, new object[] { });
            }
            else
            {
                TabPage debugTab = uiPaneTabs.TabPages.Cast<TabPage>().Where(t => t.Name == "DebugVariables")
                                                                              .FirstOrDefault();

                if (debugTab != null)
                    uiPaneTabs.TabPages.Remove(debugTab);
            }
        }

        public string ConvertDataTableToString(DataTable dt)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("[[");

            for (int i = 0; i < dt.Columns.Count - 1; i++)
                stringBuilder.AppendFormat("{0}, ", dt.Columns[i].ColumnName);

            stringBuilder.AppendFormat("{0}]]", dt.Columns[dt.Columns.Count -1].ColumnName);
            stringBuilder.AppendLine();

            foreach (DataRow rows in dt.Rows)
            {
                stringBuilder.Append("[");

                for(int i = 0; i<dt.Columns.Count-1; i++)
                    stringBuilder.AppendFormat("{0}, ", rows[i]);

                stringBuilder.AppendFormat("{0}]", rows[dt.Columns.Count - 1]);
            }
            return stringBuilder.ToString();
        }

        public string ConvertDataRowToString(DataRow row)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("[");

            for (int i = 0; i < row.ItemArray.Length - 1; i++)
                stringBuilder.AppendFormat("{0}, ", row.ItemArray[i]);

            stringBuilder.AppendFormat("{0}]", row.ItemArray[row.ItemArray.Length - 1]);
            return stringBuilder.ToString();
        }

        public string ConvertMailItemToString(MailItem mail)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"[Subject: {mail.Subject}, \n" +
                                  $"Sender: {mail.SenderName}, \n" +
                                  $"Sent On: {mail.SentOn}, \n" +
                                  $"Unread: {mail.UnRead}, \n" +
                                  $"Attachments({mail.Attachments.Count})");
            
            if (mail.Attachments.Count > 0)
            {
                stringBuilder.Append(" [");
                foreach(Attachment attachment in mail.Attachments)
                    stringBuilder.Append($"{attachment.FileName}, ");

                //trim final comma
                stringBuilder.Length = stringBuilder.Length - 2;
                stringBuilder.Append("]");
            }

            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

        public string ConvertMimeMessageToString(MimeMessage message)
        {
            int attachmentCount = 0;
            foreach (var attachment in message.Attachments)
                attachmentCount += 1;

            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"[Subject: {message.Subject}, \n" +
                                  $"Sender: {message.From}, \n" +
                                  $"Sent On: {message.Date}, \n" +
                                  $"Attachments({attachmentCount})");
            
            if (attachmentCount > 0)
            {
                stringBuilder.Append(" [");
                foreach (var attachment in message.Attachments)
                    stringBuilder.Append($"{attachment.ContentDisposition?.FileName}, " ??
                                         "attached-message.eml, ");

                //trim final comma
                stringBuilder.Length = stringBuilder.Length - 2;
                stringBuilder.Append("]");
            }

            stringBuilder.Append("]");

            return stringBuilder.ToString();
        }

        public string ConvertIWebElementToString(IWebElement element)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"[Text: {element.Text}, \n" +
                                 $"Tag Name: {element.TagName}, \n" +
                                 $"Location: {element.Location}, \n" +
                                 $"Size: {element.Size}, \n" +
                                 $"Displayed: {element.Displayed}, \n" +
                                 $"Enabled: {element.Enabled}, \n" +
                                 $"Selected: {element.Selected}]");
            return stringBuilder.ToString();
        }

        public string ConvertBitmapToString(Bitmap bitmap)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append($"Size({bitmap.Width}, {bitmap.Height})");
            return stringBuilder.ToString();
        }

        public string ConvertListToString(object list)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Type type = list.GetType().GetGenericArguments()[0];

            if (type == typeof(string))
            {
                List<string> stringList = (List<string>)list;
                stringBuilder.Append($"Count({stringList.Count}) [");

                for (int i = 0; i < stringList.Count - 1; i++)
                    stringBuilder.AppendFormat("{0}, ", stringList[i]);

                if (stringList.Count > 0)
                    stringBuilder.AppendFormat("{0}]", stringList[stringList.Count - 1]);
                else
                    stringBuilder.Length = stringBuilder.Length - 2;
            }
            else if (type == typeof(DataTable))
            {
                List<DataTable> dataTableList = (List<DataTable>)list;
                stringBuilder.Append($"Count({dataTableList.Count}) \n[");

                for (int i = 0; i < dataTableList.Count - 1; i++)
                    stringBuilder.AppendFormat("{0}, \n", ConvertDataTableToString(dataTableList[i]));

                if (dataTableList.Count > 0)
                    stringBuilder.AppendFormat("{0}]", ConvertDataTableToString(dataTableList[dataTableList.Count - 1]));
                else
                    stringBuilder.Length = stringBuilder.Length - 3;
            }
            else if (type == typeof(MailItem))
            {
                List<MailItem> mailItemList = (List<MailItem>)list;
                stringBuilder.Append($"Count({mailItemList.Count}) \n[");

                for (int i = 0; i < mailItemList.Count - 1; i++)
                    stringBuilder.AppendFormat("{0}, \n", ConvertMailItemToString(mailItemList[i]));

                if (mailItemList.Count > 0)
                    stringBuilder.AppendFormat("{0}]", ConvertMailItemToString(mailItemList[mailItemList.Count - 1]));
                else
                    stringBuilder.Length = stringBuilder.Length - 3;
            }
            else if (type == typeof(MimeMessage))
            {
                List<MimeMessage> mimeMessageList = (List<MimeMessage>)list;
                stringBuilder.Append($"Count({mimeMessageList.Count}) \n[");

                for (int i = 0; i < mimeMessageList.Count - 1; i++)
                    stringBuilder.AppendFormat("{0}, \n", ConvertMimeMessageToString(mimeMessageList[i]));

                if (mimeMessageList.Count > 0)
                    stringBuilder.AppendFormat("{0}]", ConvertMimeMessageToString(mimeMessageList[mimeMessageList.Count - 1]));
                else
                    stringBuilder.Length = stringBuilder.Length - 3;
            }
            else if (type == typeof(IWebElement))
            {
                List<IWebElement> elementList = (List<IWebElement>)list;
                stringBuilder.Append($"Count({elementList.Count}) \n[");

                for (int i = 0; i < elementList.Count - 1; i++)
                    stringBuilder.AppendFormat("{0}, \n", ConvertIWebElementToString(elementList[i]));

                if (elementList.Count > 0)
                    stringBuilder.AppendFormat("{0}]", ConvertIWebElementToString(elementList[elementList.Count - 1]));
                else
                    stringBuilder.Length = stringBuilder.Length - 3;
            }

            return stringBuilder.ToString();
        }

        public string ConvertDictionaryToString(object dictionary)
        {
            StringBuilder stringBuilder = new StringBuilder();
            Type type = dictionary.GetType().GetGenericArguments()[1];
             
            if (type == typeof(string))
            {
                Dictionary<string, string> stringDictionary = (Dictionary<string, string>)dictionary;
                stringBuilder.Append($"Count({stringDictionary.Count}) [");

                foreach (KeyValuePair<string, string> pair in stringDictionary)
                    stringBuilder.AppendFormat("[{0}, {1}], ", pair.Key, pair.Value);

                if (stringDictionary.Count > 0)
                {
                    stringBuilder.Length = stringBuilder.Length - 2;
                    stringBuilder.Append("]");
                }                   
                else
                    stringBuilder.Length = stringBuilder.Length - 2;
            }

            return stringBuilder.ToString();
        }

        public delegate DialogResult LoadErrorFormDelegate(string errorMessage);
        public DialogResult LoadErrorForm(string errorMessage)
        {
            if (InvokeRequired)
            {
                var d = new LoadErrorFormDelegate(LoadErrorForm);
                return (DialogResult)Invoke(d, new object[] { errorMessage });
            }
            else
            {
                frmError errorForm = new frmError(errorMessage);
                errorForm.Owner = this;
                errorForm.ShowDialog();
                return errorForm.DialogResult;
            }          
        }
    }
}
