import * as React from "react";
import { ICatalog } from "../../types/Catalog";
import { IProduct } from "../../types/Product";
import { Product } from "./Product";
import { Col, Container, Row } from 'react-bootstrap';

interface ICatalogState {
    products : IProduct[]
}

export class Catalog extends React.Component<ICatalog, ICatalogState> {
    constructor(props: ICatalog) {
        super(props);
        let tmpProduct: IProduct[] = [
            {Id : 1, Name: "First", BrandName :"A1", Description:"bbbbbbbbbb", Price: 10},
         {Id : 1, Name: "Secod", BrandName :"A2", Description:"aaaaa", Price: 10},
         {Id : 1, Name: "Secod", BrandName :"A2", Description:"aaaaa", Price: 10},
         {Id : 1, Name: "Secod", BrandName :"A2", Description:"aaaaa", Price: 10},
         {Id : 1, Name: "Secod", BrandName :"A2", Description:"aaaaa", Price: 10},
         {Id : 1, Name: "Secod", BrandName :"A2", Description:"aaaaa", Price: 10},
         {Id : 1, Name: "Secod", BrandName :"A2", Description:"aaaaa", Price: 10},
         {Id : 1, Name: "Secod", BrandName :"A2", Description:"aaaaa", Price: 10},
         {Id : 1, Name: "Secod", BrandName :"A2", Description:"aaaaa", Price: 10},
         {Id : 1, Name: "Secod", BrandName :"A2", Description:"aaaaa", Price: 10},
         {Id : 1, Name: "Secod", BrandName :"A2", Description:"aaaaa", Price: 10},
         {Id : 1, Name: "Secod", BrandName :"A2", Description:"aaaaa", Price: 10}
        ];
        this.state = {
            products : tmpProduct
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
        );
    };
}