using System;
using Microsoft.Kinect;
using KinectOverWeb.Web;
using KinectOverWeb.Kinect;
using KinectOverWeb.WPF;
using WebSocketSharp.Server;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace KinectOverWeb
{
    internal class Startup : IDisposable
    {
        private WebsocketManager websocketManager;
        private KinectManager kinectManager;
        private MainWindow mainWindow;
        private SystemTray systemTray;

        public Startup()
        {
#if DEBUG
            Logger.logLevel = Logger.LogLevel.Trace;
#else
            Logger.logLevel = Logger.LogLevel.Info;
#endif

            Styles.EnableThemeChecker();

            WebInit();
            KinectInit();
            //There is a possibility that the tray icon gets clicked before the MainWindow has been initalised, if that happens then the program will throw to no instance.
            SystemTrayInit();
            MainWindowInit();
        }

        public void Dispose()
        {
            mainWindow.Close();
            systemTray.Close();
            System.Windows.Application.Current.Shutdown();
        }

        #region Web
        private void WebInit()
        {
            websocketManager = new WebsocketManager(System.Net.IPAddress.Loopback, 1311);
            websocketManager.StartServer();
        }
        #endregion

        #region Kinect
        private void KinectInit()
        {
            kinectManager = new KinectManager();
            kinectManager.GotBodies += KinectManager_GotBodies;
        }

        private void KinectManager_GotBodies(IList<Body> _bodies)
        {
            if (websocketManager.enabled)
            {
                if (kinectManager.frameSources.IsSourceEnabled(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour))
                {
                    WebSocketServiceHost websocketService = websocketManager.GetEndpoint($"/kinect-over-web/{FrameSources.SourceTypes.Body_Points_Mapped_To_Colour}");
                    if (websocketService != null)
                    {
                        List<Dictionary<JointType, KinectHelper.BasicJoint>> colourBodies = null;
                        foreach(Body body in _bodies)
                        {
                            if (body.IsTracked)
                            {
                                if (colourBodies == null) { colourBodies = new List<Dictionary<JointType, KinectHelper.BasicJoint>>(); }
                                Dictionary<JointType, KinectHelper.BasicJoint> colourBody = new Dictionary<JointType, KinectHelper.BasicJoint>();
                                foreach (KeyValuePair<JointType, Joint> joint in body.Joints)
                                {
                                    ColorSpacePoint colorSpacePoint = kinectManager.kinect.CoordinateMapper.MapCameraPointToColorSpace(joint.Value.Position);
                                    colourBody.Add
                                    (
                                        joint.Key,
                                        new KinectHelper.BasicJoint
                                        {
                                            Position = new CameraSpacePoint
                                            {
                                                X = colorSpacePoint.X,
                                                Y = colorSpacePoint.Y,
                                                Z = joint.Value.Position.Z
                                            },
                                            TrackingState = joint.Value.TrackingState
                                        }
                                    );
                                }
                                colourBodies.Add(colourBody);
                            }
                        }
                        if (colourBodies != null) { websocketService.Sessions.Broadcast(JsonConvert.SerializeObject(colourBodies)); }

                        if (colourBodies != null && mainWindow.activeSourcePreview == FrameSources.SourceTypes.Body_Points_Mapped_To_Colour)
                        {
                            mainWindow.previewCanvas.Children.Clear();

                            foreach (Dictionary<JointType, KinectHelper.BasicJoint> body in colourBodies)
                            {
                                //Why did I make it like this, its so long... (because I didn't account for this when I made the above).
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.Head).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.Neck).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.Neck).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.SpineShoulder).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.SpineShoulder).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.ShoulderLeft).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.SpineShoulder).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.ShoulderRight).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.SpineShoulder).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.SpineMid).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.ShoulderLeft).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.ElbowLeft).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.ShoulderRight).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.ElbowRight).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.ElbowLeft).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.WristLeft).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.ElbowRight).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.WristRight).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.WristLeft).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.HandLeft).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.WristRight).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.HandRight).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.HandLeft).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.HandTipLeft).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.HandRight).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.HandTipRight).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.WristLeft).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.ThumbLeft).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.WristRight).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.ThumbRight).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.SpineMid).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.SpineBase).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.SpineBase).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.HipLeft).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.SpineBase).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.HipRight).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.HipLeft).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.KneeLeft).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.HipRight).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.KneeRight).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.KneeLeft).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.AnkleLeft).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.KneeRight).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.AnkleRight).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.AnkleLeft).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.FootLeft).Value);
                                mainWindow.previewCanvas.DrawLine(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, body.FirstOrDefault(kvp => kvp.Key == JointType.AnkleRight).Value, body.FirstOrDefault(kvp => kvp.Key == JointType.FootRight).Value);

                                foreach (KeyValuePair<JointType, KinectHelper.BasicJoint> Joint in body)
                                {
                                    mainWindow.previewCanvas.DrawCircle(FrameSources.SourceTypes.Body_Points_Mapped_To_Colour, Joint.Value);
                                }
                            }
                        }
                    }
                }
            }
        }
        #endregion

        #region MainWindow
        private void MainWindowInit()
        {
            mainWindow = new MainWindow();
            mainWindow.minimiseButton.Click += MinimiseButton_Click;
            mainWindow.closeButton.Click += CloseButton_Click;

            foreach(string name in Enum.GetNames(typeof(FrameSources.SourceTypes)))
            {
                FrameSources.SourceTypes key = (FrameSources.SourceTypes)Enum.Parse(typeof(FrameSources.SourceTypes), name);
                if (key == FrameSources.SourceTypes.None) { continue; }
                MainWindow.SourcesItem sourcesItem = mainWindow.AddToSources(name.Replace('_', ' '), name, key);
                sourcesItem.checkBox.Checked += (se, re) =>
                {
                    websocketManager.AddEndpoint($"/kinect-over-web/{name}");
                    kinectManager.frameSources.AddSource(key);
                    mainWindow.activeSourcePreview = key;
                };
                sourcesItem.checkBox.Unchecked += (se, re) =>
                {
                    websocketManager.RemoveEndpoint($"/kinect-over-web/{name}");
                    kinectManager.frameSources.RemoveSource(key);
                    if (mainWindow.activeSourcePreview == key)
                    {
                        mainWindow.activeSourcePreview = FrameSources.SourceTypes.None;
                        mainWindow.previewCanvas.Children.Clear();
                    }
                };
            }

            mainWindow.Show();
        }

        private void CloseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            Dispose();
        }

        private void MinimiseButton_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            systemTray.trayIcon.Visible = true;
            mainWindow.savedSourcePreview = mainWindow.activeSourcePreview;
            mainWindow.activeSourcePreview = FrameSources.SourceTypes.None;
            mainWindow.Hide();
        }
        #endregion

        #region SystemTray
        private void SystemTrayInit()
        {
            systemTray = new SystemTray();
            systemTray.trayIcon.MouseClick += TrayIcon_MouseClick;
        }

        private void TrayIcon_MouseClick(object sender, System.Windows.Forms.MouseEventArgs e)
        {
            systemTray.trayIcon.Visible = false;
            mainWindow.Show();
            mainWindow.activeSourcePreview = mainWindow.savedSourcePreview;
        }
        #endregion
    }
}
