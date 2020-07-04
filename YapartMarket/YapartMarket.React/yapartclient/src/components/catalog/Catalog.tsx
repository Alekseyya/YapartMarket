import * as React from "react";
import { ICatalog } from "../../types/Catalog";
import { Switch, Route } from "react-router-dom";
import { ProductList } from "./ProductList";
import {ProductComponent} from './Product';
//import AddCatalogForm from "./AddCatalogForm";
import { Product } from "../../types/Product";
import { connect } from 'react-redux';
import * as actions from '../../actions/catalog';

interface ICatalogProps {
    products : ProductComponent[]
}

interface ICatalogState { 
}

export class Catalog extends React.Component<ICatalogProps, ICatalogState> {
    constructor(props: ICatalogProps) {
        super(props);
    }

    render() {
        return (
            <div>
                <Switch>
                    <Route exact path={"/catalog"} component={ProductList}></Route>
                    <Route path={"/catalog/:Id?"} component={ProductComponent}></Route>
                </Switch>                
            </div>
        );
    };
}
//const mapStateToProps = state => ({
//    products : state.products
//  });

//export default connect(mapStateToProps, actions)(Catalog)