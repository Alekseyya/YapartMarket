import React from "react";
// import { Col, Container, Row } from 'react-bootstrap';
// import { ProductComponent } from './Product'
// //import { AddCatalogForm } from "./AddCatalogForm";
// import { connect } from 'react-redux';
// import  * as actions from "../../actions/catalog/index";
// import { IRootState } from "../../reducers";
// import { fetchProductsAction } from "../../actions/catalog/index";
// import * as api from "../../api/product"
// import { IProduct } from "../../types/Product";


// //type Props = ReturnType<typeof mapStateToProps> & typeof dispatchProps;

// interface IProductListProps {
//     products: IProduct[],
//     fetchProducts(): void;
//     // isLoaded: boolean
// }
//  interface IProductListState {
//      products: IProduct[],
//      isLoaded: boolean,
//      error: string;
//  }
// const mapStateToProps = (state: IRootState) => ({
//     products: state.product
// });
// const mapDispatchToProps = (dispatch : any) => ({
//     fetchProducts: () => dispatch(fetchProductsAction()),
// });
// export class ProductList extends React.Component<IProductListProps, IProductListState> {
//     constructor(prop : IProductListProps) {
//         super(prop);
//         //let tmpProduct: IProduct[] = [
//         //    { Id: 1, Article :"1", Name: "First", BrandName: "A1", Description: "bbbbbbbbbb", Price: 10 },
//         //    { Id: 2, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
//         //    { Id: 3, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
//         //    { Id: 4, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
//         //    { Id: 5, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
//         //    { Id: 6, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
//         //    { Id: 7, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
//         //    { Id: 8, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
//         //    { Id: 9, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
//         //    { Id: 10, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
//         //    { Id: 11, Article :"1",  Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 },
//         //    { Id: 12, Article: "1", Name: "Secod", BrandName: "A2", Description: "aaaaa", Price: 10 }
//         //];
//         //let tmpProduct: IProduct[] = [
//         //    { Id: 1, Article: "1", Brand: "First", OldPrice: 10, DaysDelivery: 1, Picture:"\NO-1231.png", Description: "bbbbbbbbbb", Price: 10 }
//         //];

//         //this.state = {
//         //    error: "",
//         //    isLoaded: true,
//         //    products: tmpProduct
//         //};
        
//         this.state = {
//              error: "",
//              isLoaded: false,
//              products: []
//         };

//         //console.log(this.state.products);
//         //console.log(this.props.products);
//     }

//     componentDidMount() { 
//         //console.log("AAAAAAAAAAAAAAAAAAAAAAAAAAA");
//         //alert("componentDidMount");
//         //console.log(this.props.products);        
//         //this.props.fetchProducts();
//         // this.setState({
//         //     isLoaded: true,
//         //     error: "",
//         //     products : this.props.products
//         // })
//         //console.log(this.props.products);
//          api.Get<IProduct[]>('api/Product/Products').then(response => {
//              this.setState({
//                  products: response,
//                  error: "",
//                  isLoaded: true
//              });
//          });
//     }

      

//     render() {
//          let state = this.state;
//          if (state.error) {
//              return <div>Error : {state.error}</div>;
//          } else if (!state.isLoaded) {
//              return <div>Loading...</div>;
//          } else {
//             return (
//                 <Container>
//                     <Row>
//                         {this.state.products.map(product => (<ProductComponent key={product.Id.toString()}
//                                                                       Id={product.Id}
//                                                                       Article={product.Article}
//                                                                       Description={product.Description}
//                                                                       Brand={product.Brand}
//                                                                       DaysDelivery={product.DaysDelivery}
//                                                                       OldPrice={product.OldPrice}
//                                                                       Picture={product.Picture}
//                                                                       Price={product.Price}></ProductComponent>))}
//                     </Row>
//                 </Container>
//             );
//         }
//     }
// }

// // export default connect(mapStateToProps, mapDispatchToProps)(ProductList)