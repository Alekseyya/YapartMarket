import React from 'react';
import * as ReactDOM from 'react-dom';
import './App.css';
import Routes from './components/Routes';
import { Home } from './components/Home';
import { Header } from './components/Header/Header';
import { BrowserRouter as Router } from 'react-router-dom';

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
        <Routes />
      </Router>
    )
  }
}

export default App;
