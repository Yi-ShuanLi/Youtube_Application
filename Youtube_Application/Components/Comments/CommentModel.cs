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
        public string PublishedAt { get; set; }
        public string TextDisplay { get; set; }
        public string AuthorProfileImageUrl { get; set; }
        public string AuthorDisplayName { get; set; }
        public string LikeCount { get; set; }
        public string VideoId { get; set; }
        public string AuthorChannelId { get; set; }
        public string ViewerRating { get; set; }
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

        public ICommand ApplyAddSubComment { get; set; }

        public ICommand NotifyAddSubComment { get; set; }
        public CommentModel()
        {
            ApplyAddSubComment = new RelayCommand<CommentModel>((x) =>
            {

            });

        }
    }
}
