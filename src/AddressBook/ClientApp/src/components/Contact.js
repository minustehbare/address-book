import React from 'react';
import { EditContact } from './EditContact';
import { ShowContact } from './ShowContact';

export class Contact extends React.Component {
  constructor(props) {
    super(props);
    this.state = { isEditing: false };
    this.delete = this.delete.bind(this);
    this.edit = this.edit.bind(this);
    this.save = this.save.bind(this);
    this.changed = this.changed.bind(this);
    this.cancel = this.cancel.bind(this);
  }

  edit() {
    this.setState({
      isEditing: true,
      firstName: this.props.firstName,
      lastName: this.props.lastName,
      email: this.props.email,
      phone: this.props.phone
    });
  }

  cancel() {
    this.setState({ isEditing: false });
  }

  save() {
    fetch('api/contacts/' + this.props.id, {
      method: 'PUT',
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

        throw new Error('Problem saving changes to contact');
      })
      .then(() => {
        this.setState({ isEditing: false });
        this.props.onSaveCallback();
      })
      .catch((error) => console.error(error));
  }

  delete() {
    fetch('api/contacts/' + this.props.id, {
      method: 'DELETE',
      headers: {
        'Content-Type': 'application/json'
      }
    })
      .then(response => {
        if (response.ok) {
          return response;
        }

        throw new Error('Problem deleting contact');
      })
      .then(() => {
        this.props.onDeleteCallback();
      }).catch(error => console.error(error));
  }

  changed(event) {
    this.setState({
      [event.target.name]: event.target.value
    });
  }

  render() {
    let content = !this.state.isEditing
      ? <ShowContact
        firstName={this.props.firstName}
        lastName={this.props.lastName}
        email={this.props.email}
        phone={this.props.phone}
        editHandler={this.edit}
        deleteHandler={this.delete} />
      : <EditContact
        firstName={this.props.firstName}
        lastName={this.props.lastName}
        email={this.props.email}
        phone={this.props.phone}
        changeHandler={this.changed}
        saveHandler={this.save}
        cancelHandler={this.cancel} />;

    return content;
  }
}
