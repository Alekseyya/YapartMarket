import * as React from "react";
import { MainMenu } from "./MainMenu";
import { MenuSearch } from "./MenuSearch";

export class Header extends React.Component {
    render() {
        return <div>
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
            <MainMenu />
            <MenuSearch />
        </div>
    }
}