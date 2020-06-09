import * as React from "react";
import { ICatalog } from "../../types/Catalog";
import { Switch, Route } from "react-router-dom";
import { ProductList } from "./ProductList";
import {Product} from './Product'

export class RouteCatalog {
    public static Catalog: string = "/catalog";
    public static Product: string = "/catalog/:id(\d+)";    
}

interface ICatalogState {    
}

export class Catalog extends React.Component<ICatalog, ICatalogState> {
    constructor(props: ICatalog) {
        super(props);
    }

    render() {
        return (
            <div>
                <Switch>
                    <Route exact path={RouteCatalog.Catalog} component={ProductList}></Route>
                    <Route path={RouteCatalog.Product} component={Product}></Route>
                </Switch>                
            </div>
        );
    };
}