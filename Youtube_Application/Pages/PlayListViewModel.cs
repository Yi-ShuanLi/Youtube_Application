using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Youtube_API.PlayLists.Models;
using Youtube_API.Videos.Models;
using Youtube_Application.Models.PlayList;
using Youtube_Application.Utilitys;
using Youtube_Application.ViewModels;

namespace Youtube_Application.Pages
{
    [AddINotifyPropertyChangedInterface]
    internal class PlayListViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public ObservableCollection<PlayListItemViewModel> Results { get; set; } = new ObservableCollection<PlayListItemViewModel>();
        public Youtube_API.YoutubeContext youtube = new Youtube_API.YoutubeContext(true);
        public CreateListModel CreatePlayListModel { get; set; } = new CreateListModel();
        public ICommand CreatePlayListCommand { get; set; }
        public ICommand GetAllPlayListCoverCommand { get; set; }
        public async void CreatePlayList()
        {
            AddNewPlayList addNewPlayList = await youtube.PlayList.Create(CreatePlayListModel.PlayListName, CreatePlayListModel.Authority, CreatePlayListModel.Description);
            PlayListItemViewModel playListItem = Mapper.Map<AddNewPlayList, PlayListItemViewModel>(addNewPlayList, config =>
            {
                config.ForMember(x => x.Id, y => y.MapFrom(z => z.id))//ForMeber 左邊目的，右邊來源
                      .ForMember(x => x.Name, y => y.MapFrom(z => z.snippet.title))
                      .ForMember(x => x.Authority, y => y.MapFrom(z => z.status.privacyStatus))
                      .ForMember(x => x.ContentCount, y => y.MapFrom(z => 0))
                      .ForMember(x => x.Description, y => y.MapFrom(z => z.snippet.description));
            });
            Results.Add(playListItem);
        }
        public async void GetAllPlayListCover()
        {
            AllAllocationListListCover allAllocationListListCover = await youtube.PlayList.GetMyAllCover();
            List<PlayListItemViewModel> allPlayList = Mapper.Map<AllAllocationListListCover.Item, PlayListItemViewModel>(allAllocationListListCover.items.ToList(), config =>
            {
                config.ForMember(x => x.Name, y => y.MapFrom(z => z.snippet.title))//ForMeber 左邊目的，右邊來源
                      .ForMember(x => x.Id, y => y.MapFrom(z => z.id))
                      .ForMember(x => x.Description, y => y.MapFrom(z => z.snippet.description))
                      .ForMember(x => x.Authority, y => y.MapFrom(z => z.status.privacyStatus))
                      .ForMember(x => x.ContentCount, y => y.MapFrom(z => z.contentDetails.itemCount));
            });
            foreach (var item in allPlayList)
            {
                Results.Add(item);
            }
        }

        public PlayListViewModel()
        {
            CreatePlayListCommand = new RelayCommand((x) =>
            {
                this.CreatePlayList();
            });

            GetAllPlayListCoverCommand = new RelayCommand((x) =>
            {
                this.GetAllPlayListCover();
            });
        }

    }
}
