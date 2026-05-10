using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Youtube_API.Channels.Models;
using Youtube_API.Videos.Models;
using Youtube_Application.Models.DTO;
using Youtube_Application.Models.PlayList;
using Youtube_Application.Utilitys;
using Youtube_Application.ViewModels;
using static Youtube_Application.Contract.SearchVideoContract;

namespace Youtube_Application.Presenters
{
    public class SearchVideoPresenter : ISearchVideoPresenter
    {
        public ISearchVideoModelView SearchVideoModelView;
        public Youtube_API.YoutubeContext youtube = new Youtube_API.YoutubeContext(true);
        public SearchVideoPresenter(ISearchVideoModelView view)
        {
            this.SearchVideoModelView = view;
        }
        //TODO: 待重構項目: 後續會將下方foreach 裡面的api 整理成一支，加快資料回來的速度
        public async Task SearchVideo(SearchVideoModelReq searchVideoModelReq)
        {
            SearchVideoModel searchVideo = await youtube.Video.SearchVideoResponse(searchVideoModelReq);
            List<VideoItemViewDTOModel> searchList = Mapper.Map<SearchVideoModel.Item, VideoItemViewDTOModel>(searchVideo.items.ToList(), config =>
            {
                config.ForMember(x => x.Title, y => y.MapFrom(z => z.snippet.title))//ForMeber 左邊目的，右邊來源
                      .ForMember(x => x.VideoId, y => y.MapFrom(z => z.id.videoId))
                      .ForMember(x => x.Description, y => y.MapFrom(z => z.snippet.description))
                      .ForMember(x => x.ChannelTitle, y => y.MapFrom(z => z.snippet.channelTitle))
                      .ForMember(x => x.ChannelId, y => y.MapFrom(z => z.snippet.channelId))
                      .ForMember(x => x.PublishTime, y => y.MapFrom(z => z.snippet.publishTime))
                      .ForMember(x => x.ImageUrl, y => y.MapFrom(z => z.snippet.thumbnails.medium.url));
            });

            //N+1問題 (sql面試常考)
            //透過for迴圈讀取每一筆資料，反覆去查找每一筆資料明細 (頻繁進出 Disk I/O)
            #region 原始版本
            //Stopwatch totalStopwatch = new Stopwatch();
            //totalStopwatch.Start();            
            //foreach (var item in searchList)
            //{
            //    ChannelInformModel channelInformModel = await youtube.Channel.GetChannelInform(item.ChannelId);
            //    item.ChannelImageUrl = channelInformModel.items[0].snippet.thumbnails.medium.url;
            //    if (item.VideoId != null)
            //    {
            //        VideoInformModel videoInformModel = await youtube.Video.GetVideoInform(item.VideoId);
            //        item.ViewCount = long.Parse((videoInformModel.items[0].statistics.viewCount == null ? "0" : videoInformModel.items[0].statistics.viewCount));
            //        item.LikeCount = long.Parse(videoInformModel.items[0].statistics.likeCount == null ? "0" : videoInformModel.items[0].statistics.likeCount);
            //        item.FavoriteCount = long.Parse(videoInformModel.items[0].statistics.favoriteCount == null ? "0" : videoInformModel.items[0].statistics.favoriteCount);
            //        item.CommentCount = long.Parse(videoInformModel.items[0].statistics.commentCount == null ? "0" : videoInformModel.items[0].statistics.commentCount);
            //    }
            //}
            //totalStopwatch.Stop();
            //Console.WriteLine($"原始版本只有foreach:秒數{Math.Round((double)(totalStopwatch.ElapsedMilliseconds), 2).ToString("#,##0")}");
            #endregion

            #region Parallel.For版本 X (.NET Framwork 4.7.2)無法拿到資料，射後不理，必須要.NET 8的 Parallel.ForAsync或Parallel.ForEachAsync才行
            //1. 為什麼 Parallel.For 裡面不能用 await？
            //在 C# 的世界裡，Parallel.For 的設計初衷是給**「純計算」**用的，它並不認識 await。
            //當你在 Parallel.For 裡面硬加上 async (x) => 的時候，C# 編譯器會把它變成一種叫做 async void（射後不理） 的機制。
            //用「跑腿買便當」來舉例：
            //你（UI）指派了一位總管（Task.Run）。
            //總管叫了 10 個員工（Parallel.For），命令他們去 YouTube 買便當（拿資料）。
            //因為是「射後不理」機制，這 10 個員工一聽到命令，立刻回頭跟總管說：「報告總管，我們出發了！」
            //總管一聽，以為事情已經辦完了，就馬上回報給你：「資料都拿到了！」
            //於是你的程式就繼續往下走去抓觀看人數。但實際上，那 10 個員工還在 YouTube 門口排隊呢！這就是為什麼你的觀看人數會等不到資料的原因。
            //await Task.Run(() => Parallel.For(0, searchList.Count, async (x) =>
            //{
            //    //
            //    ChannelInformModel channelInformModel = await youtube.Channel.GetChannelInform(searchList[x].ChannelId);
            //    searchList[x].ChannelImageUrl = channelInformModel.items[0].snippet.thumbnails.medium.url;
            //    if (searchList[x].VideoId != null)
            //    {
            //        VideoInformModel videoInformModel = await youtube.Video.GetVideoInform(searchList[x].VideoId);
            //        searchList[x].ViewCount = long.Parse((videoInformModel.items[0].statistics.viewCount == null ? "0" : videoInformModel.items[0].statistics.viewCount));
            //        searchList[x].LikeCount = long.Parse(videoInformModel.items[0].statistics.likeCount == null ? "0" : videoInformModel.items[0].statistics.likeCount);
            //        searchList[x].FavoriteCount = long.Parse(videoInformModel.items[0].statistics.favoriteCount == null ? "0" : videoInformModel.items[0].statistics.favoriteCount);
            //        searchList[x].CommentCount = long.Parse(videoInformModel.items[0].statistics.commentCount == null ? "0" : videoInformModel.items[0].statistics.commentCount);
            //    }
            //}));

            #endregion


            #region 優化版1 Task.Run
            //Stopwatch totalStopwatch = new Stopwatch();
            //totalStopwatch.Start();
            //foreach (var item in searchList)
            //{
            //    await Task.Run(async () =>
            //    {
            //        ChannelInformModel channelInformModel = await youtube.Channel.GetChannelInform(item.ChannelId);
            //        item.ChannelImageUrl = channelInformModel.items[0].snippet.thumbnails.medium.url;
            //        if (item.VideoId != null)
            //        {
            //            VideoInformModel videoInformModel = await youtube.Video.GetVideoInform(item.VideoId);
            //            item.ViewCount = long.Parse((videoInformModel.items[0].statistics.viewCount == null ? "0" : videoInformModel.items[0].statistics.viewCount));
            //            item.LikeCount = long.Parse(videoInformModel.items[0].statistics.likeCount == null ? "0" : videoInformModel.items[0].statistics.likeCount);
            //            item.FavoriteCount = long.Parse(videoInformModel.items[0].statistics.favoriteCount == null ? "0" : videoInformModel.items[0].statistics.favoriteCount);
            //            item.CommentCount = long.Parse(videoInformModel.items[0].statistics.commentCount == null ? "0" : videoInformModel.items[0].statistics.commentCount);
            //        }
            //    });
            //}
            //totalStopwatch.Stop();
            //Console.WriteLine($"優化版1 Task.Run:秒數{Math.Round((double)(totalStopwatch.ElapsedMilliseconds), 2).ToString("#,##0")}");

            #endregion

            #region 優化版2 Task.WhenAll
            //Stopwatch totalStopwatch = new Stopwatch();
            //totalStopwatch.Start();
            //List<Task> list = new List<Task>();
            //foreach (var item in searchList)
            //{
            //    list.Add(Task.Run(async () =>
            //    {
            //        ChannelInformModel channelInformModel = await youtube.Channel.GetChannelInform(item.ChannelId);
            //        item.ChannelImageUrl = channelInformModel.items[0].snippet.thumbnails.medium.url;
            //        if (item.VideoId != null)
            //        {
            //            VideoInformModel videoInformModel = await youtube.Video.GetVideoInform(item.VideoId);
            //            item.ViewCount = long.Parse((videoInformModel.items[0].statistics.viewCount == null ? "0" : videoInformModel.items[0].statistics.viewCount));
            //            item.LikeCount = long.Parse(videoInformModel.items[0].statistics.likeCount == null ? "0" : videoInformModel.items[0].statistics.likeCount);
            //            item.FavoriteCount = long.Parse(videoInformModel.items[0].statistics.favoriteCount == null ? "0" : videoInformModel.items[0].statistics.favoriteCount);
            //            item.CommentCount = long.Parse(videoInformModel.items[0].statistics.commentCount == null ? "0" : videoInformModel.items[0].statistics.commentCount);
            //        }
            //    }));
            //}
            //await Task.WhenAll(list);
            //totalStopwatch.Stop();
            //Console.WriteLine($"優化版2 Task.WhenAll:秒數|{Math.Round((double)(totalStopwatch.ElapsedMilliseconds), 2).ToString("#,##0")}|");
            #endregion

            #region 優化版3  LinQ搭配 Task.WhenAll
            //Stopwatch totalStopwatch = new Stopwatch();
            //totalStopwatch.Start();
            //var task = searchList.Select(async (item) =>
            //{
            //    if (item.VideoId != null)
            //    {
            //        //在Task被建立的當下，方法就開始執行了，所以讓這兩個api分別先跑

            //        Task<ChannelInformModel> task1 = youtube.Channel.GetChannelInform(item.ChannelId);
            //        Task<VideoInformModel> task2 = youtube.Video.GetVideoInform(item.VideoId);
            //        //之後再強制接回來
            //        ChannelInformModel channelInformModel = await task1;
            //        VideoInformModel videoInformModel = await task2;
            //        item.ChannelImageUrl = channelInformModel.items[0].snippet.thumbnails.medium.url;
            //        item.ViewCount = long.Parse((videoInformModel.items[0].statistics.viewCount == null ? "0" : videoInformModel.items[0].statistics.viewCount));
            //        item.LikeCount = long.Parse(videoInformModel.items[0].statistics.likeCount == null ? "0" : videoInformModel.items[0].statistics.likeCount);
            //        item.FavoriteCount = long.Parse(videoInformModel.items[0].statistics.favoriteCount == null ? "0" : videoInformModel.items[0].statistics.favoriteCount);
            //        item.CommentCount = long.Parse(videoInformModel.items[0].statistics.commentCount == null ? "0" : videoInformModel.items[0].statistics.commentCount);
            //    }

            //});
            //await Task.WhenAll(task);
            //totalStopwatch.Stop();
            //Console.WriteLine($"優化版3  LinQ搭配 Task.WhenAll:秒數|{Math.Round((double)(totalStopwatch.ElapsedMilliseconds), 2).ToString("#,##0")}|");
            #endregion

            #region 優化版4  批次發送api
            Stopwatch totalStopwatch = new Stopwatch();
            totalStopwatch.Start();
            string channelIds = string.Join(",", searchList.Select(x => x.ChannelId).Distinct());
            string videoIds = string.Join(",", searchList.Select(x => x.VideoId).Distinct());
            Task<ChannelInformModel> batchChannelTask = youtube.Channel.GetChannelInform(channelIds);
            Task<VideoInformModel> batchVideoTask = youtube.Video.GetVideoInform(videoIds);
            ChannelInformModel batchChannel = await batchChannelTask;
            VideoInformModel batchVideo = await batchVideoTask;

            foreach (var item in searchList)
            {
                var channelMatch = batchChannel.items.FirstOrDefault(y => y.id == item.ChannelId);
                item.ChannelImageUrl = channelMatch.snippet.thumbnails.medium.url;
                item.SubscriberCount = long.Parse(channelMatch.statistics.subscriberCount == null ? "0" : channelMatch.statistics.subscriberCount);
                if (item.VideoId != null)
                {
                    var videoMatch = batchVideo.items.FirstOrDefault(y => y.id == item.VideoId);
                    item.ViewCount = long.Parse((videoMatch.statistics.viewCount == null ? "0" : videoMatch.statistics.viewCount));
                    item.LikeCount = long.Parse(videoMatch.statistics.likeCount == null ? "0" : videoMatch.statistics.likeCount);
                    item.FavoriteCount = long.Parse(videoMatch.statistics.favoriteCount == null ? "0" : videoMatch.statistics.favoriteCount);
                    item.CommentCount = long.Parse(videoMatch.statistics.commentCount == null ? "0" : videoMatch.statistics.commentCount);
                }
            }



            totalStopwatch.Stop();
            Console.WriteLine($"優化版4  批次發送api :秒數|{Math.Round((double)(totalStopwatch.ElapsedMilliseconds), 2).ToString("#,##0")}|");
            #endregion



            //await Task.Delay(5000);
            SearchVideoModelView.GetSearchVideos(searchList);
        }
    }
}
