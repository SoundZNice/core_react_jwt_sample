import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>This application does:</h1>
        <p>
        <li>
            Logging in;
          </li>          
          <li>
            Registration;
          </li>
          <li>
            Store Notes.
          </li>
        </p> 
      </div>
    );
  }
}
