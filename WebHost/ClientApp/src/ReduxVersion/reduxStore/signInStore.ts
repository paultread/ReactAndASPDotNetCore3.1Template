import * as React from 'react';
import {Action, Reducer} from 'redux';
import {AppThunkAction} from '../../store/index'
import { Tracing } from 'trace_events';
import {store} from '../../index';
import axiosServiceWithAuthHeader from '../../axiosIndex';
import {AxiosResponse, AxiosError} from 'axios'

// import * as DateTime from 'react-datetime';








//type for component state
export interface iSignInInterface {
    user: iUser;
    isAuthenticated: boolean;
    jwtAccessToken: string;
    expiresInSeconds: number;
    //above sent back from server, below prop for UI
    authProcessInAction: boolean;
}

//type for user object as part of 
export interface iUser {
    accessFailedCount: number;
    email: string;
    userName: string;
    emailConfirmed: boolean;
    hobbies: string;
    lockoutEnabled: boolean;
    lockoutEnd: Date;
    phoneNumber: string;
    twFactorEnabled: boolean;
}

export const initialisedUser: iUser = {
    userName: '', 
    accessFailedCount: 0,
    email: '',
    emailConfirmed: false,
    hobbies: '',
    lockoutEnabled: false,
    lockoutEnd: new Date(),
    phoneNumber: '',
    twFactorEnabled: false
}


export interface PostSigninInAction {
    type: 'POST_SIGN_IN';
}

export interface PostSignupInAction {
    type: 'POST_SIGN_UP';
}

export interface PostRefreshInAction {
    type: 'POST_REFRESH_TOKEN'
}




export interface RecieveSignInResultAction {
    type: 'POST_SIGN_IN_RESULT_RECEIVED';
    signInState: iSignInInterface;
}

export interface RecieveSignUpResultAction {
    type: 'POST_SIGN_UP_RESULT_RECEIVED';
    signInState: iSignInInterface;
}

//#region tryingToDefineStore.Dispatch<any>type
//tried to define the type for store.dispatch<any>(actionCreators....) but was nto successful
export interface actionCreatorsType {
    postSignIn (emailAddressToSend: string, passwordToSend: string) : AppThunkAction<tSignInActionType>,

    postSignup (emailAddressToSend: string, passwordToSend: string) : AppThunkAction<tSignInActionType>
}
//#endregion

export const actionCreators = {
    postSignIn : (emailAddressToSend: string, passwordToSend: string, keepLoggedIn: boolean) : AppThunkAction<tSignInActionType> => async (dispatch, getState) => {
        store.dispatch({type:'POST_SIGN_IN'});
        await axiosServiceWithAuthHeader.post('https://localhost:44309/SignIn', {username: emailAddressToSend, password: passwordToSend, keepLoggedIn: keepLoggedIn} )
        .then((response: AxiosResponse<iSignInInterface>) => {
            const {data} = response;
            store.dispatch({type:'POST_SIGN_IN_RESULT_RECEIVED', signInState: data });
        })
        .catch((error: AxiosError) => {
            throw error;
        })
    } ,
    postSignup : (emailAddressToSend: string, passwordToSend: string) : AppThunkAction<tSignInActionType> => async (dispatch, getState) => {
        //to update isAuthenticating to stop any other requests beign submitted via e.g. useEffect repeatedly submitting
        store.dispatch({type:'POST_SIGN_UP'});
        //args need to have same name as props in the obj on server, albeit case does not matter
        //await axiosService.post('https://localhost:44309/SignUp', {username: emailAddressToSend, password: passwordToSend} )
        await axiosServiceWithAuthHeader.post('https://localhost:44309/SignUp', {username: emailAddressToSend, password: passwordToSend} )
        //await axiosService.get('http://localhost:59333/SignIn')
       
        //.then((response: AxiosResponse<iUser>) => {
        //.then((response: AxiosResponse<iSignInInterface>) => {
        .then((response: any) => {
            // const {data} = response;
            console.log(response);
            const {data} = response;
            // dispatch({type:'POST_SIGN_IN_RESULT_RECEIVED', data: data })
        })
        .catch((error: AxiosError) => {
            console.log('AXIOSERROR::' + error.message);
            throw error;
        })
    },

    postRefresh: () : AppThunkAction<tSignInActionType> => async (dispatch, getState) => {
        store.dispatch({type:'POST_REFRESH_TOKEN'});
        await axiosServiceWithAuthHeader.post('https://localhost:44309/api/refreshtoken', 
        {
            jwtToken: store.getState().authReducer.jwtAccessToken
            //, refreshToken: "isInHttpOnlyCookie"
        }
        )

        .then (() => {
            //here tomorrow - watch video see what he does with callign api to submit jwt token and gettign httpOnly cookie
        })
        .catch (() => {

        })
    }
}



export type tSignInActionType = PostSigninInAction | PostSignupInAction | RecieveSignInResultAction | RecieveSignUpResultAction | PostRefreshInAction;

const signInReducer: Reducer<iSignInInterface> = (state: iSignInInterface | undefined, incomingAction : Action): iSignInInterface => {
    if (state === undefined) {
        return {
            user: initialisedUser,
            authProcessInAction: false,
            isAuthenticated: false,
            jwtAccessToken: '',
            expiresInSeconds: 0
        }
    }
    const action = incomingAction as tSignInActionType;
    switch (action.type){
        case('POST_SIGN_IN' || 'POST_SIGN_UP' || 'POST_REFRESH_TOKEN'):
            return state
        case('POST_SIGN_IN_RESULT_RECEIVED' || 'POST_SIGN_UP_RESULT_RECEIVED'):
            return {
                user: action.signInState.user,
                authProcessInAction: false,
                isAuthenticated: action.signInState.isAuthenticated,
                jwtAccessToken: action.signInState.jwtAccessToken,
                expiresInSeconds: action.signInState.expiresInSeconds
            }
        default:
            return state;
    }
}; export default signInReducer;