using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Youtube_API.Channels.Models;
using Youtube_Application.Utilitys;

namespace Youtube_Application
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        //因為要透過NavigationService在不同的Page互傳資料，所以用static
        public static INavigationService navigationService;
        public Youtube_API.YoutubeContext youtube = new Youtube_API.YoutubeContext(false);
        public static string MyChannelID;
        /// <summary>
        /// 把App的xaml檔中的MailWindow拿掉，讓程式從此事件開始執行，目的是為了把MyChannelID先找到並儲存
        /// 如果沒有從此開始執行，直接從MainWindow開始的話，這樣子程式就無法先設定MyChannelID
        /// 事件不能用Task只能用void，因為是前端UI thread，如果寫Task會切出去執行序
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        /// <returns></returns>
        private async void Application_Startup(object sender, StartupEventArgs e)
        {
            MainWindow = new MainWindow();
            ChannelInformModel channelInform = await youtube.Channel.GetChannelInform();
            MyChannelID = channelInform.items[0].id;
            MainWindow.Show();
        }
    }
}
