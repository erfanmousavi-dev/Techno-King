export interface Product {
  id: number;
  name: string;
  price: number;
  description: string;
  brand: string;
  discountPercentage: number;
  subCategoryId: number;
  averageRating: number;
  salesCount: number;
  imageUrl1: string;
  profileImgFile1: string | null;
  imageUrl2: string;
  profileImgFile2: string | null;
  imageUrl3: string;
  profileImgFile3: string | null;
  isDeleted: boolean;
}