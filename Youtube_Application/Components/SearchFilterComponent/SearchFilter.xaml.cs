using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Youtube_Application.Components.PaginationComponent;
using Youtube_Application.Models.ViewModels;

namespace Youtube_Application.Components.SearchFilterComponent
{
    /// <summary>
    /// SearchFilter.xaml 的互動邏輯
    /// </summary>
    public partial class SearchFilter : UserControl
    {
        public SearchFilter()
        {
            InitializeComponent();
            DataContext = new SearchFilterModel();
        }
        public static readonly DependencyProperty FilterOptionsProperty = DependencyProperty.Register(
            nameof(FilterCommand), //我是誰
            typeof(ICommand),//原本宣告的 TotalCount僅是一般屬性，所以再定義
            typeof(SearchFilter),//我要掛在誰下面
            new PropertyMetadata((d, e) => //d是DependencyObject，e是DependencyProperty
            {
                // 比大小=>DependencyObject > UserControl  > DependencyProperty
                // DependencyProperty(子)必須隸屬於某個DependencyObject(父)
                // 因為會透過Register去註冊，「typeof(TaskComponent),//我要掛在誰下面」
                //「我要掛在誰下面(父)」的誰是必須要有DependencyObject(父)，本頁是UserControl
                SearchFilter searchFilter = (SearchFilter)d;
                // d是DependencyObject比 「Pagination : UserControl」大，轉回去 Pagination      
                SearchFilterModel selectedSearchFilter = (SearchFilterModel)searchFilter.DataContext;
                // 分頁元件前端是綁定 PaginationContext ，也就是Pagination建構式所寫的那樣
                // 最終在PaginationContext的 TotalCount 及 PaginationPresenter.InitialTotalCount
                // 去設定總資料筆數
                selectedSearchFilter.NotifyFilterCommand = (ICommand)e.NewValue;


            }));
        public ICommand FilterCommand
        {
            get => (ICommand)GetValue(FilterOptionsProperty);
            set
            {
                SetValue(FilterOptionsProperty, value);
            }
        }
    }
}
