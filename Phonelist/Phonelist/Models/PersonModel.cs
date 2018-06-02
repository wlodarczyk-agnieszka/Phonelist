using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Phonelist.Models
{
    public class PersonModel
    {
        //TODO: walidacja modelu - dodać atrybuty

        public int ID { get; set; }

        [Required]
        [DisplayName("Imię")]
        [MinLength(3)]
        public string FirstName { get; set; }

        [DisplayName("Nazwisko")]
        [MinLength(3)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(30)]
        [DisplayName("Telefon")]
        public string Phone { get; set; }

        [EmailAddress]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Updated { get; set; }

        public PersonModel()
        {

        }

        public PersonModel(int id, string firstName, string lastName, string phone, string email)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
        }

        public PersonModel(int id, string firstName, string lastName, string phone, string email, DateTime created, DateTime? updated)
        {
            ID = id;
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            Email = email;
            Created = created;
            Updated = updated;
        }

    }
}
