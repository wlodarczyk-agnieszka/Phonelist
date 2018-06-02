using System;
using System.Collections.Generic;
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
        public string FirstName { get; set; }
        public string LastName { get; set; }

        [Required]
        [MaxLength(30)]
        public string Phone { get; set; }

        [EmailAddress]
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
