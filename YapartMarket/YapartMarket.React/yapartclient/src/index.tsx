import React from 'react';
import ReactDOM from 'react-dom';
import { Provider } from "react-redux";
import { bindActionCreators } from 'redux';

import DevTools from './config/devtools';
import initStore from './config/store';
import './index.css';
import App from './App';
import 'bootstrap/dist/css/bootstrap.min.css';



const devTools = process.env.NODE_ENV === 'development' ? <DevTools /> : null;
const store = initStore();

// const actions = bindActionCreators({ clearAuthentication }, store.dispatch);
// setupAxiosInterceptors(() => actions.clearAuthentication('login.error.unauthorized'));

 ReactDOM.render(
  <Provider store={store}>
    {devTools}
    <App />,
  </Provider>,
  document.getElementById('root')
);