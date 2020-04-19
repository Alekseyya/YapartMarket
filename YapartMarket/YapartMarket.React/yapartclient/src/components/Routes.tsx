import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Route, Redirect, Switch } from 'react-router-dom';
import { Home } from './Home';


export class RoutePaths {
    public static Home: string = "/";
    public static Register: string = "/register/";
}


export default class Routes extends React.Component{
    render() {
        return <Switch>
            <Route exact path={RoutePaths.Home} component={Home} />
        </Switch>
    }
}