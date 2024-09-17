using System.ComponentModel.DataAnnotations;

namespace A2.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }  
        public DateTime Date { get; set; }
        public string Location { get; set; }
        public Organizer Organizer { get; set; }
    }

}