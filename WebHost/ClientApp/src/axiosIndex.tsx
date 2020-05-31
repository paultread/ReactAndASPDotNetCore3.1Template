import * as React from 'react';
import axios, { AxiosRequestConfig } from 'axios';
import {store} from './index'
import {actionCreators} from '../src/ReduxVersion/reduxStore/signInStore';

//https://github.com/axios/axios/issues/587
//'set-cookie' to get this to run on the browser, need to setup CORS on server - ensure that 'Access-Control-Allow-Credentials' header = 'true'(AllowCredentials() is added to policy)


let requestQueue: any[] = []; //any request that gets made whilst silent refresh is ongoing gets added to here
let currentlyTryingRefreshOfToken: boolean = false; // will be set to true to try the refresh once, and then set back to false so it is not tried again



axios.defaults.withCredentials = true;

const axiosServiceWithAuthHeader = axios.create({
    baseURL: process.env.NODE_ENV === 'development' ? 'https://localhost:44309/' : 'www.someurl.com/',
    timeout: 30000

});



axiosServiceWithAuthHeader.interceptors.request.use((config: AxiosRequestConfig) => {
    //console.log('store contents :', store.getState().authReducer);
    //each axios request, send the JWT token (which will be null or will have one in)
    const authToken: string = store.getState().authReducer.jwtAccessToken;
    config.headers.Authorization = `Bearer ${authToken}`;
    //config.headers.httpsAgent = true;
    //config.headers.Add("ddd");

    //HERE
    //this is initiating only once so the header never has token in it
    //just discovered store.dispatch is needed to dispatch globally, not just dispatch.
    console.log(config.headers);
    return config;
});

//any axios call uses either a null auth token 


//land on homepage - pulls in settisgs via axios call
    //does it use withAuth or standard?
        //shoudl it use withAuth or standard?
            //should; we have two axios instances or not? << doubt it




axiosServiceWithAuthHeader.interceptors.response.use(
    response => {
        console.log('jjjjj : ', response)
        return response
    },
    error => {
        //https://github.com/axios/axios/issues/838
        //this seems to be the only way to test as the preflight gets stung by a cors error when it does not meet the conditions of the auth tag at the server, so no response codes
        //404 would return on same site
        const {response: errorResponse} = error;
        console.log(errorResponse);
        if (typeof error.response === 'undefined' 
            // || error.toString() === 'Error: Network Error' 
            || error.response.status === 404
            )
        {
            return RefreshTokenThenReAttemptRequest(error);
        }
        else{
            console.log('error not met');
        }
        //Although I fetch app settings it appears I don't store them in the central state as I am setting up for only JWT, so I can just assume it is JWT... call to settings on initial landing page might be redeundant then
        return Promise.reject(error);
    }
);

function RefreshTokenThenReAttemptRequest(error: any) : any {
    try{
        const {response: errorResponse} = error;
        const retryOriginalRequest = new Promise(resolve => {
            addRequester((token: string) => {
                errorResponse.config.headers.Authorization = `Bearer ${token}`;
                resolve(axiosServiceWithAuthHeader(errorResponse.config));
            })
        });
        if (!currentlyTryingRefreshOfToken){
            currentlyTryingRefreshOfToken = true;
            console.log('before: ', store.getState());
            store.dispatch<any>(actionCreators.postRefresh())
            //console.log('just a testin');
            console.log('after : ', store.getState());
        }
        return retryOriginalRequest;
    }
    catch(err){
        return Promise.reject(err)
    }
}

function addRequester(callback: any){
    requestQueue.push(callback);
}


export default axiosServiceWithAuthHeader;