using System;
using System.Collections.Generic;
using Microsoft.Kinect;
using Microsoft.Kinect.Face;

namespace KinectOverWeb.Kinect
{
    public class KinectManager
    {
        //public event Action<ColorFrame> GotColourFrame;
        public event Action<IList<Body>> GotBodies;

        public FrameSources frameSources;
        public KinectSensor kinect;

        private MultiSourceFrameReader reader;
        private IList<Body> bodies;

        private FaceFrameSource faceSource;
        private FaceFrameReader faceReader;

        public KinectManager()
        {
            frameSources = new FrameSources();

            kinect = KinectSensor.GetDefault();

            reader = kinect.OpenMultiSourceFrameReader(FrameSourceTypes.Color | FrameSourceTypes.Body);
            reader.MultiSourceFrameArrived += Reader_MultiSourceFrameArrived;

            /*faceSource = new FaceFrameSource(kinect, 0,
                FaceFrameFeatures.BoundingBoxInColorSpace |
                FaceFrameFeatures.FaceEngagement |
                FaceFrameFeatures.LookingAway |
                FaceFrameFeatures.Glasses |
                FaceFrameFeatures.Happy |
                FaceFrameFeatures.LeftEyeClosed |
                FaceFrameFeatures.RightEyeClosed |
                FaceFrameFeatures.MouthOpen |
                FaceFrameFeatures.MouthMoved |
                FaceFrameFeatures.PointsInColorSpace |
                FaceFrameFeatures.RotationOrientation
            );
            faceReader = faceSource.OpenReader();
            faceReader.FrameArrived += FaceReader_FrameArrived;*/

            kinect.Open();
        }

        //https://pterneas.com/2014/12/21/kinect-2-face-basics/
        private void FaceReader_FrameArrived(object sender, FaceFrameArrivedEventArgs _faceFrame)
        {
            using (FaceFrame frame = _faceFrame.FrameReference.AcquireFrame())
            {
                if (frame != null)
                {
                    Logger.Info(frame.IsTrackingIdValid);

                    if (frame.FaceFrameResult != null)
                    {
                        string message = $"nose:{frame.FaceFrameResult.FacePointsInColorSpace[FacePointType.Nose]}" +
                            $"happy:{frame.FaceFrameResult.FaceProperties[FaceProperty.Happy]}" +
                            $"x:{frame.FaceFrameResult.FaceRotationQuaternion.X}" +
                            $"y:{frame.FaceFrameResult.FaceRotationQuaternion.Y}" +
                            $"z:{frame.FaceFrameResult.FaceRotationQuaternion.Z}" +
                            $"w:{frame.FaceFrameResult.FaceRotationQuaternion.W}";
                        Logger.Debug(message);
                    }
                }
            }
        }

        private void Reader_MultiSourceFrameArrived(object _sender, MultiSourceFrameArrivedEventArgs _multiSourceFrame)
        {
            MultiSourceFrame frame = _multiSourceFrame.FrameReference.AcquireFrame();

            //I need to potomise this more before I add it into the program.
            /*if (frameSources.IsSourceEnabled(FrameSources.SourceTypes.Color))
            {
                using (ColorFrame colourFrame = frame.ColorFrameReference.AcquireFrame())
                {
                    if (colourFrame != null && GotColourFrame != null)
                    {
                        GotColourFrame(colourFrame);
                    }
                }
            }*/

            if (frameSources.IsSourceEnabled(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour))
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
