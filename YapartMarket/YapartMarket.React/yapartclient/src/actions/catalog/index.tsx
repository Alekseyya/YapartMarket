import {IProduct} from "../../types/Product"
import { createAction } from 'typesafe-actions';

export const ADD_PRODUCT = "ADD_PRODUCT";
export const DELETE_PRODUCT = "DELETE_PRODUCT";
export const GET_PRODUCTS = "GET_PRODUCTS";

export const AddProduct = createAction(ADD_PRODUCT,
    (Description: string) => ({ Description: Description }))<IProduct>();
export const DeleteProduct = createAction(DELETE_PRODUCT,
    (product: IProduct) => product)<IProduct>();
export const GetProducts = createAction(GET_PRODUCTS,
    async () => {
        console.log("api");
        const url = 'api/Product/Products';
        const response = await fetch(url);
        return await response.json() as IProduct[];
    })<Promise<IProduct[]>>();