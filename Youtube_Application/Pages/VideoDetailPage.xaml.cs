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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Youtube_Application.Pages.Context;

namespace Youtube_Application.Pages
{
    /// <summary>
    /// VideoDetailPage.xaml 的互動邏輯
    /// </summary>
    public partial class VideoDetailPage : Page
    {
        public VideoDetailPage()
        {
            InitializeComponent();
            DataContext = new VideoDetailContext();
        }
    }
}
