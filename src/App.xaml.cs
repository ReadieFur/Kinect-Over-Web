using System.Windows;

namespace KinectOverNDI
{
    public partial class App : Application
    {
        private Startup app;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            app = new Startup();
        }

        //private KinectManager kinectManager;
        //private MainWindow mainWindow;
        //private NDIStream colourBodyStream;
        //private NDIManager streamManager;

        //private NDIStream bodyColourStream;

        /*private void Application_Startup(object sender, StartupEventArgs e)
        {
#if DEBUG
            Logger.logLevel = Logger.LogLevel.Trace;
#else
            Logger.logLevel = Logger.LogLevel.Info;
#endif

            kinectManager = new KinectManager();
            kinectManager.frameSources.AddSource(FrameSourceTypes.Body);

            //kinectManager.GotBodies += KinectManager_GotBodies;

            streamManager = new NDIManager();
            bodyColourStream = streamManager.CreateStream("Body - Colour (KinectOverNDI)", 1920, 1080, 30);

            if (bodyColourStream == null) { throw new ArgumentNullException("bodyColourStream", "Failed to create bodyColourStream."); }

            mainWindow = new MainWindow();
            mainWindow.Show();

            //colourBodyStream = new NDIStream("Body - Colour (KinectOverNDI)", 1920, 1080, 30);
            //colourBodyStream.StartStream();
        }*/

        /*private void KinectManager_GotBodies(IList<Body> _bodies)
        {
            colourBodyStream.graphics.Clear(Color.Transparent);

            foreach (Body body in _bodies)
            {
                if (body.IsTracked)
                {
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.Head], body.Joints[JointType.Neck]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.Neck], body.Joints[JointType.SpineShoulder]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderLeft]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.SpineShoulder], body.Joints[JointType.ShoulderRight]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.SpineShoulder], body.Joints[JointType.SpineMid]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.ShoulderLeft], body.Joints[JointType.ElbowLeft]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.ShoulderRight], body.Joints[JointType.ElbowRight]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.ElbowLeft], body.Joints[JointType.WristLeft]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.ElbowRight], body.Joints[JointType.WristRight]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.WristLeft], body.Joints[JointType.HandLeft]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.WristRight], body.Joints[JointType.HandRight]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.HandLeft], body.Joints[JointType.HandTipLeft]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.HandRight], body.Joints[JointType.HandTipRight]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.WristLeft], body.Joints[JointType.ThumbLeft]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.WristRight], body.Joints[JointType.ThumbRight]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.SpineMid], body.Joints[JointType.SpineBase]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.SpineBase], body.Joints[JointType.HipLeft]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.SpineBase], body.Joints[JointType.HipRight]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.HipLeft], body.Joints[JointType.KneeLeft]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.HipRight], body.Joints[JointType.KneeRight]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.KneeLeft], body.Joints[JointType.AnkleLeft]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.KneeRight], body.Joints[JointType.AnkleRight]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.AnkleLeft], body.Joints[JointType.FootLeft]);
                    colourBodyStream.graphics.ColourBodyFrameDrawLine(kinectManager.kinect, body.Joints[JointType.AnkleRight], body.Joints[JointType.FootRight]);

                    foreach (Joint joint in body.Joints.Values)
                    {
                        colourBodyStream.graphics.ColourBodyFrameDrawPoint(kinectManager.kinect, joint);
                    }
                }
            }
        }*/
    }
}
