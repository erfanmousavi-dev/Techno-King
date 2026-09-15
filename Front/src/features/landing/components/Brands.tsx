import { Link } from "react-router-dom";
import appleLogo from "/brands/apple.png";
import huaweiLogo from "/brands/huawei.png";

const Brands = () => {
  return (
    <section className="mb-[4rem] px-4 sm:px-8 md:px-[4rem] lg:px-[6.8rem]">
      <div className="border-b-[2px] border-[#B4B4B4] pb-[0.7rem]">
        <h3 className="text-[1.6rem] font-semibold">Top Brands</h3>
      </div>
      <div className="grid grid-cols-2 mt-[2.5rem] items-center justify-items-center gap-x-4 gap-y-8 md:grid-cols-3 lg:flex lg:justify-between lg:gap-x-6">
        <Link to="/products?brand=apple">
          <img
            src={appleLogo}
            alt="apple logo"
            className="h-[3.5rem] w-[2.8rem]"
          />
        </Link>
        <Link
          to="/products?brand=sony"
          className="text-[2rem] font-bold text-shadow-[0_4px_4px_#00000040]"
        >
          SONY
        </Link>
        <Link
          to="/products?brand=samsung"
          className="text-[2rem] font-bold text-[#063A88] text-shadow-[0_4px_4px_#00000024]"
        >
          SAMSUNG
        </Link>
        <Link
          to="/products?brand=canon"
          className="text-[2rem] font-bold text-[#C91433] text-shadow-[0_4px_4px_#00000040]"
        >
          Canon
        </Link>
        <Link to="/products?brand=huawei">
          <img
            src={huaweiLogo}
            alt="huawei logo"
            className="h-[5rem] w-[5rem]"
          />
        </Link>
        <Link
          to="/products?brand=lenovo"
          className="bg-black px-[0.5rem] text-[1.5rem] font-bold text-white"
        >
          Lenovo
        </Link>
      </div>
    </section>
  );
};

export default Brands;
