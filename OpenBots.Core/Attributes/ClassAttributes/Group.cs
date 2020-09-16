using System;

namespace OpenBots.Core.Attributes.ClassAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Group : Attribute
    {
        public string Name { get; private set; }
        public Group(string name)
        {
            Name = name;
        }
    }
}
