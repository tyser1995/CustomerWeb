using System.ComponentModel.DataAnnotations;

namespace CustomerWeb.Models
{
    public class ContactNumber
    {
        public int Id { get; set; }

        [Required]
        public string Type { get; set; }

        [Required]
        public string Number { get; set; }
        public bool IsDeleted { get; set; }
    }
}
