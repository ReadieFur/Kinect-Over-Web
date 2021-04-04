using System;
using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Kinect;

namespace KinectOverNDI.Kinect
{
    class KinectManager
    {
        //public event Action<ImageSource> CreatedColourFrame;
        public event Action<BitmapSource> CreatedColourFrame;

        public FrameSources frameSources;

        private KinectSensor kinect;
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

        private void Reader_MultiSourceFrameArrived(object sender, MultiSourceFrameArrivedEventArgs e)
        {
            var frame = e.FrameReference.AcquireFrame();

            if (frameSources.IsSourceEnabled(FrameSourceTypes.Color))
            {
                using (ColorFrame colourFrame = frame.ColorFrameReference.AcquireFrame())
                {
                    if (colourFrame != null)
                    {
                        CreatedColourFrame(colourFrame.ToBitmap());
                    }
                }
            }

            if (frameSources.IsSourceEnabled(FrameSourceTypes.Body))
            {
                using (BodyFrame bodyFrame = frame.BodyFrameReference.AcquireFrame())
                {
                    if (bodyFrame != null)
                    {
                        bodies = new Body[bodyFrame.BodyFrameSource.BodyCount];

                        bodyFrame.GetAndRefreshBodyData(bodies);

                        foreach (var body in bodies)
                        {
                            if (body.IsTracked)
                            {
                                foreach (Joint joint in body.Joints.Values)
                                {
                                    if (joint.TrackingState == TrackingState.Tracked || joint.TrackingState == TrackingState.Inferred)
                                    {
                                        if (frameSources.IsSourceEnabled(FrameSourceTypes.Color))
                                        {
                                            ColorSpacePoint colourSpacePoint = kinect.CoordinateMapper.MapCameraPointToColorSpace(joint.Position);
                                            double x = float.IsInfinity(colourSpacePoint.X) ? 0 : colourSpacePoint.X;
                                            double y = float.IsInfinity(colourSpacePoint.Y) ? 0 : colourSpacePoint.Y;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}