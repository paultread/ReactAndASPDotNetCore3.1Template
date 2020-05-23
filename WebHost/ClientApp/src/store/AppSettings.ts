import * as React from 'react';
import axiosService, {AxiosError, AxiosResponse } from 'axios';
import { AppThunkAction } from './index';
import axiosServiceWithAuthHeader from '../axiosIndex';

export interface AppSettings {
    authentication: AuthenticationSettings;
    key1: string;
    key2: string;
}

export interface AuthenticationSettings {
    provider: string;
}

export interface requestAppSettingsAction { type: 'FETCH_APPSETTINGS'; appSettings: AppSettings; }
export type KnownAction = requestAppSettingsAction;

export const actionCreators = {
    requestAppSettings: (): AppThunkAction<KnownAction> => (dispatch) => {
      //axiosServiceWithAuthHeader.get('http://localhost:59333/ClientAppSettings')
      axiosServiceWithAuthHeader.get('https://localhost:44309/ClientAppSettings')
      //axiosService.get('http://localhost:59333/ClientAppSettings')
        .then((response: AxiosResponse<AppSettings>) => {
          const { data } = response;
          console.log(response);
          dispatch({ type: 'FETCH_APPSETTINGS', appSettings: data });
        })
        .catch((error: AxiosError) => {
          console.log('AXIOSERROR::' + error.message);
          throw error;
        });
    }
}

const AppSettings = () => {

}; export default AppSettings;