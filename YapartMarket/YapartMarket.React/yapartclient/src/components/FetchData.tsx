 import * as React from 'react';
// import { connect } from 'react-redux';
// import { RouteComponentProps } from 'react-router';
// import { Link } from 'react-router-dom';
// import { ApplicationState } from '../store';
// import * as Cart from '../store/Cart';
// import * as WeatherForecastsStore from '../store/WeatherForecasts';

// // At runtime, Redux will merge together...
// // type WeatherForecastProps =
// //   //WeatherForecastsStore.WeatherForecastsState // ... state we've requested from the Redux store
// //   Cart.CartState
// //   & typeof Cart.actionCreators
// //   & WeatherForecastsStore.ProductsState
// //   & typeof WeatherForecastsStore.actionCreators // ... plus action creators we've requested
// //   & RouteComponentProps<{ startDateIndex: string }>; // ... plus incoming routing parameters


// class FetchData extends React.PureComponent<WeatherForecastProps> {
//   // This method is called when the component is first added to the document
//   public componentDidMount() {
//     this.ensureDataFetched();
//   }

//   //По идее надо просто менять состояние, чтобы не происходала постоянная загрузка forest
//   //

//   // This method is called when the route parameters change
//   // public componentDidUpdate() {
//   //   this.ensureDataFetched();
//   // }

//   public render() {
//     return (
//       <React.Fragment>
//         <h1 id="tabelLabel">Weather forecast</h1>
//         <p>This component demonstrates fetching data from the server and working with URL parameters.</p>
//         {/* {this.renderForecastsTable()} */}
//         {/* {this.renderPagination()} */}
//         {this.renderProductsTable()}
//       </React.Fragment>
//     );
//   }

//   private ensureDataFetched() {
//     // const startDateIndex = parseInt(this.props.match.params.startDateIndex, 10) || 0;
//     // this.props.requestWeatherForecasts(startDateIndex);
//     this.props.requestProducts(); // Вот тут убрать и отправить в отдельный компонент с редуксером и состоянием
    
//   }

//   // private renderForecastsTable() {
//   //     return (
//   //       <table className='table table-striped' aria-labelledby="tabelLabel">
//   //         <thead>
//   //           <tr>
//   //             <th>Date</th>
//   //             <th>Temp. (C)</th>
//   //             <th>Temp. (F)</th>
//   //             <th>Summary</th>
//   //           </tr>
//   //         </thead>
//   //         <tbody>
//   //           {this.props.forecasts.map((forecast: WeatherForecastsStore.WeatherForecast) =>
//   //             <tr key={forecast.date}>
//   //               <td>{forecast.date}</td>
//   //               <td>{forecast.temperatureC}</td>
//   //               <td>{forecast.temperatureF}</td>
//   //               <td>{forecast.summary}</td>
//   //             </tr>
//   //           )}
//   //         </tbody>
//   //       </table>
//   //     );    
//   // }
// //Это в отдельный компонент
//   private renderProductsTable(){
//       return (
//         <table className='table table-striped' aria-labelledby="tabelLabel">
//           <thead>
//             <tr>
//               <th>Id</th>
//               <th>Description</th>
//               <th>Price</th>
//               <th></th>
//             </tr>
//           </thead>
//           <tbody>
//             {this.props.products.map((product: WeatherForecastsStore.Product) =>
//               <tr key={product.Id}>
//                 <td>{product.Id}</td>
//                 <td>{product.Descriptions}</td>
//                 <td>{product.Price}</td>
//                 <td><button className="button" onClick={() => this.props.removeItem(product.Id)}>
//                   Remove </button></td>
//                 <td>
//                   <button className="addToCart" onClick={() => {this.props.addToCart(product)}}>AddToCart</button>
//                 </td>
//               </tr>
//             )}
//           </tbody>
//         </table>
//       );   
//   }
// //Попробовать сделать, чтобы по кнопке переключались страницы, отправлялись запросы
//   // private renderPagination() {
//   //   const prevStartDateIndex = (this.props.startDateIndex || 0) - 5;
//   //   const nextStartDateIndex = (this.props.startDateIndex || 0) + 5;

//   //   return (
//   //     <div className="d-flex justify-content-between">
//   //       <Link className='btn btn-outline-secondary btn-sm' to={`/fetch-data/${prevStartDateIndex}`}>Previous</Link>
//   //       {this.props.isLoading && <span>Loading...</span>}
//   //       <Link className='btn btn-outline-secondary btn-sm' to={`/fetch-data/${nextStartDateIndex}`}>Next</Link>
//   //     </div>
//   //   );
//   // }
// }
// //(state: ApplicationState) => state.weatherForecasts,// Selects which state properties are merged into the component's props 

// export default connect(  
//   (state: ApplicationState) => state.products,
//   WeatherForecastsStore.actionCreators // Selects which action creators are merged into the component's props
// )(FetchData as any);
