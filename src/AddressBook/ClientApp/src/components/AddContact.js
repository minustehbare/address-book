import React from 'react';

export class AddContact extends React.Component {
  constructor(props) {
    super(props);
    this.inputChanged = this.inputChanged.bind(this);
    this.addContact = this.addContact.bind(this);
    this.state = {
      firstName: null,
      lastName: null,
      email: null,
      phone: null
    };
    this.baseState = this.state;
  }

  inputChanged(event) {
    this.setState({
      [event.target.name]: event.target.value
    });
  }

  addContact(event) {
    event.preventDefault();
    fetch('api/contacts/new', {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json'
      },
      body: JSON.stringify({
        firstName: this.state.firstName,
        lastName: this.state.lastName,
        email: this.state.email,
        phone: this.state.phone
      })
    })
      .then((response) => {
        if (response.ok) {
          return response;
        }

        throw new Error('Failed to add contact');
      })
      .then(() => {
        this.setState(this.baseState);
        this.props.addContactCallback();
      })
      .catch((error) => console.error(error));
  }

  render() {
    return (
      <form className="form-inline" onSubmit={this.addContact}>
        <input
          type="text"
          className="form-control mr-2"
          placeholder="First name"
          name="firstName"
          value={this.state.firstName || ""}
          onChange={this.inputChanged}
          required />
        <input
          type="text"
          className="form-control mr-2"
          placeholder="Last name"
          name="lastName"
          value={this.state.lastName || ""}
          onChange={this.inputChanged}
          required />
        <input
          type="email"
          className="form-control mr-2"
          placeholder="email@example.com"
          name="email"
          value={this.state.email || ""}
          onChange={this.inputChanged} />
        <input
          type="tel"
          className="form-control mr-2"
          placeholder="Phone number (11-digit)"
          name="phone"
          value={this.state.phone || ""}
          onChange={this.inputChanged} />

        <button type="submit" className="btn btn-primary">Add</button>
      </form>
    );
  }
}

