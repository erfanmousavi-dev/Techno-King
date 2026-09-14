using App.Domain.Core.Techno_King.Data.Repositories;
using App.Domain.Core.Techno_King.DTOs.Products;
using App.Domain.Core.Techno_King.Service;
using App.Domain.Core.Techno_King.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Techno_KingService.Techno_King.Products
{
    public class ProductQueryOrchestrationService : IProductQueryOrchestrationService
    {
        #region Dependency Injection
        private readonly IProductRepository _productRepository;
        public ProductQueryOrchestrationService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        #endregion

        public async Task<List<ProductDTOs>> GetProductsAsync(ProductPageRequestDTO request, CancellationToken cancellationToken)
        {
            var queryParams = BuildQueryParams(request);
            return await _productRepository.GetProductsAsync(queryParams, cancellationToken);
        }

        #region Decision Logic
        private ProductQueryParamsDTO BuildQueryParams(ProductPageRequestDTO request)
        {
            var queryParams = new ProductQueryParamsDTO
            {
                SortBy = request.SortBy,
                Ascending = request.Ascending,
                Page = request.Page,
                PageSize = request.PageSize
            };

            switch (request.Context)
            {
                case ProductQueryContextEnum.Brand:
                    queryParams.Brand = request.Brand;
                    break;

                case ProductQueryContextEnum.SubCategory:
                    queryParams.SubCategoryId = request.SubCategoryId;
                    break;

                case ProductQueryContextEnum.Search:
                    queryParams.Name = request.SearchTerm;
                    break;

                case ProductQueryContextEnum.Discounted:
                    queryParams.MinDiscount = 1;
                    break;

                case ProductQueryContextEnum.HighDiscount:
                    queryParams.MinDiscount = 15;
                    break;

                case ProductQueryContextEnum.AllProducts:
                default:
                    break;
            }

            return queryParams;
        }
        #endregion
    }
}
