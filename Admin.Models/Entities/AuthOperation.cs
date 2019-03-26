using Admin.Models.Abstracts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Admin.Models.Entities
{
    [Table("AuthOperations")]
    public class AuthOperation : BaseEntity<Guid>
    {
        public AuthOperation()
        {
            this.Id = Guid.NewGuid();
        }

        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public virtual ICollection<AuthOperationRole> AuthOperationRoles { get; set; } = new HashSet<AuthOperationRole>();
    }
}
