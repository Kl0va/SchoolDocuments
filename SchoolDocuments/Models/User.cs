using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SchoolDocuments.Models
{
    class User
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string secondName { get; set; }
        public string middleName { get; set; }
        public string email { get; set; }
        public string role { get; set; }

        [JsonIgnore]
        public string fullName
        {
            get
            {
                return firstName + " " + secondName + " " + middleName;
            } 
        }

        [JsonConstructor]
        public User(string FirstName, string SecondName,string MiddleName,string Email,string Role)
        {
            firstName = FirstName;
            secondName = SecondName;
            middleName = MiddleName;
            email = Email;
            role = Role;
        }
    }
}
