using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube_Application.Models.DTO
{
    public class CommentViewDTOModel
    {
        public DateTime PublishedAt { get; set; }
        public string TextDisplay { get; set; }
        public string AuthorProfileImageUrl { get; set; }
        public string AuthorDisplayName { get; set; }
        public int LikeCount { get; set; }
        public string VideoId { get; set; }
        public string AuthorChannelId { get; set; }
        public string AuthorChannelUrl { get; set; }
        /// <summary>
        /// 當則Comment的Id
        /// </summary>
        public string Id { get; set; }
        public string ViewerRating { get; set; }
        public int TotalReplyCount { get; set; }

    }
}
