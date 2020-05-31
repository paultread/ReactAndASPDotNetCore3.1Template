import * as React from 'react';
import signInReducer, {iUser, iSignInInterface, initialisedUser} from '../reduxStore/signInStore';
import axiosService, {AxiosResponse, AxiosError} from 'axios';//axiosService from 'axios' does not use the interceptor I have setup to include the token in the header
import axiosServiceWithAuthHeader from '../../axiosIndex';
import {store} from '../../index';
import {actionCreators, actionCreatorsType} from '../reduxStore/signInStore';


//import { Button, FormGroup, Input, Label} from 'reactstrap';
import {
  Container, Col, Row, Form,
  FormGroup, Label, Input,
  Button,
} from 'reactstrap';

//#region commentsExplainingDispatchVsStore.Dispatch
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
//#endregion
//#region commentsHowToDisptachActionCreatorMethodOverObject
    //to use an actionCreator function instead of an action object, dispatch it via store.dispatch<any>(actionCreator.methodName(args)
    //instead of store.dispatch(type..., payload.)
    //#endregion
//#region oldVersionNotInActionCreator
//  const handleSignIn = async(eventArgs: React.FormEvent<HTMLFormElement>) => {
     
//     eventArgs.preventDefault();//behaviour not seen before, not preventing defaulty submit / refresh action - email address to URI paramters e.g. localhost:3000/signin?email=mr_treadwell%40live.co.uk

//     store.dispatch({type:'POST_SIGN_IN'});//to update isAuthenticating to stop any other requests being submitted via e.g. useEffect repeatedly submitting

//     //dispatch({type:'POST_SIGN_IN'});//this confused for a while, dispatch(...) used for context / localised state, store.dispatch(...) used for redux / prpject wide state)

//     //#region oldCode
//     //await axiosService.post('https://localhost:44309/SignIn', {username: emailAddressToSend, password: passwordToSend} )
//     //await axiosService.get('http://localhost:59333/SignIn')
//     // .then((response: AxiosResponse<iUser>) => {
//     //#endregion
//     //args need to have same name as props in the obj on server, albeit case does not matter
//     await axiosServiceWithAuthHeader.post('https://localhost:44309/SignIn', {username: emailAddressToSend, password: passwordToSend} )
//     .then((response: AxiosResponse<iSignInInterface>) => {
//         const {data} = response;
//         store.dispatch({type:'POST_SIGN_IN_RESULT_RECEIVED', signInState: data });
//         console.log(store.getState());
//     })
//     .catch((error: AxiosError) => {
//         throw error;
//     })
// };
//#endregion

const SignInComponent = () => {
    //these are local states - never to be shared across components        
    const [passwordToSend, setPasswordToSend] = React.useState('');
    const [emailAddressToSend, setEmailAddressToSend] = React.useState('');
    const [keepLoggedIn, setKeepLoggedIn] = React.useState(false);

    //called on field change / text input, are username and password to send for auth
    const updatePasswordToSend = (eventArgs: React.FormEvent<HTMLInputElement>) => {
        setPasswordToSend(eventArgs.currentTarget.value);
    }
    const updateEmailAddressToSend = (eventArgs: React.FormEvent<HTMLInputElement>) => {
        setEmailAddressToSend(eventArgs.currentTarget.value);
    }

    const updateKeepSignedIn = (evetArgs: React.FormEvent<HTMLInputElement>) => {
        setKeepLoggedIn(!keepLoggedIn);
    }
   
    const handleSignInUsingActionCreator = async(eventArgs: React.FormEvent<HTMLFormElement>) => {
        //see handleSignIn() for details of what this method does in thw action creator
        eventArgs.preventDefault();
        store.dispatch<any>(actionCreators.postSignIn(emailAddressToSend, passwordToSend, keepLoggedIn));
    };

    const handleSignUpUsingActionCreator = async(eventArgs: React.FormEvent<HTMLFormElement>) => {
        eventArgs.preventDefault();
        store.dispatch<any>(actionCreators.postSignup(emailAddressToSend, passwordToSend));
    };

    const smallFontStyle = {
     fontSize: '100%',
     color: 'black'   
    }

 
    return(
        // <div className='Login'>
        <Container>
            <h2 className='mb-3'> Sign In</h2>
            <form className="form" onSubmit={handleSignInUsingActionCreator}>
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
                        <Row style={{textAlign: 'right'}}>
                            <Col xs='3' sm='7'></Col>
                            <Col xs='7' sm='4'>
                                {/* <Label style={{fontSize: '100%'}} Keep me signed in</Label> */}
                                <Label className='' style={Object.assign(smallFontStyle)}>Keep me signed in:</Label> 
                            </Col>
                            <Col xs='2' sm='1'>
                                <Input type="checkbox" className='' label='ddd' onChange={updateKeepSignedIn}/>
                                </Col>
                        </Row>
                    </FormGroup>
           

                    <FormGroup>
                        <Button block type='submit' className='btn-info'>
                            Login
                        </Button>
                    </FormGroup>
                    <FormGroup>
                        <Button block onClick={handleSignUpUsingActionCreator} className='btn-danger'>
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


