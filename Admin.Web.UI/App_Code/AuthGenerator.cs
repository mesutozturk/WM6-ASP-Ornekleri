using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Admin.BLL.Repository;
using Admin.Models.Entities;
using Admin.Web.UI.Controllers;
using static Admin.Web.UI.App_Code.VType;

namespace Admin.Web.UI.App_Code
{
    public class AuthGenerator
    {
        private List<Type> _derrivedTypes;
        public static List<AuthGenerator> AuthGenerators { get; set; }
        public AuthGenerator()
        {
            this.Generate();
        }

        void Generate()
        {
            var baseClass = typeof(BaseController);
            Assembly asm = Assembly.GetAssembly(typeof(Admin.Web.UI.MvcApplication));

            var controllerActionList = asm.GetTypes()
                .Where(type => typeof(System.Web.Mvc.Controller).IsAssignableFrom(type))
                .SelectMany(type => type.GetMethods(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public))
                .Where(m => !m.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), true).Any())
                .Select(x => new AuthOperation
                {
                    Controller = x.DeclaringType.Name,
                    Action = x.Name,
                    ReturnType = x.ReturnType.Name,
                    Attributes = String.Join(",", x.GetCustomAttributes().Select(a => a.GetType().Name.Replace("Attribute", "")))
                })
                .OrderBy(x => x.Controller).ThenBy(x => x.Action).ToList();

            var repo = new AuthOperatonRepo();
            foreach (var item in controllerActionList)
            {
                try
                {
                    //repo.Insert(item);
                }
                catch (Exception e)
                {
                    continue;
                }
            }

            Console.WriteLine();
        }
    }
    public static class VType
    {
        public static List<Type> GetDerivedTypes(Type baseType, Assembly assembly)
        {
            // Get all types from the given assembly
            Type[] types = assembly.GetTypes();
            List<Type> derivedTypes = new List<Type>();

            for (int i = 0, count = types.Length; i < count; i++)
            {
                Type type = types[i];
                if (IsSubclassOf(type, baseType))
                {
                    // The current type is derived from the base type,
                    // so add it to the list
                    derivedTypes.Add(type);
                }
            }

            return derivedTypes;
        }

        public static bool IsSubclassOf(Type type, Type baseType)
        {
            if (type == null || baseType == null || type == baseType)
                return false;

            if (baseType.IsGenericType == false)
            {
                if (type.IsGenericType == false)
                    return type.IsSubclassOf(baseType);
            }
            else
            {
                baseType = baseType.GetGenericTypeDefinition();
            }

            type = type.BaseType;
            Type objectType = typeof(object);

            while (type != objectType && type != null)
            {
                Type curentType = type.IsGenericType ?
                    type.GetGenericTypeDefinition() : type;
                if (curentType == baseType)
                    return true;

                type = type.BaseType;
            }

            return false;
        }
    }
}