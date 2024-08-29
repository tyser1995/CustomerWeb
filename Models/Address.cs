using System.ComponentModel.DataAnnotations;

namespace CustomerWeb.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        public string Barangay { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string Province { get; set; }
        public bool IsDeleted { get; set; }
    }
}
