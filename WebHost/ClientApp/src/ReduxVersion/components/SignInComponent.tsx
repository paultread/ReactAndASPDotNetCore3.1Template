import * as React from 'react';
import signInReducer, {iUser, iSignInInterface, initialisedUser} from '../reduxStore/signInStore';
//axiosService from 'axios' does not use the interceptor I have setup to include the token in the header
import axiosService, {AxiosResponse, AxiosError} from 'axios';
import {store} from '../../index';
import axiosServiceWithAuthHeader from '../../axiosIndex';


//import { Button, FormGroup, Input, Label} from 'reactstrap';
import {
  Container, Col, Form,
  FormGroup, Label, Input,
  Button,
} from 'reactstrap';

//TODO tomorrow - access 'user' in here now that it has been updated in 'state' in teh reducer, logging it to console below is showing it to not be updated
const SignInComponent = () => {

    // THIS CONFUSED THE BAJEEBERS OUT OF ME
    //this allows you to create a local 'state' and use a reducer pattern
    //DECLARE LIKE THIS:
        // const [authenticatedObj, dispatch] = React.useReducer(
        //     signInReducer, 
        //     {
        //         user: initialisedUser,
        //         authProcessInAction: false,
        //         isAuthenticated: false,
        //         jwtAccessToken: '',
        //         expiresInSeconds: 0
        //     }
        // );
    //USE LIKE THIS WITH THE STANDARD REDUCER SETUP:
        //dispatch({type:'POST_SIGN_IN_RESULT_RECEIVED', signInState: data })
    //NOT LIKE THIS WITH THE STANDARD REDUCER SETUP:
        //import {store} from '../../index';
        //store.dispatch({type:'POST_SIGN_IN_RESULT_RECEIVED', signInState: data })
            //Above uses global store

    const [passwordToSend, setPasswordToSend] = React.useState('');
    const [emailAddressToSend, setEmailAddressToSend] = React.useState('');
 

    const updatePasswordToSend = (eventArgs: React.FormEvent<HTMLInputElement>) => {
        setPasswordToSend(eventArgs.currentTarget.value);
    }
    const updateEmailAddressToSend = (eventArgs: React.FormEvent<HTMLInputElement>) => {
        setEmailAddressToSend(eventArgs.currentTarget.value);
    }

    const postSignIn = async(eventArgs: React.FormEvent<HTMLFormElement>) => {
        //to update isAuthenticating to stop any other requests beign submitted via e.g. useEffect repeatedly submitting
        eventArgs.preventDefault();
        //behaviour not seen before, not adding the above adds email address to URI paramters e.g. localhost:3000/signin?email=mr_treadwell%40live.co.uk
        //dispatch({type:'POST_SIGN_IN'});
        store.dispatch({type:'POST_SIGN_IN'});
        //args need to have same name as props in the obj on server, albeit case does not matter
        await axiosService.post('https://localhost:44309/SignIn', {username: emailAddressToSend, password: passwordToSend} )
        //await axiosService.get('http://localhost:59333/SignIn')
       
        // .then((response: AxiosResponse<iUser>) => {
        .then((response: AxiosResponse<iSignInInterface>) => {
        //.then((response: any) => {
            // const {data} = response;
            console.log('looking for me : ', response);
            console.log(document);
            const {data} = response;
            //dispatch({type:'POST_SIGN_IN_RESULT_RECEIVED', signInState: data })
            //console.log(store.getState().authReducer);
            store.dispatch({type:'POST_SIGN_IN_RESULT_RECEIVED', signInState: data });
        })
        .then(() => {
            //console.log(store.getState().authReducer);
            //  .log('logging');
            //console.log("here I am appearing as null: ", user);
            //store.dispatch({type:'Post_SIGN_IN_STATE_UPDATED'});
        })
        .catch((error: AxiosError) => {
            //console.log('AXIOSERROR::' + error.message);
            //console.log(user);
            //console.log(store.getState().authReducer);
            throw error;
        })
    };

    const postSignUp = async(eventArgs: React.FormEvent<HTMLFormElement>) => {
        //to update isAuthenticating to stop any other requests beign submitted via e.g. useEffect repeatedly submitting
        store.dispatch({type:'POST_SIGN_UP'});
        //args need to have same name as props in the obj on server, albeit case does not matter
        await axiosService.post('https://localhost:44309/SignUp', {username: emailAddressToSend, password: passwordToSend} )
        //await axiosService.get('http://localhost:59333/SignIn')
       
        // .then((response: AxiosResponse<iUser>) => {
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
    };
 
    return(
        // <div className='Login'>
        <Container>
            <h2 className='mb-3'> Sign In</h2>
            <form className="form" onSubmit={postSignIn}>
                <Col >
                    <FormGroup>
                        <Label>Email: </Label>
                        <Input autoFocus type='email' name='email' id='emailInput' placeholder='example.email.com...' onChange={updateEmailAddressToSend} />
                    </FormGroup>
                    <FormGroup>
                        <Label>Password:</Label>
                        <Input type='password' placeholder='*****' onChange={updatePasswordToSend}/>
                    </FormGroup>
                    <FormGroup>
                        <Button block type='submit' className='btn-info'>
                            Login
                        </Button>
                    </FormGroup>
                    <FormGroup>
                        <Button block onClick={postSignUp} className='btn-danger'>
                            SignUp
                        </Button>
                    </FormGroup>
                </Col>
            </form>
        </Container>
        // </div>
    //     <div className="Login">
    //     <form onSubmit={postSignIn}>
    //       <FormGroup controlId="email" bsSize="large">
    //         <Label>Email</Label>
    //         <Input
    //           autoFocus
    //           type="email"
    //         //   value={email}
    //         //   onChange={e => setEmail(e.target.value)}
    //         />
    //       </FormGroup>
    //       <FormGroup controlId="password" bsSize="large">
    //         <Label>Password</Label>
    //         <Input
    //            value='hhh'
    //         //   onChange={e => setPassword(e.target.value)}
    //           type="password"
    //         />
    //       </FormGroup>
    //       <Button block size='large' 
    //     //   disabled={!validateForm()} 
    //       type="submit">
    //         Login
    //       </Button>
    //     </form>
    //   </div>
    )

}; export default SignInComponent;


