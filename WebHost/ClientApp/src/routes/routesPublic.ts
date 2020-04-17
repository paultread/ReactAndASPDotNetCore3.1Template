import * as React from 'react';
//import all view type components that require a route to view them
//NOTE these should all be in the 'viewcomponents' dir
//Lazy is for splitting the bundle - so only importing (and hence rendering them) when the DOM requires them... end components using these imports require suspence to be used - to have a e.g. loading tag 
const HomeViewComponent = React.lazy(() => import('../viewcomponents/Home'));
const CounterComponent = React.lazy(() => import ('../viewcomponents/Counter'));
const CounterComponentTwo = React.lazy(() => import ('../viewcomponents/CounterComponent'));
const WeatherComponent = React.lazy(() => import('../viewcomponents/FetchData'));
const PostsComponent = React.lazy(() => import('../viewcomponents/PostsComponent'));

const CompaniesReduxClassComponent = React.lazy(() => import ('../ReduxVersion/components/CompaniesComponentRedux'));
const CompaniesReduxFuncComponent = React.lazy(() => import ('../ReduxVersion/components/CompaniesComponentReduxFunc'));

const CompaniesContextFuncComponent = React.lazy(() => import ('../ContextVersions/components/CompaniesComponent'));

const routesPublic = [
    {path: '/', exact: true, name: 'Home', component: HomeViewComponent},
    {path: '/counter', exact: true, name: 'Counter', component: CounterComponent},
    {path: '/countertwo', exact: true, name: 'Counter Component', component: CounterComponentTwo},
    {path: '/weather', exact: true, name: 'Fetch Weather', component: WeatherComponent},
    {path: '/posts', exact: true, name: 'Fetch Posts from JSON API', component: PostsComponent},
    {path: '/companiescontext', exact: true, name: 'Counter', component: CompaniesContextFuncComponent},
    {path: '/companiesreduxclass', exact: true, name: 'Counter', component: CompaniesReduxClassComponent},
    {path: '/companiesreduxfunction', exact: true, name: 'Counter', component: CompaniesReduxFuncComponent},
]; export default routesPublic;

