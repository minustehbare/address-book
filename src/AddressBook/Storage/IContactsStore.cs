using AddressBook.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Storage
{
  public interface IContactsStore
  {
    List<Contact> GetContacts();
    bool Create(Contact contact);
    Contact FindById(Guid id);
    bool Update(Contact contact);
    bool Delete(Contact contact);
  }
}
