using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube_Application.Components.Comments;
using Youtube_Application.Models.DTO;
using Youtube_Application.Models.ViewModels;

namespace Youtube_Application.Contract
{
    internal class VideoDetailContract
    {

        public interface IVideoDetailView
        {
            void VideoCommentsResponse(List<CommentViewDTOModel> commentModels);
            void MyChannelInformResponse(ChannelViewDTOModel myChannel);
            void AddVideoCommentResponse(CommentViewDTOModel commentViewDTOModel);
        }
        public interface IVideoDetailPresenter
        {
            Task RatingVideo(string videoId, string ratingButtonText);
            Task GetVideoComments(string videoId, int maxResults);
            Task GetMyChannelInform();
            Task AddVideoComment(string videoId, string commentText);
            Task DeleteMyComment(string commentId);
            Task EditMyComment(string commentId, string commentText);
        }
    }
}
