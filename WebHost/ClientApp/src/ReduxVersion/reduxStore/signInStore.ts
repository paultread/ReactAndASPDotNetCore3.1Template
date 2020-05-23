import * as React from 'react';
import {Action, Reducer} from 'redux';
import { Tracing } from 'trace_events';


import {store} from '../../index';

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



export interface RecieveSignInResultAction {
    type: 'POST_SIGN_IN_RESULT_RECEIVED';
    signInState: iSignInInterface;
}

export interface RecieveSignUpResultAction {
    type: 'POST_SIGN_UP_RESULT_RECEIVED';
    signInState: iSignInInterface;
}

//added as a test so can log state to console after it has been updated

export interface PostSignInStateUpdated {
    type: 'Post_SIGN_IN_STATE_UPDATED'
}

export type tSignInActionType = PostSigninInAction | PostSignupInAction | RecieveSignInResultAction | RecieveSignUpResultAction | PostSignInStateUpdated ;

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
        case('POST_SIGN_IN' || 'POST_SIGN_UP'):
            return state
            // {
            //     ...state,//needed???
            //     user: initialisedUser,
            //     authProcessInAction: true,
            //     isAuthenticated: false,
            //     jwtAccessToken: '',
            //     expiresInSeconds: 0
            // };
        case('POST_SIGN_IN_RESULT_RECEIVED' || 'POST_SIGN_UP_RESULT_RECEIVED'):
        //console.log('jjj : ', state);
            return {
                //I need to be updating onepart of state here, not all of it
                //...state,
                user: action.signInState.user,
                authProcessInAction: false,
                isAuthenticated: action.signInState.isAuthenticated,
                jwtAccessToken: action.signInState.jwtAccessToken,
                expiresInSeconds: action.signInState.expiresInSeconds
            }
        case('Post_SIGN_IN_STATE_UPDATED'):
        //console.log("here I am not coming up as null: ", state);
        //console.log(store.getState());
            return state;
        default:
            return state;
    }
}; export default signInReducer;