import * as React from "react";
import { IProduct } from "../../types/Product";
import { Col, Container, Row } from 'react-bootstrap';
import {Product} from './Product'


interface IProductList{}
interface IProductListState{
    products : IProduct[]
}
export class ProductList extends React.Component<IProductList, IProductListState>{
     constructor(prop : IProductList) {
        super(prop);
        let tmpProduct: IProduct[] = [
            { Id: 1, Name: "First", BrandName: "A1", Description: "bbbbbbbbbb", Price: 10 },
            { Id: 2, Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
            { Id: 3, Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
            { Id: 4, Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
            { Id: 5, Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
            { Id: 6, Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
            { Id: 7, Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
            { Id: 8, Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
            { Id: 9, Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
            { Id: 10, Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
            { Id: 11, Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
            { Id: 12, Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 }
        ];
        this.state = {
            products: tmpProduct
        }        
    }
    render() {
        return (
            <Container>
                <Row>
                    {
                        this.state.products.map(product => (
                            <Product Id={product.Id}
                                Name={product.Name}
                                BrandName={product.BrandName}
                                Description={product.Description}
                                Price={product.Price} />
                        ))
                    }
                </Row>
            </Container>
        )
    }
}