using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Rabbit.Models.Entities
{
    [Table("Customers")]
    public class Customer
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, StringLength(50)]
        public string Name { get; set; }
        [Required, StringLength(50)]
        public string Surname { get; set; }
        [Required, StringLength(11)]
        public string Phone { get; set; }
        [Required, StringLength(55)]
        public string Email { get; set; }
        [Required, StringLength(150)]
        public string Address { get; set; }

        public DateTime RegisterDate { get; set; } = DateTime.Now;


        public virtual ICollection<MailLog> MailLogs { get; set; }=new HashSet<MailLog>();
    }
}
