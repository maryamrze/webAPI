﻿using System.ComponentModel.DataAnnotations;

namespace A2.Models
{
    public class EventInput
    {
        public string Start { get; set; }
        public string End { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public string Location { get; set; }

    }
}