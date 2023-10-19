using LibraryManagement.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Training.Entities
{
    public class Book : IEntity<Guid>
    {
        public Guid Id { get; set; }
        
        public string? ISBN { get; set; }
        public string? Book_Title { get; set; }
        public string? Publication_year { get; set; }
        public string? Language { get; set; }

        public string? Category_Type { get; set; }

        public string? Binding_Id { get; set; }

        public string? No_Of_Copies_Actual { get; set; }
        public string? No_Of_Copies_Current { get; set; }
        //public User? UserId { get; set; }

        public string? ActionType { get; set; }
        public DateTime ActionDate { get; set; }

    }
}
