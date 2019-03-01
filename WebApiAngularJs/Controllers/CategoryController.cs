using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApiAngularJs.Models;

namespace WebApiAngularJs.Controllers
{
    public class CategoryController : ApiController
    {
        MyCon db = new MyCon();
        [HttpGet]
        public IHttpActionResult GetAll()
        {
            try
            {
                return Ok(new
                {
                    success = true,
                    data = db.Categories.Select(x => new CategoryViewModel()
                    {
                        CategoryID = x.CategoryID,
                        CategoryName = x.CategoryName,
                        Description = x.Description
                    }).ToList()
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Bir hata oluştu {ex.Message}");
            }
        }

        [HttpGet]
        public IHttpActionResult Get(int id = 0)
        {
            try
            {
                var cat = db.Categories.Find(id);
                if (cat == null)
                {
                    return NotFound();
                }

                var data = new CategoryViewModel()
                {
                    CategoryID = cat.CategoryID,
                    CategoryName = cat.CategoryName,
                    Description = cat.Description
                };
                return Ok(new
                {
                    success = true,
                    data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Bir hata oluştu {ex.Message}");
            }
        }

        [HttpPost]
        public IHttpActionResult Add([FromBody]CategoryViewModel model)
        {
            try
            {
                db.Categories.Add(new Category()
                {
                    CategoryName = model.CategoryName,
                    Description = model.Description,
                });
                db.SaveChanges();
                return Ok(new
                {
                    success = true,
                    message = "Kategori ekleme işlemi başarılı"
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Bir hata oluştu {ex.Message}");
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id = 0)
        {
            try
            {
                db.Categories.Remove(db.Categories.Find(id));
                db.SaveChanges();
                return Ok(new
                {
                    success = true,
                    message = "Kategori silme işlemi başarılı"
                });
            }
            catch (Exception ex)
            {
                return BadRequest($"Bir hata oluştu {ex.Message}");
            }
        }

        [HttpPut]
        public IHttpActionResult PutCategory(int id, Category model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != model.CategoryID)
            {
                return BadRequest();
            }

            db.Entry(model).State = EntityState.Modified;

            try
            {
                db.SaveChanges();
                return Ok(new
                {
                    success = true,
                    message = "Kategori Güncelleme işlemi başarılı"
                });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!db.Categories.Any(x => x.CategoryID == id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }
    }

    public class CategoryViewModel
    {
        public int CategoryID { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
