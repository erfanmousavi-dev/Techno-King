using App.Domain.Core.Techno_King.App.Domain.Core;
using App.Domain.Core.Techno_King.DTOs.Products;
using App.Domain.Core.Techno_King.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techno_KingAppService.Techno_King.Products
{
    public class ProductQueryOrchestrationAppService : IProductQueryOrchestrationAppService
    {
        #region DI
        private readonly IProductQueryOrchestrationService _productQueryAppService;
        public ProductQueryOrchestrationAppService(IProductQueryOrchestrationService productQueryAppService)
        {
            _productQueryAppService = productQueryAppService;
        }
        #endregion
        public async Task<List<ProductDTOs>> GetProductsAsync(ProductPageRequestDTO request, CancellationToken cancellationToken)
        {
            return await _productQueryAppService.GetProductsAsync(request, cancellationToken);
        }
    }
}
