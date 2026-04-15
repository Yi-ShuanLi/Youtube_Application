using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube_Application.ViewModels
{
    public class CreateListModel
    {
        public string PlayListName { get; set; }
        public bool IsPublic { get; set; } = true;

        public string Authority => IsPublic ? "public" : "private";
        public string Description { get; set; }
    }
}
