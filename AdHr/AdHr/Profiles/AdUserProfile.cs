using AdHr.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdHr.Profiles
{
    public class AdUserProfile : Profile
    {
        public AdUserProfile()
        {
            CreateMap<AdhrUser, ReadLdapUserResponse>()
                .ForMember(d=>d.UserCn, o=>o.Ignore()) 
                ;

            CreateMap<AdhrUser, ResponseBase<ReadLdapUserResponse>>()
                .ForMember(d => d.Data, o => o.MapFrom(s => s))
                ;

            CreateMap<List<AdhrUser>, List<ReadLdapUserResponse>>()
                ;

            var cfg1 = new MapperConfiguration(c =>
            {
                c.CreateMap<AdhrUser, ReadLdapUserResponse>()
                    ;
            });
            var mapper1 = cfg1.CreateMapper();

            CreateMap<List<AdhrUser>, ResponseBase<IReadOnlyCollection<ReadLdapUserResponse>>>()
                .ForMember(d => d.Data, 
                            o => o.MapFrom(
                                s => new ReadOnlyCollection<ReadLdapUserResponse>(
                                    mapper1.Map<List<AdhrUser>, List<ReadLdapUserResponse>>(s)
                                )
                            )
                )
                ;
        }
    }
}
