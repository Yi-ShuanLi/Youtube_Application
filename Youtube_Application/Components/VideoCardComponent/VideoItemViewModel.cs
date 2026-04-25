using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace Youtube_Application.Models.PlayList
{
    [AddINotifyPropertyChangedInterface]
    public class VideoItemViewModel
    {

        public string Title { get; set; }
        public string ImageUrl { get; set; }
        public long ViewCount { get; set; }  //觀看人數
        public long LikeCount { get; set; }
        public long FavoriteCount { get; set; } //蒐藏人數
        public long CommentCount { get; set; }
        public string VideoId { get; set; }
        public string Description { get; set; }
        public string ChannelTitle { get; set; }
        public string ChannelImageUrl { get; set; }
        public string ChannelId { get; set; }
        public DateTime PublishTime { get; set; }

        public ICommand VideoItemModelCommand { get; set; } //綁在元件上的Command，通常是事件(click)
                                                            //因為卡片沒有事件，所以安裝套件Interaction.Triggers
                                                            //當用左鍵點擊卡片時，我的最外層會收到卡片VideoItemViewModel的所有資訊

        public ICommand GetCommand { get; set; }//供外層(MainViewModel)的Command註冊使用

        public VideoItemViewModel()
        {
            VideoItemModelCommand = new RelayCommand<VideoItemViewModel>((x) =>
            {
                this.GetCommand.Execute(x);
            });
        }
    }
}
