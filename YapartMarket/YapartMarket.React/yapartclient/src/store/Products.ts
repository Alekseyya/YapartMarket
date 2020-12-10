import { Action, Reducer } from 'redux';
import { AppThunkAction } from '.';
import { Product } from "./Models/Product";

interface ReceiveProductsAction{
    type : 'RECEIVE_PRODUCTS',
    products : Product[]
}

interface RemoveProductAction{
    type: 'REMOVE_PRODUCT',
    products : Product[]
}


type KnownAction =  ReceiveProductsAction | RemoveProductAction;

export interface ProductsState{
    isLoading: boolean;
    products: Product[];
}

export const productActionCreators = {
    requestProducts: (): AppThunkAction<KnownAction>  => (dispatch, getState) =>{
        const appState = getState();
        if(appState && appState.weatherForecasts){
            fetch(`api/Product/ListProducts`)
            .then(response => response.json() as Promise<Product[]>)
            .then(data => {
                dispatch({ type: 'RECEIVE_PRODUCTS', products: data });
            });
        }            
     },
}

const unloadedProductState: ProductsState = { products: [], isLoading: false };

export const productReducer: Reducer<ProductsState> = (state: ProductsState | undefined, incomingAction: Action): ProductsState => {
    if (state === undefined) {
        return unloadedProductState;
    }
    const action = incomingAction as KnownAction;
    switch (action.type) {        
        case 'RECEIVE_PRODUCTS':
                return {
                    products: action.products,
                    isLoading: false
                };
            break;
        case "REMOVE_PRODUCT" :
            return{
                products : action.products,
                isLoading: false
            }
            break
    }
    return state;
};