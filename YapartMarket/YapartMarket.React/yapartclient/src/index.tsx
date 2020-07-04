import * as  React from 'react';
import { Provider } from "react-redux";
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import * as serviceWorker from './serviceWorker';
import 'bootstrap/dist/css/bootstrap.min.css';
import { store } from './store';

ReactDOM.render(
  <Provider store={store}>
    <App />,
  </Provider>,
  document.getElementById('root')
);

// ReactDOM.render(
//   <Router>
//       <Routes />
//   </Router>,
//   document.getElementById("root")
// );

serviceWorker.unregister();
