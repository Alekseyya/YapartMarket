import * as React from "react";
import { Col, Container, Row } from 'react-bootstrap';
import { IProduct } from "../../types/Product";

export class Product extends React.Component<IProduct> {
    constructor(props: IProduct) {
        super(props);        
    }
    render() {
        return (
            <Col
                lg={3}
                md={4}
                sm={6}>
                <div className="product">
                    <div className="product-img-wrapper">
                        <a href="#">
                            <img
                                alt={this.props.Name}
                                className="img-responsive product-img"
                                src={this.props.Picture} />
                        </a>
                    </div>

                    <h4
                        className="ellipsis"
                        title={this.props.Description}>
                        <a href="#">
                            {this.props.Name}
                        </a>
                    </h4>

                    <h5
                        className="ellipsis product-brand-name"
                        title={this.props.BrandName}>
                        {`by ${this.props.BrandName}`}
                    </h5>

                    <div className="pull-right h4 product-price">
                        {`${this.props.Price}Руб`}
                    </div>
                </div>
            </Col>
        );
    }
}