import * as React from 'react';
import { connect } from 'react-redux';

class Home extends React.Component {
   public render(){
     return(
       <div className="contaiver">
         <div className="row">
           <div className="wrapper_product">
             <div className="image"></div>
             <div className="productModel">sadasd</div>
             <div className="description">asdasd</div>             
           </div>
         </div>
       </div>
     )
   }  
}

export default connect()(Home);
