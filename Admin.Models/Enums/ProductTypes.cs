using System.ComponentModel;

namespace Admin.Models.Enums
{
    public enum ProductTypes
    {
        [Description("Toptan")]
        Bulk = 10,
        [Description("Perakende")]
        Retail = 100
    }
}
