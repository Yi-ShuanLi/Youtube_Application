using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube_API.Videos.Models;

namespace Youtube_Application.Models.ViewModels
{
    public class SearchFilterModel
    {
        public List<OptionModel> Category { get; set; } = new List<OptionModel>() { new OptionModel("影片", false, "type", "video"), new OptionModel("shorts", false, "videoDuration", "short"), new OptionModel("頻道", false, "type", "channel"), new OptionModel("播放清單", false, "type", "playlist"), new OptionModel("電影", false, "videoType", "movie"), new OptionModel("節目劇集", false, "videoType", "episode") };

        public List<OptionModel> FilmLength { get; set; } = new List<OptionModel>() { new OptionModel("3分鐘內", false, "videoDuration", "short"), new OptionModel("3到20分鐘", false, "videoDuration", "medium"), new OptionModel("超過20分鐘", false, "videoDuration", "long") };

        public List<OptionModel> UploadDate { get; set; } = new List<OptionModel>() { new OptionModel("今天", false, null, null), new OptionModel("本週", false, null, null), new OptionModel("本月", false, null, null), new OptionModel("今年", false, null, null) };

        public List<OptionModel> Property { get; set; } = new List<OptionModel>() { new OptionModel("HD高畫質", false, "videoDefinition", "high"), new OptionModel("有字幕", false, "videoCaption", "closedCaption"), new OptionModel("無字幕", false, "videoCaption", "none"), new OptionModel("3D", false, "videoDimension", "3d"), new OptionModel("非3D", false, "videoDimension", "2d") };

        public List<OptionModel> Priority { get; set; } = new List<OptionModel>() { new OptionModel("關聯性", false, "order", "relevance"), new OptionModel("熱門程度", false, "order", "viewCount"), new OptionModel("日期", false, "order", "date") };


    }
}
