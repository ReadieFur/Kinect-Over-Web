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
    }
}
