import * as React from 'react';
import { render } from 'react-dom';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Link } from 'react-router-dom';
import { ApplicationState } from '../store';
import * as CartStore from '../store/Cart';
import { Product } from '../store/WeatherForecasts';

type CartProps =  CartStore.CartState & typeof CartStore.actionCreators; 
//todo попробовать отправять сюда данные через props
//выбор товаров и чтобы  это все попадало в корзину
class Cart extends React.PureComponent<CartProps> {
    public render() {
        return (
            <div className="cart" >
                <ul className="cartItems">
                    {this.props.products.map((product: Product) =>
                        <li key={product.id}>
                            <div>{product.description}</div>
                            <div>{product.price}</div>
                        </li>
                    )}
                </ul>
            </div>
        )
    }
}

export default connect(
    (state: ApplicationState) => state.cart, CartStore.actionCreators)(Cart as any)