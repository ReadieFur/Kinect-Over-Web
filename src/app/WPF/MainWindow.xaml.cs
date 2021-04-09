using System;
using System.Collections.Generic;
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

namespace KinectOverWeb.WPF
{
    public partial class MainWindow : Window
    {
        public Kinect.FrameSources.SourceTypes activeSourcePreview;
        public Kinect.FrameSources.SourceTypes savedSourcePreview;

        //Minimise and exit are handled by Startup.cs

        public MainWindow()
        {
            InitializeComponent();
            activeSourcePreview = Kinect.FrameSources.SourceTypes.None;
            savedSourcePreview = Kinect.FrameSources.SourceTypes.None;
            windowBorder.Visibility = Visibility.Visible;
            //browser.Navigate("http://127.0.0.1:5502/?source=Body_Points_Mapped_To_Colour");
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
        }

        private void topBar_MouseDown(object sender, MouseButtonEventArgs _e)
        {
            if (_e.ChangedButton == MouseButton.Left)
            {
                DragMove();
            }
        }

        public SourcesItem AddToSources(string _checkboxContent, string _urlPath, Kinect.FrameSources.SourceTypes _key)
        {
            SourcesItem listCheckbox = new SourcesItem(_checkboxContent);

            //Mouse down was being consumed by another event so I am using mouse up instead.
            //listCheckbox.listViewItem.MouseUp += (se, mb) => { browser.Source = new Uri($"{previewAddress}/{_urlPath}"); };
            listCheckbox.listViewItem.MouseUp += (se, mb) => { activeSourcePreview = _key; };

            sources.Items.Add(listCheckbox.listViewItem);
            return listCheckbox;
        }

        public class SourcesItem
        {
            public readonly ListViewItem listViewItem;
            public readonly CheckBox checkBox;

            public SourcesItem(string _checkboxContent)
            {
                listViewItem = new ListViewItem();
                checkBox = new CheckBox();

                checkBox.FontSize = 14;
                listViewItem.Height = 50;

                checkBox.Content = _checkboxContent;
                listViewItem.Content = checkBox;
            }
        }
    }
}
