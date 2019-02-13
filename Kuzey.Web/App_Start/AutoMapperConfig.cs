using AutoMapper;
using Kuzey.Models.Entities;
using Kuzey.Models.ViewModels;

namespace Kuzey.Web.App_Start
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.Initialize(cfg =>
            {
                CategoryMapping(cfg);
                ProductMapping(cfg);
                EmployeeMapping(cfg);
            });
        }

        private static void EmployeeMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Employee, EmployeeViewModel>()
                .ForMember(dest => dest.SubEmplyeeCount, opt => opt.MapFrom(x => x.Employees1.Count))
                .ForMember(dest => dest.ReportsName, opt => opt.MapFrom((s, d) => s.Employee1?.FirstName + " " + s.Employee1?.LastName))
                .ReverseMap();
        }

        private static void ProductMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Product, ProductViewModel>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom((s, d) => s.Category == null ? "Kategorisiz" : s.Category.CategoryName))
                .ReverseMap();
        }

        private static void CategoryMapping(IMapperConfigurationExpression cfg)
        {
            cfg.CreateMap<Category, CategoryViewModel>()
                .ForMember(dest => dest.ProductCount, opt => opt.MapFrom(x => x.Products.Count))
                .ReverseMap();
        }
    }
}