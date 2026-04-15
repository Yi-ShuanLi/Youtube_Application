using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube_Application.Utilitys
{
    internal static class Mapper
    {
        public static Ttarget Map<Tsource, Ttarget>(Tsource item, Action<IMappingExpression<Tsource, Ttarget>> action = null)
        {

            var config = new MapperConfiguration(cfg =>
            {
                var mappingExpression = cfg
                .CreateMap<Tsource, Ttarget>();
                action?.Invoke(mappingExpression);
            });
            var mapper = config.CreateMapper();
            Ttarget result = mapper.Map<Tsource, Ttarget>(item);
            return result;
        }
        public static List<Ttarget> Map<Tsource, Ttarget>(List<Tsource> items, Action<IMappingExpression<Tsource, Ttarget>> action = null)
        {
            List<Ttarget> results = new List<Ttarget>();
            for (int i = 0; i < items.Count; i++)
            {
                var config = new MapperConfiguration(cfg =>
                {
                    var mappingExpression = cfg.CreateMap<Tsource, Ttarget>();
                    action?.Invoke(mappingExpression);
                });
                var mapper = config.CreateMapper();
                Ttarget result = mapper.Map<Tsource, Ttarget>(items[i]);
                results.Add(result);
            }
            return results;
        }
    }
}
