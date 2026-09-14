using App.Domain.Core.Techno_King.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Techno_King.DTOs.Products
{
    public class ProductQueryParamsDTO
    {
        public string? Brand { get; set; }
        public int? SubCategoryId { get; set; }
        public string? Name { get; set; }
        public int? MinDiscount { get; set; }
        public int? MaxDiscount { get; set; }
        public ProductSortType? SortBy { get; set; }
        public bool Ascending { get; set; } = true;
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
