using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using AddressBook.Models;
using AddressBook.Controllers;
using AddressBook.Storage;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace AddressBook.Tests.Controllers
{
  public class ContactsControllerTests
  {
    [Fact]
    public void ContactsTest()
    {
      ContactsController controller = new ContactsController(store: new MockContactsStore());

      IEnumerable<Contact> contacts = controller.Contacts();

      Assert.NotNull(@object: contacts);
    }

    [Fact]
    public void CreateContactTest_ValidModel()
    {
      ContactsController controller = new ContactsController(store: new MockContactsStore());

      StatusCodeResult result = controller.CreateContact(model: new Contact()
      {
        FirstName = "a",
        LastName = "b",
        Email = "a@b.com"
      });

      Assert.Equal(expected: StatusCodes.Status201Created, actual: result.StatusCode);
    }

    [Fact]
    public void CreateContactTest_InvalidModel()
    {
      ContactsController controller = new ContactsController(store: new MockContactsStore());

      StatusCodeResult result = controller.CreateContact(model: new Contact()
      {
        FirstName = "a"
      });

      Assert.Equal(expected: StatusCodes.Status400BadRequest, actual: result.StatusCode);
    }

    [Fact]
    public void CreateContactTest_NoEmailOrPhone()
    {
      ContactsController controller = new ContactsController(store: new MockContactsStore());

      StatusCodeResult result = controller.CreateContact(model: new Contact()
      {
        FirstName = "a",
        LastName = "b"
      });

      Assert.Equal(expected: StatusCodes.Status400BadRequest, actual: result.StatusCode);
    }

    [Fact]
    public void DeleteContactTest()
    {
      ContactsController controller = new ContactsController(store: new MockContactsStore());
      Contact c = new Contact()
      {
        FirstName = "a",
        LastName = "b",
        Email = "a@b.com"
      };
      controller.CreateContact(model: c);
      Contact saved = controller.Contacts().First();

      StatusCodeResult result = controller.DeleteContact(id: saved.Id);

      Assert.Equal(expected: StatusCodes.Status204NoContent, actual: result.StatusCode);
    }

    [Fact]
    public void DeleteContactTest_NotExists()
    {
      ContactsController controller = new ContactsController(store: new MockContactsStore());
      Guid random = Guid.NewGuid();

      StatusCodeResult result = controller.DeleteContact(id: random);

      Assert.Equal(expected: StatusCodes.Status400BadRequest, actual: result.StatusCode);
    }

    [Fact]
    public void EditContactTest()
    {
      ContactsController controller = new ContactsController(store: new MockContactsStore());
      Contact c = new Contact()
      {
        FirstName = "a",
        LastName = "b",
        Email = "a@b.com"
      };
      controller.CreateContact(model: c);
      Contact saved = controller.Contacts().First();
      Contact updated = new Contact()
      {
        Id = saved.Id,
        FirstName = "b",
        LastName = "a",
        Email = "b@a.com"
      };

      StatusCodeResult result = controller.EditContact(model: updated);

      Assert.Equal(expected: StatusCodes.Status204NoContent, actual: result.StatusCode);
    }

    [Fact]
    public void EditContactTest_InvalidModel()
    {
      ContactsController controller = new ContactsController(store: new MockContactsStore());
      Contact c = new Contact()
      {
        FirstName = "a",
        LastName = "b",
        Email = "a@b.com"
      };
      controller.CreateContact(model: c);
      Contact saved = controller.Contacts().First();
      Contact updated = new Contact()
      {
        Id = saved.Id,
        FirstName = "b"
      };

      StatusCodeResult result = controller.EditContact(model: updated);

      Assert.Equal(expected: StatusCodes.Status400BadRequest, actual: result.StatusCode);
    }

    [Fact]
    public void EditContactTest_NoEmailOrPhone()
    {
      ContactsController controller = new ContactsController(store: new MockContactsStore());
      Contact c = new Contact()
      {
        FirstName = "a",
        LastName = "b",
        Email = "a@b.com"
      };
      controller.CreateContact(model: c);
      Contact saved = controller.Contacts().First();
      Contact updated = new Contact()
      {
        Id = saved.Id,
        FirstName = "b",
        LastName = "a"
      };

      StatusCodeResult result = controller.EditContact(model: updated);

      Assert.Equal(expected: StatusCodes.Status400BadRequest, actual: result.StatusCode);
    }

    [Fact]
    public void EditContactTest_NotExists()
    {
      ContactsController controller = new ContactsController(store: new MockContactsStore());
      Contact c = new Contact()
      {
        Id = Guid.NewGuid()
      };

      StatusCodeResult result = controller.EditContact(model: c);

      Assert.Equal(expected: StatusCodes.Status400BadRequest, actual: result.StatusCode);
    }

    private class MockContactsStore : IContactsStore
    {
      List<Contact> _contacts;

      public bool Create(Contact contact)
      {
        _contacts.Add(item: contact);
        return true;
      }

      public bool Delete(Contact contact)
      {
        return _contacts.Remove(item: contact);
      }

      public Contact FindById(Guid id)
      {
        return _contacts.Where(x => x.Id == id).FirstOrDefault();
      }

      public List<Contact> GetContacts()
      {
        return _contacts;
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

          return true;
        }

        return false;
      }

      public MockContactsStore()
      {
        _contacts = new List<Contact>();
      }
    }
  }
}
