import React from 'react';

export function ShowContact(props) {
  return (
    <tr>
      <td>{props.firstName}</td>
      <td>{props.lastName}</td>
      <td>{props.email}</td>
      <td>{props.phone}</td>
      <td>
        <button className="btn btn-link mr-3" onClick={props.editHandler}>
          <div><span className="text-warning fas fa-pencil-alt" /></div>
        </button>
        <button className="btn btn-link" type="submit" onClick={props.deleteHandler}>
          <div><span className="text-danger fas fa-times" /></div>
        </button>
      </td>
    </tr>
  );
}
