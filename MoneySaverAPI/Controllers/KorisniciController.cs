using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MoneySaverAPI.Models;
using System.Web.Http.Description;
using MoneySaverAPI.Helper;

namespace MoneySaverAPI.Controllers
{
    public class KorisniciController : AuthToken
    {
        private MoneySaverEntities db = new MoneySaverEntities();


        // POST: api/Korisnici
        [ResponseType(typeof(Korisnik))]

        public IHttpActionResult PostKorisnici([FromBody] Korisnik korisnik)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            db.Korisnik.Add(korisnik);

            try
            {
                db.SaveChanges();
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        
                        raise = new InvalidOperationException(message, raise);
                    }
                }

                throw raise;
            }
            return Ok();
        }

    }
}
