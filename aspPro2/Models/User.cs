using System.ComponentModel.DataAnnotations;

namespace aspPro2.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? Name { get; set; }

        public String? LastName { get; set; }

        public String? password { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public String? Email { get; set; }

        public List<Enrollment> Enrollments { get; set; }
    }
}
