using MoneySaverAPI.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MoneySaverAPI.Models;
using MoneySaverAPI.ModelViewModels;

namespace MoneySaverAPI.Controllers
{
    public class AutentifikacijaController : AuthToken
    {
        protected string GetIPAddress()
        {
            System.Web.HttpContext context = System.Web.HttpContext.Current;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return context.Request.ServerVariables["REMOTE_ADDR"];
        }


        private MoneySaverEntities db = new MoneySaverEntities();

        [HttpGet]
        [Route("api/Autentifikacija/Login/{username}/{pass}")]
        public IHttpActionResult Login(string username, string pass)
        {
            string token = Guid.NewGuid().ToString();

            Korisnik korisnik = db.Korisnik
                .Where(x => x.KorisnickoIme == username && x.LozinkaSalt == pass).FirstOrDefault();

            if (korisnik != null)
            {
                AutentifikacijaResultVM a = new AutentifikacijaResultVM();
                a.KorisnikId = korisnik.KorisnikID;
                a.KorisnickoIme = korisnik.KorisnickoIme;
                a.LozinkaSalt = korisnik.LozinkaSalt;
                a.Token = token;


                db.AutorizacijskiToken.Add(new AutorizacijskiToken
                {
                    Vrijednost = a.Token,
                    KorisnikID = a.KorisnikId,
                    VrijemeEvidentiranja = DateTime.Now,
                    IpAdresa = GetIPAddress()
                });

                db.SaveChanges();

                return Ok(a);
            }

            AutentifikacijaResultVM empty = new AutentifikacijaResultVM();
            empty.KorisnickoIme = "pogresanLogin";
            return Ok(empty);
        }

        [HttpDelete]
        [Route("api/Autentifikacija/Logout")]
        public IHttpActionResult Logout()
        {
            IzbrisiToken();
            return Ok();
        }
    }
}
