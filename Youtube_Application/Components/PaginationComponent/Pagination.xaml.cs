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
using Youtube_Application.Models;

namespace Youtube_Application.Components.PaginationComponent
{
    /// <summary>
    /// Pagination.xaml 的互動邏輯
    /// </summary>
    public partial class Pagination : UserControl
    {
        public Pagination()
        {
            InitializeComponent();
            DataContext = new PaginationContext();
        }
        public static readonly DependencyProperty TotalCountProperty = DependencyProperty.Register(
      nameof(TotalCount), //我是誰
      typeof(int),//原本宣告的 TotalCount僅是一般屬性，所以再定義
      typeof(Pagination),//我要掛在誰下面
      new PropertyMetadata((d, e) => //d是DependencyObject，e是DependencyProperty
      {
          // 比大小=>DependencyObject > UserControl  > DependencyProperty
          // DependencyProperty(子)必須隸屬於某個DependencyObject(父)
          // 因為會透過Register去註冊，「typeof(TaskComponent),//我要掛在誰下面」
          //「我要掛在誰下面(父)」的誰是必須要有DependencyObject(父)，本頁是UserControl
          Pagination pagination = (Pagination)d;
          // d是DependencyObject比 「Pagination : UserControl」大，轉回去 Pagination      
          PaginationContext paginationContext = (PaginationContext)pagination.DataContext;
          // 分頁元件前端是綁定 PaginationContext ，也就是Pagination建構式所寫的那樣
          // 最終在PaginationContext的 TotalCount 及 PaginationPresenter.InitialTotalCount
          // 去設定總資料筆數
          paginationContext.TotalCount = (int)e.NewValue;

      }));

        public static readonly DependencyProperty CurrentPageProperty = DependencyProperty.Register(
            nameof(CurrentPageCommand),//我是誰
            typeof(ICommand),//原本宣告的 CurrentPageCommand僅是一般屬性，所以再定義
            typeof(Pagination),//我要掛在誰下面
            new PropertyMetadata((d, e) =>
            {
                // 比大小=>DependencyObject > UserControl  > DependencyProperty
                // DependencyProperty(子)必須隸屬於某個DependencyObject(父)
                // 因為會透過Register去註冊，「typeof(TaskComponent),//我要掛在誰下面」
                //「我要掛在誰下面(父)」的誰是必須要有DependencyObject(父)，本頁是UserControl
                Pagination pagination = (Pagination)d;
                ICommand command = (ICommand)e.NewValue; // 註冊傳入，會是MainViewModel的   這個Command，傳入
                // d是DependencyObject比 「Pagination : UserControl」大，轉回去 Pagination      
                PaginationContext paginationContext = (PaginationContext)pagination.DataContext;
                paginationContext.NotifyPageChangeCommand = command;
            }));

        public int TotalCount
        {
            get => (int)GetValue(TotalCountProperty);
            set
            {
                SetValue(TotalCountProperty, value);
            }
        }
        public ICommand CurrentPageCommand
        {
            get => (ICommand)GetValue(CurrentPageProperty);
            set
            {
                SetValue(CurrentPageProperty, value);
            }
        }
    }
}
