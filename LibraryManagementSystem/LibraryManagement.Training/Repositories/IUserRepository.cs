using LibraryManagement.Data;
using LibraryManagement.Training.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Training.Repositories
{
    public interface IUserRepository : IRepository<User, Guid>
    {
    }
}
