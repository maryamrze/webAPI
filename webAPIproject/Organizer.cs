using System.ComponentModel.DataAnnotations;

namespace A2.Models
{
    public class Organizers
    {
        [Key]
        public string Name { get; set; }
        public string Password { get; set; }

    }
}