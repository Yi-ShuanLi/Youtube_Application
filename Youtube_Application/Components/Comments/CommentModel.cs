using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using Youtube_API.Videos.Enums;
using Youtube_Application.Models.DTO;

namespace Youtube_Application.Components.Comments
{
    [AddINotifyPropertyChangedInterface]
    public class CommentModel
    {
        public DateTime PublishedAt { get; set; }
        public string TextDisplay { get; set; }
        public string AuthorProfileImageUrl { get; set; }
        public string AuthorDisplayName { get; set; }
        public int LikeCount { get; set; }
        public string VideoId { get; set; }
        public string AuthorChannelId { get; set; }
        public string ViewerRating { get; set; }
        /// <summary>
        /// 綁定編輯留言
        /// </summary>
        public string EditText { get; set; }
        /// <summary>
        /// 綁定Comment.xaml編輯狀態顯示
        /// </summary>
        public bool IsEditing { get; set; }
        public string ViewerRatingShow
        {
            get
            {
                if (ViewerRating == "like")
                    return "👍";
                else if (ViewerRating == "dislike")
                    return "👎";
                else return "";
            }
            set
            {

            }
        }
        public Visibility IsMyComment
        {
            get
            {
                if (AuthorChannelId == App.MyChannelID)
                    return Visibility.Visible;
                return Visibility.Collapsed;
            }
            set
            {

            }
        }
        public string AuthorChannelUrl { get; set; }
        /// <summary>
        /// 當則Comment的Id
        /// </summary>
        public string Id { get; set; }

        public int TotalReplyCount { get; set; }

        public ICommand ApplyAddSubComment { get; set; }

        public ICommand NotifyAddSubComment { get; set; }
        public ICommand ApplyDeleteComment { get; set; }

        public ICommand NotifyDeleteComment { get; set; }
        public ICommand ApplyCancelComment { get; set; }
        public ICommand ApplyEditComment { get; set; }
        public ICommand NotifyEditComment { get; set; }
        public ICommand OpenEditComment { get; set; }
        public CommentModel()
        {

            ApplyAddSubComment = new RelayCommand<CommentModel>((x) =>
            {

            });
            ApplyDeleteComment = new RelayCommand<string>((x) =>
            {
                this.NotifyDeleteComment.Execute(x);
            });
            ApplyCancelComment = new RelayCommand((x) =>
            {
                this.IsEditing = false;
            });
            ApplyEditComment = new RelayCommand<CommentModel>((x) =>
            {
                this.IsEditing = false;
                this.TextDisplay = this.EditText;
                this.NotifyEditComment.Execute(x);
            });
            OpenEditComment = new RelayCommand((x) =>
            {
                this.IsEditing = true;
                this.EditText = this.TextDisplay;
            });

        }
    }
}
