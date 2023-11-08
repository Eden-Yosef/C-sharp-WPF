using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectExams
{
    public class User
    {
        public string ID { get; set; }

        public string Name { get; set; }

        public string Password { get; set; }

        public bool IsStudent { get; set; }

        public User( string id ,string name, string password, bool isStudent )
        {
            this.ID = id;
            this.Name = name;
            this.Password = password;
            this.IsStudent = isStudent;
        }
        public User() : this("" , "", "", false)
        {

        }
    }
}
