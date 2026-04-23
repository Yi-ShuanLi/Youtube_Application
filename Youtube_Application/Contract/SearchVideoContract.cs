using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube_API.Videos.Models;
using Youtube_Application.Models.DTO;
using Youtube_Application.Models.PlayList;
using Youtube_Application.ViewModels;

namespace Youtube_Application.Contract
{
    public class SearchVideoContract
    {
        public interface ISearchVideoModelView
        {
            void GetSearchVideos(List<VideoItemViewDTOModel> videoItemViewDTOModel);
        }
        public interface ISearchVideoPresenter
        {
            Task SearchVideo(SearchVideoModelReq searchVideoModelReq);
        }
    }
}
