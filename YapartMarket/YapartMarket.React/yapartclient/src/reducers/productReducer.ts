import { Product, ProductState } from "../types/Product";
import { FetchProducts, FETCH_PRODUCTS_COMPLETED, FetchProductsCompleted } from "../types/types";

// export type ProjectAction = ActionType<typeof actions>

// export type ProductState = Readonly<{
//     products: IProduct[]
// }>;
// const initialState: ProductState = {
//     products: []
// };

//ProductState
// const initialState: ProductState = {
//     products: new Promise<Product[]>((null) as any)
// };
interface FetchState{
    products : Product[]
}

//Устанавливливается начальное состояние в пустой массив[]
export function productReducer(state: Product[] = [], action: FetchProducts) {
    switch (action.type){
        case FETCH_PRODUCTS_COMPLETED:
            return handleFetchProductCompleted(state, action as FetchProductsCompleted);
        // case actionTypes.GET_PRODUCT_BYID:
        //     return handleFetchProductByIdCompleted({Id =}, action.payload)
        default:
            return state;
    }
};

const handleFetchProductCompleted = (state: Product[], action: FetchProductsCompleted): Product[] => {
    return action.payload;
};

//const handleFetchProductCompleted = (state: Product[], payload: Product[]) : Product[] => {
//    return payload;
//};

const handleFetchProductByIdCompleted = (state: Product, payload: Product): Product => {
    return payload;
  };


// export const todos = createReducer([
//   {Id: 0, Name: "First", BrandName: "Brand", Description : "Descr", Price : 10},
// ] as IProduct[])
//   .handleAction(actions.AddProduct, (state : IProduct[], action: ActionType<typeof actions>) => [...state, action.payload])
//   .handleAction(actions.DeleteProduct, (state : IProduct[], action: ActionType<typeof actions>) =>
//     state.filter(i => i.Id !== action.payload.Id)
//   );

// export default function(state = initialState, action: ProductsActionTypes) : ProductState{
//         switch (action.type) {
//             case FETCH_PRODUCTS :
//                 return {...state, products : action.payload}
//             // case ADD_PRODUCT:               
//             //     return [
//             //         {
//             //             Id: state.reduce((maxId, product) => Math.max(product.Id, maxId), -1) + 1,
//             //             Article :action.payload.Article,
//             //             Brand: action.payload.Brand,
//             //             Description: action.payload.Description,
//             //             Price: action.payload.Price,
//             //             OldPrice: action.payload.OldPrice,
//             //             Picture: action.payload.Picture,
//             //             DaysDelivery : action.payload.DaysDelivery
//             //         }, ...state
//             //     ];
//             // case DELETE_PRODUCT:
//             //     return state.filter(project => project.Id !== action.payload.Id);
//             // case GET_PRODUCTS:
//             //     return state;
//             default:
//                 return state;
//         }
//     };
// export default combineReducers<ProductState, ProjectAction>({
//     products: (state = initialState.products, action: ProjectAction) => {
//         switch (action.type) {
//             case ADD_PRODUCT:               
//                 return [
//                     {
//                         Id: state.reduce((maxId, product) => Math.max(product.Id, maxId), -1) + 1,
//                         Article :action.payload.Article,
//                         Brand: action.payload.Brand,
//                         Description: action.payload.Description,
//                         Price: action.payload.Price,
//                         OldPrice: action.payload.OldPrice,
//                         Picture: action.payload.Picture,
//                         DaysDelivery : action.payload.DaysDelivery
//                     }, ...state
//                 ];
//             case DELETE_PRODUCT:
//                 return state.filter(project => project.Id !== action.payload.Id);
//             case GET_PRODUCTS:
//                 return state;
//             default:
//                 return state;
//         }
//     }
// });