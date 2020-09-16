using System;

namespace OpenBots.Core.Model.EngineModel
{
    public class LineNumberChangedEventArgs : EventArgs
    {
        public int CurrentLineNumber { get; set; }
    }
}
