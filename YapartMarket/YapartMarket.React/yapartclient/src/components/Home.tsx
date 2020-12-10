import * as React from 'react';
import { connect } from 'react-redux';
import ProductListComponent from './product/ProductList';
import { ApplicationState } from '../store';
import * as WeatherForecastsStore from '../store/WeatherForecasts';
import { WeatherForecastsState } from '../store/WeatherForecasts';


class Home extends React.Component {
    public render() {
        return(
            <div className="contaiver">
                <div className="row">
                    <div className="wrapper_product">
                        <div className="image"></div>
                        <div className="productModel">sadasd</div>
                        <div className="description">asdasd</div>
                        <ProductListComponent /> 
                    </div>
                </div>
            </div>
        );
    }  
}

export default connect(
    (state: ApplicationState) => state.products,
    WeatherForecastsStore.actionCreators // Selects which action creators are merged into the component's props
)(ProductListComponent as any);
