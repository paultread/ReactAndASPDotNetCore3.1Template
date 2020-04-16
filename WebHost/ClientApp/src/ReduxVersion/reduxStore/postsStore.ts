import {Action, Reducer} from 'redux';
//import {Post} from '../components/PostsComponent';



export interface Post {
    userId: number;
    id: number;
    title: string;
    body: string;
}


export interface iPostsState {
    isLoading: boolean;
    posts: Post[];
}

export interface RequestPostsAction {
    type: 'REQUEST_POSTS';
    //startDateIndex: number;
}

export interface ReceivePostsAction {
    type: 'RECEIVE_POSTS';
    //startDateIndex: number;
    posts: Post[];
}



export type KnownAction = RequestPostsAction | ReceivePostsAction;


const postsStoreReducer: Reducer<iPostsState> = (state: iPostsState | undefined, incomingAction: Action): iPostsState => {
    if (state === undefined) {
        return { isLoading: true, posts: [] };
    }

    const action = incomingAction as KnownAction;
    switch (action.type) {
        case 'RECEIVE_POSTS':
            return { 
                isLoading: false, 
                posts: action.posts
            };
        case 'REQUEST_POSTS':
            return {
                isLoading: true, 
                posts: state.posts
            }
        default:
            return state;
    }
}

export default postsStoreReducer;

