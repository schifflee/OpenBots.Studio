using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Common;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls;
using taskt.UI.CustomControls.CustomUIControls;

namespace taskt.Commands
{
    [Serializable]
    [Group("Image Commands")]
    [Description("This command captures an image on screen and stores it as a Bitmap variable.")]
    public class CaptureImageCommand : ScriptCommand
    {
        [XmlAttribute]
        [PropertyDescription("Capture Search Image")]
        [InputSpecification("Use the tool to capture an image that will be located on screen during execution.")]
        [SampleUsage("")]
        [Remarks("Images with larger color variance will be found more quickly than those with a lot of white space. \n" +
                 "For images that are primarily white space, tagging color to the top-left corner of the image and setting \n" +
                 "the relative click position will produce faster results.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowImageCaptureHelper)]
        public string v_ImageCapture { get; set; }

        [XmlAttribute]
        [PropertyDescription("Output Image Variable")]
        [InputSpecification("Create a new variable or select a variable from the list.")]
        [SampleUsage("vUserVariable")]
        [Remarks("Variables not pre-defined in the Variable Manager will be automatically generated at runtime.")]
        public string v_OutputUserVariableName { get; set; }

        public CaptureImageCommand()
        {
            CommandName = "CaptureImageCommand";
            SelectionName = "Capture Image";
            CommandEnabled = true;
            CustomRendering = true;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;

            //user image to bitmap
            Bitmap capturedBmp = new Bitmap(Common.Base64ToImage(v_ImageCapture));
            capturedBmp.StoreInUserVariable(engine, v_OutputUserVariableName);
        }

        public override List<Control> Render(IfrmCommandEditor editor)
        {
            base.Render(editor);

            UIPictureBox imageCapture = new UIPictureBox();
            imageCapture.Width = 200;
            imageCapture.Height = 200;
            imageCapture.DataBindings.Add("EncodedImage", this, "v_ImageCapture", false, DataSourceUpdateMode.OnPropertyChanged);

            RenderedControls.Add(CommandControls.CreateDefaultLabelFor("v_ImageCapture", this));
            RenderedControls.AddRange(CommandControls.CreateUIHelpersFor("v_ImageCapture", this, new Control[] { imageCapture }, editor));
            RenderedControls.Add(imageCapture);

            RenderedControls.AddRange(CommandControls.CreateDefaultOutputGroupFor("v_OutputUserVariableName", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [Capture Image on Screen - Store Image in '{v_OutputUserVariableName}']";
        }
    } 
}
