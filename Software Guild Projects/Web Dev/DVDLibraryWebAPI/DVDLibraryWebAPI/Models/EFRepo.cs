using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace DVDLibraryWebAPI.Models
{
    public class EFRepo : IDVDRepo
    {
        public void Create(DVDView dvd)
        {
            var repository = new DVDCatalogEntities();

            repository.DVDs.Add(dvd);
            repository.SaveChanges();
        }

        public bool Delete(int id)
        {
            var repository = new DVDCatalogEntities();
            bool deleted = false;

            var dvd = repository.DVDs.FirstOrDefault(d => d.DVDId == id);

            if(dvd != null)
            {
                deleted = true;
                repository.DVDs.Remove(dvd);
                repository.SaveChanges();
            }

            return deleted;
        }

        public List<DVDView> GetAll()
        {
            var repository = new DVDCatalogEntities();

            return repository.DVDs.ToList();
        }

        public List<DVDView> GetSearch(string category, string term)
        {
            var repository = new DVDCatalogEntities();
            var results = new List<DVDView>();

            if(category == "director")
            {
                results = repository.DVDs.Where(d => d.Director.Contains(term)).ToList();
            }
            else if(category == "title")
            {
                results = repository.DVDs.Where(d => d.Title.Contains(term)).ToList();
            }
            else if(category == "rating")
            {
                results = repository.DVDs.Where(d => d.Rating == term).ToList();
            }
            else
            {
                int searchYear = int.Parse(term);
                results = repository.DVDs.Where(d => d.ReleaseYear == searchYear).ToList();
            }

            return results;
        }

        public void Update(DVDView updatedDVD)
        {
            var repository = new DVDCatalogEntities();

            repository.Entry(updatedDVD).State = EntityState.Modified;
            repository.SaveChanges();
        }
    }
}