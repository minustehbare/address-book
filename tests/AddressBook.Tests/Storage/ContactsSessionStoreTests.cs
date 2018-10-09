using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AddressBook.Models;
using AddressBook.Storage;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace AddressBook.Tests.Storage
{
  public class ContactsSessionStoreTests
  {
    [Fact]
    public void GetContactsTest()
    {
      ContactsSessionStore store = new ContactsSessionStore(httpContextAccessor: new MockHttpContextAccessor());

      List<Contact> contacts = store.GetContacts();

      Assert.NotNull(contacts);
    }

    [Fact]
    public void CreateTest()
    {
      ContactsSessionStore store = new ContactsSessionStore(httpContextAccessor: new MockHttpContextAccessor());

      bool success = store.Create(contact: new Contact()
      {
        FirstName = "a",
        LastName = "b",
        Email = "a@b.com"
      });

      Assert.True(condition: success);
      Assert.Single(store.GetContacts());
    }

    [Fact]
    public void FindByIdTest()
    {
      ContactsSessionStore store = new ContactsSessionStore(httpContextAccessor: new MockHttpContextAccessor());
      Contact c = new Contact()
      {
        FirstName = "a",
        LastName = "b",
        Email = "a@a.com"
      };
      store.Create(contact: c);

      Contact found = store.FindById(id: c.Id);

      Assert.Equal(expected: c.Id, actual: found.Id);
    }

    [Fact]
    public void UpdateTest()
    {
      ContactsSessionStore store = new ContactsSessionStore(httpContextAccessor: new MockHttpContextAccessor());
      Contact original = new Contact()
      {
        FirstName = "a",
        LastName = "b",
        Email = "a@b.com"
      };
      Contact updated = new Contact()
      {
        Id = original.Id,
        FirstName = "b"
      };
      store.Create(contact: original);

      bool success = store.Update(contact: updated);
      Contact saved = store.FindById(id: original.Id);

      Assert.True(condition: success);
      Assert.Equal(expected: updated.FirstName, actual: saved.FirstName);
    }

    [Fact]
    public void DeleteTest()
    {
      ContactsSessionStore store = new ContactsSessionStore(httpContextAccessor: new MockHttpContextAccessor());
      Contact c = new Contact()
      {
        FirstName = "a",
        LastName = "b",
        Email = "a@b.com"
      };
      store.Create(contact: c);

      bool success = store.Delete(contact: c);

      Assert.True(condition: success);
      Assert.Empty(collection: store.GetContacts());
    }

    private class MockHttpContextAccessor : IHttpContextAccessor
    {
      public HttpContext HttpContext { get; set; }

      public MockHttpContextAccessor()
      {
        HttpContext = new DefaultHttpContext();
        HttpContext.Session = new MockSession();
      }
    }

    private class MockSession : ISession
    {
      private Dictionary<string, byte[]> _store;

      public bool IsAvailable => throw new NotImplementedException();

      public string Id => throw new NotImplementedException();

      public IEnumerable<string> Keys => _store.Keys;

      public void Clear()
      {
        _store.Clear();
      }

      public Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
      {
        throw new NotImplementedException();
      }

      public Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
      {
        throw new NotImplementedException();
      }

      public void Remove(string key)
      {
        _store.Remove(key: key);
      }

      public void Set(string key, byte[] value)
      {
        _store[key: key] = value;
      }

      public bool TryGetValue(string key, out byte[] value)
      {
        return _store.TryGetValue(key: key, value: out value);
      }

      public MockSession()
      {
        _store = new Dictionary<string, byte[]>();
      }
    }
  }
}
