using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenBots.Core.Attributes.ClassAttributes;
using OpenBots.Core.Attributes.PropertyAttributes;
using OpenBots.Core.Command;
using OpenBots.Core.Enums;
using OpenBots.Core.Infrastructure;
using OpenBots.Core.Utilities.CommonUtilities;
using OpenBots.Engine;

namespace OpenBots.Commands
{
    [Serializable]
    [Group("Web Browser Commands")]
    [Description("This command parses and extracts data from an HTML source object or a successful **GetHTMLSourceCommand**.")]

    public class QueryHTMLSourceCommand : ScriptCommand
    {
        [PropertyDescription("HTML")]
        [InputSpecification("Enter the HTML to be queried.")]
        [SampleUsage("<!DOCTYPE html><html><head><title>Example</title></head></html> || {vMyHTML}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_HTMLVariable { get; set; }

        [PropertyDescription("XPath Query")]
        [InputSpecification("Enter the XPath Query and the item will be extracted.")]
        [SampleUsage("@//*[@id=\"aso_search_form_anchor\"]/div/input || {vMyXPath}")]
        [Remarks("You can use Chrome Dev Tools to click an element and copy the XPath.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_XPathQuery { get; set; }

        [PropertyDescription("Output Query Result Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("{vUserVariable}")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public QueryHTMLSourceCommand()
        {
            CommandName = "QueryHTMLSourceCommand";
            SelectionName = "Query HTML Source";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(v_HTMLVariable.ConvertUserVariableToString(engine));

            var div = doc.DocumentNode.SelectSingleNode(v_XPathQuery);
            string divString = div.InnerText;

            divString.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_HTMLVariable", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_XPathQuery", this, editor));
            RenderedControls.AddRange(commandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Query '{v_HTMLVariable}' - Store Result in '{v_OutputUserVariableName}']";
        }
    }
}