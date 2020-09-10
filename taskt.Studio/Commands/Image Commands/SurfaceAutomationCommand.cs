using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security;
using System.Threading;
using System.Windows.Forms;
using System.Xml.Serialization;
using taskt.Core.Attributes.ClassAttributes;
using taskt.Core.Attributes.PropertyAttributes;
using taskt.Core.Command;
using taskt.Core.Common;
using taskt.Core.Enums;
using taskt.Core.Infrastructure;
using taskt.Core.Utilities.CommandUtilities;
using taskt.Core.Utilities.CommonUtilities;
using taskt.Engine;
using taskt.UI.CustomControls.CustomUIControls;
using taskt.UI.Forms.Supplement_Forms;
using taskt.Utilities;
using WindowsInput;

namespace taskt.Commands
{
    [Serializable]
    [Group("Image Commands")]
    [Description("This command attempts to find and perform an action on an existing image on screen.")]
    public class SurfaceAutomationCommand : ScriptCommand
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

        [XmlElement]
        [PropertyDescription("Element Action")]
        [PropertyUISelectionOption("Click Image")]
        [PropertyUISelectionOption("Set Text")]
        [PropertyUISelectionOption("Set Secure Text")]
        [PropertyUISelectionOption("Check If Image Exists")]
        [PropertyUISelectionOption("Wait For Image To Exist")]
        [InputSpecification("Select the appropriate corresponding action to take once the image has been located.")]
        [SampleUsage("")]
        [Remarks("Selecting this field changes the parameters required in the following step.")]
        public string v_ImageAction { get; set; }

        [XmlElement]
        [PropertyDescription("Additional Parameters")]
        [InputSpecification("Additional Parameters will be required based on the action settings selected.")]
        [SampleUsage("data || {vData}")]
        [Remarks("Additional Parameters range from adding offset coordinates to specifying a variable to apply element text to.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public DataTable v_ImageActionParameterTable { get; set; }

        [XmlAttribute]
        [PropertyDescription("Accuracy (0-1)")]
        [InputSpecification("Enter a value between 0 and 1 to set the match Accuracy. Set to 1 for a perfect match.")]
        [SampleUsage("0.8 || 1 || {vAccuracy}")]
        [Remarks("Accuracy must be a value between 0 and 1.")]
        [PropertyUIHelper(UIAdditionalHelperType.ShowVariableHelper)]
        public string v_MatchAccuracy { get; set; }

        public bool TestMode = false;

        [XmlIgnore]
        [NonSerialized]
        private DataGridView _imageGridViewHelper;

        [XmlIgnore]
        [NonSerialized]
        private ComboBox _imageActionDropdown;

        [XmlIgnore]
        [NonSerialized]
        private List<Control> _imageParameterControls;

        public SurfaceAutomationCommand()
        {
            CommandName = "SurfaceAutomationCommand";
            SelectionName = "Surface Automation";
            CommandEnabled = true;
            CustomRendering = true;
           
            v_MatchAccuracy = "0.8";

            v_ImageActionParameterTable = new DataTable
            {
                TableName = "ImageActionParamTable" + DateTime.Now.ToString("MMddyy.hhmmss")
            };
            v_ImageActionParameterTable.Columns.Add("Parameter Name");
            v_ImageActionParameterTable.Columns.Add("Parameter Value");

            _imageGridViewHelper = new DataGridView();
            _imageGridViewHelper.AllowUserToAddRows = true;
            _imageGridViewHelper.AllowUserToDeleteRows = true;
            _imageGridViewHelper.Size = new Size(400, 250);
            _imageGridViewHelper.ColumnHeadersHeight = 30;
            _imageGridViewHelper.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            _imageGridViewHelper.DataBindings.Add("DataSource", this, "v_ImageActionParameterTable", false, DataSourceUpdateMode.OnPropertyChanged);
            _imageGridViewHelper.AllowUserToAddRows = false;
            _imageGridViewHelper.AllowUserToDeleteRows = false;
            //_imageGridViewHelper.AllowUserToResizeRows = false;
            _imageGridViewHelper.MouseEnter += ImageGridViewHelper_MouseEnter;
        }

        public override void RunCommand(object sender)
        {
            var engine = (AutomationEngineInstance)sender;
            bool testMode = TestMode;
            //user image to bitmap
            Bitmap userImage = new Bitmap(Common.Base64ToImage(v_ImageCapture));
            double accuracy;
            try
            {
                accuracy = double.Parse(v_MatchAccuracy.ConvertUserVariableToString(engine));
                if (accuracy > 1 || accuracy < 0)
                    throw new ArgumentOutOfRangeException("Accuracy value is out of range (0-1)");
            }
            catch (Exception)
            {
                throw new InvalidDataException("Accuracy value is invalid");
            }           

            if (testMode)
            {
                FindImageElement(userImage, accuracy);
                return;
            }
                
            dynamic element = null;
            if (v_ImageAction == "Wait For Image To Exist")
            {
                var timeoutText = (from rw in v_ImageActionParameterTable.AsEnumerable()
                                   where rw.Field<string>("Parameter Name") == "Timeout (Seconds)"
                                   select rw.Field<string>("Parameter Value")).FirstOrDefault();

                timeoutText = timeoutText.ConvertUserVariableToString(engine);
                int timeOut = Convert.ToInt32(timeoutText);
                var timeToEnd = DateTime.Now.AddSeconds(timeOut);

                while (timeToEnd >= DateTime.Now)
                {
                    try
                    {
                        element = FindImageElement(userImage, accuracy);

                        if (element == null)
                            throw new Exception("Image Element Not Found");
                        else
                            break;
                    }
                    catch (Exception)
                    {
                        engine.ReportProgress("Element Not Yet Found... " + (timeToEnd - DateTime.Now).Seconds + "s remain");
                        Thread.Sleep(1000);
                    }
                }

                if (element == null)
                    throw new Exception("Image Element Not Found");

                return;
            }
            else
                element = FindImageElement(userImage, accuracy);

            try
            {
                string clickPosition;
                int xAdjust;
                int yAdjust;
                switch (v_ImageAction)
                {
                    case "Click Image":
                        string clickType = (from rw in v_ImageActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Click Type"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault();
                        clickPosition = (from rw in v_ImageActionParameterTable.AsEnumerable()
                                                where rw.Field<string>("Parameter Name") == "Click Position"
                                                select rw.Field<string>("Parameter Value")).FirstOrDefault();
                        xAdjust = Convert.ToInt32((from rw in v_ImageActionParameterTable.AsEnumerable()
                                                       where rw.Field<string>("Parameter Name") == "X Adjustment"
                                                       select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertUserVariableToString(engine));
                        yAdjust = Convert.ToInt32((from rw in v_ImageActionParameterTable.AsEnumerable()
                                                       where rw.Field<string>("Parameter Name") == "Y Adjustment"
                                                       select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertUserVariableToString(engine));

                        Point clickPositionPoint = GetClickPosition(clickPosition, element); 

                        //move mouse to position
                        var mouseMove = new SendMouseMoveCommand
                        {
                            v_XMousePosition = (clickPositionPoint.X + xAdjust).ToString(),
                            v_YMousePosition = (clickPositionPoint.Y + yAdjust).ToString(),
                            v_MouseClick = clickType
                        };

                        mouseMove.RunCommand(sender);
                        break;

                    case "Set Text":
                        string textToSet = (from rw in v_ImageActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Text To Set"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertUserVariableToString(engine);
                        clickPosition = (from rw in v_ImageActionParameterTable.AsEnumerable()
                                         where rw.Field<string>("Parameter Name") == "Click Position"
                                         select rw.Field<string>("Parameter Value")).FirstOrDefault();
                        xAdjust = Convert.ToInt32((from rw in v_ImageActionParameterTable.AsEnumerable()
                                                   where rw.Field<string>("Parameter Name") == "X Adjustment"
                                                   select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertUserVariableToString(engine));
                        yAdjust = Convert.ToInt32((from rw in v_ImageActionParameterTable.AsEnumerable()
                                                   where rw.Field<string>("Parameter Name") == "Y Adjustment"
                                                   select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertUserVariableToString(engine));
                        string encryptedData = (from rw in v_ImageActionParameterTable.AsEnumerable()
                                                where rw.Field<string>("Parameter Name") == "Encrypted Text"
                                                select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        if (encryptedData == "Encrypted")
                            textToSet = EncryptionServices.DecryptString(textToSet, "OPENBOTS");

                        Point setTextPositionPoint = GetClickPosition(clickPosition, element);

                        //move mouse to position and set text
                        var setTextMouseMove = new SendMouseMoveCommand
                        {
                            v_XMousePosition = (setTextPositionPoint.X + xAdjust).ToString(),
                            v_YMousePosition = (setTextPositionPoint.Y + yAdjust).ToString(),
                            v_MouseClick = "Left Click"
                        };
                        setTextMouseMove.RunCommand(sender);

                        var simulator = new InputSimulator();
                        simulator.Keyboard.TextEntry(textToSet);
                        Thread.Sleep(100);
                        break;

                    case "Set Secure Text":
                        var secureString = (from rw in v_ImageActionParameterTable.AsEnumerable()
                                            where rw.Field<string>("Parameter Name") == "Secure String Variable"
                                            select rw.Field<string>("Parameter Value")).FirstOrDefault();
                        clickPosition = (from rw in v_ImageActionParameterTable.AsEnumerable()
                                         where rw.Field<string>("Parameter Name") == "Click Position"
                                         select rw.Field<string>("Parameter Value")).FirstOrDefault();
                        xAdjust = Convert.ToInt32((from rw in v_ImageActionParameterTable.AsEnumerable()
                                                   where rw.Field<string>("Parameter Name") == "X Adjustment"
                                                   select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertUserVariableToString(engine));
                        yAdjust = Convert.ToInt32((from rw in v_ImageActionParameterTable.AsEnumerable()
                                                   where rw.Field<string>("Parameter Name") == "Y Adjustment"
                                                   select rw.Field<string>("Parameter Value")).FirstOrDefault().ConvertUserVariableToString(engine));

                        var secureStrVariable = secureString.ConvertUserVariableToObject(engine);

                        if (secureStrVariable is SecureString)
                            secureString = ((SecureString)secureStrVariable).ConvertSecureStringToString();
                        else
                            throw new ArgumentException("Provided Argument is not a 'Secure String'");

                        Point setSecureTextPositionPoint = GetClickPosition(clickPosition, element);

                        //move mouse to position and set text
                        var setSecureTextMouseMove = new SendMouseMoveCommand
                        {
                            v_XMousePosition = (setSecureTextPositionPoint.X + xAdjust).ToString(),
                            v_YMousePosition = (setSecureTextPositionPoint.Y + yAdjust).ToString(),
                            v_MouseClick = "Left Click"
                        };
                        setSecureTextMouseMove.RunCommand(sender);

                        var simulator2 = new InputSimulator();
                        simulator2.Keyboard.TextEntry(secureString);
                        Thread.Sleep(100);
                        break;

                    case "Check If Image Exists":
                        var outputVariable = (from rw in v_ImageActionParameterTable.AsEnumerable()
                                              where rw.Field<string>("Parameter Name") == "Output Bool Variable Name"
                                              select rw.Field<string>("Parameter Value")).FirstOrDefault();

                        //remove brackets from variable
                        outputVariable = outputVariable.Replace("{", "").Replace("}", "");

                        if (element != null)
                            "True".StoreInUserVariable(engine, outputVariable);
                        else
                            "False".StoreInUserVariable(engine, outputVariable);
                        break;
                    default:
                        break;                       
                }
                UIControlsHelper.ShowAllForms();
            }
            catch (Exception ex)
            {
                UIControlsHelper.ShowAllForms();
                if (element == null)
                    throw new Exception("Specified image was not found in window!");
                else
                    throw ex;
            }                
        }   

        public override List<Control> Render(IfrmCommandEditor editor, ICommandControls commandControls)
        {
            base.Render(editor, commandControls);

            UIPictureBox imageCapture = new UIPictureBox();
            imageCapture.Width = 200;
            imageCapture.Height = 200;
            imageCapture.DataBindings.Add("EncodedImage", this, "v_ImageCapture", false, DataSourceUpdateMode.OnPropertyChanged);

            RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_ImageCapture", this));
            RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_ImageCapture", this, new Control[] { imageCapture }, editor));
            RenderedControls.Add(imageCapture);

            _imageActionDropdown = (ComboBox)commandControls.CreateDropdownFor("v_ImageAction", this);
            RenderedControls.Add(commandControls.CreateDefaultLabelFor("v_ImageAction", this));
            RenderedControls.AddRange(commandControls.CreateUIHelpersFor("v_ImageAction", this, new Control[] { _imageActionDropdown }, editor));
            _imageActionDropdown.SelectionChangeCommitted += ImageAction_SelectionChangeCommitted;
            RenderedControls.Add(_imageActionDropdown);

            _imageParameterControls = new List<Control>();
            _imageParameterControls.Add(commandControls.CreateDefaultLabelFor("v_ImageActionParameterTable", this));
            _imageParameterControls.AddRange(commandControls.CreateUIHelpersFor("v_ImageActionParameterTable", this, new Control[] { _imageGridViewHelper }, editor));
            _imageParameterControls.Add(_imageGridViewHelper);
            RenderedControls.AddRange(_imageParameterControls);

            RenderedControls.AddRange(commandControls.CreateDefaultInputGroupFor("v_MatchAccuracy", this, editor));

            return RenderedControls;
        }

        public override string GetDisplayValue()
        {
            return base.GetDisplayValue() + $" [{v_ImageAction} on Screen - Accuracy '{v_MatchAccuracy}']";
        }

        public ImageElement FindImageElement(Bitmap smallBmp, double accuracy)
        {
            UIControlsHelper.HideAllForms();
            bool testMode = TestMode;
            dynamic element = null;
            double tolerance = 1.0 - accuracy;

            Bitmap bigBmp = ImageMethods.Screenshot();

            Bitmap smallTestBmp = new Bitmap(smallBmp);

            Bitmap bigTestBmp = new Bitmap(bigBmp);
            Graphics bigTestGraphics = Graphics.FromImage(bigTestBmp);

            BitmapData smallData =
              smallBmp.LockBits(new Rectangle(0, 0, smallBmp.Width, smallBmp.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format24bppRgb);
            BitmapData bigData =
              bigBmp.LockBits(new Rectangle(0, 0, bigBmp.Width, bigBmp.Height),
                       ImageLockMode.ReadOnly,
                       PixelFormat.Format24bppRgb);

            int smallStride = smallData.Stride;
            int bigStride = bigData.Stride;

            int bigWidth = bigBmp.Width;
            int bigHeight = bigBmp.Height - smallBmp.Height + 1;
            int smallWidth = smallBmp.Width * 3;
            int smallHeight = smallBmp.Height;

            int margin = Convert.ToInt32(255.0 * tolerance);

            unsafe
            {
                byte* pSmall = (byte*)(void*)smallData.Scan0;
                byte* pBig = (byte*)(void*)bigData.Scan0;

                int smallOffset = smallStride - smallBmp.Width * 3;
                int bigOffset = bigStride - bigBmp.Width * 3;

                bool matchFound = true;

                for (int y = 0; y < bigHeight; y++)
                {
                    for (int x = 0; x < bigWidth; x++)
                    {
                        byte* pBigBackup = pBig;
                        byte* pSmallBackup = pSmall;

                        //Look for the small picture.
                        for (int i = 0; i < smallHeight; i++)
                        {
                            int j = 0;
                            matchFound = true;
                            for (j = 0; j < smallWidth; j++)
                            {
                                //With tolerance: pSmall value should be between margins.
                                int inf = pBig[0] - margin;
                                int sup = pBig[0] + margin;
                                if (sup < pSmall[0] || inf > pSmall[0])
                                {
                                    matchFound = false;
                                    break;
                                }

                                pBig++;
                                pSmall++;
                            }

                            if (!matchFound) 
                                break;

                            //We restore the pointers.
                            pSmall = pSmallBackup;
                            pBig = pBigBackup;

                            //Next rows of the small and big pictures.
                            pSmall += smallStride * (1 + i);
                            pBig += bigStride * (1 + i);
                        }

                        //If match found, we return.
                        if (matchFound)
                        {
                            element = new ImageElement
                            {
                                LeftX = x,
                                MiddleX = x + smallBmp.Width / 2,
                                RightX = x + smallBmp.Width,
                                TopY = y,
                                MiddleY = y + smallBmp.Height / 2,
                                BottomY = y + smallBmp.Height
                            };

                            if (testMode)
                            {
                                //draw on output to demonstrate finding
                                var Rectangle = new Rectangle(x, y, smallBmp.Width - 1, smallBmp.Height - 1);
                                Pen pen = new Pen(Color.Red);
                                pen.Width = 5.0F;
                                bigTestGraphics.DrawRectangle(pen, Rectangle);

                                frmImageCapture captureOutput = new frmImageCapture();
                                captureOutput.pbTaggedImage.Image = smallTestBmp;
                                captureOutput.pbSearchResult.Image = bigTestBmp;                               
                                captureOutput.TopMost = true;
                                captureOutput.Show();
                            }
                            break;
                        }
                        //If no match found, we restore the pointers and continue.
                        else
                        {
                            pBig = pBigBackup;
                            pSmall = pSmallBackup;
                            pBig += 3;
                        }
                    }

                    if (matchFound) 
                        break;

                    pBig += bigOffset;
                }
            }

            bigBmp.UnlockBits(bigData);
            smallBmp.UnlockBits(smallData);
            bigTestGraphics.Dispose();
            return element;
        }
        private void ImageAction_SelectionChangeCommitted(object sender, EventArgs e)
        {
            SurfaceAutomationCommand cmd = this;
            DataTable actionParameters = cmd.v_ImageActionParameterTable;

            if (sender != null)
                actionParameters.Rows.Clear();

            DataGridViewComboBoxCell mouseClickPositionBox = new DataGridViewComboBoxCell();
            mouseClickPositionBox.Items.Add("Center");
            mouseClickPositionBox.Items.Add("Top Left");
            mouseClickPositionBox.Items.Add("Top Middle");
            mouseClickPositionBox.Items.Add("Top Right");
            mouseClickPositionBox.Items.Add("Bottom Left");
            mouseClickPositionBox.Items.Add("Bottom Middle");
            mouseClickPositionBox.Items.Add("Bottom Right");
            mouseClickPositionBox.Items.Add("Middle Left");
            mouseClickPositionBox.Items.Add("Middle Right");

            switch (_imageActionDropdown.SelectedItem)
            {
                 case "Click Image":
                    foreach (var ctrl in _imageParameterControls)
                        ctrl.Show();

                    DataGridViewComboBoxCell mouseClickTypeBox = new DataGridViewComboBoxCell();
                    mouseClickTypeBox.Items.Add("Left Click");
                    mouseClickTypeBox.Items.Add("Middle Click");
                    mouseClickTypeBox.Items.Add("Right Click");
                    mouseClickTypeBox.Items.Add("Left Down");
                    mouseClickTypeBox.Items.Add("Middle Down");
                    mouseClickTypeBox.Items.Add("Right Down");
                    mouseClickTypeBox.Items.Add("Left Up");
                    mouseClickTypeBox.Items.Add("Middle Up");
                    mouseClickTypeBox.Items.Add("Right Up");
                    mouseClickTypeBox.Items.Add("Double Left Click");

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Click Type", "Left Click");
                        actionParameters.Rows.Add("Click Position", "Center");
                        actionParameters.Rows.Add("X Adjustment", 0);
                        actionParameters.Rows.Add("Y Adjustment", 0);                      
                    }

                    _imageGridViewHelper.Rows[0].Cells[1] = mouseClickTypeBox;
                    _imageGridViewHelper.Rows[1].Cells[1] = mouseClickPositionBox;

                    break;

                case "Set Text":
                    foreach (var ctrl in _imageParameterControls)
                        ctrl.Show();

                    DataGridViewComboBoxCell encryptedBox = new DataGridViewComboBoxCell();
                    encryptedBox.Items.Add("Not Encrypted");
                    encryptedBox.Items.Add("Encrypted");

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Text To Set");
                        actionParameters.Rows.Add("Click Position", "Center");
                        actionParameters.Rows.Add("X Adjustment", 0);
                        actionParameters.Rows.Add("Y Adjustment", 0);
                        actionParameters.Rows.Add("Encrypted Text", "Not Encrypted");
                        actionParameters.Rows.Add("Optional - Click to Encrypt 'Text To Set'");                     

                        var buttonCell = new DataGridViewButtonCell();
                        _imageGridViewHelper.Rows[5].Cells[1] = buttonCell;
                        _imageGridViewHelper.Rows[5].Cells[1].Value = "Encrypt Text";
                        _imageGridViewHelper.CellContentClick += ImageGridViewHelper_CellContentClick;
                    }

                    _imageGridViewHelper.Rows[1].Cells[1] = mouseClickPositionBox;
                    _imageGridViewHelper.Rows[4].Cells[1] = encryptedBox;

                    break;

                case "Set Secure Text":
                    foreach (var ctrl in _imageParameterControls)
                        ctrl.Show();

                    if (sender != null)
                    {
                        actionParameters.Rows.Add("Secure String Variable");
                        actionParameters.Rows.Add("Click Position", "Center");
                        actionParameters.Rows.Add("X Adjustment", 0);
                        actionParameters.Rows.Add("Y Adjustment", 0);                       
                    }

                    _imageGridViewHelper.Rows[1].Cells[1] = mouseClickPositionBox;

                    break;

                case "Check If Image Exists":
                    foreach (var ctrl in _imageParameterControls)
                        ctrl.Show();

                    if (sender != null)
                        actionParameters.Rows.Add("Output Bool Variable Name", "");
                    break;

                 case "Wait For Image To Exist":
                    foreach (var ctrl in _imageParameterControls)
                        ctrl.Show();

                    if (sender != null)
                        actionParameters.Rows.Add("Timeout (Seconds)", 30);
                    break;

                default:
                    break;
            }
        }

        private void ImageGridViewHelper_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            var targetCell = _imageGridViewHelper.Rows[e.RowIndex].Cells[e.ColumnIndex];

            if (targetCell is DataGridViewButtonCell && targetCell.Value.ToString() == "Encrypt Text")
            {
                var targetElement = _imageGridViewHelper.Rows[0].Cells[1];

                if (string.IsNullOrEmpty(targetElement.Value.ToString()))
                    return;

                var warning = MessageBox.Show($"Warning! Text should only be encrypted one time and is not reversible in the builder. " +
                                               "Would you like to proceed and convert '{targetElement.Value.ToString()}' to an encrypted value?",
                                               "Encryption Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (warning == DialogResult.Yes)
                {
                    targetElement.Value = EncryptionServices.EncryptString(targetElement.Value.ToString(), "OPENBOTS");
                    _imageGridViewHelper.Rows[4].Cells[1].Value = "Encrypted";
                }
            }
        }

        private Point GetClickPosition(string clickPosition, ImageElement element)
        {
            int clickPositionX = 0;
            int clickPositionY = 0;
            switch (clickPosition)
            {
                case "Center":
                    clickPositionX = element.MiddleX;
                    clickPositionY = element.MiddleY;
                    break;
                case "Top Left":
                    clickPositionX = element.LeftX;
                    clickPositionY = element.TopY;
                    break;
                case "Top Middle":
                    clickPositionX = element.MiddleX;
                    clickPositionY = element.TopY;
                    break;
                case "Top Right":
                    clickPositionX = element.RightX;
                    clickPositionY = element.TopY;
                    break;
                case "Bottom Left":
                    clickPositionX = element.LeftX;
                    clickPositionY = element.BottomY;
                    break;
                case "Bottom Middle":
                    clickPositionX = element.MiddleX;
                    clickPositionY = element.BottomY;
                    break;
                case "Bottom Right":
                    clickPositionX = element.RightX;
                    clickPositionY = element.BottomY;
                    break;
                case "Middle Left":
                    clickPositionX = element.LeftX;
                    clickPositionY = element.MiddleX;
                    break;
                case "Middle Right":
                    clickPositionX = element.RightX;
                    clickPositionY = element.MiddleY;
                    break;
            }
            return new Point(clickPositionX, clickPositionY);
        }
        private void ImageGridViewHelper_MouseEnter(object sender, EventArgs e)
        {
            ImageAction_SelectionChangeCommitted(null, null);
        }
    }
}