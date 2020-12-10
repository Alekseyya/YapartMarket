import * as React from 'react';
import { connect } from 'react-redux';
import { Product } from "../../store/Models/Product";
import * as Products from '../../store/Products';
import { ApplicationState } from '../../store';

interface IState {
    products : Product[]
}

interface IProp{
    products : Product[]
}
type ProductsProps =
  //WeatherForecastsStore.WeatherForecastsState // ... state we've requested from the Redux store
  Products.ProductsState
  & typeof Products.productActionCreators;


class ProductListComponent extends  React.PureComponent<ProductsProps>{
    constructor(props : ProductsProps) {
        super(props);
        // let a: Product[] = [
        //     { Id : 1, Descriptions : "111", Price : 10, count : 6 },
        //     { Id : 2, Descriptions : "222", Price : 101, count : 4 },
        //     { Id : 3, Descriptions : "333", Price : 101, count : 5 },
        //     { Id : 4, Descriptions : "444", Price : 102, count : 6 },
        // ];
        //this.state = {products : a};        
    }
    
    public componentDidMount() {
        this.props.requestProducts();
    }

    render() {
        return(
            <ul>
                {this.props.products.map((product: Product) =>
                    <div>{product.Article} - {product.Descriptions}</div>
                )}
            </ul>
        );
    }
}
export default connect(  
    (state: ApplicationState) => state.products,
    Products.productActionCreators // Selects which action creators are merged into the component's props
  )(ProductListComponent as any);