using MoneySaverAPI.Helper;
using MoneySaverAPI.Models;
using MoneySaverAPI.ModelViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace MoneySaverAPI.Controllers
{
    public class AktivnostiController : AuthToken
    {

        private MoneySaverEntities db = new MoneySaverEntities();

        #region GetAktivnostiALL
        [HttpGet]
        [Route("api/Aktivnosti/getAktivnostiAll/")]
        public IHttpActionResult getAktivnostiByKorisnikID()
        {


            AktivnostiResultVM model = new AktivnostiResultVM
            {
                rows = db.Aktivnost
                
                .Select(s => new AktivnostiResultVM.Row
                {
                    AktivnostId = s.AktivnostID,
                    Naziv = s.Naziv

                }).ToList()
            };
            if (model.rows.Count == 0)
                model.imaPodataka = "NemaPodataka";
            return Ok(model);
        }

        #endregion

        #region GetAktivnostByKorisnikID
        [HttpGet]
        [Route("api/Aktivnosti/getAktivnostiByKorisnikID/{korisnikID}")]
        public IHttpActionResult getAktivnostiByKorisnikID(int korisnikID)
        {
            

            AktivnostiResultVM model = new AktivnostiResultVM
            {
                rows = db.Aktivnost
                .Where(x=>x.KorisnikID==korisnikID)
                .Select(s => new AktivnostiResultVM.Row
                {
                    AktivnostId = s.AktivnostID,
                    Naziv = s.Naziv

                }).ToList()
            };
            if (model.rows.Count == 0)
                model.imaPodataka = "NemaPodataka";
            return Ok(model);
        }

        #endregion

        #region PostAktivnost
        [ResponseType(typeof(Aktivnost))]
        public IHttpActionResult PostAktivnost([FromBody] AktivnostAddVM aktivnost)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Aktivnost akt = new Aktivnost
            {
                KorisnikID=aktivnost.KorisnikId,
                Naziv=aktivnost.Naziv
            };

            try
            {
                db.Aktivnost.Add(akt);
                db.SaveChanges();
            }
            catch (Exception ex) {
                throw ex;
            }

            return Ok();
        }
        #endregion

        #region DeleteAktivnost
        [HttpDelete]
        [Route("api/Aktivnosti/DeleteAktivnost/{aktivnostId}")]
        public IHttpActionResult DeleteAktivnost(int aktivnostId)
        {
            List<Trosak> troskovi = db.Trosak.Where(y => y.AktivnostID == aktivnostId).ToList();

            if (troskovi.Count > 0)
            {
                foreach(var y in troskovi)
                {
                    db.Trosak.Remove(y);
                    db.SaveChanges();
                }
            }
            Aktivnost x = db.Aktivnost.Find(aktivnostId);
            if (x != null)
            {
                db.Aktivnost.Remove(x);
              db.SaveChanges();
            }
            return Ok();
            
        }
        #endregion

        #region PutAktivnost
        [ResponseType(typeof(Aktivnost))]
        public IHttpActionResult PutAktivnost([FromBody] AktivnostEditVM aktivnost)
        {

        
            Aktivnost put=db.Aktivnost.Where(a => a.AktivnostID == aktivnost.AktivnostId).FirstOrDefault();
            try
            {
                put.Naziv = aktivnost.Naziv;
                db.Aktivnost.Attach(put);
                db.Entry(put).State = EntityState.Modified;
                db.SaveChanges();
            }catch(Exception ex) { throw ex; }

            return Ok();

        }
        #endregion

    }
}
