using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube_Application.Models.ViewModels
{
    public class MyRatingVideoItemModel
    {
        public string Title { get; set; }
        public string VideoId { get; set; }
        public string VideoCoverImgUrl { get; set; }
        public string Description { get; set; }
        public string ChannelTitle { get; set; }//頻道名字
        public string ChannelId { get; set; }
        public DateTime PublishTime { get; set; }
    }
}

