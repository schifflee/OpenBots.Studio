using System.Collections.Generic;
using System.Windows.Forms;
using OpenBots.Core.Command;

namespace OpenBots.Core.Infrastructure
{
    public interface ICommandControls
    {
        List<Control> CreateDefaultInputGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor, int height = 30, int width = 300);
        List<Control> CreateDefaultPasswordInputGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor);
        List<Control> CreateDefaultOutputGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor);
        List<Control> CreateDefaultDropdownGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor);
        List<Control> CreateDataGridViewGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor);
        List<Control> CreateDefaultWindowControlGroupFor(string parameterName, ScriptCommand parent, IfrmCommandEditor editor);
        Control CreateDefaultLabelFor(string parameterName, ScriptCommand parent);
        void CreateDefaultToolTipFor(string parameterName, ScriptCommand parent, Control label);
        Control CreateDefaultInputFor(string parameterName, ScriptCommand parent, int height = 30, int width = 300);

        CheckBox CreateCheckBoxFor(string parameterName, ScriptCommand parent);
        Control CreateDropdownFor(string parameterName, ScriptCommand parent);

        ComboBox CreateStandardComboboxFor(string parameterName, ScriptCommand parent);
        List<Control> CreateUIHelpersFor(string parameterName, ScriptCommand parent, Control[] targetControls, IfrmCommandEditor editor);

    }
}
