using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryManagement.Data
{
    public interface IEntity<T>
    {
         T Id { get; set; }
        string ActionType { get; set; }
        DateTime ActionDate { get; set; }

    }
}
