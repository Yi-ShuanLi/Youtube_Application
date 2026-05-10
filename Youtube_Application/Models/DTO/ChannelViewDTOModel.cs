using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube_Application.Models.DTO
{
    public class ChannelViewDTOModel
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// 頻道網址
        /// </summary>
        public string CustomUrl { get; set; }
        /// <summary>
        /// 頻道頭像
        /// </summary>
        public string ImageUrl { get; set; }
        public DateTime PublishedAt { get; set; }
        public string ViewCount { get; set; }
        public string SubscriberCount { get; set; } = "0";
        public string VideoCount { get; set; } = "0";
        /// <summary>
        /// 該頻道所有上傳的影片ID，之後用當搜尋playList內容的playListID使用去
        /// 找出所有已上傳影片
        /// </summary>
        public string Uploads { get; set; }
        public int TotalPlayListResults { get; set; } = 0;
        /// <summary>
        /// 該頻道所有公開撥放清單(封面)
        /// </summary>
        public List<PlayListCover> AllPublicPlayListCover { get; set; }
        public class PlayListCover
        {
            public string Id { get; set; }
            public string PublicPlayListImageURL { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string Status { get; set; }
            public DateTime PublishDate { get; set; }
            public int ItemCount { get; set; }
        }
    }
}
