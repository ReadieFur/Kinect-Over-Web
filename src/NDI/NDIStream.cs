using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using NewTek.NDI;

namespace KinectOverWeb.NDI
{
    public class NDIStream : IDisposable
    {
        private readonly string name;
        private readonly int width;
        private readonly int height;
        private readonly float aspectRatio;
        private readonly int framerate;
        private readonly int framerateDenominator;

        private VideoFrame videoFrame;
        private Bitmap bitmap;
        private bool streamEnabled;
        private Task streamTask;

        public Graphics graphics;

        public NDIStream(string _name, int _width, int _height, int _framerate)
        {
            name = _name;
            width = _width;
            height = _height;
            aspectRatio = (float)width / height;
            framerate = _framerate;
            framerateDenominator = 1;

            videoFrame = new VideoFrame(width, height, aspectRatio, framerate, framerateDenominator);
            bitmap = new Bitmap(videoFrame.Width, videoFrame.Height, videoFrame.Stride, PixelFormat.Format32bppPArgb, videoFrame.BufferPtr);
            graphics = Graphics.FromImage(bitmap);
            graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

            streamEnabled = false;
        }

        public void StartStream()
        {
            if (!streamEnabled)
            {
                streamEnabled = true;
                streamTask = Task.Run(Stream);
            }
        }

        public void StopStream()
        {
            streamEnabled = false;
        }

        private void Stream()
        {
            Sender stream = new Sender(name, true, false, null, $"{System.Net.Dns.GetHostName()} (${name})");

            while (streamEnabled)
            {
                if (stream.GetConnections(1000) < 1)
                {
                    Logger.Debug("No connections. Not sending data.");
                    System.Threading.Thread.Sleep(100);
                }
                else
                {
                    stream.Send(videoFrame);
                }
            }
        }

        public void Dispose()
        {
            StopStream();
            streamTask.Dispose();
            videoFrame.Dispose();
            bitmap.Dispose();
            graphics.Dispose();
        }
    }
}
