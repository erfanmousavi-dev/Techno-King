import accessories from "/category/accessories.png";
import laptop from "/category/laptop.png";
import camera from "/category/camera.png";
import smartPhone from "/category/smart-phone.png";
import gaming from "/category/gaming.png";
import smartWatch from "/category/smart-watch.png";
import { Link } from "react-router-dom";

const categories = [
  { subcategoryId: 21, image: accessories, label: "Accessories" },
  { subcategoryId: 18, image: camera, label: "Camera" },
  { subcategoryId: 4, image: laptop, label: "Laptop" },
  { subcategoryId: 5, image: smartPhone, label: "Smart Phone" },
  { subcategoryId: 20, image: gaming, label: "Gaming" },
  { subcategoryId: 16, image: smartWatch, label: "Smart Watch" },
];

const CategoryList = () => {
  return (
    <ul className="my-12 grid grid-cols-2 gap-6 px-4 sm:px-8 md:px-[4rem] lg:px-[6.8rem] sm:grid-cols-3 lg:grid-cols-6">
      {categories.map((category) => (
        <li key={category.subcategoryId}>
          <Link
            to={`/products?subcategory=${category.subcategoryId}`}
            className="flex min-h-[170px] flex-col items-center justify-center rounded-xl bg-white p-4 shadow-md transition duration-300 hover:-translate-y-1 hover:shadow-lg"
          >
            <img
              className="h-37 w-37 object-contain"
              src={category.image}
              alt={category.label}
            />
            <span className="mt-4 text-center text-sm font-medium">
              {category.label}
            </span>
          </Link>
        </li>
      ))}
    </ul>
  );
};

export default CategoryList;