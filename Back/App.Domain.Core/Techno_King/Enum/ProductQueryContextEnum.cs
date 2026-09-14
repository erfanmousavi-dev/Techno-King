using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace App.Domain.Core.Techno_King.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProductQueryContextEnum
    {
        AllProducts = 1,
        Brand = 2,
        SubCategory = 3,
        Search = 4,
        Discounted = 5,
        HighDiscount = 6
    }
}
