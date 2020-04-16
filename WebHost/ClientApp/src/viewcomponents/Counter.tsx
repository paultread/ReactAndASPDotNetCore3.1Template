import * as React from 'react';
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { ApplicationState } from '../store';
import * as CounterStore from '../store/Counter';

type CounterProps =
    CounterStore.CounterState &
    typeof CounterStore.actionCreators &
    RouteComponentProps<{}>;

class Counter extends React.PureComponent<CounterProps> {
    public render() {
        console.log(this.props);
        return (
            <React.Fragment>
                <h1>Counter</h1>

                <p>This is a simple example of a React component.</p>
                <p aria-live="polite">Current count: <strong>{this.props.count}</strong></p>

                <button type="button"
                    className="btn btn-primary btn-lg"
                    onClick={() => { this.props.increment(); }}>
                    Increment
                </button>
            </React.Fragment>
        );
    }
};

//connect is a function that returns a higher order wrapping component setup with state management (redux)... 
//...this then wraps the Counter and returns it
//export default connect(
//    mapStateToProps,
//    CounterStore.actionCreators
//)(Counter);
export default connect(
    (state: ApplicationState) => state.counter,
    CounterStore.actionCreators
)(Counter);
