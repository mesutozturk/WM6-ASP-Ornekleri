using System;

namespace Admin.Web.UI.App_Code
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method |
                    AttributeTargets.Property | AttributeTargets.Field,
        Inherited = true)]
    public class InheritedAttribute : Attribute
    { }
}