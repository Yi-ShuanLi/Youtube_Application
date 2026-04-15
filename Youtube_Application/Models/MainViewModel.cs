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
    public class MainViewModel : INotifyPropertyChanged, ISearchVideoModelView
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ISearchVideoPresenter SearchVideoPresenter;
        public ObservableCollection<VideoItemViewModel> Results { get; set; } = new ObservableCollection<VideoItemViewModel>();
        public ObservableCollection<VideoItemViewModel> TotalResults { get; set; } = new ObservableCollection<VideoItemViewModel>();
        public int CurrentPageNumber { get; set; } = 1;
        public bool PreviousPageButtonIsEnable { get; set; } = true;
        public bool NextPageButtonIsEnable { get; set; } = true;
        public ObservableCollection<OptionModel> PageSource { get; set; } = new ObservableCollection<OptionModel>();
        public ICommand PreviousNextPageCommand { get; set; }
        public ICommand PageChangeCommand { get; set; }
        public SearchFilterModel FilterOptions { get; set; } = new SearchFilterModel();
        //TODO: 之後拆分成元件之後會需要這包物件
        //public SelectedSearchFilterOptionsModel SelectedSearchFilterOptionsModel { get; set; } = new SelectedSearchFilterOptionsModel();
        public SearchVideoModelReq SearchVideoModelReq { get; set; } = new SearchVideoModelReq();

        public SearchModel SearchVideoModel { get; set; } = new SearchModel();
        public ICommand SearchVideoCommand { get; set; }
        public int totalCount = 375;
        public int pageCount = 0;
        //public Dictionary<int,List<int>> keyValuePairs { get; set; }= new Dictionary<int,List<int>>();
        public void GetSearchVideos(List<VideoItemViewModel> videoItemViewModels) // TODO: 這裡要改成回傳DTO 再用Mapper去轉
        {
            Results.Clear();
            TotalResults.Clear();
            TotalResults = new ObservableCollection<VideoItemViewModel>(videoItemViewModels);

            int pageOneCount = totalCount > 10 ? 10 : totalCount;
            //for (int i = 0; i < pageOneCount; i++)
            //{
            //    Results.Add(TotalResults[i]);
            //}
            pageCount = totalCount % 10 == 0 ? totalCount / 10 : (totalCount / 10) + 1;
            PageSource.Clear();
            int firstPageNum = (CurrentPageNumber);
            int count = (CurrentPageNumber + 9) > pageCount ? (pageCount - CurrentPageNumber + 1) : 10;
            int lastPageNum = firstPageNum + count;
            for (int i = 0; i < count; i++)
            {
                int num = firstPageNum + i;
                OptionModel option = new OptionModel();
                option.Name = $"{num}";
                option.IsSelected = false;
                if (firstPageNum == num)
                {
                    option.IsSelected = true;
                }
                PageSource.Add(option);
            }
            PreviousNextPageIsEnableState();
        }

        public MainViewModel()
        {
            this.SearchVideoPresenter = new SearchVideoPresenter(this);
            SearchVideoCommand = new RelayCommand(async (x) =>
            {
                SetSearchVideoModelReq();
                await this.SearchVideoPresenter.SearchVideo(SearchVideoModelReq);
            });
            PageChangeCommand = new RelayCommand((x) =>
            {
                ResetEachPageButton(x.ToString());
                PreviousNextPageIsEnableState();
            });

            PreviousNextPageCommand = new RelayCommand((x) =>
            {
                int pageChange = int.Parse(x.ToString());
                if ((CurrentPageNumber + pageChange) < 0)
                    CurrentPageNumber = 1;
                else if ((CurrentPageNumber + pageChange) > pageCount)
                    CurrentPageNumber = pageCount;
                else
                    CurrentPageNumber += pageChange;

                if (pageChange < 0)
                {
                    if (CurrentPageNumber < int.Parse(PageSource[0].Name))
                    {
                        PageSource.Clear();
                        int firstPageNum = CurrentPageNumber % 10 == 0 ? (((CurrentPageNumber - 1) / 10) * 10 + 1) : (CurrentPageNumber / 10) * 10 + 1;
                        int count = (firstPageNum + 10) - 1 > pageCount ? (pageCount - firstPageNum + 1) : 10;
                        int lastPageNum = firstPageNum + count;
                        for (int i = 0; i < count; i++)
                        {
                            int num = firstPageNum + i;
                            OptionModel option = new OptionModel();
                            option.Name = $"{num}";
                            option.IsSelected = false;
                            if (num == CurrentPageNumber)
                            {
                                option.IsSelected = true;
                            }
                            PageSource.Add(option);
                        }
                    }
                }
                else
                {
                    if (CurrentPageNumber > int.Parse(PageSource.Last().Name))
                    {
                        PageSource.Clear();
                        int firstPageNum = CurrentPageNumber % 10 == 0 ? (((CurrentPageNumber - 1) / 10) * 10 + 1) : (CurrentPageNumber / 10) * 10 + 1;
                        int count = (firstPageNum + 10) - 1 > pageCount ? (pageCount - firstPageNum + 1) : 10;
                        int lastPageNum = firstPageNum + count;
                        for (int i = 0; i < count; i++)
                        {
                            int num = firstPageNum + i;
                            OptionModel option = new OptionModel();
                            option.Name = $"{num}";
                            option.IsSelected = false;
                            if (num == CurrentPageNumber)
                            {
                                option.IsSelected = true;
                            }
                            PageSource.Add(option);
                        }
                    }
                }
                ResetEachPageButton(CurrentPageNumber.ToString());
                PreviousNextPageIsEnableState();
            });

        }
        private void PreviousNextPageIsEnableState()
        {
            if (CurrentPageNumber == 1)
                PreviousPageButtonIsEnable = false;
            if (CurrentPageNumber > 1)
                PreviousPageButtonIsEnable = true;
            if (CurrentPageNumber == pageCount)
                NextPageButtonIsEnable = false;
            if (CurrentPageNumber < pageCount)
                NextPageButtonIsEnable = true;
        }
        private void ResetEachPageButton(string pageName)
        {
            foreach (var item in PageSource)
            {
                item.IsSelected = false;
                if (item.Name == pageName)
                {
                    item.IsSelected = true;
                }

            }
            ;
            //Results.Clear();
            //for (int i = 0; i < TotalResults.Count; i++)
            //{
            //    if (i > (CurrentPageNumber - 1) * 10 && i < CurrentPageNumber * 10)
            //        Results.Add(TotalResults[i]);
            //    if (i >= CurrentPageNumber * 10)
            //        break;
            //}
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
