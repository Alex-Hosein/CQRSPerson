using System;
using System.Collections.Generic;

namespace CQRSPerson.Domain.Entities
{
    public partial class Person
    {
        public int PersonId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public string Interests { get; set; }
        public string Image { get; set; }
    }
}
