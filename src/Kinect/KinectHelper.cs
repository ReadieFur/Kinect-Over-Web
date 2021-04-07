using System.Runtime.InteropServices;
using System.Windows;
using Media = System.Windows.Media;
using Imaging = System.Windows.Media.Imaging;
using Drawing = System.Drawing;
using Microsoft.Kinect;

namespace KinectOverWeb.Kinect
{
    /* I could make this non-static and then create an instance for each kinect if I decide do add support for multiple devices in the future.
     * The downside of this is that I would not be able to create extention functions (this type name).
     */
    public static class KinectHelper
    {
        public static readonly Media.PixelFormat format = Media.PixelFormats.Bgr32;
        public static readonly double dpi = 96.0;
        public static readonly int bpp = (format.BitsPerPixel + 7) / 8;

        private static Imaging.WriteableBitmap bitmap = null;
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
        public static Imaging.BitmapSource ToBitmap(this ColorFrame _frame)
        {
            if (bitmap == null)
            {
                width = _frame.FrameDescription.Width;
                height = _frame.FrameDescription.Height;
                pixels = new byte[width * height * bpp];
                bitmap = new Imaging.WriteableBitmap(width, height, dpi, dpi, format, null);
            }

            if (_frame.RawColorImageFormat == ColorImageFormat.Bgra) { _frame.CopyRawFrameDataToArray(pixels); }
            else { _frame.CopyConvertedFrameDataToArray(pixels, ColorImageFormat.Bgra); }

            bitmap.Lock();
            Marshal.Copy(pixels, 0, bitmap.BackBuffer, pixels.Length);
            bitmap.AddDirtyRect(new Int32Rect(0, 0, width, height));
            bitmap.Unlock();
            return bitmap;
        }

        public static void ColourBodyFrameDrawLine(this Drawing.Graphics _graphics, KinectSensor _kinect, Joint _joint1, Joint _joint2)
        {
            if (_joint1.TrackingState == TrackingState.NotTracked || _joint2.TrackingState == TrackingState.NotTracked) { return; }

            //Converts the 3D position to the 2D location for the colour camera.
            ColorSpacePoint colourSpacePoint1 = _kinect.CoordinateMapper.MapCameraPointToColorSpace(_joint1.Position);
            ColorSpacePoint colourSpacePoint2 = _kinect.CoordinateMapper.MapCameraPointToColorSpace(_joint2.Position);
            if (float.IsInfinity(colourSpacePoint1.X) || float.IsInfinity(colourSpacePoint1.Y) || float.IsInfinity(colourSpacePoint2.X) || float.IsInfinity(colourSpacePoint2.Y)) { return; }

            float strokeThickness = (_joint1.TrackingState == TrackingState.Inferred || _joint2.TrackingState == TrackingState.Inferred ? 4 : 8) *
                ((1 / (_joint1.Position.Z < 0 ? 0 : _joint1.Position.Z)) + (1 / (_joint2.Position.Z < 0 ? 0 : _joint2.Position.Z)) / 2);
            if (float.IsInfinity(strokeThickness)) { return; }

            Drawing.Pen pen = new Drawing.Pen(
                _joint1.TrackingState == TrackingState.Inferred || _joint2.TrackingState == TrackingState.Inferred ? Drawing.Color.Yellow : Drawing.Color.Green,
                strokeThickness
            );
            _graphics.DrawLine(
                pen,
                colourSpacePoint1.X,
                colourSpacePoint1.Y,
                colourSpacePoint2.X,
                colourSpacePoint2.Y
            );

            pen.Dispose();
        }

        public static void ColourBodyFrameDrawPoint(this Drawing.Graphics _graphics, KinectSensor _kinect, Joint _joint)
        {
            if (_joint.TrackingState == TrackingState.NotTracked) { return; }

            ColorSpacePoint colourSpacePoint = _kinect.CoordinateMapper.MapCameraPointToColorSpace(_joint.Position);
            if (float.IsInfinity(colourSpacePoint.X) || float.IsInfinity(colourSpacePoint.Y)) { return; }

            float circleSize = 30.0f * (1 / (_joint.Position.Z < 0 ? 0 : _joint.Position.Z));

            Drawing.SolidBrush brush = new Drawing.SolidBrush(_joint.TrackingState == TrackingState.Tracked ? Drawing.Color.Green : Drawing.Color.Yellow);
            _graphics.FillEllipse(
                brush,
                colourSpacePoint.X - circleSize / 2,
                colourSpacePoint.Y - circleSize / 2,
                circleSize,
                circleSize
            );

            brush.Dispose();
        }
    }
}
