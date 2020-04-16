import * as React from 'react';
import {CompaniesActionTypes} from '../reduxStore/companiesStore';
import { iCompanyState, iCompaniesState} from '../reduxStore/companiesStore';
import { AppThunkAction, ApplicationState } from '../../store/index';
import axiosService, {AxiosResponse, AxiosError} from 'axios';
import companiesReducer from '../reduxStore/companiesStore';
import {connect} from 'react-redux';
import { RouteComponentProps } from 'react-router';
import { Table, TableRow, TableCell, TableHeader } from 'carbon-react/lib/components/table';



const CompaniesComponenteduxFunc =() =>{
    //const [companies, setCompanies] = React.useState([{compId: 50, compName: 'ggg'}]);
    //const allReducerStuff = React.useReducer(companiesReducer, {companies:[], isLoading: true});
    //console.log(allReducerStuff);
    //console.log(companiesReducer);
    const [companies, dispatch] = React.useReducer(companiesReducer, {companies:[], isLoading: true}); 

    //const requestCompanies: AppThunkAction<CompaniesActionTypes> = async(dispatch, getState) => {
    const requestCompanies = async() => {
        //const appState = getState();
        if (companies.isLoading){
            await axiosService.get('http://localhost:59333/Companies')
            .then((response: AxiosResponse<iCompanyState[]>) => {
                const {data} = response;
                console.log(data);
                dispatch({type:'GET_COMPANIES_FETCHED_REDUX', isLoading: false, companies: data})
            })
            .catch((error: AxiosError) => {
                console.log('AXIOSERROR::' + error.message);
                throw error;
            })
        }
    };

    React.useEffect(() =>{
        requestCompanies()
    }, []);


    return(
          <React.Fragment>
          <Table
           paginate // Show the pagination footer
           currentPage="1" // Required - Current visible page
           pageSize="2" // Required - Number of records to show per page
           totalRecords={companies.companies.length|| 0} // Required - Total number of records
          >
              {renderHeadersForTable()}
              {companies && companies.companies && companies.companies.length ? renderRowsForTable(companies.companies) : renderLoadingRowForTable()}
          </Table>
      </React.Fragment>
    )


}; export default CompaniesComponenteduxFunc;

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
    let RenderCompaniesInCarbonTable = companies.map((company: iCompanyState) => {
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