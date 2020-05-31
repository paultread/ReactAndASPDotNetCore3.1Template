import * as React from 'react';
import { iCompanyState, iCompaniesState} from '../reduxStore/companiesStore';
import axiosService, {AxiosResponse, AxiosError} from 'axios';
import axiosServiceWithAuthHeader from '../../axiosIndex';
import { Table, TableRow, TableCell, TableHeader } from 'carbon-react/lib/components/table';

import {AppThunkAction, ApplicationState } from '../../store/index';
import {CompaniesActionTypes} from '../reduxStore/companiesStore';
import {connect} from 'react-redux';
import { RouteComponentProps } from 'react-router';


type PostComponentTypes = 
    iCompaniesState & 
    typeof actionCreators &
    RouteComponentProps<{}>;

    //QUESTION: how to make this an asybc? see ContextVersaions > components > CompaniesComponent.tsx
export const actionCreators = {
    requestCompanies: () : AppThunkAction<CompaniesActionTypes> => (dispatch, getState) => {
        const appState = getState();
        if (appState && appState.companiesReducer && appState.companiesReducer.isLoading){
            //axiosService.get('http://localhost:59333/Companies')
            axiosServiceWithAuthHeader.get('http://localhost:59333/Companies')
            .then((response: AxiosResponse<iCompanyState[]>) => {
                const {data} = response;
                dispatch({type:'GET_COMPANIES_FETCHED_REDUX', isLoading: false, companies: data})
            })
            .catch((error: AxiosError) => {
                console.log('AXIOSERROR::' + error.message);
                throw error;
            })
        }
      
    }
};


class CompanyComponentRedux extends React.PureComponent<PostComponentTypes> {
    public componentDidMount = () => {
        this.props.requestCompanies();
    }
    public componentDidUpdate(){
        this.props.requestCompanies();
    }

    public render() {
        //#region propsInRender
        let headersForTable = (
            <TableRow key={-1}>
                <TableHeader>Id</TableHeader>
                <TableHeader>Name</TableHeader>
            </TableRow>
        );
        let RenderCompaniesInCarbonTable = this.props.companies.length  
            ? this.props.companies.map(company => {
                return(
                        <TableRow key={company.compId} >
                        <TableCell>
                            {company.compId}
                        </TableCell>
                        <TableCell>
                            {company.compName}
                        </TableCell>
                    </TableRow>
                )
            })          
            : <TableRow>
                <TableCell>
                    Data Loading...
                </TableCell>
            </TableRow>
        ;

        //#endregion
    
        return (
            <React.Fragment>
                 <Table
                    paginate // Show the pagination footer
                    currentPage="1" // Required - Current visible page
                    pageSize="2" // Required - Number of records to show per page
                    totalRecords={this.props.companies.length|| 0} // Required - Total number of records
                    >
                    {renderHeadersForTable()}
                    {this.props.companies && this.props.companies.length ? renderRowsForTable(this.props.companies) : renderLoadingRowForTable()}
                </Table>
            </React.Fragment>
        );
    }
}; export default connect(
    (state: ApplicationState) => state.companiesReducer,
    actionCreators
)(CompanyComponentRedux as any);



const renderHeadersForTable = () => {
    return (
        <TableRow key={-1}>
        <TableHeader>Id</TableHeader>
        <TableHeader>Name</TableHeader>
    </TableRow>
    )
}
const renderLoadingRowForTable = () => {
    return(
        <TableRow>
            <TableCell>
                Data Loading...
            </TableCell>
        </TableRow>
    )
}
const renderRowsForTable = (companies :iCompanyState[]) => {
    let RenderCompaniesInCarbonTable = companies.map(company => {
            return(
                    <TableRow key={company.compId} >
                    <TableCell>
                        {company.compId}
                    </TableCell>
                    <TableCell>
                        {company.compName}
                    </TableCell>
                </TableRow>
            )
        })          
    ;
    return(
        RenderCompaniesInCarbonTable
    )
}