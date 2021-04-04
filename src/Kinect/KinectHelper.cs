using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace KinectOverNDI.Kinect
{
    /* I could make this non-static and then create an instance for each kinect if I decide do add support for multiple devices in the future.
     * The downside of this is that I would not be able to create extention functions (this type name).
     */
    static class KinectHelper
    {
        public static readonly PixelFormat format = PixelFormats.Bgr32;
        public static readonly double dpi = 96.0;
        public static readonly int bpp = (format.BitsPerPixel + 7) / 8;

        private static WriteableBitmap bitmap = null;
        private static int width;
        private static int height;
        private static byte[] pixels = null;

        //This takes up A LOT of CPU useage (on my i7-8700, this alone uses 7-8%), look into using a CUDA library like 'ManagedCuda' to render this on the GPU.
        /*public static ImageSource ToBitmap(this ColorFrame _frame)
        {
            byte[] pixels = new byte[_frame.FrameDescription.Width * _frame.FrameDescription.Height * bpp];
            if (_frame.RawColorImageFormat == ColorImageFormat.Bgra) { _frame.CopyRawFrameDataToArray(pixels); }
            else { _frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra); }
            return BitmapSource.Create(_frame.FrameDescription.Width, _frame.FrameDescription.Height, dpi, dpi, format, null, pixels, _frame.FrameDescription.Width * format.BitsPerPixel / 8);
        }*/

        //Writeable bitmap method.
        //This method uses less CPU than the one above but is still sitting around 4-5%.
        public static BitmapSource ToBitmap(this ColorFrame _frame)
        {
            if (bitmap == null)
            {
                width = _frame.FrameDescription.Width;
                height = _frame.FrameDescription.Height;
                pixels = new byte[width * height * bpp];
                bitmap = new WriteableBitmap(width, height, dpi, dpi, format, null);
            }

            if (_frame.RawColorImageFormat == ColorImageFormat.Bgra) { _frame.CopyRawFrameDataToArray(pixels); }
            else { _frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra); }

            bitmap.Lock();
            Marshal.Copy(pixels, 0, bitmap.BackBuffer, pixels.Length);
            bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            bitmap.Unlock();
            return bitmap;
        }
    }
}
