using System.Drawing;
using System.Windows.Forms;

namespace OpenBots.Core.Utilities.CommonUtilities
{
    public class ImageMethods
    {
        public static Bitmap Screenshot()
        {
            var screen = Screen.PrimaryScreen;
            var rect = screen.Bounds;
            Size size = new Size((int)(rect.Size.Width * 1), (int)(rect.Size.Height * 1));

            Bitmap bmpScreenshot = new Bitmap(size.Width, size.Height);
            Graphics g = Graphics.FromImage(bmpScreenshot);
            g.CopyFromScreen(0, 0, 0, 0, size);

            return bmpScreenshot;
        }
    }
}
