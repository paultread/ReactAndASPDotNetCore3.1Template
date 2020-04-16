//REDUX in classes
//REDUX in functional components
//REDUX - use of useState() hook
    //Tut2 Vid9 - useState() in func comp
//Redux - use of useEffect() hook
    //Tut2 Vid12 - useEffect in func comp
//Context Hooks Effects in classes
    //useState()
//Context Hooks Effects in functional components


//postsStore - class components & Redux
//companyContext - class components & Context


//REDUX vs CONTEXT
//Context pros - simpler to use / no extra dependency / shrinks bundle size = 
    //use for low frequency updates (i.e. state changes that don't occur multiple times per second)
        //When any state changes occur, the UI is rerendered with context so has a performance issue
//Redux pros - doesn't have to traverse the component tree to detect state changes


//                                 REDUX OVERVIEW
//--------------------------------------------------------------------------------
//Intialisation starts in & goes through: index.tsx > App.tsx >  configureStore > StoreIndex.ts 
//STORE FOLDER
    //----------------------------------------------------------------------------
    //STOREINDEX.TS
    //----------------------------------------------------------------------------
        //The ApplicationState interface declares a var for each state
        //A single reducer object is declared in here with all reducers
    //---------------------------------------------------------------------------
    //COMPONENTSTORENAME.TS
    //---------------------------------------------------------------------------
        //Action interfaces declared in here
        //Action creators declared in here
            //Action creators which utilise Thunk declared in here and use dispatch, getState
        //Reducer declared in here
//-------------------------------------------------------------------------------
//COMPONENTS FOLDER
    //---------------------------------------------------------------------------
    //COMPONENTNAME.TSX
    //---------------------------------------------------------------------------
        //type for the class componet declared in here (stateInterface & typeof actioncreator(s) & routeComponentPropsInterface )
        //class component declared in here of type declared above
        //has a public render() function that renders the html / jsx
            //can get this.props inside
        //without use of effects:
            //this.componentDidMount() used to retrieve intilaising data from a database
            //this.componentDidUpdate() used to (at least) refetch the route history / paramters
        //exports default connect(state: mainStateTypeInterface) => state.componentName, actionCreator(s) object)(component)
            //...()(componentName as any) when using thunk


//                                 CONTEXT HOOKS & EFFECTS OVERVIEW
//




//context - purpose - 
    //allows you to grab data from props without pipelining it through each nested component (like Redux but less complex & nt suitable for global data- just a subset of components - although can be used for global data)
    //should only be used to wrap a subset of components and nested components, not the whol app (this is what Redux is for)
    //also should consider component composition - passign the component through the pipeline ratther than the props - this makes reusing the component less difficult 
//setting up context
    //CompanyComponent
        //<CompanyContextProvider> <CompaniesList/></CompanyContextProvider>
    //CompanyContextComponent 
        //returns no html
        //has state defined in here
        //has functions defined in here that children components need
        //renders() {returns(<CompanyCOntext.Provider value{{...this.state, functionsThatNeedToBePassedToChildrenToUse: this.functionsThatNeedToBePassedToChildrenToUse}}> {this.props.children} </CompanyContext.Provider>);}
//Setting up context in a child
    //ways to GET data using context
        //in a CLASS COMPONENT: static contextType = Company Context > this.context (Tut2 Vid4)
        //in a FUNCTIONAL COMPONENT: <CompanyContext.Consumer>{(context) => {return{ ...jsx goes here... grab context with the context }}}</CompanyContext.Consumer> (Tut2 Vid5)
    //ways to PUT / POST / DELETE / MUTATE data using context
        //this.setState in CompanyContext (Tut2 Vid6)
//Hooks (not lifecycle hooks) - purpose
    //give us functionality inside funcitonal components usually only available in class components
        //useState() - access and mutate state
        //useContext() - access and mutate context
        //useEffect() - lifecycle hook inside of functional component
            //useReducer()?




//with contexts, the initial state is passed into the reducer and not handled in the reducer
//context - getting context in a class = static contextType = CompanyContext > then this.context becomes available - cannot do this ins a function (static is a static property - for the class)
//context - getting context in a funcitonal component - can use CompanyContext.Consumer (likeCompanyContext.Provider) > surround your jsx with the tags <CompanyContext.Consumer>{(context) => {return{ ...jsx goes here...}}}</CompanyContext.Consumer}  > can use context (not this.context) in teh JSX then 








//DISPATCH({action}) function is defined in STORE but pipelined through, grabbed in, and called in a COMPONENT in MAPDISPATCHTOPROPS
//In a COMPONENT >  defining MAPDISPATCHTOPROPS(dispatch, ...) and then adding it as the second arg in CONNECT() > any property or functon that is returned from here is added to 'props' > because DISPATCH is piped in, it can be called in here - purpose: used to let a component make changes to the state by callign teh dispatch method in the functions that it returns
//MAPDISPATCHTOPROPS cannot be used in vis stud for somereason, instead, 



//MAPSTATETOPROPS(state, propsPriorToAddingNewPropsFromState)purpose: is used to get things from the state in a component > is defined in COMPONENT > grabs the state from the store which has been pipelined in > grabs the specific part of the the particular substore from the store and assigns it to a property on PROPS > this is returned - to be used by CONNECT(mapstatetoprops)

//on route to STORE calls by reducer to generate a modified version of the state using the optional PAYLOAD if it needs it > the modified version of state is then dispatched and modified in the state > all subscribign components are noptified & updated 

//---------------------------------------------------STORE----------------------------------------------------------------------
    //Each store has the following: 
        //A single reducer (function) passed as a parameter
    //A simple single store for one component-group (e.g. Companies) Redux.createStore(reducer) which is unlikely
    //A store created using multiple sub-stores is much more complex, but generally:
        //multiple substores created with their respective single reducer
        //combined to one single store in configureStore.ts
    //Each store allows the following:
        //A Dispatch function
    //On build
        //when each substore is created, the reducer is created and runs, actions are created and run, which is why initial state is required


//---------------------------------------------------REDUCER-------------------------------------------------------------------
    //The purpose of a reducer is to return an object that represents a modified instance of a state to the dispatch function using an action to tell it how & provide the data it needs to do this 
    //It should not directly modify teh state, but use teh existing state to generate another, modified instance
    //Each reducer has the following parameters:
        //State
            //initial states need handling otherwise error (i.e. waiting for data being pulled from the db)
        //Action
    //Reducers shoudl do the following:
        //Check the type of the action
        //Return an object that represents the whole state of the store
            //requires state to be spread (...state) in here before returning
        //Call dispatch to modify teh state in teh store if required
            //Dispatch should be pipelined from the store into the reducer and called within here in the pattern we use
    
//---------------------------------------------------ACTION-------------------------------------------------------------------
    //The purpose of an action is to tell a reducer what to do (type) with some optional data (payload)
    //Each action has the following
        //Type
            //is a string to describe the action to do on the store
        //Payload (optional)
            //can be a property, an object, an array etc
            //elements in here need to match the state properties (i.e. names the same etc)

//---------------------------------------------------DISPATCH---------------------------------------------------------------------
    //Dispatch takes an ACTION object     
    //To get the 'Dispatch' function from the store, it needs pipelining through from file to file, from Store to wherever it is required.
    //this is always called ON the store though, and is used to modify the data
    //the pattern we use pipelines the 'Dispatch' function into the COMPONENT, and it is called in there (only)


//-----------------ACTION CREATORS
    //Action creators return an action object
    //https://youtu.be/z2XCUu2wIl0
    //THUNK Added as middleware in the storeallows action creators to return a function instead of an action object - which is needed to do e.g. API calls, as these cannot be done within an object
    //this will call the dispatch(action, payload) inside of it




//QUESTION - where is the 'state' object defined / maintained? It is e.g. gotten in the reducer we can use const state = getState() to get the state - which is done in actionCreators... but where does state come from? 