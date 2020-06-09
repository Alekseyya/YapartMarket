import * as React from "react";
import { ICatalog } from "../../types/Catalog";
import { Switch, Route } from "react-router-dom";
import { ProductList } from "./ProductList";
import {Product} from './Product'

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
                    <Route exact path="/catalog" component={ProductList}></Route>
                    <Route path="/catalog/:id(\d+)" component={Product}></Route>
                </Switch>                
            </div>
        );
    };
}