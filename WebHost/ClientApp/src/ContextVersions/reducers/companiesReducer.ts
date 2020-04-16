import * as React from 'react';
import {CompanyAction} from '../contexts/CompaniesContext';
import {iCompaniesComponentType} from '../components/CompaniesComponent';

export const companyReducer = (state: iCompaniesComponentType, action: CompanyAction) => {
    switch (action.type){
        // case undefined :
        //     return state;
        case 'GET_COMPANIES_FETCH':
            return {
                ...state

                ,isLoading: false
                //,Companies: action.Companies
                //state: state.Companies
            }
        case 'GET_COMPANIES_FETCH_COMPLETE':
        return {
            ...state
            ,isLoading: false
            //,Companies: [{compId: 1, compName: 'ddd'}]
            ,Companies: action.Companies
            //state: state.Companies
        }
        default:
            return state;
    }

}