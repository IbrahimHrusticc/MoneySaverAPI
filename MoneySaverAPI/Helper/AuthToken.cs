using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

using MoneySaverAPI.Models;
using System.Collections.Specialized;
using System.Web;

namespace MoneySaverAPI.Helper
{
    public class AuthToken : ApiController
    {
        private MoneySaverEntities db = new MoneySaverEntities();

        protected string GetAuthToken()
        {
            string authToken = null;
            NameValueCollection headers = HttpContext.Current.Request.Headers;

            if (headers["AuthToken"] != null)
                authToken = headers["AuthToken"];

            return authToken;
        }

        protected bool ProvjeriValidnostTokena()
        {
            string token = GetAuthToken();

            AutorizacijskiToken TokenCheck = db.AutorizacijskiToken
                .Where(s => s.Vrijednost == token)
                .FirstOrDefault();

            if (TokenCheck != null)
            {
                if (TokenCheck.VrijemeEvidentiranja >= DateTime.Now.AddDays(-2))
                {
                    return true;
                }
            }

            return false;
        }
        protected void IzbrisiToken()
        {
            string token = GetAuthToken();

            AutorizacijskiToken TokenCheck = db.AutorizacijskiToken
                .Where(s => s.Vrijednost == token)
                .FirstOrDefault();

            if (TokenCheck != null)
            {
                db.AutorizacijskiToken.Remove(TokenCheck);
                db.SaveChanges();
            }
        }
    }
}
