using Microsoft.Office.Interop.Outlook;
using MimeKit;
using OpenQA.Selenium;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace OpenBots.Core.Infrastructure
{
    public interface IfrmScriptBuilder
    {
        string ScriptFilePath { get; set; }
        int DebugLine { get; set; }
        IfrmScriptEngine CurrentEngine { get; set; }
        bool IsScriptRunning { get; set; }
        bool IsScriptPaused { get; set; }
        bool IsScriptSteppedOver { get; set; }
        bool IsScriptSteppedInto { get; set; }
        bool IsUnhandledException { get; set; }
        void Notify(string notificationText);
        void RemoveDebugTab();
        DialogResult LoadErrorForm(string errorMessage);
        string HTMLElementRecorderURL { get; set; }

        string ConvertDataTableToString(DataTable dt);
        string ConvertDataRowToString(DataRow row);
        string ConvertMailItemToString(MailItem mail);
        string ConvertMimeMessageToString(MimeMessage message);
        string ConvertIWebElementToString(IWebElement element);
        string ConvertBitmapToString(Bitmap bitmap);
        string ConvertListToString(object list);
    }
}
