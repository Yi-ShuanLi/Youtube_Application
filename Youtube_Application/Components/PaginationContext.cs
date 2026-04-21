using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Youtube_Application.Models.ViewModels;
using static Youtube_Application.Components.Contracts.PaginationContract;
using static Youtube_Application.Contract.SearchVideoContract;

namespace Youtube_Application.Components
{
    [AddINotifyPropertyChangedInterface]
    internal class PaginationContext : IPaginationView
    {
        public IPaginationPresenter PaginationPresenter;
        public int CurrentPageNumber { get; set; } = 1;
        public bool PreviousPageButtonIsEnable { get; set; } = false;
        public bool NextPageButtonIsEnable { get; set; } = false;
        public ObservableCollection<OptionModel> PageSource { get; set; } = new ObservableCollection<OptionModel>();
        public ICommand PreviousNextPageCommand { get; set; }
        public ICommand PageChangeCommand { get; set; }
        public ICommand NotifyPageChangeCommand { get; set; }
        private int _TotalCount;
        public int TotalCount
        {
            get => _TotalCount;
            set
            {
                _TotalCount = value;
                this.pageCount = TotalCount % 10 == 0 ? TotalCount / 10 : (TotalCount / 10) + 1;
                this.PaginationPresenter.InitialTotalCount(value);
            }
        }
        private int pageCount = 0;

        public PaginationContext()
        {
            PaginationPresenter = new PaginationPresenter(this);
            //this.PaginationPresenter.InitialTotalCount(TotalCount);
            PageChangeCommand = new RelayCommand((x) =>
            {
                this.PaginationPresenter.JumpPageRequest(int.Parse(x.ToString()));
                PreviousNextPageIsEnableState();
                NotifyPageChangeCommand.Execute(CurrentPageNumber);
            });
            PreviousNextPageCommand = new RelayCommand((x) =>
            {
                this.PaginationPresenter.ChangePageRequest(x.ToString());
                PreviousNextPageIsEnableState();
                NotifyPageChangeCommand.Execute(CurrentPageNumber);
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
        }

        public void OnPageRenderResponse(List<int> pages)
        {
            var pageList = pages.Select(x => new OptionModel(x.ToString(), false, "", "")).ToList();
            PageSource = new ObservableCollection<OptionModel>(pageList);
        }

        public void ActivePageNumber(int page)
        {
            CurrentPageNumber = page;
            ResetEachPageButton(page.ToString());
            PreviousNextPageIsEnableState();
        }
    }
}
