using Admin.BLL.Repository;
using Admin.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Admin.Web.UI.Controllers
{
    public class BaseController : Controller
    {
        protected List<SelectListItem> GetCategorySelectList()
        {
            var categories = new CategoryRepo()
                .GetAll(x => x.SupCategoryId == null)
                .OrderBy(x => x.CategoryName);
            var list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Üst Kategorisi Yok",
                    Value = "0"
                }
            };
            foreach (var category in categories)
            {
                if (category.Categories.Any())
                {
                    list.Add(new SelectListItem()
                    {
                        Text = category.CategoryName,
                        Value = category.Id.ToString()
                    });
                    list.AddRange(GetSubCategories(category.Categories.OrderBy(x => x.CategoryName).ToList()));
                }
                else
                {
                    list.Add(new SelectListItem()
                    {
                        Text = category.CategoryName,
                        Value = category.Id.ToString()
                    });
                }
            }

            List<SelectListItem> GetSubCategories(List<Category> categories2)
            {
                var list2 = new List<SelectListItem>();
                foreach (var category in categories2)
                {
                    if (category.Categories.Any())
                    {
                        list2.Add(new SelectListItem()
                        {
                            Text = category.CategoryName,
                            Value = category.Id.ToString()
                        });
                        list2.AddRange(GetSubCategories(category.Categories.OrderBy(x => x.CategoryName).ToList()));
                    }
                    else
                    {
                        list2.Add(new SelectListItem()
                        {
                            Text = category.CategoryName,
                            Value = category.Id.ToString()
                        });
                    }
                }
                return list2;
            }

            return list;
        }
    }
}