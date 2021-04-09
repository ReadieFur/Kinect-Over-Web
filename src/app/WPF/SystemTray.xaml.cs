using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Forms = System.Windows.Forms;

namespace KinectOverWeb.WPF
{
    public partial class SystemTray : Window
    {
        public Forms.NotifyIcon trayIcon;

        public SystemTray()
        {
            InitializeComponent();
            trayIcon = new Forms.NotifyIcon();
            trayIcon.Icon = new System.Drawing.Icon("Resources/Kinect.ico");
            trayIcon.Text = "Kinect Over Web";
        }

        protected override void OnClosing(CancelEventArgs _e)
        {
            trayIcon.Dispose();
            base.OnClosing(_e);
        }

        private void NotifyIcon_MouseClick(object sender, Forms.MouseEventArgs _e)
        {
            //For now there won't be a context menu. Handled by Startup.cs
            /*if (_e.Button == Forms.MouseButtons.Right)
            {
                Top = Forms.Cursor.Position.Y - Height;
                Left = Forms.Cursor.Position.X - Width;
                Show();
                Activate();
            }*/
        }

        private void Window_MouseLeave(object sender, MouseEventArgs _e)
        {
            Hide();
        }
    }
}
