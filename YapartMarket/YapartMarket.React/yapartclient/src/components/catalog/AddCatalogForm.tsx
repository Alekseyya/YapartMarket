import * as React from 'react';
import { connect } from 'react-redux';
import { AddProduct } from '../../actions/catalog';

const dispatchProps = {
    addItem: AddProduct,
  };

interface IState {
    Name : string
}

interface IProps {
    addItem : (Name: string) => void
}

export class AddCatalogForm extends React.Component<IProps, IState>{   
    constructor(prop : IProps) {
        super(prop);
        this.state = { Name : ''}; 
        this.handleAddClick = this.handleAddClick.bind(this);
        this.handleChange = this.handleChange.bind(this);       
    }

    handleChange: React.ReactEventHandler<HTMLInputElement> = e =>{
        this.setState({Name : e.currentTarget.value});
    }
    handleAddClick(){
        this.props.addItem(this.state.Name);
        this.setState({ Name: '' });
    }

    render(){
        return(
            <form>
                <input style={{width : 65}} onChange={this.handleChange}></input>
                <button type="submit" onClick={this.handleAddClick}>Add</button>
            </form>
        )
    }
}

export default connect(null,dispatchProps)(AddCatalogForm)