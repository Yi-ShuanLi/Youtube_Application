using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Youtube_Application.Utilitys
{
    public interface INavigationService
    {
        void Navigate(Page page, object data = null);
        void SetFrame(Frame frame);
    }
}
