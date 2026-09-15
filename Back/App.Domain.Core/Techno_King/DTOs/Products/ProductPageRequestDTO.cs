using App.Domain.Core.Techno_King.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Techno_King.DTOs.Products
{
    public class ProductPageRequestDTO
    {
            public ProductQueryContextEnum Context { get; set; } = ProductQueryContextEnum.AllProducts;
            public string? Brand { get; set; }
            public int? SubCategoryId { get; set; }
            public string? SearchTerm { get; set; }

            public ProductSortType SortBy { get; set; } = ProductSortType.CreatedAt;
            public bool Ascending { get; set; } = true;

            public int Page { get; set; } = 1;
            public int PageSize { get; set; } = 20;
        
    }
}
