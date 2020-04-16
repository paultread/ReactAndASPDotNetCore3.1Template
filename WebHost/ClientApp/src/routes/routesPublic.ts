import * as React from 'react';
//import all view type components that require a route to view them
//NOTE these should all be in the 'viewcomponents' dir
const HomeViewComponent = React.lazy(() => import('../viewcomponents/Home'));
const CounterComponent = React.lazy(() => import ('../viewcomponents/Counter'));
const CounterComponentTwo = React.lazy(() => import ('../viewcomponents/CounterComponent'));
const WeatherComponent = React.lazy(() => import('../viewcomponents/FetchData'));
const PostsComponent = React.lazy(() => import('../viewcomponents/PostsComponent'));

const CompaniesReduxClassComponent = React.lazy(() => import ('../ReduxVersion/components/CompaniesComponentRedux'));
const CompaniesReduxFuncComponent = React.lazy(() => import ('../ReduxVersion/components/CompaniesComponentReduxFunc'));

const CompaniesContextFuncComponent = React.lazy(() => import ('../ContextVersions/components/CompaniesComponent'));

const routesPublic = [
    {path: '/', exact: true, name: 'Home', component: HomeViewComponent}
]; export default routesPublic;

