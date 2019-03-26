using Admin.Models.Abstracts;
using System;
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

        public string Controller { get; set; }
        public string Action { get; set; }
        public string Attributes { get; set; }
        public string ReturnType { get; set; }

        public virtual ICollection<AuthOperationRole> AuthOperationRoles { get; set; } = new HashSet<AuthOperationRole>();
        public override string ToString()
        {
            return $"{Controller} {Action} {Attributes} {ReturnType}";
        }
    }
}
