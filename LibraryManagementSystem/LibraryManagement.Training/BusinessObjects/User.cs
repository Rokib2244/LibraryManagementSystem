using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Training.BusinessObjects
{
    public class User
    {
        public string? UserName { get; set; }
        public int Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string ContactNumber { get; set; }
        public string EmergencyContactNumber { get; set; }
        public string BloodGroup { get; set; }
        public string ActionType { get; set; }
        public DateTime ActionDate { get; set; }
    }
}
