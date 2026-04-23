using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
using Youtube_Application.Presenters;
using Youtube_Application.Utilitys;
using Youtube_Application.ViewModels;
using static Youtube_Application.Contract.SearchVideoContract;

namespace Youtube_Application.Models
{
    [AddINotifyPropertyChangedInterface]
    public class MainViewModel : ISearchVideoModelView
    {

        public ISearchVideoPresenter SearchVideoPresenter;
        public ObservableCollection<VideoItemViewModel> Results { get; set; } = new ObservableCollection<VideoItemViewModel>();
        public ObservableCollection<VideoItemViewModel> TotalResults { get; set; } = new ObservableCollection<VideoItemViewModel>();

        public int TotalResultsCount
        {
            get => TotalResults.Count;
        }

        public SearchVideoModelReq SearchVideoModelReq { get; set; } = new SearchVideoModelReq();

        public ICommand PageChangeCommand { get; set; }

        public ICommand FilterButtonCommand { get; set; }

        public void GetSearchVideos(List<VideoItemViewDTOModel> videoItemViewDTOModel) // TODO: 這裡要改成回傳DTO 再用Mapper去轉
        {
            Results.Clear();
            TotalResults.Clear();
            List<VideoItemViewModel> videoItemViewModels = Mapper.Map<VideoItemViewDTOModel, VideoItemViewModel>(videoItemViewDTOModel);
            TotalResults = new ObservableCollection<VideoItemViewModel>(videoItemViewModels);
            Results = new ObservableCollection<VideoItemViewModel>(TotalResults.Take(10));
        }

        public MainViewModel()
        {
            this.SearchVideoPresenter = new SearchVideoPresenter(this);

            PageChangeCommand = new RelayCommand<int>((x) =>
            {
                Results.Clear();
                Results = new ObservableCollection<VideoItemViewModel>(TotalResults.Skip(x * 10 - 10).Take(10));
            });
            FilterButtonCommand = new RelayCommand<SelectedSearchFilterOptionsModel>(async (x) =>
            {
                PropertyInfo[] propertyInfos =
                x.GetType()
                .GetProperties()
                .Where(z =>
                {
                    if (z.Name == "SearchQ")
                        return false;
                    if (x.Category.Name != "影片")
                    {
                        if (z.Name == "FilmLength" || z.Name == "Property")
                            return false;
                        return true;
                    }
                    return true;
                }).ToArray();

                for (int i = 0; i < propertyInfos.Length; i++)
                {
                    OptionModel optionModel = (OptionModel)propertyInfos[i].GetValue(x);
                    if (propertyInfos[i].Name != "UploadDate")
                    {
                        Type t = typeof(SearchVideoModelReq);
                        PropertyInfo f = t.GetProperty(optionModel.PropName);
                        f.SetValue(this.SearchVideoModelReq, optionModel.PropValue);
                        continue;
                    }
                    String publishedAfterString = "";
                    String publishedBeforeString = DateTime.Now.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss") + "%2B08:00";
                    if (optionModel.Name == "今天")
                    {
                        publishedAfterString = DateTime.Now.ToString("yyyy-MM-dd") + "T" + "00:00:01" + "%2B08:00";
                    }
                    else if (optionModel.Name == "本週")
                    {
                        publishedAfterString = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "T" + "00:00:01" + "%2B08:00";
                    }
                    else if (optionModel.Name == "本月")
                    {
                        publishedAfterString = DateTime.Now.AddDays(-31).ToString("yyyy-MM-dd") + "T" + "00:00:01" + "%2B08:00";
                    }
                    else if (optionModel.Name == "今年")
                    {
                        publishedAfterString = DateTime.Now.AddDays(-365).ToString("yyyy-MM-dd") + "T" + "00:00:01" + "%2B08:00";
                    }
                    SearchVideoModelReq.publishedBefore = publishedBeforeString;
                    SearchVideoModelReq.publishedAfter = publishedAfterString;

                }

                SearchVideoModelReq.q = x.SearchQ;
                await this.SearchVideoPresenter.SearchVideo(SearchVideoModelReq);
            });
        }

    }
}
