using PropertyChanged;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Youtube_API.Videos.Models;
using Youtube_Application.Components.SearchFilterComponent;

namespace Youtube_Application.Models.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class SearchFilterModel
    {
        public ICommand ApplyFilterCommand { get; set; }
        public ICommand NotifyFilterCommand { get; set; }
        public ICommand OpenPopupCommand { get; set; }
        public ICommand ClosePopupCommand { get; set; }
        public string SearchQ { get; set; }
        public List<OptionModel> Category { get; set; } = new List<OptionModel>() { new OptionModel("影片", true, "type", "video"), new OptionModel("shorts", false, "videoDuration", "short"), new OptionModel("頻道", false, "type", "channel"), new OptionModel("播放清單", false, "type", "playlist"), new OptionModel("電影", false, "videoType", "movie"), new OptionModel("節目劇集", false, "videoType", "episode") };

        public List<OptionModel> FilmLength { get; set; } = new List<OptionModel>() { new OptionModel("3分鐘內", false, "videoDuration", "short"), new OptionModel("3到20分鐘", false, "videoDuration", "medium"), new OptionModel("超過20分鐘", true, "videoDuration", "long") };

        public List<OptionModel> UploadDate { get; set; } = new List<OptionModel>() { new OptionModel("今天", false, null, null), new OptionModel("本週", true, null, null), new OptionModel("本月", false, null, null), new OptionModel("今年", false, null, null) };

        public List<OptionModel> Property { get; set; } = new List<OptionModel>() { new OptionModel("HD高畫質", false, "videoDefinition", "high"), new OptionModel("有字幕", false, "videoCaption", "closedCaption"), new OptionModel("無字幕", true, "videoCaption", "none"), new OptionModel("3D", false, "videoDimension", "3d"), new OptionModel("非3D", false, "videoDimension", "2d") };

        public List<OptionModel> Priority { get; set; } = new List<OptionModel>() { new OptionModel("關聯性", true, "order", "relevance"), new OptionModel("熱門程度", false, "order", "viewCount"), new OptionModel("日期", false, "order", "date") };

        public Visibility IsVideo { get; set; }

        public ICommand VisibilityCommand { get; set; }
        public bool IsOpen { get; set; }
        public SearchFilterModel()
        {
            this.OpenPopupCommand = new RelayCommand((x) =>
            {
                this.IsOpen = true;
            });
            this.ClosePopupCommand = new RelayCommand((x) =>
            {
                this.IsOpen = false;
            });
            this.VisibilityCommand = new RelayCommand<OptionModel>(x =>
            {
                if (x.Name == "影片")
                    IsVideo = Visibility.Visible;
                else
                    IsVideo = Visibility.Collapsed;
            });
            this.ApplyFilterCommand = new RelayCommand((z) =>
            {
                // 包裝選中的結果物件
                SelectedSearchFilterOptionsModel selected = new SelectedSearchFilterOptionsModel();
                PropertyInfo[] filterOptionsProps = typeof(SearchFilterModel).GetProperties().Where(x => x.PropertyType == typeof(List<OptionModel>)).ToArray();
                for (int i = 0; i < filterOptionsProps.Length; i++)
                {
                    List<OptionModel> options = (List<OptionModel>)filterOptionsProps[i].GetValue(this);
                    OptionModel option = options.First(x => x.IsSelected);
                    Type t = typeof(SelectedSearchFilterOptionsModel);
                    PropertyInfo f = t.GetProperty(filterOptionsProps[i].Name);
                    f.SetValue(selected, option);
                }
                selected.SearchQ = this.SearchQ;
                this.NotifyFilterCommand.Execute(selected);
            });

        }
    }
}
