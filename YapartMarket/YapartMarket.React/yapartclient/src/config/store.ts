import { createStore, compose, applyMiddleware } from 'redux';
import reducer, { IRootState } from '../reducers';
import DevTools from './devtools';
import thunkMiddleware from 'redux-thunk';
import promiseMiddleware from 'redux-promise-middleware';
import notificationMiddleware from './notification-middleware';
import errorMiddleware from './error-middleware';
import loggerMiddleware from './logger-middleware';
import { loadingBarMiddleware } from 'react-redux-loading-bar';

const defaultMiddlewares = [
  thunkMiddleware,
  errorMiddleware,
  notificationMiddleware,
  promiseMiddleware,
  loadingBarMiddleware(),
  loggerMiddleware
];

const composedMiddlewares = (middlewares: any) =>
  {
    return process.env.NODE_ENV === 'development'
      ? compose(applyMiddleware(...defaultMiddlewares, ...middlewares), DevTools.instrument())
      : compose(applyMiddleware(...defaultMiddlewares, ...middlewares));
  };

const initialize = (initialState?: IRootState, middlewares = []) => createStore(reducer, initialState, composedMiddlewares(middlewares));
export default initialize;