using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube_API.Videos.Enums;

namespace Youtube_Application.Models.DTO
{
    internal class RatingCommentDTOModel
    {
        public string CommentID { get; set; }
        public RatingType RatingType { get; set; }

        public RatingCommentDTOModel(string commentID, RatingType ratingType)
        {
            CommentID = commentID;
            RatingType = ratingType;
        }
    }
}
