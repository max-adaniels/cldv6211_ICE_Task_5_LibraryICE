using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace mvcLibrary.Models
{
    public class Loan
    {
        [Key]
        public int LoanID { get; set; }
        public DateTime? LoanDate { get; set; }
        public Book? Book { get; set; }
        public int BookID { get; set; }
    }
}
