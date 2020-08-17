import { IProduct } from "./Product";
// interface IFetchProducts {
//     type : typeof FETCH_PRODUCTS,
//     payload : Promise<Product[]>
// }

// interface IAddProduct{
//     type: typeof ADD_PRODUCT,
//     payload : Product
// }

export const ADD_PRODUCT= "ADD_PRODUCT";
export const GET_PRODUCT_BYID= "GET_PRODUCT_BYID";
export const DELETE_PRODUCT= "DELETE_PRODUCT";
export const FETCH_PRODUCTS_COMPLETED= "FETCH_PRODUCTS_COMPLETED";

export interface FetchProductsCompleted {
    type: typeof FETCH_PRODUCTS_COMPLETED,
    payload: IProduct[]
}


interface FetchProductByIdCompleted{
    type: typeof GET_PRODUCT_BYID,
    payload: IProduct;
}

export type FetchProducts = FetchProductsCompleted| FetchProductByIdCompleted;