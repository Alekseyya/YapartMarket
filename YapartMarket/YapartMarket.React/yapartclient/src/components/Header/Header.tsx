import * as React from 'react';
import { Collapse, Container, Navbar, NavbarBrand, NavbarToggler, NavItem, NavLink } from 'reactstrap';
import { Link } from 'react-router-dom';
import '../../styles/Header.css';
import { MainMenu } from "./MainMenu";
import { MenuSearch } from "./MenuSearch";

export default class NavMenu extends React.PureComponent<{}, { isOpen: boolean }> {
    public state = {
        isOpen: false
    };
    public render() {
        return (
            <header>
                <div>
                    <div className="sectionHeader">
                        <div className="header container-fluid">
                            <div className="wrapper row">
                                <div className="headerLogo col-sm-5">
                                    <a href="">
                                        <div className="logo"></div>
                                    </a>
                                    <span className="logoNameCompany">интернет-магазин автозапчастей</span>
                                </div>
                                <div className="addressCompany col-sm-3">
                                    <div className="title">Наш адрес:</div>
                                    <div className="address">ул. Пионерская </div>
                                </div>
                                <div className="contacts col-sm-3 row">
                                    <div className="phoneLogo col-sm-2"></div>
                                    <div className="phones col-sm-10 row align-items-center">
                                        <div className="itemPhone">+7 (953)192-25-25</div>
                                        <div className="itemPhone">+7 (956)154-54-89</div>
                                    </div>
                                </div>
                                <div className="cart col-sm-1">
                                    <div className="cartLogo"></div>
                                    <div className="cartInformation">
                                        <div className="goods">Товаров
                            <span className="goodsValue"></span>
                                        </div>
                                        <div className="amount">
                                            Сумма
                            <span className="amountValue"></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>                
                <MainMenu />
                <MenuSearch />
            </header>
        );
    }


    // public render() {
    //     return (
    //         <header>
    //             <Navbar className="navbar-expand-sm navbar-toggleable-sm border-bottom box-shadow mb-3" light>
    //                 <Container>
    //                     <NavbarBrand tag={Link} to="/">WebApplication1</NavbarBrand>
    //                     <NavbarToggler onClick={this.toggle} className="mr-2"/>
    //                     <Collapse className="d-sm-inline-flex flex-sm-row-reverse" isOpen={this.state.isOpen} navbar>
    //                         <ul className="navbar-nav flex-grow">
    //                             <NavItem>
    //                                 <NavLink tag={Link} className="text-dark" to="/">Home</NavLink>
    //                             </NavItem>
    //                             <NavItem>
    //                                 <NavLink tag={Link} className="text-dark" to="/counter">Counter</NavLink>
    //                             </NavItem>
    //                             <NavItem>
    //                                 <NavLink tag={Link} className="text-dark" to="/fetch-data">Fetch data</NavLink>
    //                             </NavItem>
    //                             <NavItem>
    //                                 <NavLink tag = {Link} className="text-dark" to="/cart">Cart</NavLink>
    //                             </NavItem>
    //                         </ul>
    //                     </Collapse>
    //                 </Container>
    //             </Navbar>
    //         </header>
    //     );
    // }

    private toggle = () => {
        this.setState({
            isOpen: !this.state.isOpen
        });
    }
}
