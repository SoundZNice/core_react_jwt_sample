import React, { Component } from 'react';
import { Route } from 'react-router';
import { Layout } from './components/Layout';
import { routes } from './components/Pages'

export default class App extends Component {
  static displayName = App.name;//
  constructor(props){
    super(props)

    this.state = {state: 'auth'}
  } 

  render () {    
    const r = routes.get(this.state.state)
    return (
      <Layout menu={r.filter(route => route.nav)}>
        {r.map(({route, component}) => 
          <Route exact path={route} key={route} component={component}/> )}
      </Layout>
    );
  }
}
