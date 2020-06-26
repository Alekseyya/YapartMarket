import * as React from "react";
import { Col, Container, Row } from 'react-bootstrap';
import { IProduct } from "../../types/Product";
import { RouteComponentProps } from 'react-router';


//type TParams = {id : string}

export class Product extends React.Component<IProduct> {
    constructor(props: IProduct) {
        super(props);
        //let prodId = this.props.match.params.id;
        //console.log(prodId);
    }
    render() {
        return (
            <Col key = {this.props.Id}
                lg={3}
                md={4}
                sm={6}>
                <div className="product">
                    <div className="product-img-wrapper">
                        <a href={"catalog/" + this.props.Id}>
                            <img
                                alt={this.props.Description}
                                className="img-responsive product-img"
                                src={this.props.Picture} />
                        </a>
                    </div>

                    <h4
                        className="ellipsis"
                        title={this.props.Description}>
                        <a href="#">
                            {this.props.Description}
                        </a>
                    </h4>

                    <h5
                        className="ellipsis product-brand-name"
                        title={this.props.Brand}>
                        {`by ${this.props.Brand}`}
                    </h5>

                    <div className="pull-right h4 product-price">
                        {`${this.props.Price} Руб`}
                    </div>
                </div>
            </Col>
        );
    }
}