using LibraryManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Training.Entities
{
    public class User : IEntity<Guid>
    {
        public Guid Id { get; set; }

        public Guid UserAccount { get; set; }
        public string? UserName { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? AccountNumber { get; set; }
    }
}
