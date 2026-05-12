using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Youtube_API.PlayLists.Models;
using Youtube_API.Videos.Enums;
using Youtube_API.Videos.Models;
using Youtube_Application.Components.Comments;
using Youtube_Application.Components.ReplyComment;
using Youtube_Application.Components.ReplyComment.Enums;
using Youtube_Application.Models.DTO;
using Youtube_Application.Models.PlayList;
using Youtube_Application.Models.ViewModels;
using Youtube_Application.Presenters;
using Youtube_Application.Utilitys;
using static System.Net.Mime.MediaTypeNames;
using static Youtube_API.Videos.Models.SearchVideoModel;
using static Youtube_Application.Contract.VideoDetailContract;

namespace Youtube_Application.Pages.Context
{
    [AddINotifyPropertyChangedInterface]
    internal class VideoDetailContext : INavigationAware, IVideoDetailView
    {
        public IVideoDetailPresenter VideoDetailPresenter;
        public bool SeeMoreVideoInfo { get; set; } = false;
        public string SeeMoreVideoInfoButtonText
        {
            get
            {
                if (!SeeMoreVideoInfo)
                    return "顯示詳細資訊";
                return "顯示部分資訊";
            }
        }
        public ObservableCollection<CommentModel> CommentsModel { get; set; }
        public VideoItemViewModel VideoModel { get; set; }
        public ReplyCommentModel ReplyCommentModel { get; set; }
        private ChannelViewDTOModel MyChannel;
        public ICommand VideoRatingCommand { get; set; }
        public ICommand RatingCommentCommand { get; set; }
        public ICommand SeeMoreVideoInformCommand { get; set; }
        public ICommand AddCommentCommand { get; set; }
        public ICommand DeleteMyCommentCommand { get; set; }
        public ICommand EditMyCommentCommand { get; set; }
        public void OnDataReceived(object data)
        {
            this.VideoModel = (VideoItemViewModel)data;
            this.VideoDetailPresenter.GetVideoComments(VideoModel.VideoId, 50);
        }
        public void VideoCommentsResponse(List<CommentViewDTOModel> commentModels)
        {
            List<CommentModel> commentsDTO = Mapper.Map<CommentViewDTOModel, CommentModel>(commentModels);
            CommentsModel = new ObservableCollection<CommentModel>(commentsDTO);
        }
        public void MyChannelInformResponse(ChannelViewDTOModel myChannel)
        {
            MyChannel = myChannel;
        }

        public void AddVideoCommentResponse(CommentViewDTOModel commentViewDTOModel)
        {
            CommentModel comment = Mapper.Map<CommentViewDTOModel, CommentModel>(commentViewDTOModel);
            CommentsModel.Insert(0, comment);
        }

        public VideoDetailContext()
        {
            this.VideoDetailPresenter = new VideoDetailPresenter(this);
            this.VideoDetailPresenter.GetMyChannelInform();
            this.VideoRatingCommand = new RelayCommand<string>(async (x) =>
            {
                await VideoDetailPresenter.RatingVideo(VideoModel.VideoId, x);
            });
            this.RatingCommentCommand = new RelayCommand<CommentViewDTOModel>((x) =>
            {

            });
            this.SeeMoreVideoInformCommand = new RelayCommand((x) =>
            {
                SeeMoreVideoInfo = !SeeMoreVideoInfo;
            });
            this.AddCommentCommand = new RelayCommand<string>(async (x) =>
            {
                await this.VideoDetailPresenter.AddVideoComment(VideoModel.VideoId, x);
            });
            this.DeleteMyCommentCommand = new RelayCommand<string>(async (x) =>
            {
                await this.VideoDetailPresenter.DeleteMyComment(x);
                CommentModel comment = CommentsModel.FirstOrDefault(y => y.Id == x);
                CommentsModel.Remove(comment);
            });
            this.EditMyCommentCommand = new RelayCommand<CommentModel>(async (x) =>
            {
                await this.VideoDetailPresenter.EditMyComment(x.Id, x.EditText);
            });
        }
    }
}
