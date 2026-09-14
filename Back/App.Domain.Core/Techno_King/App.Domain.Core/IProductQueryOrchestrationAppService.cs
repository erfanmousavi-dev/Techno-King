using App.Domain.Core.Techno_King.DTOs.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Domain.Core.Techno_King.App.Domain.Core
{
    public interface IProductQueryOrchestrationAppService
    {
        Task<List<ProductDTOs>> GetProductsAsync(ProductPageRequestDTO request, CancellationToken cancellationToken);
    }
}
