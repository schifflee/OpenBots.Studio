using System;
using System.IO;

namespace OpenBots.Core.Settings
{
    /// <summary>
    /// Defines application/client-level settings which can be managed by the user
    /// </summary>
    [Serializable]
    public class ClientSettings
    {
        public bool AntiIdleWhileOpen { get; set; }
        public string RootFolder { get; set; }
        public bool InsertCommandsInline { get; set; }
        public bool EnableSequenceDragDrop { get; set; }
        public bool MinimizeToTray { get; set; }
        public string AttendedTasksFolder { get; set; }
        public string StartupMode { get; set; }
        public bool PreloadBuilderCommands { get; set; }
        public bool UseSlimActionBar { get; set; }

        public ClientSettings()
        {
            MinimizeToTray = false;
            AntiIdleWhileOpen = false;
            InsertCommandsInline = true;
            RootFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "OpenBotsStudio");
            StartupMode = "Builder Mode";
            AttendedTasksFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "OpenBotsStudio", "My Scripts");
            EnableSequenceDragDrop = true;
            PreloadBuilderCommands = false;
            UseSlimActionBar = true;
        }
    }
}
