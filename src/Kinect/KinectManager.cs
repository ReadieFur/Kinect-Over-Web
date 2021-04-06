using System;
using System.Collections.Generic;
using Microsoft.Kinect;

namespace KinectOverNDI.Kinect
{
    class KinectManager
    {
        public event Action<ColorFrame> GotColourFrame;
        public event Action<IList<Body>> GotBodies;

        public FrameSources frameSources;
        public KinectSensor kinect;

        private MultiSourceFrameReader reader;
        private IList<Body> bodies;

        public KinectManager()
        {
            frameSources = new FrameSources();

            kinect = KinectSensor.GetDefault();

            reader = kinect.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
            reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

            kinect.Open();
        }

        private void Reader_MultiSourceFrameArrived(object _sender, MultiSourceFrameArrivedEventArgs _multiSourceFrame)
        {
            var frame = _multiSourceFrame.FrameReference.AcquireFrame();

            if (frameSources.IsSourceEnabled(FrameSources.SourceTypes.Color))
            {
                using (ColorFrame colourFrame = frame.ColorFrameReference.AcquireFrame())
                {
                    if (colourFrame != null && GotColourFrame != null)
                    {
                        GotColourFrame(colourFrame);
                    }
                }
            }

            if (frameSources.IsSourceEnabled(FrameSources.SourceTypes.BodyColour))
            {
                using (BodyFrame bodyFrame = frame.BodyFrameReference.AcquireFrame())
                {
                    if (bodyFrame != null && GotBodies != null)
                    {
                        bodies = new Body[bodyFrame.BodyFrameSource.BodyCount];
                        bodyFrame.GetAndRefreshBodyData(bodies);
                        GotBodies(bodies);
                    }
                }
            }
        }
    }
}
