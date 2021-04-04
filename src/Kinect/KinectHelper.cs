using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace KinectOverNDI.Kinect
{
    static class KinectHelper
    {
        public static readonly PixelFormat format = PixelFormats.Bgr32;
        public static readonly double dpi = 96.0;
        public static readonly int bpp = (format.BitsPerPixel + 7) / 8;

        public static ImageSource ToBitmap(this ColorFrame _frame)
        {
            byte[] pixels = new byte[_frame.FrameDescription.Width * _frame.FrameDescription.Height * bpp];
            if (_frame.RawColorImageFormat == ColorImageFormat.Bgra) { _frame.CopyRawFrameDataToArray(pixels); }
            else { _frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra); }
            return BitmapSource.Create(_frame.FrameDescription.Width, _frame.FrameDescription.Height, dpi, dpi, format, null, pixels, _frame.FrameDescription.Width * format.BitsPerPixel / 8);
        }
    }
}
