using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using Admin.Web.UI.Controllers;
using static Admin.Web.UI.App_Code.VType;

namespace Admin.Web.UI.App_Code
{
    public class AuthGenerator
    {
        private List<Type> _derrivedTypes;
        public Dictionary<string, List<string>> ActionLists { get; set; } = new Dictionary<string, List<string>>();
        public AuthGenerator()
        {
            this.Generate();
        }

        void Generate()
        {
            var baseClass = typeof(BaseController);
            _derrivedTypes = GetDerivedTypes(typeof(BaseController),
                Assembly.GetExecutingAssembly());
            foreach (var derrivedType in _derrivedTypes)
            {
                Console.WriteLine(derrivedType);
                var actionList = new List<string>();
                foreach (var item in derrivedType.GetMembers())
                {
                    if (item.DeclaringType == null) continue;


                    if (item.MemberType == MemberTypes.Method && item.DeclaringType.IsPublic)
                    {
                        MethodInfo aaa = item.DeclaringType.GetMethod(item.Name);
                        if (aaa.ReturnType.FullName == typeof(ActionResult).FullName || IsSubclassOf(aaa.ReturnType, typeof(ActionResult)))
                        {

                        }
                    }
                }
                //ActionLists.Add(derrivedType.Name,);
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