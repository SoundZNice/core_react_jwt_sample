import React, { Component } from 'react';

export class Home extends Component {
  static displayName = Home.name;

  render () {
    return (
      <div>
        <h1>This application can:</h1>
        <p>
          <ul>
            <li>
              <a href="/notes">Store Notes</a>
            </li>
          </ul>
        </p>
      </div>
    );
  }
}
