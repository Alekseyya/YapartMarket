import * as React from "react";
import { IProduct } from "../../types/Product";
import { Col, Container, Row } from 'react-bootstrap';
import { Product } from './Product'
import { AddCatalogForm } from "./AddCatalogForm";
import { connect } from 'react-redux';
import { ProductState } from "../../reducers/reducer";
import { RouteComponentProps, withRouter } from 'react-router';


// const mapStateToProps = (state : IProduct) => {
//     products: state.
// }

interface IProductList {
    products: IProduct[],
    //deleteProduct: (product: IProduct) => void;
}
interface IProductListState {
    products: IProduct[],
    isLoaded: boolean,
    error: string;
}
export class ProductList extends React.Component<IProductList, IProductListState>{
    constructor(prop: IProductList) {
        super(prop);
        //let tmpProduct: IProduct[] = [
        //    { Id: 1, Article :"1", Name: "First", BrandName: "A1", Description: "bbbbbbbbbb", Price: 10 },
        //    { Id: 2, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
        //    { Id: 3, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
        //    { Id: 4, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
        //    { Id: 5, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
        //    { Id: 6, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
        //    { Id: 7, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
        //    { Id: 8, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
        //    { Id: 9, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
        //    { Id: 10, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
        //    { Id: 11, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
        //    { Id: 12, Article: "1", Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 }
        //];
        //let tmpProduct: IProduct[] = [
        //    { Id: 1, Article: "1", Brand: "First", OldPrice: 10, DaysDelivery: 1, Picture:"\NO-1231.png", Description: "bbbbbbbbbb", Price: 10 }
        //];

        //this.state = {
        //    error: "",
        //    isLoaded: true,
        //    products: tmpProduct
        //};

        this.state = {
            error: "",
            isLoaded : false,
            products: []
        };
        this.api<IProduct[]>('api/Product/Products').then(response => {
            this.setState({
                products: response,
                error: "",
                isLoaded : true
            });
        });
        //console.log(this.state.products);
        //console.log(this.props.products);
    }
    //componentDidMount()
    api<T>(url: string): Promise<T> {
        return fetch(url)
            .then(response => {
                if (!response.ok) {
                    this.setState({
                        products :[],
                        isLoaded: true,
                        error : response.statusText
                    });
                }
                return response.json();
            });

    }

    render() {
        let state = this.state;
        if (state.error) {
            return <div>Error : {state.error}</div>;
        }else if (!state.isLoaded) {
            return <div>Loading...</div>;
        } else {
            return (
                <Container>
                    <Row>
                        {this.state.products.map(product => (<Product key={product.Id}
                                                                 Id={product.Id}
                                                                 Article={product.Article}
                                                                 Description={product.Description}
                                                                 Brand={product.Brand}
                                                                 DaysDelivery={product.DaysDelivery}
                                                                 OldPrice={product.OldPrice}
                                                                 Picture={product.Picture}
                                                                 Price={product.Price}></Product>))}
                    </Row>
                </Container>
            );
        }
    }
}

const mapStateToProps = (state: ProductState) => {
    return {
        products : state.products
    };
};
export default connect(mapStateToProps, null)(ProductList)