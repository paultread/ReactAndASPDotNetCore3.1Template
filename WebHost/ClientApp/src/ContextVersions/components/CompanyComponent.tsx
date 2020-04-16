import * as React from 'react';
import CompanyContextProvider from '../contexts/CompaniesContext';
import CompanyList from './CompaniesComponent';

export interface iCompanyType {
    compId: number,
    compName: string
}


const CompanyComponent = () => {
    return(
        <div>
            <CompanyContextProvider>
                <CompanyList></CompanyList>
            </CompanyContextProvider>
        </div>
    )
}; export default CompanyComponent;