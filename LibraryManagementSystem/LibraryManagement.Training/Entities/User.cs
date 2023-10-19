using LibraryManagement.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Training.Entities
{
    public class User : IEntity<Guid>
    {
        public Guid Id {  get; set; }
        [Required]
        public string? UserName { get; set; }
        [Required]
        public int Gender { get; set; }
        public DateTime BirthDate { get; set; }
        public string? ContactNumber { get; set; }
        public string? EmergencyContactNumber { get; set; }
        public string? BloodGroup { get; set; }
        //public List<Book> BookBorrowed { get; set; }
        public string? ActionType { get; set; }
        public DateTime ActionDate { get; set; }

    }
}
