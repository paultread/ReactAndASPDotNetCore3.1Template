import * as React from 'react';
import {AppThunkAction} from '../store/index';
//import { Dispatch } from 'redux';
//the above is just a type
import { connect } from 'react-redux';
import { RouteComponentProps } from 'react-router';
import {RequestPostsAction, ReceivePostsAction, iPostsState} from '../ReduxVersion/reduxStore/postsStore';
import { ApplicationState } from '../store';
import axiosService from 'axios'
import axiosServiceWithAuthHeader from '../axiosIndex';
import { AxiosResponse, AxiosError } from 'axios'
import {Post} from '../ReduxVersion/reduxStore/postsStore';
//this gave me jip importing it (below) as in javascript not ts, two ways to fix this - in tsconfig.js added line "noImplicitAny": false in compiler options, but better way was to add file table.d.ts and declared this module here (I have no idea how this links up to here though... maybe through the folder structure? Didnt have to change any settings anyway)
import { Table, TableRow, TableCell, TableHeader } from 'carbon-react/lib/components/table';


type KnownAction = RequestPostsAction | ReceivePostsAction;
export const actionCreators = {
    requestPosts: (): AppThunkAction<KnownAction> => (dispatch, getState) => {
        // Only load data if it's something we don't already have (and are not already loading)
        const appState = getState();
        console.log(appState);
        if (appState && appState.postsComponentReducer && appState.postsComponentReducer.isLoading) {
          //axiosService.get(`https://jsonplaceholder.typicode.com/posts`)
          axiosServiceWithAuthHeader.get('https://jsonplaceholder.typicode.com/posts')
          //axiosService.get('https://jsonplaceholder.typicode.com/posts')
            .then((response: AxiosResponse<Post[]>) => {
                console.log(response);
              const { data } = response;
              console.log('axios called');
              dispatch({ type: 'RECEIVE_POSTS', posts: data });
            })
            .catch((error: AxiosError) => {
              console.log('AXIOSERROR::' + error.message);
              throw error;
            });
           //dispatch({ type: 'REQUEST_POSTS' });
        }
      }
};

//up to here> jsut need to convert the below:

type PostsComponentProps =
    iPostsState &
    typeof actionCreators &
    RouteComponentProps<{}>;

    

class PostsComponent extends React.PureComponent<PostsComponentProps> {
    postsReceieved: Post[] = [];
    public componentDidMount = () => {
        this.props.requestPosts();
    }
    public componentDidUpdate(){
        this.props.requestPosts();
    }

    
    public render() {
        const PostsHtml = this.props.posts.length 
            ? this.props.posts.map(post => {
                return (
                    post.id < 10 
                    ?
                    <div key={post.id}>
                        <div>ID: {post.id}</div>
                        <div>Post: {post.title}</div>
                        <div> BODY: {post.body}</div>
                        <p></p>
                    </div>
                    : 
                    <div key={post.id}></div>
                    
                )
            })
            : (<div>'Loading...'</div>)
        ;

        let RenderPostsInCarbonTable = this.props.posts.length  
            ? this.props.posts.map(post => {
                return(
                    post.id < 10 
                    ?
                         <TableRow key={post.id} >
                            <TableCell>
                            {post.id}
                            </TableCell>
                            <TableCell>
                            {post.userId}
                            </TableCell>
                            <TableCell>
                            {post.title}
                            </TableCell>
                            {/* <TableCell>
                            {post.body}
                            </TableCell> */}
              
                        </TableRow>
                    : 
                        <TableRow TableRow key={post.id} hidden>
                        </TableRow>
                )
            })          

            : <TableRow>
                <TableCell>
                    Data Loading...
                </TableCell>
            </TableRow>
        ;

        return (
            <React.Fragment>
                <h1>JSON placeholder data</h1>
                {/* {PostsHtml} */}
                <div>
                <Table>
                    {RenderPostsInCarbonTable}
                </Table>
                </div>
                
                
            </React.Fragment>
        );
    }
};

export default connect(
    (state: ApplicationState) => state.postsComponentReducer,
    actionCreators
)(PostsComponent as any);
