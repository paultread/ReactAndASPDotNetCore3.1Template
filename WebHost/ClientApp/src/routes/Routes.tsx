import * as React from 'react';
import {Dispatch} from 'redux';

import {Route, Switch} from 'react-router-dom';

import routesPublic from '../routes/routesPublic';
import { ApplicationState } from '../store/index';
import { connect } from 'react-redux';


interface RoutesComponentProps {
    dispatch: Dispatch;
}

const Routes = (props: RoutesComponentProps) => {
    return (
        <React.Fragment>
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