using System;

namespace OpenBots.Core.Attributes.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property)]
    public class Remarks : Attribute
    {
        public string Remark { get; private set; }
        public Remarks(string remark)
        {
            Remark = remark;
        }
    }
}
