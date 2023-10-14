using AutoMapper;
using LibraryManagement.Training.BusinessObjects;
using LibraryManagement.Training.UnitOfWorks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Training.Services
{
    public class UserService : IUserService
    {
        private readonly ITrainingUnitOfWork _trainingUnitOfWork;
        private readonly IMapper _mapper;
        public UserService(ITrainingUnitOfWork trainingUnitOfWork, IMapper mapper)
        {
            _trainingUnitOfWork = trainingUnitOfWork;
            _mapper = mapper;   
        }

        public void CreateUser(Member user)
        {
            throw new NotImplementedException();
            if(user == null)
            {
                throw new InvalidOperationException("User was not found");
            }
            _trainingUnitOfWork.Users.Add(_mapper.Map<Entities.Member>(user));
            _trainingUnitOfWork.Save();
        }

        public IList<Member> GetUsers()
        {
           var userData= _trainingUnitOfWork.Users.GetAll();
            var resultData = (from user in userData select _mapper.Map<Member>(user)).ToList();
            return resultData;
            //throw new NotImplementedException();
        }
    }
}
