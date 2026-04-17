using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Youtube_Application.Components.Contracts
{
    internal class PaginationContract
    {
        public interface IPaginationView
        {
            /// <summary>
            /// 回傳分頁List
            /// </summary>
            /// <param name="pages"></param>
            void OnPageRenderResponse(List<int> pages);

            /// <summary>
            /// 將指定頁碼設定為已選中狀態
            /// </summary>
            /// <param name="page"></param>
            void ActivePageNumber(int page);
        }
        public interface IPaginationPresenter
        {
            /// <summary>
            /// 初始化所有資料筆數
            /// </summary>
            /// <param name="totalCount"></param>
            void InitialTotalCount(int totalCount);
            /// <summary>
            /// 往前往後1頁或往前往後5頁
            /// </summary>
            /// <param name="direction"></param>
            void ChangePageRequest(string direction);
            /// <summary>
            /// 直接點及分頁Button直接到該頁
            /// </summary>
            /// <param name="direction"></param>
            void JumpPageRequest(int pageNumber);
        }
    }
}
