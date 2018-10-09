using AddressBook.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace AddressBook.Storage
{
  public class ContactsSessionStore : IContactsStore
  {
    private IHttpContextAccessor _httpContextAccessor;
    private ISession _session => _httpContextAccessor.HttpContext.Session;
    private List<Contact> _contacts;

    protected List<Contact> Contacts
    {
      get
      {
        return _contacts ?? GetContacts();
      }
      private set
      {
        _contacts = value;
      }
    }

    public ContactsSessionStore(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    public List<Contact> GetContacts()
    {
      string s = _session.GetString(key: "contacts");
      List<Contact> temp = s != null ? ((JArray)JsonConvert.DeserializeObject(value: s)).ToObject<List<Contact>>() : new List<Contact>();
      Contacts = temp;
      return Contacts;
    }

    public bool Create(Contact contact)
    {
      Contacts.Add(item: contact);
      return SaveChanges();
    }

    public Contact FindById(Guid id)
    {
      return Contacts.Where(x => x.Id == id).FirstOrDefault();
    }

    public bool Update(Contact contact)
    {
      Contact toUpdate = FindById(id: contact.Id);
      if (toUpdate != null)
      {
        toUpdate.FirstName = contact.FirstName;
        toUpdate.LastName = contact.LastName;
        toUpdate.Email = contact.Email;
        toUpdate.Phone = contact.Phone;

        return SaveChanges();
      }

      return false;
    }

    public bool Delete(Contact contact)
    {
      return Contacts.Remove(item: contact) && SaveChanges();
    }

    private bool SaveChanges()
    {
      try
      {
        _session.SetString(key: "contacts", value: JsonConvert.SerializeObject(value: Contacts));
        return true;
      }
      catch
      {
      }

      return false;
    }
  }
}
