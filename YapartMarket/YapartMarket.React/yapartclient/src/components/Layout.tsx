import * as React from 'react';
import { Container } from 'reactstrap';
import Header from './header/Header';

export default (props: { children?: React.ReactNode }) => (
    <React.Fragment>
        <Header/>
        <Container>
            {props.children}
        </Container>
    </React.Fragment>
);
