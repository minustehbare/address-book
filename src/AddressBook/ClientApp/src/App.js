import React, { Component } from 'react';
import { AddContact } from './components/AddContact';
import { ContactList } from './components/ContactList';

export default class App extends Component {
  displayName = App.name
  constructor(props) {
    super(props);
    this.state = { loading: true };
    this.getContacts = this.getContacts.bind(this);

    this.getContacts(true);
  }

  getContacts(delay) {
    if (delay && !this.state.loading) {
      this.setState({ loading: true });
    }
    fetch('api/contacts')
      .then((response) => {
        if (response.ok) {
          return response.json();
        }

        throw new Error('Problem retrieving contacts');
      })
      .then(data => {
        data.sort((a, b) => { return a.lastName > b.lastName; });
        setTimeout(() => this.setState({
          contacts: data,
          loading: false
        }), delay ? 1000 : 0);
      })
      .catch((error) => console.error(error));
  }

  render() {
    let contactList = this.state.loading
      ? <p className="lead">Loading contacts...</p>
      : <ContactList contacts={this.state.contacts} saveContactCallback={this.getContacts} deleteContactCallback={this.getContacts} />;

    return (
      <div className="container">
        <h1 className="my-3">Address Book</h1>
        <div className="mb-3">
          <h3>Add Contact</h3>
          <AddContact addContactCallback={this.getContacts} />
        </div>
        <div>
          <h3 className="mb-3">Your Contacts</h3>
          {contactList}
        </div>
      </div>
    );
  }
}
