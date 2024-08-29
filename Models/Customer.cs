using System.ComponentModel.DataAnnotations;

namespace CustomerWeb.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string FullName { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Birthdate { get; set; }

        [Required]
        public string Gender { get; set; }

        public List<ContactNumber> ContactNumbers { get; set; } = new List<ContactNumber>();

        public List<Address> Addresses { get; set; } = new List<Address>();
    }
}
