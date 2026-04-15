using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube_API.Channels.Models;
using Youtube_API.Videos.Models;
using Youtube_Application.Models.PlayList;
using Youtube_Application.Utilitys;
using Youtube_Application.ViewModels;
using static Youtube_Application.Contract.SearchVideoContract;

namespace Youtube_Application.Presenters
{
    public class SearchVideoPresenter : ISearchVideoPresenter
    {
        public ISearchVideoModelView SearchVideoModelView;
        public Youtube_API.YoutubeContext youtube = new Youtube_API.YoutubeContext(true);
        public SearchVideoPresenter(ISearchVideoModelView view)
        {
            this.SearchVideoModelView = view;
        }
        //TODO: 待重構項目: 後續會將下方foreach 裡面的api 整理成一支，加快資料回來的速度
        public async Task SearchVideo(SearchVideoModelReq searchVideoModelReq)
        {
            SearchVideoModel searchVideo = await youtube.Video.SearchVideoResponse(searchVideoModelReq);
            List<VideoItemViewModel> searchList = Mapper.Map<SearchVideoModel.Item, VideoItemViewModel>(searchVideo.items.ToList(), config =>
            {
                config.ForMember(x => x.Title, y => y.MapFrom(z => z.snippet.title))//ForMeber 左邊目的，右邊來源
                      .ForMember(x => x.VideoId, y => y.MapFrom(z => z.id.videoId))
                      .ForMember(x => x.Description, y => y.MapFrom(z => z.snippet.description))
                      .ForMember(x => x.ChannelTitle, y => y.MapFrom(z => z.snippet.channelTitle))
                      .ForMember(x => x.ChannelId, y => y.MapFrom(z => z.snippet.channelId))
                      .ForMember(x => x.PublishTime, y => y.MapFrom(z => z.snippet.publishTime))
                      .ForMember(x => x.ImageUrl, y => y.MapFrom(z => z.snippet.thumbnails.medium.url));
            });

            foreach (var item in searchList)
            {
                ChannelInformModel channelInformModel = await youtube.Channel.GetChannelInform(item.ChannelId);
                item.ChannelImageUrl = channelInformModel.items[0].snippet.thumbnails.medium.url;
                if (item.VideoId != null)
                {
                    VideoInformModel videoInformModel = await youtube.Video.GetVideoInform(item.VideoId);
                    item.ViewCount = long.Parse((videoInformModel.items[0].statistics.viewCount == null ? "0" : videoInformModel.items[0].statistics.viewCount));
                    item.LikeCount = long.Parse(videoInformModel.items[0].statistics.likeCount == null ? "0" : videoInformModel.items[0].statistics.likeCount);
                    item.FavoriteCount = long.Parse(videoInformModel.items[0].statistics.favoriteCount == null ? "0" : videoInformModel.items[0].statistics.favoriteCount);
                    item.CommentCount = long.Parse(videoInformModel.items[0].statistics.commentCount == null ? "0" : videoInformModel.items[0].statistics.commentCount);
                }
            }
            SearchVideoModelView.GetSearchVideos(searchList);
        }
    }
}
