import { Action, Reducer } from 'redux';
//import { IncrementCountAction, DecrementCountAction } from '../store/Counter';
import { IncrementCountAction, DecrementCountAction } from '../../viewcomponents/CounterComponent';

export interface iCounterState {
    numCounter: number;
}
export type KnownAction = IncrementCountAction | DecrementCountAction;


const counterReducer: Reducer<iCounterState> = (state: iCounterState | undefined, incomingAction: Action): iCounterState => {
    //different way to handle initial state which seems cleaner than having initState
    //console.log(state);
    //console.log(' i am a test');
    if (state === undefined) {
        return { numCounter: 0 };
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'INCREMENT_COUNT_IN_COMPONENT':
            return { numCounter: state.numCounter + 1 };
        case 'DECREMENT_COUNT_IN_COMPONENT':
            return { numCounter: state.numCounter - 1 };
        default:
            return state;
    }
}

export default counterReducer;


//export const reducer: Reducer<CounterState> = (state: CounterState | undefined, incomingAction: Action): CounterState => {
//    if (state === undefined) {
//        return { count: 0 };
//    }

//    const action = incomingAction as KnownAction;
//    switch (action.type) {
//        case 'INCREMENT_COUNT':
//            return { count: state.count + 1 };
//        case 'DECREMENT_COUNT':
//            return { count: state.count - 1 };
//        default:
//            return state;
//    }
//};