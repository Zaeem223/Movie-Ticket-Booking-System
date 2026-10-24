using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TicketsBookingApp.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Cnic { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Email { get; set; }
        public string Location { get; set; }
        public string Role { get; set; }

        public User(string userName, string password,
                string cnic, string gender, string email, string location)
        {
            //UserId = userId;
            UserName = userName;
            Password = password;

            Cnic = cnic;
            Gender = gender;
            Email = email;
            Location = location;

        }
        public User()
        {

        }
    }
}
