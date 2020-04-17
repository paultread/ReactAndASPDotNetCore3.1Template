import * as React from 'react';
import {Dispatch} from 'redux';

import {Route, Switch} from 'react-router-dom';
import { Container } from 'reactstrap';
import routesPublic from '../routes/routesPublic';
import { ApplicationState } from '../store/index';
import { connect } from 'react-redux';

import Layout from '../components/Layout';


interface RoutesComponentProps {
    dispatch: Dispatch;
}

const loadingDiv = () => {
    return (
        <div>Loading...</div>
    )
}

const Routes = (props: RoutesComponentProps) => {
    return (
        <React.Fragment>
            <Layout>
                 {/* Bootstrap React container */}
                <Container>
                    <React.Suspense fallback={loadingDiv()}>{/* Needed as if JSX is not rendered yet it needs a placeholder... needed because we use React.lazy() to import elements*/}
                        <Switch>
                            {routesPublic.map((route, idx) => {
                                return route.component ? (
                                    <Route
                                    key={idx}
                                    path={route.path}
                                    exact={route.exact}
                                    component={route.component}
                                    />
                                )
                                : 
                                null
                            })}
                        </Switch>
                    </React.Suspense>
                
                </Container>
           
            </Layout>
           
        </React.Fragment>
    )
}

function mapStateToProps(state: ApplicationState){
    return{
        testerProp: 'pass in the data you need to be in the store from here'
    };
}

function mapDispatchToProps(dispatch: Dispatch){
    return {
        dispatch
    };
}

export default connect(mapStateToProps, mapDispatchToProps)(Routes);