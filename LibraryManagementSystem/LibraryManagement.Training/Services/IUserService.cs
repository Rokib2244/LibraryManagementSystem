using LibraryManagement.Training.BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Training.Services
{
    public interface IUserService
    {
        void CreateUser(Member user);
        IList<Member> GetUsers();
    }
}
