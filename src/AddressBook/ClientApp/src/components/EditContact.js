import React from 'react';

export function EditContact(props) {
  return (
    <tr>
      <td>
        <input
          type="text"
          className="form-control mr-2"
          placeholder="First name"
          name="firstName"
          defaultValue={props.firstName}
          onChange={props.changeHandler}
          required />
      </td>
      <td>
        <input
          type="text"
          className="form-control mr-2"
          placeholder="Last name"
          name="lastName"
          defaultValue={props.lastName}
          onChange={props.changeHandler}
          required />
      </td>
      <td>
        <input
          type="email"
          className="form-control mr-2"
          placeholder="email@example.com"
          name="email"
          defaultValue={props.email}
          onChange={props.changeHandler} />
      </td>
      <td>
        <input
          type="tel"
          className="form-control mr-2"
          placeholder="Phone number (11-digit)"
          name="phone"
          defaultValue={props.phone}
          onChange={props.changeHandler} />
      </td>
      <td>
        <button type="submit" className="btn btn-primary mr-3" onClick={props.saveHandler}>Save</button>
        <button className="btn btn-secondary" onClick={props.cancelHandler}>Cancel</button>
      </td>
    </tr>
  );
}
