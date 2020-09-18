using System;
using System.Windows;
using System.Windows.Forms;

namespace OpenBots.Core.User32
{
    public class KeyDownEventArgs : EventArgs
    {
        public Keys Key { get; set; }
        public Point MouseCoordinates { get; set; }
    }
}
