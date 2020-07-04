export interface Product{
    Id: number;
    Article: string;
    Description: string;
    Price: number;
    DaysDelivery: number;
    OldPrice: number;
    Brand: string;
    Picture: string;
}

export interface ProductState {
    products: Promise<Product[]>
};