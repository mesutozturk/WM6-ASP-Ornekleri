using Microsoft.AspNet.Identity.EntityFramework;
using System.ComponentModel.DataAnnotations;

namespace Admin.Models.IdentityModels
{
    public class Role : IdentityRole
    {
        public Role()
        {

        }

        public Role(string description)
        {
            Description = description;
        }
        [StringLength(100)]
        public string Description { get; set; }
    }
}
