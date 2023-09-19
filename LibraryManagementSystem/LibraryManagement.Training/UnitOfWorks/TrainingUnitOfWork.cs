using LibraryManagement.Data;
using LibraryManagement.Training.Contexts;
using LibraryManagement.Training.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Training.UnitOfWorks
{
    public class TrainingUnitOfWork : UnitOfWork, ITrainingUnitOfWork
    {
        public IUserRepository Users { get; private set; }
        public TrainingUnitOfWork(TrainingContext context, IUserRepository users):base(context)
        {
            Users = users;
        }
    }
}
