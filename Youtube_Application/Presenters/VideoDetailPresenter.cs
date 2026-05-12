using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube_API.Channels.Models;
using Youtube_API.Comments.Models;
using Youtube_API.PlayLists.Models;
using Youtube_API.Videos.Enums;
using Youtube_API.Videos.Models;
using Youtube_Application.Components.Comments;
using Youtube_Application.Models.DTO;
using Youtube_Application.Models.ViewModels;
using Youtube_Application.Utilitys;
using static Youtube_Application.Contract.VideoDetailContract;

namespace Youtube_Application.Presenters
{
    internal class VideoDetailPresenter : IVideoDetailPresenter
    {
        public Youtube_API.YoutubeContext youtube = new Youtube_API.YoutubeContext(true);
        public IVideoDetailView videoDetailView;
        public async Task RatingVideo(string videoId, string ratingButtonText)
        {

            VideoRatingType videoRatingType = (VideoRatingType)Enum.Parse(typeof(VideoRatingType), ratingButtonText);
            MyRatingVideosModel myLikeVideos = await youtube.Video.GetMyRatingVideos(videoRatingType);
            List<MyRatingVideoItemModel> myLikeVideoItems = Mapper.Map<MyRatingVideosModel.Item, MyRatingVideoItemModel>(myLikeVideos.items.ToList(), config =>
            {
                config.ForMember(y => y.Title, y => y.MapFrom(z => z.snippet.title))//ForMeber 左邊目的，右邊來源
                      .ForMember(y => y.VideoId, y => y.MapFrom(z => z.id))
                      .ForMember(y => y.Description, y => y.MapFrom(z => z.snippet.description))
                      .ForMember(y => y.ChannelTitle, y => y.MapFrom(z => z.snippet.channelTitle))
                      .ForMember(y => y.ChannelId, y => y.MapFrom(z => z.snippet.channelId))
                      .ForMember(y => y.PublishTime, y => y.MapFrom(z => z.snippet.publishedAt))
                      .ForMember(y => y.VideoCoverImgUrl, y => y.MapFrom(z => z.snippet.thumbnails.medium.url));
            });
            RatingType ratingType = (RatingType)Enum.Parse(typeof(RatingType), ratingButtonText);
            if (myLikeVideos == null)
            {
                await youtube.Video.RatingVideo(videoId, ratingType);
                return;
            }
            bool isRating = myLikeVideoItems.Select(y => y.VideoId).Contains(videoId);
            if (isRating == false)
                await youtube.Video.RatingVideo(videoId, ratingType);
            else
                await youtube.Video.RatingVideo(videoId, RatingType.None);
        }

        public async Task GetVideoComments(string videoId, int maxResults)
        {
            VideoCommentsModel videoCommentsModel = await youtube.Comment.GetComments(videoId, maxResults);
            List<CommentViewDTOModel> videoComments = Mapper.Map<VideoCommentsModel.Item, CommentViewDTOModel>(videoCommentsModel.items.ToList(), config =>
            {
                config.ForMember(y => y.PublishedAt, y => y.MapFrom(z => z.snippet.topLevelComment.snippet.publishedAt))//ForMeber 左邊目的，右邊來源
                      .ForMember(y => y.TextDisplay, y => y.MapFrom(z => z.snippet.topLevelComment.snippet.textDisplay))
                      .ForMember(y => y.AuthorProfileImageUrl, y => y.MapFrom(z => z.snippet.topLevelComment.snippet.authorProfileImageUrl))
                      .ForMember(y => y.AuthorDisplayName, y => y.MapFrom(z => z.snippet.topLevelComment.snippet.authorDisplayName))
                      .ForMember(y => y.LikeCount, y => y.MapFrom(z => z.snippet.topLevelComment.snippet.likeCount))
                      .ForMember(y => y.VideoId, y => y.MapFrom(z => z.snippet.videoId))
                      .ForMember(y => y.AuthorChannelId, y => y.MapFrom(z => z.snippet.topLevelComment.snippet.authorChannelId.value))
                      .ForMember(y => y.Id, y => y.MapFrom(z => z.id))
                      .ForMember(y => y.AuthorChannelUrl, y => y.MapFrom(z => z.snippet.topLevelComment.snippet.authorChannelUrl));
            });
            string CommentIds = string.Join(",", videoComments.Select(x => x.Id).Distinct());
            Task<CommentInformModel> batchCommentsInform = youtube.Comment.GetCommentInform(CommentIds);
            CommentInformModel batchComments = await batchCommentsInform;
            foreach (var item in videoComments)
            {
                var commentMatch = batchComments.items.FirstOrDefault(y => y.id == item.Id);
                item.ViewerRating = commentMatch.snippet.viewerRating;
            }

            videoDetailView.VideoCommentsResponse(videoComments);
        }

        public async Task GetMyChannelInform()
        {
            ChannelInformModel channelInformModel = await youtube.Channel.GetChannelInform();
            List<ChannelViewDTOModel> myChannelInform = Mapper.Map<ChannelInformModel.Item, ChannelViewDTOModel>(channelInformModel.items.ToList(), config =>
            {
                config.ForMember(y => y.Id, y => y.MapFrom(z => z.id))//ForMeber 左邊目的，右邊來源
                      .ForMember(y => y.Title, y => y.MapFrom(z => z.snippet.title))
                      .ForMember(y => y.Description, y => y.MapFrom(z => z.snippet.description))
                      .ForMember(y => y.CustomUrl, y => y.MapFrom(z => z.snippet.customUrl))
                      .ForMember(y => y.ImageUrl, y => y.MapFrom(z => z.snippet.thumbnails.medium.url))
                      .ForMember(y => y.PublishedAt, y => y.MapFrom(z => z.snippet.publishedAt))
                      .ForMember(y => y.ViewCount, y => y.MapFrom(z => z.statistics.viewCount))
                      .ForMember(y => y.SubscriberCount, y => y.MapFrom(z => z.statistics.subscriberCount))
                      .ForMember(y => y.VideoCount, y => y.MapFrom(z => z.statistics.videoCount))
                      .ForMember(y => y.Uploads, y => y.MapFrom(z => z.contentDetails.relatedPlaylists.uploads));
            });

            Stopwatch totalStopwatch = new Stopwatch();
            totalStopwatch.Start();
            AllAllocationListListCover batchPlayListCover = await youtube.PlayList.GetAllCover();
            myChannelInform[0].TotalPlayListResults = batchPlayListCover.pageInfo.totalResults;
            myChannelInform[0].AllPublicPlayListCover = Mapper.Map<AllAllocationListListCover.Item, ChannelViewDTOModel.PlayListCover>(batchPlayListCover.items.ToList(), config =>
            {
                config.ForMember(y => y.Id, y => y.MapFrom(z => z.id))//ForMeber 左邊目的，右邊來源
                      .ForMember(y => y.PublicPlayListImageURL, y => y.MapFrom(z => z.snippet.thumbnails.medium.url))
                      .ForMember(y => y.Title, y => y.MapFrom(z => z.snippet.title))
                      .ForMember(y => y.Description, y => y.MapFrom(z => z.snippet.description))
                      .ForMember(y => y.Status, y => y.MapFrom(z => z.status.privacyStatus))
                      .ForMember(y => y.PublishDate, y => y.MapFrom(z => z.snippet.publishedAt))
                      .ForMember(y => y.ItemCount, y => y.MapFrom(z => z.contentDetails.itemCount));
            });
            totalStopwatch.Stop();
            Console.WriteLine($"優化版4  批次發送api :秒數|{Math.Round((double)(totalStopwatch.ElapsedMilliseconds), 2).ToString("#,##0")}|");
            videoDetailView.MyChannelInformResponse(myChannelInform[0]);
        }



        public async Task AddVideoComment(string videoId, string commentText)
        {
            AddCommentModel addCommentModel = await youtube.Comment.AddComment(videoId, commentText);
            CommentViewDTOModel commentModel = new CommentViewDTOModel();
            commentModel.VideoId = addCommentModel.snippet.videoId;
            commentModel.PublishedAt = addCommentModel.snippet.topLevelComment.snippet.publishedAt;
            commentModel.AuthorProfileImageUrl = addCommentModel.snippet.topLevelComment.snippet.authorProfileImageUrl;
            commentModel.TextDisplay = addCommentModel.snippet.topLevelComment.snippet.textDisplay;
            commentModel.AuthorDisplayName = addCommentModel.snippet.topLevelComment.snippet.authorDisplayName;
            commentModel.LikeCount = addCommentModel.snippet.topLevelComment.snippet.likeCount;
            commentModel.AuthorChannelId = addCommentModel.snippet.topLevelComment.snippet.authorChannelId.value;
            commentModel.ViewerRating = addCommentModel.snippet.topLevelComment.snippet.viewerRating;
            commentModel.Id = addCommentModel.id;
            commentModel.TotalReplyCount = addCommentModel.snippet.totalReplyCount;
            videoDetailView.AddVideoCommentResponse(commentModel);
        }

        public async Task DeleteMyComment(string commentId)
        {
            await youtube.Comment.DeleteComment(commentId);
        }

        public async Task EditMyComment(string commentId, string commentText)
        {
            await youtube.Comment.EditComment(commentId, commentText);
        }

        public VideoDetailPresenter(IVideoDetailView view)
        {
            videoDetailView = view;
        }
    }
}
