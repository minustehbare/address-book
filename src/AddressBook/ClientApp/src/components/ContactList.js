import React from 'react';
import { Contact } from './Contact';

export function ContactList(props) {
  return (
    <table className="table table-striped">
      <thead>
        <tr>
          <th scope="col">First Name</th>
          <th scope="col">Last Name</th>
          <th scope="col">Email</th>
          <th scope="col">Phone</th>
          <th scope="col">Actions</th>
        </tr>
      </thead>
      <tbody>
        {props.contacts.map(contact =>
          <Contact
            key={contact.id}
            id={contact.id}
            firstName={contact.firstName}
            lastName={contact.lastName}
            email={contact.email}
            phone={contact.phone}
            onSaveCallback={props.saveContactCallback}
            onDeleteCallback={props.deleteContactCallback} />
        )}
      </tbody>
    </table>
  );
}
