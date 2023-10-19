using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EO = LibraryManagement.Training.Entities;
using BO = LibraryManagement.Training.BusinessObjects;

namespace LibraryManagement.Training.Profiles
{
    public class TrainingProfile : Profile
    {
        public TrainingProfile()
        {
            CreateMap<EO.User, BO.User>();
           // CreateMap<EO.User, BO.User>().ForMember(x => x.UserAccount, opt => opt.Ignore());
        }
    }
}
