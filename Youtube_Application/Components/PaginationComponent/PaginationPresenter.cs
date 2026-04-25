using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Youtube_Application.Models.ViewModels;
using static Youtube_Application.Components.Contracts.PaginationContract;

namespace Youtube_Application.Components
{
    internal class PaginationPresenter : IPaginationPresenter
    {
        IPaginationView PaginationView;
        public int CurrentPageNumber { get; set; } = 1;
        public int TotalDataCount { get; set; }
        public int MaxPageNumber { get; set; }
        public PaginationPresenter(IPaginationView view)
        {
            this.PaginationView = view;
        }

        public void ChangePageRequest(string direction)
        {
            int pageChange = int.Parse(direction.ToString());
            int start = ((CurrentPageNumber - 1) / 10) * 10 + 1;
            int countBefore = (start + 9) > MaxPageNumber ? (MaxPageNumber - start) : 9;
            int end = start + countBefore;
            if ((CurrentPageNumber + pageChange) <= 0)
                CurrentPageNumber = 1;
            else if ((CurrentPageNumber + pageChange) >= MaxPageNumber)
                CurrentPageNumber = MaxPageNumber;
            else
                CurrentPageNumber += pageChange;

            if (pageChange < 0)
            {
                if (CurrentPageNumber < start)
                {
                    int firstPageNum = CurrentPageNumber % 10 == 0 ? (((CurrentPageNumber - 1) / 10) * 10 + 1) : (CurrentPageNumber / 10) * 10 + 1;
                    int count = (firstPageNum + 10) - 1 > MaxPageNumber ? (MaxPageNumber - firstPageNum + 1) : 10;
                    int lastPageNum = firstPageNum + count;
                    List<int> pages = new List<int>();
                    for (int i = 0; i < count; i++)
                    {
                        int num = firstPageNum + i;
                        pages.Add(num);
                    }
                    PaginationView.OnPageRenderResponse(pages);
                }
            }
            else
            {
                if (CurrentPageNumber > end)
                {
                    int firstPageNum = CurrentPageNumber % 10 == 0 ? (((CurrentPageNumber - 1) / 10) * 10 + 1) : (CurrentPageNumber / 10) * 10 + 1;
                    int count = (firstPageNum + 10) - 1 > MaxPageNumber ? (MaxPageNumber - firstPageNum + 1) : 10;
                    int lastPageNum = firstPageNum + count;
                    List<int> pages = new List<int>();
                    for (int i = 0; i < count; i++)
                    {
                        int num = firstPageNum + i;
                        pages.Add(num);
                    }
                    PaginationView.OnPageRenderResponse(pages);
                }
            }
            PaginationView.ActivePageNumber(CurrentPageNumber);
        }

        public void InitialTotalCount(int totalCount)
        {
            CurrentPageNumber = 1;
            this.TotalDataCount = totalCount;
            this.MaxPageNumber = totalCount % 10 == 0 ? totalCount / 10 : (totalCount / 10) + 1;
            List<int> pages = new List<int>();
            int firstPageNum = (CurrentPageNumber);
            int count = this.MaxPageNumber - firstPageNum + 1;
            if (this.MaxPageNumber > 10)
                count = 10;
            int lastPageNum = firstPageNum + count;
            for (int i = 0; i < count; i++)
            {
                int num = firstPageNum + i;
                pages.Add(num);
            }
            PaginationView.OnPageRenderResponse(pages);
            PaginationView.ActivePageNumber(CurrentPageNumber);
        }

        public void JumpPageRequest(int pageNumber)
        {
            CurrentPageNumber = pageNumber;
            PaginationView.ActivePageNumber(pageNumber);
        }
    }
}
