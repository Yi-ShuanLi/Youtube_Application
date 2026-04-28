using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using Youtube_API.Channels.Models;
using Youtube_API.PlayLists.Models;
using Youtube_API.Videos;
using Youtube_API.Videos.Models;
using Youtube_Application.Models.DTO;
using Youtube_Application.Models.PlayList;
using Youtube_Application.Models.ViewModels;
using Youtube_Application.Pages;
using Youtube_Application.Presenters;
using Youtube_Application.Utilitys;
using Youtube_Application.ViewModels;
using static Youtube_Application.Contract.SearchVideoContract;

namespace Youtube_Application.Models
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel
    {

        INavigationService navigationService;
        public MainViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            this.navigationService.Navigate(new VideoSearchPage(), "Hello World");

        }

    }
}
