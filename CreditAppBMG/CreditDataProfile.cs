using AutoMapper;
using CreditAppBMG.Entities;
using CreditAppBMG.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreditAppBMG
{
    public class CreditDataProfile : Profile
    {
        public CreditDataProfile()
   : this("MyProfile")
        {
        }
        protected CreditDataProfile(string profileName)
        : base(profileName)
        {
            CreateMap<CreditDataEntity, CreditData>().ForMember(x => x.CompanyTypeName, opt => opt.UseValue("zzz"));
            CreateMap<CreditData,CreditDataEntity>().ForSourceMember(x => x.CompanyTypeName, opt => opt.Ignore());
            
        }
    }
}
