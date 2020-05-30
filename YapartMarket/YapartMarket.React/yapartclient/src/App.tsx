import React from 'react';
import * as ReactDOM from 'react-dom';
import './App.css';
import Routes from './components/Routes';
import { BrowserRouter as Router, Switch } from 'react-router-dom';

// function App() {
//   return (
//     <div className="App111">
//       {Home}
//     </div>
//   );
// }

export class App extends React.Component {
  render() {
      return (
          <Router>
              <Routes/>
          </Router>
      );
  }
}

export default App;
