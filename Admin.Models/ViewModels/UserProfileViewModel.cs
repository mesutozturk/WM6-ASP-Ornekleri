using System.ComponentModel.DataAnnotations;
using System.Web;

namespace Admin.Models.ViewModels
{
    public class UserProfileViewModel
    {
        public string Id { get; set; }
        [Required]
        [Display(Name = "Ad")]
        [StringLength(25)]
        public string Name { get; set; }
        [StringLength(35)]
        [Required]
        [Display(Name = "Soyad")]
        public string Surname { get; set; }
        [Required]
        [Display(Name = "Kullanıcı Adı")]
        public string UserName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Display(Name = "Telefon No.")]
        public string PhoneNumber { get; set; }

        public string AvatarPath { get; set; }
        public HttpPostedFileBase PostedFile { get; set; }
    }
}
