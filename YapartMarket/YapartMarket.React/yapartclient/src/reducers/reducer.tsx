import { IProduct } from "../types/Product";
import {ADD_PRODUCT, DELETE_PRODUCT} from "../constants/ActionType"
import { handleActions, Action } from "redux-actions";

export type IState = IProduct[];

export default handleActions<IState, IProduct>({
    [ADD_PRODUCT]: (state : IState, action : Action<IProduct>) : IState =>{
        return [{
            Id: state.reduce((maxId, product) => Math.max(product.Id, maxId), -1) + 1,
            Name: "NewName",
            BrandName : "NewBrandName",
            Description : "NewDescr",
            Price : 24
          }, ...state];
    },
    [DELETE_PRODUCT]: (state: IState, action: Action<IProduct>): IState => {
        return state.filter(product =>
          product.Id !== action.payload.id
        );
      }
});
