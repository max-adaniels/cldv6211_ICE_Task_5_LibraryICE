using System.ComponentModel.DataAnnotations;

namespace mvcLibrary.Models
{
    public class BookType
    {
        [Key]
        public int TypeID { get; set; }
        public string? Type { get; set; }
    }
}
