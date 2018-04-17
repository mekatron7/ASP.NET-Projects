using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DVDLibraryWebAPI.Models
{
    public class InMemoryRepo : IDVDRepo
    {
        public static List<DVDView> _DVDs { get; set; } = new List<DVDView>()
            {
                new DVDView { DVDId = 1, Title = "Django Unchained", Genre = "Action", Rating = "R", Director = "Quentin Tarentino", ReleaseYear = 2012 },
                new DVDView { DVDId = 2, Title = "Jumanji: Welcome to the Jungle", Genre = "Action/Comedy", Rating = "PG-13", Director = "Jake Kasdan", ReleaseYear = 2017 },
                new DVDView { DVDId = 3, Title = "Wall-E", Genre = "Family", Rating = "PG", Director = "Andrew Stanton", ReleaseYear = 2008 },
                new DVDView { DVDId = 4, Title = "The Revenant", Genre = "Action/Suspense", Rating = "R", Director = "Alejandro Iñárritu", ReleaseYear = 2016 },
                new DVDView { DVDId = 5, Title = "Star Wars: The Force Awake and Bakens", Genre = "Sci-Fi", Rating = "R", Director = "JJ Dabrams", ReleaseYear = 2016 }
            };

        public List<DVDView> GetAll()
        {
            return _DVDs;
        }

        public void Create(DVDView newDVD)
        {
            if (_DVDs.Any())
            {
                newDVD.DVDId = _DVDs.Max(d => d.DVDId) + 1;
            }
            else
            {
                newDVD.DVDId = 1;
            }

            _DVDs.Add(newDVD);
        }

        public void Update(DVDView updatedDVD)
        {
            _DVDs.RemoveAll(d => d.DVDId == updatedDVD.DVDId);
            _DVDs.Add(updatedDVD);
        }

        public bool Delete(int id)
        {
            return _DVDs.RemoveAll(d => d.DVDId == id) > 0;
        }

        public List<DVDView> GetSearch(string category, string term)
        {
            if(category == "director")
            {
                return _DVDs.Where(d => d.Director.Contains(term)).ToList();
            }
            else if (category == "title")
            {
                return _DVDs.Where(d => d.Title.Contains(term)).ToList();
            }   
            else if (category == "rating")
            {
                return _DVDs.Where(d => d.Rating == term).ToList();
            }            
            else
            {
                return _DVDs.Where(d => d.ReleaseYear == int.Parse(term)).ToList();
            }
                
        }
    }
}