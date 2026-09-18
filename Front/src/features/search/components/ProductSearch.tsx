import SearchIcon from "/icons/search-2.svg";
import closehIcon from "/icons/close-circle.svg";
import SearchPanelSugestions from "./SearchPanelSuggestions";
import SearchPanelResults from "./SearchPanelResults";
import { useEffect, useState } from "react";
import { useQueryProducts } from "../../products/hooks/useQueryProducts";
import { useNavigate } from "react-router-dom";

interface ProductSearchProps {
  onClose: () => void;
}

const ProductSearch = ({ onClose }: ProductSearchProps) => {
  const [inputValue, setInputValue] = useState("");
  const [searchQuery, setSearchQuery] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    const timeoutId = setTimeout(() => {
      setSearchQuery(inputValue.trim());
    }, 400);

    return () => clearTimeout(timeoutId);
  }, [inputValue]);

  const {
    data: products,
    isLoading,
    isError,
  } = useQueryProducts(
    { context: "Search", searchTerm: searchQuery },
    searchQuery.length > 0,
  );

  function handleSearch() {
    const trimmedValue = inputValue.trim();
    if (!trimmedValue) return;

    navigate(`/products?q=${encodeURIComponent(trimmedValue)}`);
    onClose();
  }

  function handleKeyDown(e: React.KeyboardEvent<HTMLInputElement>) {
    if (e.key === "Enter") handleSearch();
  }

  return (
    <div
      className="fixed inset-x-0 top-16 bottom-0 z-40 overflow-y-auto bg-white px-4 py-4
                 md:inset-0 md:top-0 md:z-50 md:flex md:items-start md:justify-center
                 md:bg-black/40 md:pt-24"
      onClick={onClose}
    >
      <div
        className="w-full md:max-w-3xl md:rounded-lg md:bg-white md:px-12 md:py-8"
        onClick={(e) => e.stopPropagation()}
      >
        <div className="mb-6 flex items-center justify-between gap-3 sm:gap-10">
          <div className="flex w-[85%] items-center justify-between rounded-lg border-2 p-2">
            <input
              type="text"
              value={inputValue}
              onChange={(e) => setInputValue(e.target.value)}
              onKeyDown={handleKeyDown}
              placeholder="What can we help you to find ?"
              className="w-full border-none text-sm outline-none sm:text-base"
              autoFocus
            />
            <button onClick={handleSearch} type="button">
              <img src={SearchIcon} alt="Search icon" />
            </button>
          </div>
          <button onClick={onClose} type="button">
            <img src={closehIcon} alt="Close icon" />
          </button>
        </div>

        {searchQuery.trim().length === 0 && <SearchPanelSugestions />}

        {searchQuery.trim().length > 0 && (
          <SearchPanelResults
            products={products}
            isLoading={isLoading}
            isError={isError}
          />
        )}
      </div>
    </div>
  );
};

export default ProductSearch;