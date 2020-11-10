import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';
import { KnownAction } from './Counter';
import { Product } from './WeatherForecasts';

//State
export interface CartState{
    isLoading: boolean;
    products: Product[]
}

//Action
export interface AddToCartAction{
    type: 'ADD_TO_CART'
    products: Product[]
}

interface RemoveFromCartAction{
    type: 'REMOVE_FROM_CART',
    products: Product[]
}


//declaretedUnion
export type CartActions = AddToCartAction | RemoveFromCartAction

//ActionCreators

export const actionCreators = {
    addToCart: (product: Product) : AppThunkAction<CartActions> => (dispatch, getState) => {
        const appState = getState();
        if(appState && appState.cart){
            var products = appState.cart.products;
            if(!appState.cart.products.some(x =>x.id == product.id)){
                products.push(product);
            }
            dispatch({type: 'ADD_TO_CART', products: products })
        }
    },
    removeFromCart: (id : number) : AppThunkAction<CartActions> => (dispatch, getState) => {
        const appState = getState();
        if(appState && appState.cart && id != 0){
            var products = appState.cart.products.filter(x=>x.id != id);
            dispatch({type: 'REMOVE_FROM_CART', products: products})
        }
    }
}

const unloadedState: CartState = {products:[],  isLoading: false };
//Reducer
export const cartReducer : Reducer<CartState> = (state : CartState | undefined, incomingAction : Action): CartState =>{
    if (state === undefined) {
        return unloadedState;
    }    
    const action = incomingAction as CartActions
    switch(action.type){
        case 'ADD_TO_CART' :
            return{
                products: action.products,
                isLoading: false
            }
            break;
        case 'REMOVE_FROM_CART' :
                return{
                    products: action.products,
                    isLoading: false
                }
            break;
            default :
            return{
                products: [],
                isLoading: false
            }
            break;
    }  
}

