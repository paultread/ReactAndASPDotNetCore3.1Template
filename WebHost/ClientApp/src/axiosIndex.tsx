import * as React from 'react';
import axios, { AxiosRequestConfig } from 'axios';
import {store} from './index'
//overview of potential fix:
//1. HttpOnly, Secure, SameSite NOT needed on the actual cookie as per other guidance <, in order for Chrome to set them
//2. Access-Control-Allow-Credentials set to true might be needed
//3. Access-Control-Allow-Origin might need to be set to a specific site not *
//4 - get rid of the 'on starting' shebang


// this!
//https://github.com/axios/axios/issues/587
axios.defaults.withCredentials = true

const axiosServiceWithAuthHeader = axios.create({
    baseURL: process.env.NODE_ENV === 'development' ? 'https://localhost:44309/' : 'www.someurl.com/',
    timeout: 30000

});



export const axiosServiceWithSignInCredToTrue= axios.create({
    // baseURL: process.env.NODE_ENV === 'development' ? 'https://localhost:44309/' : 'www.someurl.com/',
    // timeout: 30000,
    withCredentials: true
    // ,
    // headers: {
    //     'Accept': 'application/json',
    //     'Content-Type': 'x-www-form-urlencoded',
    //   },
});


axiosServiceWithAuthHeader.interceptors.request.use((config: AxiosRequestConfig) => {
    console.log('store contents :', store.getState().authReducer);
    const authToken: string = store.getState().authReducer.jwtAccessToken;
    config.headers.Authorization = `Bearer ${authToken}`;
    //config.headers.httpsAgent = true;
    // config.headers.Add("ddd");

    //HERE
    //this is initiating only once so the header never has token in it
    //just discovered store.dispatch is needed to dispatch globally, not just dispatch.
    console.log(config.headers);
    return config;
});

// axiosServiceWithAuthHeader.interceptors.response.use(
//     response => response,
//     error => {
//         console.log("heeerree :", error);
//     }
// );

export default axiosServiceWithAuthHeader;