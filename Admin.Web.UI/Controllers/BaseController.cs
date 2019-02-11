using System;
using Admin.BLL.Repository;
using Admin.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Admin.BLL.Identity;
using Admin.Models.Enums;

namespace Admin.Web.UI.Controllers
{
    [Authorize]
    [RequireHttps]
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

        protected List<SelectListItem> GetProductSelectList()
        {
            var products = new ProductRepo()
                .GetAll(x => x.SupProductId == null && x.ProductType == ProductTypes.Retail)
                .OrderBy(x => x.ProductName);
            var list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Perakende Ürünü Yok",
                    Value = new Guid().ToString()
                }
            };
            foreach (var product in products)
            {
                if (product.Products.Any(x => x.ProductType == ProductTypes.Retail))
                {
                    list.Add(new SelectListItem()
                    {
                        Text = product.ProductName,
                        Value = product.Id.ToString()
                    });
                    list.AddRange(GetSubProducts(product.Products.Where(x => x.ProductType == ProductTypes.Retail).OrderBy(x => x.ProductName).ToList()));
                }
                else
                {
                    list.Add(new SelectListItem()
                    {
                        Text = product.ProductName,
                        Value = product.Id.ToString()
                    });
                }
            }

            List<SelectListItem> GetSubProducts(List<Product> products2)
            {
                var list2 = new List<SelectListItem>();
                foreach (var product in products2)
                {
                    if (product.Products.Any(x => x.ProductType == ProductTypes.Retail))
                    {
                        list2.Add(new SelectListItem()
                        {
                            Text = product.ProductName,
                            Value = product.Id.ToString()
                        });
                        list2.AddRange(GetSubProducts(product.Products.Where(x => x.ProductType == ProductTypes.Retail).OrderBy(x => x.ProductName).ToList()));
                    }
                    else
                    {
                        list2.Add(new SelectListItem()
                        {
                            Text = product.ProductName,
                            Value = product.Id.ToString()
                        });
                    }
                }
                return list2;
            }

            return list;
        }

        protected List<SelectListItem> GetUserList()
        {
            var data = new List<SelectListItem>();
            MembershipTools.NewUserStore().Users
                .ToList()
                .ForEach(x =>
                {
                    data.Add(new SelectListItem()
                    {
                        Text = $"{x.Name} {x.Surname}",
                        Value = x.Id
                    });
                });
            return data;
        }

        protected List<SelectListItem> GetRoleList()
        {
            var data = new List<SelectListItem>();
            MembershipTools.NewRoleStore().Roles
                .ToList()
                .ForEach(x =>
                {
                    data.Add(new SelectListItem()
                    {
                        Text = $"{x.Name}",
                        Value = x.Id
                    });
                });
            return data;
        }
    }
}