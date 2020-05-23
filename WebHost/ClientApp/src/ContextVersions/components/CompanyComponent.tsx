import * as React from 'react';
import CompanyContextProvider from '../../ContextVersions/contexts/CompaniesContext';
import CompaniesComponent from './CompaniesComponent';

export interface iCompanyType {
    compId: number,
    compName: string
}


const CompanyComponent = () => {
    return(
        <div>
            <CompanyContextProvider>
                <CompaniesComponent></CompaniesComponent>
            </CompanyContextProvider>
        </div>
    )
}; export default CompanyComponent;