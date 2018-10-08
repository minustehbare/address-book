using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using AddressBook.Models;

namespace AddressBook.Controllers
{
  [Route("api/contacts")]
  [ApiController]
  public class ContactsController : ControllerBase
  {
    [HttpGet]
    [Route(template: "")]
    public IEnumerable<Contact> Contacts()
    {
      return GetContacts();
    }

    [HttpPost]
    [Route(template: "new")]
    public StatusCodeResult CreateContact(Contact model)
    {
      if (ModelState.IsValid && (!string.IsNullOrWhiteSpace(value: model.Email) || !string.IsNullOrWhiteSpace(value: model.Phone)))
      {
        List<Contact> contacts = GetContacts();
        contacts.Add(item: new Contact(contact: model));
        SetContacts(contacts: contacts);
        return new StatusCodeResult(statusCode: StatusCodes.Status201Created);
      }

      return new StatusCodeResult(statusCode: StatusCodes.Status400BadRequest);
    }

    [HttpDelete]
    [Route(template: "{id}")]
    public StatusCodeResult DeleteContact(Guid id)
    {
      List<Contact> contacts = GetContacts();
      contacts.Remove(item: contacts.Where(x => x.Id == id).FirstOrDefault());
      SetContacts(contacts: contacts);
      return new StatusCodeResult(statusCode: StatusCodes.Status204NoContent);
    }

    [HttpPut]
    [Route(template: "{id}")]
    public StatusCodeResult EditContact(Contact model, Guid id)
    {
      if (ModelState.IsValid && (!string.IsNullOrWhiteSpace(value: model.Email) || !string.IsNullOrWhiteSpace(value: model.Phone)))
      {
        List<Contact> contacts = GetContacts();
        Contact toEdit = contacts.Where(x => x.Id == id).FirstOrDefault();
        toEdit.Update(newInfo: model);
        SetContacts(contacts: contacts);
        return new StatusCodeResult(statusCode: StatusCodes.Status204NoContent);
      }

      return new StatusCodeResult(statusCode: StatusCodes.Status400BadRequest);
    }

    [NonAction]
    private List<Contact> GetContacts()
    {
      string s = HttpContext.Session.GetString(key: "contacts");
      return s != null ? ((JArray)JsonConvert.DeserializeObject(value: s)).ToObject<List<Contact>>() : new List<Contact>();
    }

    [NonAction]
    private void SetContacts(List<Contact> contacts)
    {
      HttpContext.Session.SetString(key: "contacts", value: JsonConvert.SerializeObject(value: contacts));
    }
  }
}
