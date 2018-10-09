using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AddressBook.Models;
using AddressBook.Storage;

namespace AddressBook.Controllers
{
  [Route("api/contacts")]
  [ApiController]
  public class ContactsController : ControllerBase
  {
    private readonly IContactsStore _contactsStore;

    public ContactsController(IContactsStore store)
    {
      _contactsStore = store;
    }

    [HttpGet]
    [Route(template: "")]
    public IEnumerable<Contact> Contacts()
    {
      return _contactsStore.GetContacts();
    }

    [HttpPost]
    [Route(template: "new")]
    public StatusCodeResult CreateContact(Contact model)
    {
      if (ModelState.IsValid && (!string.IsNullOrWhiteSpace(value: model.Email) || !string.IsNullOrWhiteSpace(value: model.Phone)))
      {
        if (_contactsStore.Create(contact: new Contact(contact: model)))
        {
          return new StatusCodeResult(statusCode: StatusCodes.Status201Created);
        }
      }

      return new StatusCodeResult(statusCode: StatusCodes.Status400BadRequest);
    }

    [HttpDelete]
    [Route(template: "{id}")]
    public StatusCodeResult DeleteContact(Guid id)
    {
      Contact contact = _contactsStore.FindById(id: id);
      if (_contactsStore.Delete(contact: contact))
      {
        return new StatusCodeResult(statusCode: StatusCodes.Status204NoContent);
      }

      return new StatusCodeResult(statusCode: StatusCodes.Status400BadRequest);
    }

    [HttpPut]
    [Route(template: "{id}")]
    public StatusCodeResult EditContact(Contact model)
    {
      if (ModelState.IsValid && (!string.IsNullOrWhiteSpace(value: model.Email) || !string.IsNullOrWhiteSpace(value: model.Phone)))
      {
        if (_contactsStore.Update(contact: model))
        {
          return new StatusCodeResult(statusCode: StatusCodes.Status204NoContent);
        }
      }

      return new StatusCodeResult(statusCode: StatusCodes.Status400BadRequest);
    }
  }
}
