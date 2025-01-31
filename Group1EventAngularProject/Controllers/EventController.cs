﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Group1EventAngularProject.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Group1EventAngularProject.Controllers
{
    [Route("api/[controller]")]
    public class EventController : Controller
    {
        EventDbContext dbContext = new EventDbContext();

        [HttpGet("Events")]
        public List<Event> GetEvents()
        {
            return dbContext.Events.OrderBy(e=>e.Date).Where(events=>events.Date>=DateTime.Now).ToList();
        }

        [HttpGet("Favorites")]
        public List<Event> GetFavorites()
        {
            List<int> favIds = new List<int>();
            List<Event> favorites = new List<Event>();

            favIds = dbContext.Favorites.Select(y => y.EventsId).ToList();

            foreach (int n in favIds)
            {
                Event E = dbContext.Events.FirstOrDefault(f => f.Id == n);
                favorites.Add(E);
            }
            return favorites;
        }
        [HttpPost()]
        public Event addEvent(string _category, string _name, DateTime _date, string _location)
        {
            Event newEvent = new Event()
            {
                Category = _category,
                Name = _name,
                Date = _date,
                Location = _location
            };
            dbContext.Events.Add(newEvent);
            dbContext.SaveChanges();

            return newEvent;
        }
        [HttpGet("{id}")]
        public Event getDetails(int id)
        {
            return dbContext.Events.FirstOrDefault(e => e.Id == id);
        }

        [HttpPost("addFavorite")]
        public Favorite addFavorite(string _username, int _eventsid)
        {
            Favorite newFavorite = new Favorite()
            {
                UserName = _username,
                EventsId = _eventsid
            };
            dbContext.Favorites.Add(newFavorite);
            dbContext.SaveChanges();

            return newFavorite;

        }

        [HttpDelete("removeFavorite")]
        public Favorite removeFavorite(int _id)
        {
            Favorite fav = dbContext.Favorites.FirstOrDefault(f=> f.Id ==_id); 

            dbContext.Favorites.Remove(fav);
            dbContext.SaveChanges();

            return fav;
        }

        [HttpGet("listFavorite")]
        public List <Favorite> listFavorite() 
        {
            return dbContext.Favorites.ToList();
        }

        [HttpGet("Favorites/{user}")]
        public List<Event> userFavorites(string user)
        {
            List<Favorite> userfavs = dbContext.Favorites.Where(u => u.UserName == user).ToList();
            List<Event> events = new List<Event>();
            foreach(Favorite E in userfavs)
            {
                Event fav = dbContext.Events.FirstOrDefault(f => f.Id == E.EventsId);
                events.Add(fav);
            }
            return events.Where(e => e.Date >= DateTime.Now).OrderBy(e => e.Date).ToList();
        }

        [HttpGet("chooseCategory/{category}")]
        //[HttpGet("chooseCategory")]
        public List<Event> chooseCategory(string category)
        {
            if (string.IsNullOrEmpty(category))
            {
                return dbContext.Events.ToList();
            }
            else
            {
                return dbContext.Events.Where(e => e.Category == category && e.Date >= DateTime.Now).OrderBy(e => e.Date).ToList();
            }
        }

    }
}

