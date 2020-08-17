import { IProduct } from "../../types/Product"
import { createAction } from 'typesafe-actions';
import { GET_PRODUCT_BYID, FETCH_PRODUCTS_COMPLETED, ADD_PRODUCT, FetchProducts } from "../../types/types";
import { Action } from "redux";

// export const AddProduct = createAction(ADD_PRODUCT,
//     (Description: string) => ({ Description: Description }))<Product>();
// export const DeleteProduct = createAction(DELETE_PRODUCT,
//     (product: Product) => product)<Product>();

//export const fetchProducts = createAction(FETCH_PRODUCTS,
//    async () => {
//        const url = 'api/Product/Products';
//        const response = await fetch(url);
//        return await response.json() as IProduct[];
//    })<Promise<IProduct[]>>();



const fetchProductsCompleted = (products: IProduct[]) : FetchProducts=> ({
    type:  FETCH_PRODUCTS_COMPLETED,
    payload: products
})
export const fetchProductsAction = () => (dispatch: any) => {
    api<IProduct[]>('api/Product/Products').then((products) => {
        dispatch(fetchProductsCompleted(products));
    });
}


// const fetchProductByIdAction = (product: Product): FetchProducts => ({
//     type: GET_PRODUCT_BYID,
//     payload: product
// });

// export const fetchProductByIdCompleted = (id : number) => (dispatch: any) => {
//     api<Product>('api/Product/GetProductById?id=' + id).then((product) => {
//         dispatch(fetchProductByIdAction(product));
//     });
// }

 export function addProduct(product: IProduct) {
     return {
         type: ADD_PRODUCT,
         payload: product
     }
 }

function api<T>(url: string): Promise<T> {
    return fetch(url)
        .then(response => {
            if (!response.ok) {

            }
            return response.json();
        });
}
