using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube_Application.ViewModels
{
    public class SearchModel
    {
        public string QueryKeyword { get; set; }
        public string QueryType { get; set; } = "video";
    }
}
