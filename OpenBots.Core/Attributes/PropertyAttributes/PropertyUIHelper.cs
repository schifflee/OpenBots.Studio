using System;
using OpenBots.Core.Enums;

namespace OpenBots.Core.Attributes.PropertyAttributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    public sealed class PropertyUIHelper : Attribute
    {
        public UIAdditionalHelperType AdditionalHelper { get; private set; }
        public PropertyUIHelper(UIAdditionalHelperType helperType)
        {
            AdditionalHelper = helperType;
        }
    }
}