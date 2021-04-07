using System;
using Microsoft.Kinect;
using KinectOverWeb.Kinect;
using KinectOverWeb.NDI;

namespace KinectOverWeb
{
    internal class Startup
    {
        private KinectManager kinectManager;
        private NDIManager streamManager;

        private NDIStream bodyColourStream;

        public Startup()
        {
#if DEBUG
            Logger.logLevel = Logger.LogLevel.Trace;
#else
            Logger.logLevel = Logger.LogLevel.Info;
#endif

            kinectManager = new KinectManager();
            kinectManager.frameSources.AddSource(FrameSources.SourceTypes.BodyColour);

            streamManager = new NDIManager();
            bodyColourStream = streamManager.CreateStream("Body - Colour (KinectOverNDI)", 1920, 1080, 30);

            if (bodyColourStream == null) { throw new ArgumentNullException("bodyColourStream", "Failed to create bodyColourStream."); }

            kinectManager.GotBodies += KinectManager_GotBodies;

            bodyColourStream.StartStream();
        }

        private void KinectManager_GotBodies(System.Collections.Generic.IList<Body> _bodies)
        {
            if (kinectManager.frameSources.IsSourceEnabled(FrameSources.SourceTypes.BodyColour))
            {
                bodyColourStream.graphics.Clear(System.Drawing.Color.Transparent);

                foreach (Body body in _bodies)
                {
                    if (body.IsTracked)
                    {
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.Head], body.Joints[JointType.Neck]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.Neck], body.Joints[JointType.SpineShoulder]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderLeft]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderRight]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.SpineShoulder], body.Joints[JointType.SpineMid]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.WristRight], body.Joints[JointType.HandRight]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.HandLeft], body.Joints[JointType.HandTipLeft]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.HandRight], body.Joints[JointType.HandTipRight]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.WristLeft], body.Joints[JointType.ThumbLeft]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.WristRight], body.Joints[JointType.ThumbRight]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.SpineMid], body.Joints[JointType.SpineBase]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.SpineBase], body.Joints[JointType.HipLeft]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.SpineBase], body.Joints[JointType.HipRight]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.KneeLeft], body.Joints[JointType.AnkleLeft]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.KneeRight], body.Joints[JointType.AnkleRight]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.AnkleLeft], body.Joints[JointType.FootLeft]);
                        bodyColourStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.AnkleRight], body.Joints[JointType.FootRight]);

                        foreach (Joint joint in body.Joints.Values)
                        {
                            bodyColourStream.graphics.ColourBodyFrameDrawPoint(kinectManager.kinect, joint);
                        }
                    }
                }
            }
        }
    }
}
