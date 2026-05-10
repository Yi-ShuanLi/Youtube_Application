using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube_Application.Models.ViewModels
{
    public class CommentPageModel
    {
        public bool IsSelected { get; set; }
        public string PageName { get; set; }
        public string NextPageToken { get; set; }
    }
}
