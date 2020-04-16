import * as React from 'react';
import { Action, Reducer } from 'redux';

export interface iCompanyState {
    compId: number,
    compName: string
}

export interface iCompaniesState {
    isLoading: boolean,
    companies: iCompanyState[]
}

export interface GetCompaniesAction {
    type: 'GET_COMPANIES_REDUX',
    isLoading: true
}

export interface GetCompaniesFetchedAction{
    type: 'GET_COMPANIES_FETCHED_REDUX',
    isLoading: false,
    companies: iCompanyState[]
}

export type CompaniesActionTypes = GetCompaniesAction | GetCompaniesFetchedAction;


//QUESTION: can we pass in an initial state here so it doesn't have to be be handled in the redcuer
//QUESTION - if so, which is best way (in teh contexct example I do the above ^^).
//problem here - React has a reducer as well as Redux - Redux is the correct one but quixk fix reached for React varoant here
const companiesReducer: Reducer<iCompaniesState> = (state: iCompaniesState | undefined, incomingAction: Action): iCompaniesState => {
    if (state === undefined)
        return {isLoading: true, companies: []};
    const action = incomingAction as CompaniesActionTypes
    switch(action.type){
        case ('GET_COMPANIES_REDUX'):
            return{
                isLoading: action.isLoading,
                companies: []
            };
            case ('GET_COMPANIES_FETCHED_REDUX'):
                return{
                    isLoading: action.isLoading,
                    companies: action.companies
                };
        default:
            return state

    }
}; export default companiesReducer;