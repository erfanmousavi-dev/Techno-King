import { apiClient } from "../../../shared/api/client";

export type ProductsQueryContext =
  | "AllProducts"
  | "Brand"
  | "SubCategory"
  | "Search"
  | "Discounted"
  | "HighDiscount";

export type ProductsQueryParams = {
  context?: ProductsQueryContext;
  brand?: string;
  subCategoryId?: number;
  searchTerm?: string;
  sortBy?: "Price" | "Rating" | "SalesCount" | "DiscountPercentage" | "CreatedAt";
  ascending?: boolean;
  pageSize?: number;
};

export async function queryProducts(params: ProductsQueryParams) {
  const { data } = await apiClient.get("/api/Products/Query", {
    params: {
      Context: params.context,
      Brand: params.brand,
      SubCategoryId: params.subCategoryId,
      SearchTerm: params.searchTerm,
      SortBy: params.sortBy,
      Ascending: params.ascending,
      PageSize: params.pageSize,
    },
  });
  return data;
}