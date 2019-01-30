using System.ComponentModel;

namespace Admin.Models.Enums
{
    public enum OrderTypes
    {
        [Description("Alış")]
        Buying = 10,
        [Description("Satış")]
        Selling = 20
    }
}
