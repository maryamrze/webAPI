using A2.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Collections.Generic;
namespace A2.Data
{
    public interface IA2Repo
    {
        //Checks if user is registered
        public bool ValidLogin(string username, string password);

        //Checks if user is organizer
        public bool IsOrganizer(string userName, string password);
        //Checks if user is in system
        public Users GetUserByName(string n);
        //Checks if Organizer is in system
        public Organizers GetOrganizerByName(string n);
        //Gets number of events
        public int EventCount();
        //Gets event by ID
        public Events GetEventByID(int id);

        //Endpoint 1
        public Users Register(Users user);

        //Endpoint 2
        public Signs PurchaseSign(string id);

        //Endpoint 3
        public Events AddEvent(Events EI);
    }
}