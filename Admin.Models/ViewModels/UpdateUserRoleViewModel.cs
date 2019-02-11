using System.Collections.Generic;

namespace Admin.Models.ViewModels
{
    public  class UpdateUserRoleViewModel
    {
        public string Id { get; set; }
        public List<string> Roles { get; set; }
    }
}
