import ProductSegmentation from "../../../shared/components/ProductSegmentation";
import ProductOnSale from "../../products/components/ProductOnSale";
import { useQueryProducts } from "../../products/hooks/useQueryProducts";
import ProductSearch from "../../search/components/ProductSearch";
import Brands from "../components/Brands";
import CategoryList from "../components/CategoryList";
import FeaturedProducts from "../components/FeaturedProducts";
import HeroSection from "../components/HeroSection";
import OurBlogs from "../components/OurBlogs";
import PromoBanner from "../components/PromoBanner";
import Services from "../components/Services";

const HomePage = () => {
  const { data: newestData } = useQueryProducts({
    sortBy: "CreatedAt",
    ascending: false,
    pageSize: 4,
  });

  const { data: bestSellingData } = useQueryProducts({
    sortBy: "SalesCount",
    ascending: false,
    pageSize: 4,
  });

  return (
    <div>
      <HeroSection />

      {/* <ProductSearch /> */}
      <CategoryList />
      {/* <ProductOnSale /> */}
      <ProductSegmentation data={newestData} type="newest" />
      <FeaturedProducts />
      <ProductSegmentation data={bestSellingData} type="bestselling" />
      <Brands />
      <PromoBanner />
      <OurBlogs />
      <Services />
    </div>
  );
};

export default HomePage;