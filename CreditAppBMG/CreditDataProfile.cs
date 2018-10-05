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
            CreateMap<CreditDataEntity, CreditData>()
                .ForMember(x => x.CompanyTypeName, opt => opt.Ignore())
                .ForSourceMember(x => x.CreditDataFiles, opt => opt.Ignore());

            CreateMap<CreditData, CreditDataEntity>()
                .ForSourceMember(x => x.CompanyTypeName, opt => opt.Ignore())
                .ForMember(x => x.CreditDataFiles, opt => opt.Ignore());

            CreateMap<CreditDataFilesEntity, CreditDataFiles>()
                .ForMember(x => x.LicenseFileMessage, opt => opt.Ignore());

            CreateMap<CreditDataFiles, CreditDataFilesEntity>()
                .ForSourceMember(x => x.LicenseFileMessage, opt => opt.Ignore());
        }
    }
}
