import * as React from "react";
import { Container, Row } from "reactstrap";
import "../../styles/mainMenu.css";
export class MainMenu extends React.Component {
    render() {
        return <div className="sectionMainMenu">
            <Container className="mainMenu" >
                <Row className="mainMenuWrapper">
                <ul className="menuList nav">
                        <li className="nav-item">
                            <a href="/catalog">
                                <div className="block_icon"></div>
                                <div className="block_title">Каталог товаров</div>
                            </a>
                        </li>
                        <li className="nav-item">
                            <a href="/">
                                <div className="block_icon"></div>
                                <div className="block_title">Как заказать</div>
                            </a>

                        </li>
                        <li className="nav-item">
                            <a href="/">
                                <div className="block_icon"></div>
                                <div className="block_title">Доставка и оплата</div>
                            </a>

                        </li>
                        <li className="nav-item">
                            <a href="/">
                                <div className="block_icon"></div>
                                <div className="block_title">Полезные советы</div>
                            </a>

                        </li>
                        <li>
                            <a href="/">
                                <div className="block_icon"></div>
                                <div className="block_title">Контакты</div>
                            </a>

                        </li>
                        <li className="nav-item">
                            <a href="/">
                                <div className="block_icon"></div>
                                <div className="block_title">Оптовикам</div>
                            </a>

                        </li>
                        <li className="nav-item">
                            <a href="/">
                                <div className="block_icon"></div>
                                <div className="block_title">Акции</div>
                            </a>

                        </li>
                    </ul>
                </Row>
            </Container>            
        </div>
    }
}