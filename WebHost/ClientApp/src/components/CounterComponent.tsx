import * as React from 'react';
//import { Dispatch } from 'redux';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import { iCounterState } from '../store/counterReducer';
//import counterActions from '../actions/counterActions';
//import { IncrementCountAction, DecrementCountAction } from '../actions/counterActions'
//import * as CounterStore from '../store/Counter';


export interface IncrementCountAction { type: 'INCREMENT_COUNT_IN_COMPONENT' }
export interface DecrementCountAction { type: 'DECREMENT_COUNT_IN_COMPONENT' }

type CounterComponentProps =
    iCounterState &
    typeof actionCreators &
    RouteComponentProps<{}>;


class CounterComponent extends React.PureComponent<CounterComponentProps> {
    public render() {
        return (
            <React.Fragment>
                <h1>Counter Component</h1>
                <p>This is a simple example of a React component.</p>
                <p aria-live="polite">Current count: <strong>{this.props.numCounter}</strong></p>
                <button type="button"
                    className="btn btn-primary btn-lg"
                    onClick={() => { this.props.incrementMe(); }}>
                    Increment
                </button>
            </React.Fragment>
        );
    }
};



//even though using mapDispatchToProps, still have to declare actionCreators - to set the component type
//use thunk here if you dont want to return an action object but a function instead, and then you can call e.g. API endpoints
//IMPORTANT: if these strings are the same as any other action they might end up mutating another state instance (bot counter and countercomponent were being mutated)
export const actionCreators = {
    incrementMe: () => ({ type: 'INCREMENT_COUNT_IN_COMPONENT' } as IncrementCountAction),
    decrementMe: () => ({ type: 'DECREMENT_COUNT_IN_COMPONENT' } as DecrementCountAction)
};


// STATE - This defines the type of data maintained in the Redux store.



// const mapStateToProps = (state: ApplicationState) => {
//     return {
//         counterComponentReducer: state.counterComponentReducer
//     }
// }

export default connect(
    //mapStateToProps,
    (state: ApplicationState) => state.counterComponentReducer,
    actionCreators
    //mapDispatchToProps
    //actionCreators2
)(CounterComponent);

//type CounterProps =
//    iCounterState &
//    typeof counterActions &
//    RouteComponentProps<{}>;

//type CounterProps2 =
    //iCounterState &
    //typeof mapStateToProps &
    //ApplicationState &
    //typeof mapDispatchToProps &
    //typeof mapDispatchToProps &
    //Function &
    
    //IncrementCountAction &
    //typeof actionCreators2 &
    //iMapDispatchToProps &
    //typeof mapDispatchToProps.incrementMe &
    //typeof actionCreators &
    //RouteComponentProps<{}>;




// interface iMapDispatchToProps {
//     incrementMe: () => IncrementCountAction
// }

// const mapDispatchToProps = (dispatch: Dispatch ) => {
//     console.log('I am a testing this out');
//     //const mapDispatchToProps = () => {
//     return {
//         incrementMe: () => ({ type: 'INCREMENT_COUNT' } as IncrementCountAction)
//         //incrementCounter: () => {dispatch(counterActions.incrementCounter())},
//         //decrementCounter: () => {dispatch(counterActions.decrementCounter())}
//         //incrementMe: () => (dispatch({ type: 'INCREMENT_COUNT' }) as IncrementCountAction),
//         //decrementMe: () => (dispatch({ type: 'DECREMENT_COUNT' }) as DecrementCountAction)
//     }
// }


// const mapDispatchToPropss = (dispatch: Dispatch) => {
//     dispatch({ type: 'INCREMENT_COUNT_STRAIGHT_ACTION' })
// }



//export const actionCreators2 = {
    //incrementMe: (dispatch: Dispatch) => (dispatch({ type: 'INCREMENT_COUNT' }) as IncrementCountAction),
    //decrementMe: (dispatch: Dispatch) => (dispatch({ type: 'DECREMENT_COUNT' }) as DecrementCountAction)
//};





