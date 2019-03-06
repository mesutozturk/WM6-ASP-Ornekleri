using Rabbit.Models.Entities;
using System;

namespace Rabbit.BLL.Repository
{
    public class CustomerRepo : RepositoryBase<Customer, Guid> { }
    public class MailLogRepo : RepositoryBase<MailLog, Guid> { }
}
