// import { createStore, applyMiddleware, compose} from 'redux';
// import thunk from "redux-thunk";

import { Store, createStore, compose, applyMiddleware } from 'redux';
import reduxThunk from 'redux-thunk';
//import { state, State } from '../reducers/root-Reducer';
import { rootReducer } from '../reducers/root-Reducer';


// import { createEpicMiddleware } from 'redux-observable';
// import { RootAction, RootState, Services } from 'typesafe-actions';

// import { composeEnhancers } from './utils';
//import rootReducer from '../reducers/root-Reducer';
//import { composeEnhancers } from './utils';
// import rootEpic from './root-epic';
// import services from '../services';

// export const epicMiddleware = createEpicMiddleware<
//   RootAction,
//   RootAction,
//   RootState,
//   Services
// >({
//   dependencies: services,
// });

// const middlewares = [epicMiddleware];
// const enhancer = composeEnhancers(applyMiddleware(...middlewares));

// declare global {
//     interface Window {
//         __REDUX_DEVTOOLS_EXTENSION_COMPOSE__?: typeof compose;
//     }
// }

// const initialState = {};
// const composeEnhancers = window.__REDUX_DEVTOOLS_EXTENSION_COMPOSE__ as typeof compose || compose;
// const store = createStore(rootReducer, initialState, composeEnhancers(applyMiddleware(thunk)));
// //epicMiddleware.run(rootEpic);
// export default store;
export const store = createStore(
    rootReducer,
    compose(
      applyMiddleware(reduxThunk),
    )
  );
