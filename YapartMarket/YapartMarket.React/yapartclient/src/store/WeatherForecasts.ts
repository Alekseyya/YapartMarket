import { Action, Reducer } from 'redux';
import { AppThunkAction } from './';
import { Product } from './Models/Product';
//import { AddToCartAction, CartActions } from './Cart';

// -----------------
// STATE - This defines the type of data maintained in the Redux store.
//СОСТОЯНИЕ - определяет тип данных, хранящихся в хранилище Redux.
export interface WeatherForecastsState {
    isLoading: boolean;
    startDateIndex?: number;
    forecasts: WeatherForecast[];
    products: Product[];
}
export interface WeatherForecast {
    date: string;
    temperatureC: number;
    temperatureF: number;
    summary: string;
}

// -----------------
// ACTIONS - These are serializable (hence replayable) descriptions of state transitions.
// They do not themselves have any side-effects; they just describe something that is going to happen.

//ДЕЙСТВИЯ - это сериализуемые (следовательно, воспроизводимые) описания переходов между состояниями.
//Сами по себе они не имеют побочных эффектов; они просто описывают то, что должно произойти.
//Используйте @typeName и isActionType для определения типа, которое работает даже после сериализации / десериализации.

interface RequestWeatherForecastsAction {
    type: 'REQUEST_WEATHER_FORECASTS';
    startDateIndex: number;
}

interface ReceiveWeatherForecastsAction {
    type: 'RECEIVE_WEATHER_FORECASTS';
    startDateIndex: number;
    forecasts: WeatherForecast[];
}


// Declare a 'discriminated union' type. This guarantees that all references to 'type' properties contain one of the
// declared type strings (and not any other arbitrary string).
type KnownAction = RequestWeatherForecastsAction | ReceiveWeatherForecastsAction 
// | ReceiveProductsAction | RemoveProductAction | AddToCartAction;

// ----------------
// ACTION CREATORS - These are functions exposed to UI components that will trigger a state transition.
// They don't directly mutate state, but they can have external side-effects (such as loading data).

//СОЗДАТЕЛИ ДЕЙСТВИЙ - это функции, доступные для компонентов пользовательского интерфейса, которые запускают переход состояния.
//Они не изменяют состояние напрямую, но могут иметь внешние побочные эффекты (например, загрузка данных).

export const actionCreators = {    
    //  requestProducts: (): AppThunkAction<KnownAction>  => (dispatch, getState) =>{
    //     const appState = getState();
    //     if(appState && appState.weatherForecasts){
    //         fetch(`api/Product/ListProducts`)
    //         .then(response => response.json() as Promise<Product[]>)
    //         .then(data => {
    //             dispatch({ type: 'RECEIVE_PRODUCTS', products: data });
    //         });

    //     //dispatch({ type: 'REQUEST_WEATHER_FORECASTS', startDateIndex: startDateIndex });
    //     }            
    //  },
    //  removeItem: (item: number): AppThunkAction<KnownAction> => (dispatch, getState) =>{
    //     const appState = getState();
    //      if(appState && appState.products){
    //         const prodictsList = appState.products.products.slice().filter(x=> x.Id != item);
    //         dispatch({ type: 'REMOVE_PRODUCT', products: prodictsList });
    //      }        
    //  },
    //  addToCart: (product: Product) : AppThunkAction<CartActions> => (dispatch, getState) => {
    //     const appState = getState();
    //     if(appState && appState.cart){
    //         var products = appState.cart.products.slice();
    //         if(products.length > 0){
    //             var allreadyExist = false;
    //             products.forEach(element => {
    //                 if(typeof(element) !== 'undefined' && element.Id == product.Id){
    //                     element.count++;
    //                     allreadyExist = true;
    //                 }
    //             });
    //             if(!allreadyExist){
    //                 products.push({...product, count: 1});
    //             }            
    //         }            
    //         dispatch({type: 'ADD_TO_CART', products: products })
    //     }
    // }
};

// ----------------
// REDUCER - For a given state and action, returns the new state. To support time travel, this must not mutate the old state.
//REDUCER - для данного состояния и действия возвращает новое состояние. 
//Для поддержки путешествий во времени это не должно изменять старое состояние.
const unloadedState: WeatherForecastsState = {products:[],  forecasts: [], isLoading: false };

export const reducer: Reducer<WeatherForecastsState> = (state: WeatherForecastsState | undefined, incomingAction: Action): WeatherForecastsState => {
    if (state === undefined) {
        return unloadedState;
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'REQUEST_WEATHER_FORECASTS':
            return {
                startDateIndex: action.startDateIndex,
                forecasts: state.forecasts,
                isLoading: true,
                products: state.products
            };
        case 'RECEIVE_WEATHER_FORECASTS':
            // Only accept the incoming data if it matches the most recent request. This ensures we correctly
            // handle out-of-order responses.
            if (action.startDateIndex === state.startDateIndex) {
                return {
                    startDateIndex: action.startDateIndex,
                    forecasts: action.forecasts,
                    isLoading: false,
                    products: state.products
                };
            }
            break;
            // case 'RECEIVE_PRODUCTS':
            //     return {
            //         products: action.products,
            //         isLoading: false,
            //         forecasts: state.forecasts
            //     };
            // break;            
    }
    return state;
};


