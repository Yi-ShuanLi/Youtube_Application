using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube_Application.Models.ViewModels
{
    public class SelectedSearchFilterOptionsModel
    {
        public string SearchQ { get; set; }
        public OptionModel Category { get; set; }

        public OptionModel FilmLength { get; set; }

        public OptionModel UploadDate { get; set; }

        public OptionModel Property { get; set; }

        public OptionModel Priority { get; set; }
    }
}
