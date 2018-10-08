import React, { Component } from 'react';

export default class App extends Component {
  displayName = App.name

  render() {
    return (
      <div className="container">
        <h1>Address Book</h1>
      </div>
    );
  }
}
