using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace OpenBots.Core.Script
{
    [Serializable]
    public class ScriptVariable
    {
        /// <summary>
        /// name that will be used to identify the variable
        /// </summary>
        public string VariableName { get; set; }

        /// <summary>
        /// value of the variable or current index
        /// </summary>
        public object VariableValue { get; set; }

        /// <summary>
        /// To check if the value is a secure string
        /// </summary>
        public bool IsSecureString { get; set; }
    }
}
