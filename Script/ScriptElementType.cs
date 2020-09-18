using System.ComponentModel;

namespace OpenBots.Core.Script
{
    public enum ScriptElementType
    {
        [Description("XPath")]
        XPath,
        [Description("ID")]
        ID,
        [Description("Name")]
        Name,
        [Description("Tag Name")]
        TagName,
        [Description("Class Name")]
        ClassName,
        [Description("CSS Selector")]
        CSSSelector,
        [Description("Link Text")]
        LinkText
    }
}
