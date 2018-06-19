using AdHr.Models;
using AutoMapper;
using LDAPLibrary.Interfarces;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AdHr.Profiles
{
    public class LdapUserProfile : Profile
    {
        public LdapUserProfile()
        {
            CreateMap<ILdapUser, ReadLdapUserResponse>()
                .ForMember(d => d.UserCn, o => o.MapFrom(s => s.GetUserCn()))
                .ForMember(d => d.UserSn, o => o.MapFrom(s => s.GetUserSn()))
                .ForMember(d => d.UserDn, o => o.MapFrom(s => s.GetUserDn()))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.GetUserAttribute("description")))
                ;

            CreateMap<IList<ILdapUser>, List<ReadLdapUserResponse>>()
                ;

            CreateMap<ILdapUser, ResponseBase<ReadLdapUserResponse>>()
                .ForMember(d => d.Data, o => o.MapFrom(s => s))
                //todo: az attributumokat is áthozni
                ;

            var cfg = new MapperConfiguration(c =>
            {
                c.CreateMap<ILdapUser, ReadLdapUserResponse>()
                .ForMember(d => d.UserCn, o => o.MapFrom(s => s.GetUserCn()))
                .ForMember(d => d.UserSn, o => o.MapFrom(s => s.GetUserSn()))
                .ForMember(d => d.UserDn, o => o.MapFrom(s => s.GetUserDn()))
                .ForMember(d => d.Description, o => o.MapFrom(s => s.GetUserAttribute("description")))
                    ;
                c.CreateMap<IList<ILdapUser>, List<ReadLdapUserResponse>>()
                    ;
            });
            var mapper = cfg.CreateMapper();

            CreateMap<IList<ILdapUser>, ResponseBase<IReadOnlyCollection<ReadLdapUserResponse>>>()
                .ForMember(d => d.Data, o => o.MapFrom(
                        s => new ReadOnlyCollection<ReadLdapUserResponse>(
                                mapper.Map<IList<ILdapUser>, List<ReadLdapUserResponse>>(s))))
                //todo: az attributumokat is áthozni
                ;

        }
    }
}
