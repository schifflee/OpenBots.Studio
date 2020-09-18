using System;

namespace OpenBots.Core.Attributes.ClassAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class UsesDescription : Attribute
    {
        public string Description { get; private set; }
        public UsesDescription(string description)
        {
            Description = description;
        }
    }
}
