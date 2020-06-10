import {IProduct} from "../../types/Product"
import { createAction } from 'redux-actions';
import {ADD_PRODUCT, DELETE_PRODUCT} from '../../constants/ActionType'

const AddProduct = createAction<IProduct, string>(
    ADD_PRODUCT,
   (Name: string) => ({ Name: Name, Brand: "AAAA"})
);

const DeleeProduct = createAction<IProduct, string>(
    DELETE_PRODUCT,
    (product: IProduct) => product
);

export {AddProduct, DeleeProduct}