using DVDLibraryWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace DVDLibraryWebAPI.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class DVDController : ApiController
    {
        IDVDRepo repo = RepoFactory.CreateRepo();

        [Route("dvds/")]
        [AcceptVerbs("GET")]
        public IHttpActionResult GetAll()
        {
            return Ok(repo.GetAll());
        }

        [Route("dvd/{id}")]
        [AcceptVerbs("PUT")]
        public void EditDVD(int id, DVDView dvd)
        {
            repo.Update(dvd);
        }

        [Route("dvd")]
        [AcceptVerbs("POST")]
        public IHttpActionResult AddDVD(DVDView dvd)
        {
            repo.Create(dvd);

            return Created($"dvd/{dvd.DVDId}", dvd);
        }

        [Route("dvd/{id}")]
        [AcceptVerbs("DELETE")]
        public IHttpActionResult DeleteDVD(int id)
        {
            if (repo.Delete(id))
            {
                return Ok();
            }        
            
            return NotFound();
        }

        [Route("dvds/{searchCat}/{searchTerm}")]
        [AcceptVerbs("GET")]
        public List<DVDView> SearhDVDs(string searchCat, string searchTerm)
        {
            return repo.GetSearch(searchCat, searchTerm);
        }
    }
}
