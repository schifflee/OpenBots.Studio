using System;
using System.Data;

namespace OpenBots.Core.Script
{
    [Serializable]
    public class ScriptElement
    {
        public string ElementName { get; set; }
        public DataTable ElementValue { get; set; }
    }
}
