import React from 'react';
import * as ReactDOM from 'react-dom';
import './App.css';
import Routes from './components/Routes';
import { BrowserRouter as Router, Switch } from 'react-router-dom';
import { Header } from './components/Header/Header';
import { Footer } from './components/Footer/Footer';

export class App extends React.Component {
    render() {
        return (
            <Router>
                <div>
                    <Header />
                    <Routes />
                    <Footer />
                </div>
            </Router>
        );
    }
}

export default App;
