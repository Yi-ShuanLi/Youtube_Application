using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube_Application.Models.ViewModels
{
    [AddINotifyPropertyChangedInterface]
    public class OptionModel
    {
        public bool IsSelected { get; set; }
        public string Name { get; set; }

        public string PropName { get; set; }
        public string PropValue { get; set; }
        public OptionModel(string name, bool isSelected, string propName, string propValue)
        {
            this.Name = name;
            this.IsSelected = isSelected;
            PropName = propName;
            PropValue = propValue;
        }
        public OptionModel() { }
    }
}
