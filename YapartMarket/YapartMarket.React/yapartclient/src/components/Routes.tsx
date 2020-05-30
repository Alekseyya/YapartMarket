import * as React from 'react';
import * as ReactDOM from 'react-dom';
import { Route, Redirect, Switch } from 'react-router-dom';
import { Home } from './Home';
import { Catalog } from './catalog/Catalog'


export class RoutePaths {
    public static Home: string = "/";
    public static Register: string = "/register/";
    public static About: string = "/about";
    public static Catalog: string = "/catalog";
}


export default class Routes extends React.Component {
    render() {
        return <Switch>
            <Route exact path={RoutePaths.Home} component={Home} />
            <Route path={RoutePaths.Catalog} component={Catalog} />
        </Switch>;
    }
}