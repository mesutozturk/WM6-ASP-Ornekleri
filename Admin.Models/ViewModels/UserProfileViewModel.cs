using System.ComponentModel.DataAnnotations;

namespace Admin.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        [StringLength(50)]
        [Required]
        public string Name { get; set; }
        [StringLength(60)]
        [Required]
        public string Surname { get; set; }
        public string PhoneNumber { get; set; }
        public string UserName { get; set; }
    }
}
