using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rabbit.Models.Entities
{
    [Table("MailLogs")]
   public class MailLog
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Message { get; set; }
        public string Subject { get; set; }
        public Guid CustomerId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
    }
}
