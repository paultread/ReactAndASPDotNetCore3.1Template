import * as React from 'react';
import {useDispatch} from 'react-redux';
//import { Route } from 'react-router';
import { actionCreators } from './store/AppSettings'

import Routes from './routes/Routes';


// import Layout from './components/Layout';
// import Home from './viewcomponents/Home';
// import Counter from './viewcomponents/Counter';
// import FetchData from './viewcomponents/FetchData';
// import CounterComponent from './viewcomponents/CounterComponent'
// import PostsComponent from './viewcomponents/PostsComponent';
// import CompanyComponent from './ContextVersions/components/CompanyComponent';
// import CompanyComponentRedux from './ReduxVersion/components/CompaniesComponentRedux';
// import CompanyComponentReduxFunc from './ReduxVersion/components/CompaniesComponentReduxFunc';
// import 'carbon-react/lib/utils/css'; // import Sage Carbon styles
//import './scss/main.scss'; // import global styles


import './custom.css'

// export default () => (
        // layout is navbar
//     <Layout>
//          <Route exact path='/' component={Home} />
//          <Route path='/counter' component={Counter} />
//          <Route exact path='/countercomponent' component={CounterComponent} />
//          <Route path='/fetch-data/:startDateIndex?' component={FetchData} />
//          <Route path='/posts' component={PostsComponent} />
//          <Route path='/companiescontext' component={CompanyComponent} />
//          <Route path='/companiesredux' component={CompanyComponentRedux} />
//          <Route path='/companiesreduxfunc' component={CompanyComponentReduxFunc} />
//     </Layout>
// );

const App = () => {
     const dispatch: any = useDispatch();
     React.useEffect(() => dispatch(actionCreators.requestAppSettings()), [dispatch]);
  
    return (
        // This is the nav bar
        // <Layout> 
            <Routes/>
        // </Layout>

    )
}; 
export default App;
