using DevexOdata.Models;
using System.Linq;
using System.Web.Http;

namespace DevexOdata.Controllers.WebApi
{
    public class CustomerController : ApiController
    {
        public IHttpActionResult GetAll()
        {
            ApplicationDbContext db = new ApplicationDbContext();
            return Ok(new
            {
                success = true,
                data = db.Customers.ToList()
            });
        }
    }
}
