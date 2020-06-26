import { IProduct } from "../types/Product";
import { combineReducers } from "redux";
import { ActionType } from "typesafe-actions";
import * as actions from '../actions/catalog';
import { createReducer } from 'typesafe-actions';
import { ADD_PRODUCT, DELETE_PRODUCT, GET_PRODUCTS } from "../actions/catalog";


export type ProjectAction = ActionType<typeof actions>

export type ProductState = Readonly<{
    products: IProduct[]
}>;
const initialState: ProductState = {
    products: []
};

export default combineReducers<ProductState, ProjectAction>({
    products: (state = initialState.products, action: ProjectAction) => {
        switch (action.type) {
            case ADD_PRODUCT:               
                return [
                    {
                        Id: state.reduce((maxId, product) => Math.max(product.Id, maxId), -1) + 1,
                        Article :action.payload.Article,
                        Brand: action.payload.Brand,
                        Description: action.payload.Description,
                        Price: action.payload.Price,
                        OldPrice: action.payload.OldPrice,
                        Picture: action.payload.Picture,
                        DaysDelivery : action.payload.DaysDelivery
                    }, ...state
                ];
            case DELETE_PRODUCT:
                return state.filter(project => project.Id !== action.payload.Id);
            case GET_PRODUCTS:
                return state;
            default:
                return state;
        }
    }
});


// export const todos = createReducer([
//   {Id: 0, Name: "First", BrandName: "Brand", Description : "Descr", Price : 10},
// ] as IProduct[])
//   .handleAction(actions.AddProduct, (state : IProduct[], action: ActionType<typeof actions>) => [...state, action.payload])
//   .handleAction(actions.DeleteProduct, (state : IProduct[], action: ActionType<typeof actions>) =>
//     state.filter(i => i.Id !== action.payload.Id)
//   );
