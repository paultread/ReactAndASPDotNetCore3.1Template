import * as React from 'react';
import { Route } from 'react-router';
import Layout from './components/Layout';
import Home from './components/Home';
import Counter from './components/Counter';
import FetchData from './components/FetchData';
import CounterComponent from './components/CounterComponent'
import PostsComponent from './components/PostsComponent';
import CompanyComponent from './ContextVersions/components/CompanyComponent';
import CompanyComponentRedux from './ReduxVersion/components/CompaniesComponentRedux';
import CompanyComponentReduxFunc from './ReduxVersion/components/CompaniesComponentReduxFunc';
// import 'carbon-react/lib/utils/css'; // import Sage Carbon styles
//import './scss/main.scss'; // import global styles


import './custom.css'

export default () => (
    <Layout>
        <Route exact path='/' component={Home} />
        <Route path='/counter' component={Counter} />
        <Route exact path='/countercomponent' component={CounterComponent} />
        <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
        <Route path='/posts' component={PostsComponent} />
        <Route path='/companiescontext' component={CompanyComponent} />
        <Route path='/companiesredux' component={CompanyComponentRedux} />
        <Route path='/companiesreduxfunc' component={CompanyComponentReduxFunc} />
    </Layout>
);
