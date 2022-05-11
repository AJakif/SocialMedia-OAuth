using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SocialBOL
{
    public class Authentication
    {
        public string OID { get; set; }
        public string Id { get; set; }
        public string Password { get; set; }
        public string GoogleId { get; set; }
        public string FacebookId { get; set; }
        public string LoginType { get; set; }
        public string Status { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public string Email1 { get; set; }
        public string Email2 { get; set; }
        public string SecurityQuestion { get; set; }
        public string SecurityAnswer { get; set; }
        public string FullName
        {
            get
            {
                return this.FirstName + " " + this.LastName;
            }
        }
    }
}
