using A2.Models;
using A2.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using Microsoft.EntityFrameworkCore.ChangeTracking;

public class A2Repo : IA2Repo
{
    private readonly A2DbContext _dbcontext;
    public A2Repo(A2DbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }

    //Checks if user is registered
    public bool ValidLogin(string userName, string password)
    {
        Users c=_dbcontext.Users.FirstOrDefault(u => u.UserName == userName && u.Password==password);
        if (c == null)
            return false;
        else 
            return true;
    }

    //Checks if user is organizer
    public bool IsOrganizer(string userName, string password)
    {
        Organizers org = _dbcontext.Organizers.FirstOrDefault(o => o.Name== userName && o.Password == password);
        if (org == null)
            return false;
        else
            return true;
    }

    //Checks if user is in system
    public Users GetUserByName(string n)
    {
        return _dbcontext.Users.FirstOrDefault(name => name.UserName==n);
    }

    //Checks if Organizer is in system
    public Organizers GetOrganizerByName(string n)
    {
        return _dbcontext.Organizers.FirstOrDefault(name => name.Name == n);
    }

    //Gets event by ID
    public Events GetEventByID(int id)
    {
        return _dbcontext.Events.FirstOrDefault(e => e.Id == id);
    }

    //Gets number of events
    public int EventCount()
    {
        return _dbcontext.Events.Count();
    }

    //Endpoint 1
    public Users Register(Users un)
    {
        Users user=_dbcontext.Users.FirstOrDefault(u => u.UserName==un.UserName);
        if(user==null)
        {
            EntityEntry<Users> u=_dbcontext.Users.Add(un);
            Users usr = u.Entity;
            _dbcontext.SaveChanges();
            return usr;
        } else
        {
            return null;
        }
    }

    //Endpoint 2
    public Signs PurchaseSign(string id)
    {
        Signs sn = _dbcontext.Signs.FirstOrDefault(s => s.Id == id);
        if(sn==null)
        {
            return null;
        } else
        {
            return sn;
        }
    }

    //Endpoint 3
    public Events AddEvent(Events e)
    {
        var addedEvent = _dbcontext.Events.Add(e);
        _dbcontext.SaveChanges();
        return addedEvent.Entity;
    }

    public IEnumerable<User> GetLeaderboard()
    {
        return _dbContext.Users
            .OrderByDescending(u => u.Progresses.Sum(p => p.PracticeHours))
            .Take(10)
            .ToList();
    }

    public bool ValidLoginUser(string userName, string password)
    {
        User user = _dbContext.Users.FirstOrDefault(u => u.UserName == userName);
        if (user == null || !VerifyPassword(password, user.Password))
        {
            return false;
        }
        return true;
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }
}