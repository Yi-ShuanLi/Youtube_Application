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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Youtube_Application.Models;
using Youtube_Application.Utilitys;

namespace Youtube_Application
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            App.navigationService = new Utilitys.NavigationService();
            //把MainWindow的<Frame x:Name="frame" />傳入去設定NavigationService的_frame
            App.navigationService.SetFrame(frame);
            //把navigationService傳入到MainViewModel裡去，所以就能call by reference都是同個navigationService
            DataContext = new MainViewModel(App.navigationService);
        }
    }
}
