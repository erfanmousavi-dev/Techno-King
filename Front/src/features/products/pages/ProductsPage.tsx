import { FaSortAmountDown } from "react-icons/fa";
import { NavLink, useSearchParams } from "react-router-dom";
import ProductCardSkeleton from "../components/ProductCardSkeleton";
import { useProductsPageQuery } from "../hooks/useProductsPageQuery";
import { BASE_URL } from "../../../shared/api/base";

const sortOptions = [
  { label: "Best selling", sortBy: "SalesCount", ascending: false },
  { label: "Highest score", sortBy: "Rating", ascending: false },
  { label: "Most expensive", sortBy: "Price", ascending: false },
  { label: "The cheapest", sortBy: "Price", ascending: true },
  { label: "Latest", sortBy: "CreatedAt", ascending: false },
];

const ProductsPage = () => {
  const { products, isLoading, isError } = useProductsPageQuery();
  const [searchParams] = useSearchParams();

  const buildSortLink = (sortBy: string, ascending: boolean) => {
    const params = new URLSearchParams(searchParams);
    params.set("sortBy", sortBy);
    params.set("ascending", String(ascending));
    return `/products?${params.toString()}`;
  };

  return (
    <div className="my-10 flex justify-between sm:px-8 md:px-[4rem] lg:px-[6.8rem]">
      <div className="h-[500px] w-1/5 bg-red-600"></div>

      <div className="w-4/5">
        <div className="flex gap-5">
          <div className="flex items-center gap-2">
            <FaSortAmountDown />
            <span>sorting:</span>
          </div>

          <ul className="flex gap-5">
            {sortOptions.map((opt) => {
              const isActive =
                searchParams.get("sortBy") === opt.sortBy &&
                searchParams.get("ascending") === String(opt.ascending);

              return (
                <li key={opt.label}>
                  <NavLink
                    to={buildSortLink(opt.sortBy, opt.ascending)}
                    className={isActive ? "font-bold text-primary" : ""}
                  >
                    {opt.label}
                  </NavLink>
                </li>
              );
            })}
          </ul>
        </div>

        <div className="mt-5 grid grid-cols-1 gap-4 md:grid-cols-3 lg:grid-cols-4 lg:gap-5">
          {isError && (
            <p className="col-span-full text-red-600">خطا در دریافت محصولات</p>
          )}

          {isLoading &&
            Array.from({ length: 8 }).map((_, index) => (
              <ProductCardSkeleton key={index} />
            ))}

          {!isLoading &&
            products?.map((item) => (
              <div
                key={item.id}
                className="flex h-full flex-col rounded-xl bg-white p-6 shadow-md"
              >
                <div className="flex aspect-[4/3] w-full items-center justify-center">
                  <img
                    src={`${BASE_URL}/${item.imageUrl1}`}
                    alt={item.name}
                    className="h-full w-full object-contain"
                  />
                </div>
                <span className="mt-4 line-clamp-2">{item.name}</span>
                <span className="mt-auto pt-2 font-semibold">
                  ${item.price}
                </span>
              </div>
            ))}
        </div>
      </div>
    </div>
  );
};

export default ProductsPage;