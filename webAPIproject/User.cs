using System.ComponentModel.DataAnnotations;

namespace A2.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }  

        public List<Progress> Progresses { get; set; }
    }

}
