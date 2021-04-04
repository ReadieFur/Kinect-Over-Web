using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Kinect;
using KinectOverNDI.Kinect;

namespace KinectOverNDI
{
    public partial class App : Application
    {
        private KinectManager kinectManager;
        private MainWindow mainWindow;

        private void Application_Startup(object sender, StartupEventArgs e)
        {
#if DEBUG
            Logger.logLevel = Logger.LogLevel.Trace;
#else
            Logger.logLevel = Logger.LogLevel.Info;
#endif

            kinectManager = new KinectManager();
            kinectManager.frameSources.AddSource(FrameSourceTypes.Color);
            kinectManager.frameSources.AddSource(FrameSourceTypes.Body);

            kinectManager.CreatedColourFrame += KinectManager_CreatedColourFrame;

            mainWindow = new MainWindow();
            mainWindow.Show();
        }

        private void KinectManager_CreatedColourFrame(System.Windows.Media.Imaging.BitmapSource _image)
        {
            mainWindow.ColourImage.Source = _image;
        }
    }
}
