import * as React from "react";
import { ICatalog } from "../../types/Catalog";
import { Switch, Route } from "react-router-dom";
import { ProductList } from "./ProductList";
import {Product} from './Product';
import AddCatalogForm from "./AddCatalogForm";
import { IProduct } from "../../types/Product";
import { connect } from 'react-redux';
import * as actions from '../../actions/catalog';

interface ICatalogProps {
    products : IProduct[]
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
            <AddCatalogForm/>
                <Switch>
                    <Route exact path={"/catalog"} component={ProductList}></Route>
                    <Route path={"/catalog/:Id?"} component={Product}></Route>
                </Switch>                
            </div>
        );
    };
}
//const mapStateToProps = state => ({
//    products : state.products
//  });

//export default connect(mapStateToProps, actions)(Catalog)