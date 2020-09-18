using System;
using System.Windows;

namespace OpenBots.Core.User32
{
    public class MouseCoordinateEventArgs : EventArgs
    {
        public Point MouseCoordinates { get; set; }
    }
}
