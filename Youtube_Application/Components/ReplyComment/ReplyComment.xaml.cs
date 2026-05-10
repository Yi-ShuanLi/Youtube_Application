using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Youtube_Application.Components.Comments;

namespace Youtube_Application.Components.ReplyComment
{
    /// <summary>
    /// ReplyComment.xaml 的互動邏輯
    /// </summary>
    public partial class ReplyComment : UserControl
    {
        public ReplyComment()
        {
            InitializeComponent();
            DataContext = new ReplyCommentModel();
        }
        //接收最外層window的Command把它轉換到VideoItemViewModel專門接收最外層的Command去儲存
        public static readonly DependencyProperty CancelCommentProperty = DependencyProperty.Register(
           nameof(CancelCommentCommand), //我是誰
           typeof(ICommand),//原本宣告的 TotalCount僅是一般屬性，所以再定義
           typeof(ReplyComment),//我要掛在誰下面
           new PropertyMetadata((d, e) => //d是DependencyObject，e是DependencyProperty
           {
               // 比大小=>DependencyObject > UserControl  > DependencyProperty
               // DependencyProperty(子)必須隸屬於某個DependencyObject(父)
               // 因為會透過Register去註冊，「typeof(TaskComponent),//我要掛在誰下面」
               //「我要掛在誰下面(父)」的誰是必須要有DependencyObject(父)，本頁是UserControl
               ReplyComment replyComment = (ReplyComment)d;
               // d是DependencyObject比 「Pagination : UserControl」大，轉回去 Pagination      
               ReplyCommentModel replyCommentModel = (ReplyCommentModel)replyComment.DataContext;
               // 分頁元件前端是綁定 PaginationContext ，也就是Pagination建構式所寫的那樣
               // 最終在PaginationContext的 TotalCount 及 PaginationPresenter.InitialTotalCount
               // 去設定總資料筆數
               replyCommentModel.NotifyCancelComment = (ICommand)e.NewValue;
           }));
        public ICommand CancelCommentCommand //在外層的window上寫的欄位(屬性)，當作進入點，把外層window的command綁定送進來
        {
            get => (ICommand)GetValue(CancelCommentProperty);
            set
            {
                SetValue(CancelCommentProperty, value);
            }
        }

        //接收最外層window的Command把它轉換到VideoItemViewModel專門接收最外層的Command去儲存
        public static readonly DependencyProperty ReplyCommentProperty = DependencyProperty.Register(
           nameof(ReplyCommentCommand), //我是誰
           typeof(ICommand),//原本宣告的 TotalCount僅是一般屬性，所以再定義
           typeof(ReplyComment),//我要掛在誰下面
           new PropertyMetadata((d, e) => //d是DependencyObject，e是DependencyProperty
           {
               // 比大小=>DependencyObject > UserControl  > DependencyProperty
               // DependencyProperty(子)必須隸屬於某個DependencyObject(父)
               // 因為會透過Register去註冊，「typeof(TaskComponent),//我要掛在誰下面」
               //「我要掛在誰下面(父)」的誰是必須要有DependencyObject(父)，本頁是UserControl
               ReplyComment replyComment = (ReplyComment)d;
               // d是DependencyObject比 「Pagination : UserControl」大，轉回去 Pagination      
               ReplyCommentModel replyCommentModel = (ReplyCommentModel)replyComment.DataContext;
               // 分頁元件前端是綁定 PaginationContext ，也就是Pagination建構式所寫的那樣
               // 最終在PaginationContext的 TotalCount 及 PaginationPresenter.InitialTotalCount
               // 去設定總資料筆數
               replyCommentModel.NotifyReplyComment = (ICommand)e.NewValue;
           }));
        public ICommand ReplyCommentCommand //在外層的window上寫的欄位(屬性)，當作進入點，把外層window的command綁定送進來
        {
            get => (ICommand)GetValue(ReplyCommentProperty);
            set
            {
                SetValue(ReplyCommentProperty, value);
            }
        }


    }
}
