import { useSearchParams } from "react-router-dom";
import { useQueryProducts } from "./useQueryProducts";
import type {
  ProductsQueryContext,
  ProductsQueryParams,
} from "../api/queryProducts";

export function useProductsPageQuery() {
  const [searchParams] = useSearchParams();

  const brand = searchParams.get("brand") ?? undefined;
  const subCategoryIdRaw = searchParams.get("subcategory");
  const searchTerm = searchParams.get("q") ?? undefined;
  const sortBy =
    (searchParams.get("sortBy") as ProductsQueryParams["sortBy"]) ?? undefined;
  const ascendingRaw = searchParams.get("ascending");
  const ascending = ascendingRaw ? ascendingRaw === "true" : undefined;

  let context: ProductsQueryContext = "AllProducts";
  if (searchTerm) context = "Search";
  else if (brand) context = "Brand";
  else if (subCategoryIdRaw) context = "SubCategory";

  const { data, isLoading, isError } = useQueryProducts({
    context,
    brand,
    subCategoryId: subCategoryIdRaw ? Number(subCategoryIdRaw) : undefined,
    searchTerm,
    sortBy,
    ascending,
  });

  return { products: data, isLoading, isError };
}
