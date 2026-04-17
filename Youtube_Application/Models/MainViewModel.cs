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
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Xml.Linq;
using Youtube_API.Channels.Models;
using Youtube_API.PlayLists.Models;
using Youtube_API.Videos;
using Youtube_API.Videos.Models;
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

        public SearchFilterModel FilterOptions { get; set; } = new SearchFilterModel();
        //TODO: 之後拆分成元件之後會需要這包物件
        //public SelectedSearchFilterOptionsModel SelectedSearchFilterOptionsModel { get; set; } = new SelectedSearchFilterOptionsModel();
        public SearchVideoModelReq SearchVideoModelReq { get; set; } = new SearchVideoModelReq();

        public SearchModel SearchVideoModel { get; set; } = new SearchModel();
        public ICommand SearchVideoCommand { get; set; }


        public void GetSearchVideos(List<VideoItemViewModel> videoItemViewModels) // TODO: 這裡要改成回傳DTO 再用Mapper去轉
        {
            Results.Clear();
            TotalResults.Clear();
            TotalResults = new ObservableCollection<VideoItemViewModel>(videoItemViewModels);

        }

        public MainViewModel()
        {
            this.SearchVideoPresenter = new SearchVideoPresenter(this);
            SearchVideoCommand = new RelayCommand(async (x) =>
            {
                SetSearchVideoModelReq();
                await this.SearchVideoPresenter.SearchVideo(SearchVideoModelReq);
            });
        }


        private void SetSearchVideoModelReq()
        {
            PropertyInfo[] propertyInfos = FilterOptions.GetType().GetProperties();
            List<OptionModel> selectedList = propertyInfos.Select(x =>
            {
                List<OptionModel> options = (List<OptionModel>)x.GetValue(FilterOptions);
                return options.First(y => y.IsSelected);
            }).ToList();
            //selectedList.Add(FilterOptions.Category.First(x => x.IsSelected == true));
            //selectedList.Add(FilterOptions.FilmLength.First(x => x.IsSelected == true));
            //selectedList.Add(FilterOptions.UploadDate.First(x => x.IsSelected == true));
            //selectedList.Add(FilterOptions.Property.First(x => x.IsSelected == true));
            //selectedList.Add(FilterOptions.Priority.First(x => x.IsSelected == true));
            //Dictionary<string, string> keyValuePairs = selected.ToDictionary(x => x.Name, y => y.IsSelected.ToString());
            for (int i = 0; i < selectedList.Count; i++)
            {
                OptionModel selected = selectedList[i];
                Type t = typeof(SearchVideoModelReq);
                if (selected.PropName != null)
                {
                    PropertyInfo f = t.GetProperty(selected.PropName);
                    f.SetValue(SearchVideoModelReq, selected.PropValue);
                    continue;
                }
                String publishedAfterString = "";
                String publishedBeforeString = DateTime.Now.ToString("yyyy-MM-dd") + "T" + DateTime.Now.ToString("HH:mm:ss") + "%2B08:00";
                if (selected.Name == "今天")
                {
                    publishedAfterString = DateTime.Now.ToString("yyyy-MM-dd") + "T" + "00:00:01" + "%2B08:00";
                }
                else if (selected.Name == "本週")
                {
                    publishedAfterString = DateTime.Now.AddDays(-7).ToString("yyyy-MM-dd") + "T" + "00:00:01" + "%2B08:00";
                }
                else if (selected.Name == "本月")
                {
                    publishedAfterString = DateTime.Now.AddDays(-31).ToString("yyyy-MM-dd") + "T" + "00:00:01" + "%2B08:00";
                }
                else if (selected.Name == "今年")
                {
                    publishedAfterString = DateTime.Now.AddDays(-365).ToString("yyyy-MM-dd") + "T" + "00:00:01" + "%2B08:00";
                }
                SearchVideoModelReq.publishedBefore = publishedBeforeString;
                SearchVideoModelReq.publishedAfter = publishedAfterString;
            }

        }

    }
}
