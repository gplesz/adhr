using AdHr.Models;
using AdHr.ViewModels;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdHr.Profiles
{
    public class ViewModelProfile : Profile
    {
        public ViewModelProfile()
        {

            CreateMap<AdhrValue, AdhrValueViewModel>()
                ;

            CreateMap<KeyValuePair<string, IReadOnlyCollection<AdhrValue>>, AdhrPropertyViewModel>()
                .ForMember(d => d.Name, o => o.MapFrom(s => s.Key))
                .ForMember(d => d.Values, o => o.MapFrom(s => s.Value))
                ;

            //CreateMap<Dictionary<string,IReadOnlyCollection<string>>, ObservableCollection<AdhrPropertyViewModel>>();

            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<AdhrValue, AdhrValueViewModel>()
                    ;

                c.CreateMap<KeyValuePair<string, IReadOnlyCollection<AdhrValue>>, AdhrPropertyViewModel>()
                    .ForMember(d => d.Name, o => o.MapFrom(s => s.Key))
                    .ForMember(d => d.Values, o => o.MapFrom(s => s.Value))
                    ;
            });
            var mapper = cfg.CreateMapper();

            CreateMap<ReadLdapUserResponse, AdhrUserViewModel>()
                .ForMember(d => d.Properties, o => o.MapFrom(d => ToObservableList(d.Properties, mapper)))
                ;

        }

        private static ObservableCollection<AdhrPropertyViewModel> ToObservableList(Dictionary<string, IReadOnlyCollection<AdhrValue>> list , IMapper mapper)
        {
            var result = mapper.Map<List<AdhrPropertyViewModel>>(list);
            var roresult = new ObservableCollection<AdhrPropertyViewModel>(result);
            return roresult;
        }
    }
}
