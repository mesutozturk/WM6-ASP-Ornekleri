using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Admin.Models.Abstracts;
using Admin.Models.IdentityModels;

namespace Admin.Models.Entities
{
    [Table("AuthOperationRoles")]
    public class AuthOperationRole : BaseEntity2<Guid, string>
    {
        [ForeignKey("Id")]
        public virtual AuthOperation AuthOperation { get; set; }
        [ForeignKey("Id2")]
        public virtual Role Role { get; set; }
    }
}
