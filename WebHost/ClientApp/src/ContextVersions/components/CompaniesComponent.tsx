import * as React from 'react';
import thunk from 'redux-thunk';
import {iCompanyType} from './CompanyComponent';
import {CompanyContext} from '../contexts/CompaniesContext';
import axiosService from 'axios'
import {Table, TableRow, TableCell, TableHeader} from 'carbon-react/lib/components/table';
import { AxiosResponse, AxiosError } from 'axios'
 

export interface iCompaniesComponentType {
    Companies: iCompanyType[]
    ,isLoading: boolean
}

const CompanListComponent = () => {
    const {state, dispatch} = React.useContext(CompanyContext);

    const fetchCompanies = async() => {
        dispatch({ type: 'GET_COMPANIES_FETCH', isLoading: true });
        if (state.isLoading = true){
            try{
                await axiosService.get('http://localhost:59333/Companies')
                .then((response: any) => {
                    //const data: iCompanyType[] = response.data;
                    const {data} = response;
                    //console.log(data);
                    dispatch({type: 'GET_COMPANIES_FETCH_COMPLETE', Companies: data, isLoading: false
                });
                })
                .catch((error: AxiosError) => {
                    console.log('pos1: ',error);
                })
            }
            catch(error){
                console.log('pos2: ', error);
            }
        }
       

    };
    //will run on render / rerender (componentDidMount() / componentDidupdate())
    React.useEffect(() =>{
        fetchCompanies();
    }, []);

    let tableRows = (
        state.Companies && state.Companies.map((company: iCompanyType, key: number) => {
            return (
                <TableRow key={key}>
                  <TableCell>
                    {company.compId!== 0 ? company.compId : ''}
                  </TableCell>
                  <TableCell>
                    {company.compName}
                  </TableCell>
                </TableRow>
            )
        })
    )
    tableRows.unshift(
        <TableRow key={-1}>
                <TableHeader>Id</TableHeader>
                <TableHeader>Name</TableHeader>
        </TableRow>
    );
    return(
        <React.Fragment>
            <Table>
                {tableRows}
            </Table>

        </React.Fragment>

    )
}; export default CompanListComponent;