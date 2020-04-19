import * as React from "react";

export class MainMenu extends React.Component {
    render() {
        return <div className="sectionMainMenu">
            <div className="mainMenu container">
                <div className="mainMenuWrapper row">
                    <ul className="menuList">
                        <li>
                            <a href="/">
                                <div className="block_icon"></div>
                                <div className="block_title">Каталог товаров</div>
                            </a>
                        </li>
                        <li>
                            <a href="/">
                                <div className="block_icon"></div>
                                <div className="block_title">Как заказать</div>
                            </a>

                        </li>
                        <li>
                            <a href="/">
                                <div className="block_icon"></div>
                                <div className="block_title">Доставка и оплата</div>
                            </a>

                        </li>
                        <li>
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
                        <li>
                            <a href="/">
                                <div className="block_icon"></div>
                                <div className="block_title">Оптовикам</div>
                            </a>

                        </li>
                        <li>
                            <a href="/">
                                <div className="block_icon"></div>
                                <div className="block_title">Акции</div>
                            </a>

                        </li>
                    </ul>
                </div>
            </div>
        </div>
    }
}