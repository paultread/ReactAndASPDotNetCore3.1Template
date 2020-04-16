import * as WeatherForecasts from './WeatherForecasts';
import * as Counter from './Counter';
import postsStoreReducer, {iPostsState}  from '../ReduxVersion/reduxStore/postsStore';

import counterReducer, { iCounterState } from '../store/counterReducer'

//Redux version
import companiesReducer, {iCompaniesState} from '../ReduxVersion/reduxStore/companiesStore';


//import { IncrementCountAction, DecrementCountAction } from '../actions/counterActions';

// The top-level state object - definition for it
export interface ApplicationState {
    counter: Counter.CounterState | undefined;
    counterComponentReducer: iCounterState | undefined;
    weatherForecasts: WeatherForecasts.WeatherForecastsState | undefined;
    postsComponentReducer: iPostsState | undefined;
    //redux version
    companiesReducer: iCompaniesState | undefined;
}

// Whenever an action is dispatched, Redux will update each top-level application state property using
// the reducer with the matching name. It's important that the names match exactly, and that the reducer
// acts on the corresponding ApplicationState property type.
export const reducers = {
    counter: Counter.reducer,
    counterComponentReducer: counterReducer,
    weatherForecasts: WeatherForecasts.reducer,
    postsComponentReducer: postsStoreReducer,
    //redux version
    companiesReducer: companiesReducer
};

// This type can be used as a hint on action creators so that its 'dispatch' and 'getState' params are
// correctly typed to match your store.
export interface AppThunkAction<TAction> {
    (dispatch: (action: TAction) => void, getState: () => ApplicationState): void;
}
