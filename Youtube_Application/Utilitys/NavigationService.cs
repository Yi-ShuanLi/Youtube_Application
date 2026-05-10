using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Youtube_Application.Utilitys
{
    public class NavigationService : INavigationService
    {
        Frame _frame;
        public void Navigate(Page page, object data = null)
        {
            //當今天傳入的Page.DataContext是有繼承INavigationAware的情況才傳遞資料
            if (data != null && page.DataContext is INavigationAware navigationAware)
            {
                navigationAware.OnDataReceived(data);
            }
            this._frame.Navigate(page);
        }

        public void SetFrame(Frame frame)
        {
            this._frame = frame;
        }
    }
}
