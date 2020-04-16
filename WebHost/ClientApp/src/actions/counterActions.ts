//do these need exporting or are they only required internally?
export interface IncrementCountAction { type: 'INCREMENT_COUNT' }
export interface DecrementCountAction { type: 'DECREMENT_COUNT' }

const counterActions = {
    incrementCounter: () => ({ type: 'INCREMENT_COUNT' } as IncrementCountAction),
    decrementCounter: () => ({ type: 'DECREMENT_COUNT' } as DecrementCountAction)
};

export default counterActions