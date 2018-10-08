using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AddressBook.Models
{
  public class Contact
  {
    public Guid Id { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [Phone]
    [RegularExpression(pattern: "\\d{11}")]
    public string Phone { get; set; }

    public void Update(Contact newInfo)
    {
      FirstName = newInfo.FirstName;
      LastName = newInfo.LastName;
      Email = newInfo.Email;
      Phone = newInfo.Phone;
    }

    public Contact(Contact contact)
    {
      Id = Guid.NewGuid();
      FirstName = contact.FirstName;
      LastName = contact.LastName;
      Email = contact.Email;
      Phone = contact.Phone;
    }

    public Contact()
    {
    }
  }
}
