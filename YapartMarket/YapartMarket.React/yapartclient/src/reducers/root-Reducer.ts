import { combineReducers } from "redux";
import { Product } from "../types/Product";
import { productReducer } from "./productReducer"

// export type ProjectAction = ActionType<typeof actions>

// export type ProductState = Readonly<{
//     products: IProduct[]
// }>;
// const initialState: ProductState = {
//     products: []
// };

//export interface State {
//    products: Product[]
//}

//export const state = combineReducers<State>({
//    products: productReducer
//});

export interface State {
    products: Product[]
}

export const rootReducer = combineReducers({
    products: productReducer
});
//export type RootState = ReturnType<typeof rootReducer>
