import { useQuery } from "@tanstack/react-query";
import { queryProducts, type ProductsQueryParams } from "../api/queryProducts";

export function useQueryProducts(params: ProductsQueryParams, enabled = true) {
  return useQuery({
    queryKey: ["products", "query", params],
    queryFn: () => queryProducts(params),
    enabled,
  });
}
