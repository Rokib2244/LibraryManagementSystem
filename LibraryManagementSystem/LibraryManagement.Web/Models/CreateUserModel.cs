using System.ComponentModel.DataAnnotations;

namespace LibraryManagement.Web.Models
{
    public class CreateUserModel
    {
        public Guid Id { get; set; }
        [Required, MaxLength(60, ErrorMessage = "Length should be within 60 character")]
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
