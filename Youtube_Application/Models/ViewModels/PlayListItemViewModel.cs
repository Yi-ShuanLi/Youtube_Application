using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube_Application.Models.PlayList
{
    [AddINotifyPropertyChangedInterface]
    public class PlayListItemViewModel
    {
        public string Name { get; set; }
        public string Id { get; set; }
        public string Description { get; set; }
        public string Authority { get; set; }
        public int ContentCount { get; set; }


        //Mapper.Map<DAO,DTO>(dao, config=>{
        // config.ForMeber(x=>x.Id,y=>y.id)
        //})

    }
}
