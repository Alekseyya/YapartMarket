import * as React from "react";

export class MenuSearch extends React.Component {
    render() {
        return <div className="sectionSearch container">
            <div className="search row">
                <div className="searchBlock">
                    <div className="title">Поиск в каталоге</div>
                    {/* <input type="text" className="text_search" placeholder="Введите фразу для поиска...">
                        <div className="search"></div>
                    </input> */}
                </div>
                <div className="mottoCompany">
                    <span>Наша цель</span>
                    - внимание к каждому клиенту
                </div>
            </div>
        </div>
    }
}