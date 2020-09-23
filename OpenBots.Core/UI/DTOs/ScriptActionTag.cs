using System.Collections.Generic;
using System.Windows.Forms;

namespace OpenBots.Core.UI.DTOs
{
    public class ScriptActionTag
    {
        public List<List<ListViewItem>> UndoList { get; set; } = new List<List<ListViewItem>>();
        public List<List<ListViewItem>> RedoList { get; set; } = new List<List<ListViewItem>>();
    }
}
