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
    public class TroskoviController : AuthToken
    {
        private MoneySaverEntities db = new MoneySaverEntities();


        #region GetMjesecniTroskoviByKorisnikId
        [HttpGet]
        [Route("api/Troskovi/getMjesecniTroskoviByKorisnikID/{korisnikID}")]
        public IHttpActionResult getMjesecniTroskoviByKorisnikID(int korisnikID)
        {

            List<string> mjeseci=new List<string>();
            List<Trosak> korisnikoviTroskovi = db.Trosak.Where(x => x.Aktivnost.KorisnikID == korisnikID).ToList();
            List<double> troskovi = new List<double>();
            List<int> troskoviId = new List<int>();

            foreach (var x in korisnikoviTroskovi)
            {
                String provjeriMjesecGodinu = x.Datum.Value.Month.ToString() + "/" + x.Datum.Value.Year.ToString();

                if (mjeseci.Count == 0)
                {
                    troskoviId.Add(x.TrosakID);
                    mjeseci.Add(provjeriMjesecGodinu);
                    double trosak = 0;

                    foreach (var tr in korisnikoviTroskovi)
                    {
                        if (tr.Datum.Value.Month == x.Datum.Value.Month && tr.Datum.Value.Year == x.Datum.Value.Year)
                        {
                            trosak += (double)tr.Iznos;
                        }
                    }

                    troskovi.Add(trosak);
                }

                else
                {
                    bool postojiMjesec = false;

                    foreach (var y in mjeseci)
                    {
                        if (y.Equals(provjeriMjesecGodinu))
                            postojiMjesec = true;
                    }

                    if (postojiMjesec == false)
                    {
                        troskoviId.Add(x.TrosakID);
                        mjeseci.Add(provjeriMjesecGodinu);
                        double trosak = 0;

                        foreach (var tr in korisnikoviTroskovi)
                        {
                            if (tr.Datum.Value.Month == x.Datum.Value.Month && tr.Datum.Value.Year == x.Datum.Value.Year)
                            {
                                trosak += (double)tr.Iznos;
                            }
                        }

                        troskovi.Add(trosak);
                    }
                }
            }


            TrosakMjesecniPregledVM mjesecnoGodisnjiTroskovi = new TrosakMjesecniPregledVM();
            mjesecnoGodisnjiTroskovi.lista = new List<TrosakMjesecniPregledVM>();

            for (int i = 0; i < mjeseci.Count; i++)
            {
                TrosakMjesecniPregledVM pojedinacniTrosak = new TrosakMjesecniPregledVM
                {
                    KorisnikId = korisnikID,
                    MjesecGodina = mjeseci[i],
                    Trosak = troskovi[i],
                    TrosakId=troskoviId[i]
                };
                if(troskovi[i]!=0)
                    mjesecnoGodisnjiTroskovi.lista.Add(pojedinacniTrosak);

            }
                        

            
            return Ok(mjesecnoGodisnjiTroskovi);
        }
        #endregion

        #region getMjesecniTroskoviDetaljnoByTrosakAktivnost
        [HttpGet]
        [Route("api/Troskovi/getMjesecniTroskoviDetaljnoByTrosakAktivnost/{KorisnikId}/{TrosakId}")]
        public IHttpActionResult getMjesecniTroskoviDetaljnoByTrosakAktivnost(int KorisnikId,int TrosakId)
        {

             List<Aktivnost> aktivnosti = new List<Aktivnost>();
             List<Trosak> korisnikoviTroskovi = db.Trosak
                 .Where(x => x.Aktivnost.KorisnikID == KorisnikId).ToList();
             List<double> troskovi = new List<double>();


                Trosak trosak = db.Trosak.Where(x => x.TrosakID == TrosakId).FirstOrDefault();
            if (trosak!= null)
            {
                DateTime datumMjesecGodina = (DateTime)trosak.Datum;
                List<Trosak> troskoviUovomPeriodu = new List<Trosak>();

                foreach (var x in korisnikoviTroskovi)
                    if (x.Datum.Value.Month.Equals(datumMjesecGodina.Month) && x.Datum.Value.Year.Equals(datumMjesecGodina.Year))
                        troskoviUovomPeriodu.Add(x);
                

                foreach (var x in troskoviUovomPeriodu) { 
                        if (aktivnosti.Count == 0)
                        {
                            aktivnosti.Add(x.Aktivnost);
                        }
                        else
                        {

                            foreach (var y in troskoviUovomPeriodu)
                            {
                                bool postojiAktivnost = false;

                                    foreach (var akt in aktivnosti)
                                    {
                                        if (akt.AktivnostID == y.AktivnostID)
                                            postojiAktivnost = true;
                                    }
                                    if (postojiAktivnost == false)
                                        aktivnosti.Add(y.Aktivnost); 
                            }
                        }
                    }
                //grupisu se aktivnosti (distinct group by) na kojima su stvarani troskovi u tom periodu

                foreach (var x in aktivnosti)
                {
                    double trosakZaMjesec = 0;
                    foreach (var y in troskoviUovomPeriodu)
                    {
                            if (x.AktivnostID == y.AktivnostID)
                            {
                                trosakZaMjesec += (double)y.Iznos;
                            } 
                    }
                    troskovi.Add(trosakZaMjesec);
                }//svi grupisani vec zajedno sa troskovima nakon ovog foreacha

                TrosakMjesecniPregledDetaljnoVM grupisaniMjesecniTroskovi = new TrosakMjesecniPregledDetaljnoVM();
                grupisaniMjesecniTroskovi.lista = new List<TrosakMjesecniPregledDetaljnoVM>();


                for (int i = 0; i < aktivnosti.Count; i++)
                {
                    TrosakMjesecniPregledDetaljnoVM pojedinacnaGrupa = new TrosakMjesecniPregledDetaljnoVM
                    {
                        KorisnikId = KorisnikId,
                        Aktivnost = aktivnosti[i].Naziv,
                        Trosak = troskovi[i],
                        AktivnostId = aktivnosti[i].AktivnostID,
                        MjesecGodina = datumMjesecGodina.Month.ToString() + "/" + datumMjesecGodina.Year.ToString(),
                        Mjesec = datumMjesecGodina.Month,
                        Godina = datumMjesecGodina.Year
                    };
                    if (troskovi[i] != 0)
                        grupisaniMjesecniTroskovi.lista.Add(pojedinacnaGrupa);
                }
                grupisaniMjesecniTroskovi.Ukupno = 0;
                for (int i = 0; i < troskovi.Count; i++)
                    grupisaniMjesecniTroskovi.Ukupno += troskovi[i];
                grupisaniMjesecniTroskovi.MjesecGodina = datumMjesecGodina.Month.ToString() + "/" + datumMjesecGodina.Year.ToString();
                grupisaniMjesecniTroskovi.Mjesec = datumMjesecGodina.Month;
                grupisaniMjesecniTroskovi.Godina = datumMjesecGodina.Year;
                return Ok(grupisaniMjesecniTroskovi);
            }
            else return Ok();
        }
        #endregion

        #region getMjesecniTroskoviDetaljnoByDatumAktivnost
        [HttpGet]
        [Route("api/Troskovi/getMjesecniTroskoviDetaljnoByDatumAktivnost/{KorisnikId}/{AktivnostId}/{Mjesec}/{Godina}")]
        public IHttpActionResult getMjesecniTroskoviDetaljnoByDatumAktivnost(int KorisnikId, int AktivnostId,int Mjesec, int Godina)
        {
            List<string> datumi = new List<string>();
            List<double> troskovi = new List<double>();
            List<int> troskoviId = new List<int>();
            Aktivnost aktivnost = db.Aktivnost.Where(x => x.AktivnostID == AktivnostId).FirstOrDefault();
            List<Trosak> korisnikoviTroskovi = db.Trosak
                 .Where(x => x.Aktivnost.KorisnikID == KorisnikId)
                 .Where(x=>x.AktivnostID==AktivnostId)
                 .ToList();

            foreach(var x in korisnikoviTroskovi)
            {
                if (x.Datum.Value.Month==Mjesec&&x.Datum.Value.Year==Godina)
                {
                    datumi.Add(x.Datum.Value.Day.ToString() + "." + x.Datum.Value.Month.ToString() + '.' + x.Datum.Value.Year.ToString());
                    troskovi.Add((double)x.Iznos);
                    troskoviId.Add(x.TrosakID);
                }
            }

            TrosakMjesecniPregledDetaljnoAktivnostiVM model = new TrosakMjesecniPregledDetaljnoAktivnostiVM();
            model.lista = new List<TrosakMjesecniPregledDetaljnoAktivnostiVM>();
            model.KorisnikId = KorisnikId;
            model.Aktivnost = aktivnost.Naziv;

            for (int i=0;i<datumi.Count; i++)
            {
                TrosakMjesecniPregledDetaljnoAktivnostiVM add = new TrosakMjesecniPregledDetaljnoAktivnostiVM
                {
                    Datum = datumi[i],
                    Iznos = troskovi[i],
                    KorisnikId = KorisnikId,
                    TrosakId = troskoviId[i],
                    Aktivnost=aktivnost.Naziv
                };
                if (troskovi[i] != 0)
                    model.lista.Add(add);
            }

            return Ok(model);
        }
        #endregion

        #region DeleteTrosak
        [HttpDelete]
        [Route("api/Troskovi/DeleteTrosak/{TrosakId}")]
        public IHttpActionResult DeleteAktivnost(int trosakId)
        {
            Trosak x = db.Trosak.Find(trosakId);
            if (x != null)
            {
                db.Trosak.Remove(x);
                db.SaveChanges();
            }
            return Ok();

        }
        #endregion

        #region PostTrosak
        [ResponseType(typeof(Trosak))]
        public IHttpActionResult PostTrosak([FromBody] TrosakAddVM trosak)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Trosak tr = new Trosak
            {
                AktivnostID=trosak.AktivnostId,
                Datum=new DateTime(trosak.Godina, trosak.Mjesec, trosak.Dan),
                Iznos=(decimal)trosak.Iznos
            };

                db.Trosak.Add(tr);
                db.SaveChanges();
            


            return Ok();
        }
        #endregion

        #region PutTrosak
        [ResponseType(typeof(Trosak))]
        public IHttpActionResult PutTrosak([FromBody] TrosakEditVM trosak)
        {


            Trosak put = db.Trosak.Where(a => a.TrosakID == trosak.TrosakId).FirstOrDefault();
            try
            {
                put.Iznos = (decimal)trosak.Iznos;
                db.Trosak.Attach(put);
                db.Entry(put).State = EntityState.Modified;
                db.SaveChanges();
            }
            catch (Exception ex) { throw ex; }

            return Ok();

        }
        #endregion
    }
}
