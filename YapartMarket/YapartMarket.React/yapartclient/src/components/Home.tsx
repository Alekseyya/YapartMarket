import * as React from "react";
import { Link, Redirect, RouteComponentProps } from 'react-router-dom';
import { RoutePaths } from './Routes';
import { IWeatherForecast } from '../services/WeatherForecast'
import { Header } from "./Header/Header";
import { Footer } from "./Footer/Footer";


export class Home extends React.Component<RouteComponentProps<any>, any>{
    constructor(props: any) {
        super(props);
        this.state = { weatherForecasts: [] };
        var data = fetch("https://localhost:44346/api/WeatherForecast/list")
            .then(response => response.json() as Promise<IWeatherForecast[]>)
            .then(data => {
                this.setState({ weatherForecasts: data });
            });
    }

    private onChange() {

    }

    render() {
        let content = this.renderWeatherForecastsTable(this.state.weatherForecasts);
        return (
            <div>
                <Header />
                <div>
                    {content}
                </div>
                <div>
                </div>
                <Footer />
            </div>
        );
    };


    private renderWeatherForecastsTable(weatherForecasts: IWeatherForecast[]) {
        return <table className='table'>
            <thead>
                <tr>
                    <th>EmployeeId</th>
                </tr>
            </thead>
            <tbody>
                {weatherForecasts.map(e =>
                    <tr key={e.id}>
                        <td></td>
                        <td>{e.summary}</td>
                    </tr>
                )}
            </tbody>
        </table>;
    }
}