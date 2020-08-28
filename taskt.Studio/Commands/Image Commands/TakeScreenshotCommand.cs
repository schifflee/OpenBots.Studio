using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.User32;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Image Commands")]
    [Description("This command takes a screenshot and saves it to a specified location.")]
    public class TakeScreenshotCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Window Name")]
        [InputSpecification("Select the name of the window that you want to take a screenshot of.")]
        [SampleUsage("Untitled - Notepad || Current Window")]
        [Remarks("")]
        public string v_ScreenshotWindowName { get; set; }

        [XmlAttribute]
        [PropertyDescription("Image Location")]
        [InputSpecification("Enter or Select the path of the folder to save the image to.")]
        [SampleUsage(@"C:\temp || {vFolderPath} || {ProjectPath}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        [PropertyUIHelper(UIAdditionalHelperType.ShowFolderSelectionHelper)]
        public string v_FolderPath { get; set; }

        [XmlAttribute]
        [PropertyDescription("Image File Name")]
        [InputSpecification("Enter or Select the name of the image file.")]
        [SampleUsage("myFile.png || {vFilename}")]
        [Remarks("")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_FileName { get; set; }

        public TakeScreenshotCommand()
        {
            CommandName = "TakeScreenshotCommand";
            SelectionName = "Take Screenshot";
            CommandEnabled = true;
            CustomRendering = true;
            v_ScreenshotWindowName = "Current Window";
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            string vFolderPath = v_FolderPath.ConvertUserVariableToString(engine);
            string vFileName = v_FileName.ConvertUserVariableToString(engine);
            string vFilePath = Path.Combine(vFolderPath, vFileName);

            Bitmap image;

            if (v_ScreenshotWindowName == "Current Window")
                image = ImageMethods.Screenshot();
            else
                image = User32Functions.CaptureWindow(v_ScreenshotWindowName);

            image.Save(vFilePath);
        }
        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            //create window name helper control
            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_ScreenshotWindowName", this));
            var WindowNameControl = CommandControls.CreateStandardComboboxFor("v_ScreenshotWindowName", this).AddWindowNames();
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_ScreenshotWindowName", this, new Control[] { WindowNameControl }, editor));
            RenderedControls.Add(WindowNameControl);

            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FolderPath", this, editor));
            RenderedControls.AddRange(CommandControls.CreateDefaultInputGroupFor("v_FileName", this, editor));

            return RenderedControls;
        }


        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Target Window '{v_ScreenshotWindowName}' - Save to File Path '{v_FolderPath}\\{v_FileName}']";
        }
    }
}