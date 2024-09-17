using System.ComponentModel.DataAnnotations;

namespace A2.Models
{
    public class Sign
    {
        public int SignId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }  
        public string DifficultyLevel { get; set; } 
        public string VideoUrl { get; set; }  
        public string Caption { get; set; }  

        public List<Purchase> Purchases { get; set; }
    }

}

