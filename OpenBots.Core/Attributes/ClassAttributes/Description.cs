using System;

namespace OpenBots.Core.Attributes.ClassAttributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class Description : Attribute
    {
        public string CommandFunctionalDescription { get; private set; }
        public Description(string description)
        {
            CommandFunctionalDescription = description;
        }
    }
}
