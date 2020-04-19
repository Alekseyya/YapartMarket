import * as React from "react";
import { RoutePaths } from '../Routes';
import { RouteComponentProps } from "react-router";
import { Link, Redirect } from 'react-router-dom';

export class Header extends React.Component{
    render(){
        return <div className="container">
            <div className="wrap headerContainer">
                <div className="headerLogo">
                    <a href="/" className="aLogo">
                        <div className="logo"></div>
                    </a>
                </div>
            </div>
            <div className="row">
                <h1>Hello3333333333333333333 </h1>
            </div>
        </div>
    }
}