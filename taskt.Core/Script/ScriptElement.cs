using System;
using System.Data;

namespace taskt.Core.Script
{
    [Serializable]
    public class ScriptElement
    {
        public string ElementName { get; set; }
        public DataTable ElementValue { get; set; }
    }
}
