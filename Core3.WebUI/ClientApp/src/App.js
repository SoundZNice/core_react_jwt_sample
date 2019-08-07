import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { routes } from './components/Pages'

export default class App extends Component {
  static displayName = App.name;//
  constructor(props){
    super(props)

    this.state = {state: 'noauth'}
  } 

  render () {    
    //const r = routes.get(this.state.state)
    return (
      <Layout>
        {/* {r.map(({route, component}) => 
          { return <Route exaxt path={route} component={component}/>} )} */}
      </Layout>
    );
  }
}
