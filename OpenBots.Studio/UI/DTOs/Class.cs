using System.Collections.Generic;

namespace OpenBots.UI.DTOs
{
    public class Class
    {
        public string ClassName { get; set; }
        public List<Method> Methods { get; set; } = new List<Method>();
    }
}
