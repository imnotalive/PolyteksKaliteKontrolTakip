using ClosedXML.Excel;
using PolyteksKaliteKontrolTakip.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace PolyteksKaliteKontrolTakip.Controllers
{
    [Authorize]
    public class MusteriTakipFormuController : Controller
    {
        POLY_QDMSEntities db = new POLY_QDMSEntities();

        [AllowAnonymous]
        public ActionResult Index()
        {

            
            return View(db.Qdms_MusteriSikayet.OrderByDescending(q => q.SikayetReklamasyonGelisTarihi));
        }
        [AllowAnonymous]

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_MusteriSikayet qdms_MusteriTakipFormu = db.Qdms_MusteriSikayet.Find(id);
            if (qdms_MusteriTakipFormu == null)
            {
                return HttpNotFound();
            }
            return View(qdms_MusteriTakipFormu);
        }


        [AllowAnonymous]
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_MusteriSikayet qdms_MusteriTakipFormu = db.Qdms_MusteriSikayet.Find(id);
            if (qdms_MusteriTakipFormu == null)
            {
                return HttpNotFound();
            }
            List<Qdms_SikayetGrubu> CountryList = db.Qdms_SikayetGrubu.ToList();
            ViewBag.CountryList = new SelectList(CountryList, "SikayetGrubuID", "SikayetGrubu");

            return View(qdms_MusteriTakipFormu);
        }
        [AllowAnonymous]
        public JsonResult GetStateList(int SikayetGrubuID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Qdms_SikayetSebebi> SikayetSebebiID = db.Qdms_SikayetSebebi.Where(x => x.SikayetGrubuID == SikayetGrubuID).OrderBy(x => x.SikayetSebebi).ToList();
            return Json(SikayetSebebiID, JsonRequestBehavior.AllowGet);

        }
        [AllowAnonymous]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Qdms_MusteriSikayet qdms_MusteriTakipFormu,int? id)
        {
            Qdms_MusteriSikayet fisDetays = db.Qdms_MusteriSikayet.FirstOrDefault(x => x.ID == id);
            if (ModelState.IsValid)
            {
                fisDetays.AksiyonGerceklestirmeTermini = qdms_MusteriTakipFormu.AksiyonGerceklestirmeTermini;
                fisDetays.AksiyonTermini = qdms_MusteriTakipFormu.AksiyonTermini;
                fisDetays.CevapTarihi = qdms_MusteriTakipFormu.CevapTarihi;
                fisDetays.Cile = qdms_MusteriTakipFormu.Cile;
                fisDetays.FirmaYetkilisi = qdms_MusteriTakipFormu.FirmaYetkilisi;
                fisDetays.GecikmeNedeni = qdms_MusteriTakipFormu.GecikmeNedeni;
                fisDetays.IadeBedelBirimi = qdms_MusteriTakipFormu.IadeBedelBirimi;
                fisDetays.IadeBedeli = qdms_MusteriTakipFormu.IadeBedeli;
                fisDetays.IadeMiktari = qdms_MusteriTakipFormu.IadeMiktari;
                fisDetays.IskontoBedeli = qdms_MusteriTakipFormu.IskontoBedeli;
                fisDetays.IskontoBedeliBirimi = qdms_MusteriTakipFormu.IskontoBedeliBirimi;
                fisDetays.KabulEdilenSikayetReklamasyonMiktari = qdms_MusteriTakipFormu.KabulEdilenSikayetReklamasyonMiktari;
                fisDetays.KapatilmaSuresi = qdms_MusteriTakipFormu.KapatilmaSuresi;
                fisDetays.KokNeden = qdms_MusteriTakipFormu.KokNeden;
                fisDetays.MutabakatFormu = qdms_MusteriTakipFormu.MutabakatFormu;
                fisDetays.NumuneGelisTarihi = qdms_MusteriTakipFormu.NumuneGelisTarihi;
                fisDetays.NumuneTipi = qdms_MusteriTakipFormu.NumuneTipi;
                fisDetays.OnaylandiMi = qdms_MusteriTakipFormu.OnaylandiMi;
                fisDetays.Onaylayan = qdms_MusteriTakipFormu.Onaylayan;
                fisDetays.SikayetReklamasyonDuyuruTarihi = qdms_MusteriTakipFormu.SikayetReklamasyonDuyuruTarihi;
                fisDetays.SonucOlarakYapilacakCalisma = qdms_MusteriTakipFormu.SonucOlarakYapilacakCalisma;
                fisDetays.Sorumlu = qdms_MusteriTakipFormu.Sorumlu;
                fisDetays.TeknikZiyareti = qdms_MusteriTakipFormu.TeknikZiyareti;
                fisDetays.SikayetKabulRed = qdms_MusteriTakipFormu.SikayetKabulRed;
                fisDetays.Hazirlayan = qdms_MusteriTakipFormu.Hazirlayan;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.SikayetSebebiID = new SelectList(db.Qdms_SikayetSebebi, "SikayetSebebiID", "SikayetSebebi");
            ViewBag.SikayetGrubuID = new SelectList(db.Qdms_SikayetGrubu, "SikayetGrubuID", "SikayetGrubu");
            return View(qdms_MusteriTakipFormu);
        }

        [AllowAnonymous]
        public ActionResult EditSikayet(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_MusteriSikayet qdms_MusteriTakipFormu = db.Qdms_MusteriSikayet.Find(id);
            if (qdms_MusteriTakipFormu == null)
            {
                return HttpNotFound();
            }
          
         
            ViewBag.SikayetSebebiID = new SelectList(db.Qdms_SikayetSebebi, "SikayetSebebiID", "SikayetSebebi" );
            ViewBag.SikayetGrubuID = new SelectList(db.Qdms_SikayetGrubu, "SikayetGrubuID", "SikayetGrubu");
            return View(qdms_MusteriTakipFormu);
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditSikayet(Qdms_MusteriSikayet qdms_MusteriTakipFormu,int? id)
        {
            Qdms_MusteriSikayet fisDetays = db.Qdms_MusteriSikayet.FirstOrDefault(x => x.ID == id);
            if (ModelState.IsValid)
            {
                fisDetays.AksiyonGerceklestirmeTermini = qdms_MusteriTakipFormu.AksiyonGerceklestirmeTermini;
                //fisDetays.NumuneTipi = qdms_MusteriTakipFormu.NumuneTipi;
                fisDetays.AksiyonTermini = qdms_MusteriTakipFormu.AksiyonTermini;
                fisDetays.Bobin = qdms_MusteriTakipFormu.Bobin;
                fisDetays.BukumSeciliMi = qdms_MusteriTakipFormu.BukumSeciliMi;
                fisDetays.BukumYorum = qdms_MusteriTakipFormu.BukumYorum;
                fisDetays.CevapTarihi = qdms_MusteriTakipFormu.CevapTarihi;
                fisDetays.Cile = qdms_MusteriTakipFormu.Cile;
                fisDetays.Diger = qdms_MusteriTakipFormu.Diger;
                fisDetays.Durum = qdms_MusteriTakipFormu.Durum;
                fisDetays.EESeciliMi = qdms_MusteriTakipFormu.EESeciliMi;
                fisDetays.EEYorum = qdms_MusteriTakipFormu.EEYorum;
                fisDetays.FirmaYetkilisi = qdms_MusteriTakipFormu.FirmaYetkilisi;
                fisDetays.GecikmeNedeni = qdms_MusteriTakipFormu.GecikmeNedeni;
                fisDetays.IadeBedelBirimi = qdms_MusteriTakipFormu.IadeBedelBirimi;
                fisDetays.IadeBedeli = qdms_MusteriTakipFormu.IadeBedeli;
                fisDetays.IadeMiktari = qdms_MusteriTakipFormu.IadeMiktari;
                fisDetays.IcDis = qdms_MusteriTakipFormu.IcDis;
                fisDetays.IplikKullanimYeri = qdms_MusteriTakipFormu.IplikKullanimYeri;
                fisDetays.IskontoBedeli = qdms_MusteriTakipFormu.IskontoBedeli;
                fisDetays.IskontoBedeliBirimi = qdms_MusteriTakipFormu.IskontoBedeliBirimi;
                fisDetays.KabulEdilenSikayetReklamasyonMiktari = qdms_MusteriTakipFormu.KabulEdilenSikayetReklamasyonMiktari;
                fisDetays.Kalite = qdms_MusteriTakipFormu.Kalite;
                fisDetays.Kalite2 = qdms_MusteriTakipFormu.Kalite2;
                fisDetays.KapatilmaSuresi = qdms_MusteriTakipFormu.KapatilmaSuresi;
                fisDetays.KokNeden = qdms_MusteriTakipFormu.KokNeden;
                fisDetays.Kumas = qdms_MusteriTakipFormu.Kumas;
                fisDetays.LabOzet = qdms_MusteriTakipFormu.LabOzet;
                fisDetays.LabSeciliMi = qdms_MusteriTakipFormu.LabSeciliMi;
                fisDetays.LabYorum = qdms_MusteriTakipFormu.LabYorum;
                fisDetays.MBSeciliMi = qdms_MusteriTakipFormu.MBSeciliMi;
                fisDetays.MBYorum = qdms_MusteriTakipFormu.MBYorum;
                fisDetays.MutabakatFormu = qdms_MusteriTakipFormu.MutabakatFormu;
                fisDetays.NumuneGelisTarihi = qdms_MusteriTakipFormu.NumuneGelisTarihi;
                fisDetays.NumuneTipi = qdms_MusteriTakipFormu.NumuneTipi;
                fisDetays.OnaylandiMi = qdms_MusteriTakipFormu.OnaylandiMi;
                fisDetays.Onaylayan = qdms_MusteriTakipFormu.Onaylayan;
                fisDetays.PaketlemeSeciliMi = qdms_MusteriTakipFormu.PaketlemeSeciliMi;
                fisDetays.PaketlemeYorum = qdms_MusteriTakipFormu.PaketlemeYorum;
                fisDetays.ProsesSeciliMi = qdms_MusteriTakipFormu.ProsesSeciliMi;
                fisDetays.ProsesYorum = qdms_MusteriTakipFormu.ProsesYorum;


                fisDetays.SatinalmaSeciliMi = qdms_MusteriTakipFormu.SatinalmaSeciliMi;
                fisDetays.SatinalmaYorum = qdms_MusteriTakipFormu.SatinalmaYorum;
                fisDetays.SatisMusteriTemsilcisiAd = qdms_MusteriTakipFormu.SatisMusteriTemsilcisiAd;
                fisDetays.SatisMusteriTemsilcisiTarih = qdms_MusteriTakipFormu.SatisMusteriTemsilcisiTarih;
                fisDetays.SevkiyatSeciliMi = qdms_MusteriTakipFormu.SevkiyatSeciliMi;
                fisDetays.SevkiyatYorum = qdms_MusteriTakipFormu.SevkiyatYorum;
                fisDetays.SevkMiktari1 = qdms_MusteriTakipFormu.SevkMiktari1;
                fisDetays.SevkMiktari2 = qdms_MusteriTakipFormu.SevkMiktari2;
                fisDetays.SevkMiktari3 = qdms_MusteriTakipFormu.SevkMiktari3;
                fisDetays.SevkTarihi1 = qdms_MusteriTakipFormu.SevkTarihi1;
                fisDetays.SevkTarihi2 = qdms_MusteriTakipFormu.SevkTarihi2;
                fisDetays.SevkTarihi3 = qdms_MusteriTakipFormu.SevkTarihi3;
                fisDetays.SikayetIletisim = qdms_MusteriTakipFormu.SikayetIletisim;
                fisDetays.SikayetReklamasyonDuyuruTarihi = qdms_MusteriTakipFormu.SikayetReklamasyonDuyuruTarihi;
                fisDetays.SikayetReklamasyonGelisTarihi = qdms_MusteriTakipFormu.SikayetReklamasyonGelisTarihi;
                fisDetays.SikayetSonucu = qdms_MusteriTakipFormu.SikayetSonucu;
                fisDetays.SonucOlarakYapilacakCalisma = qdms_MusteriTakipFormu.SonucOlarakYapilacakCalisma;
                fisDetays.Sorumlu = qdms_MusteriTakipFormu.Sorumlu;
                fisDetays.Status = qdms_MusteriTakipFormu.Status;
                fisDetays.TeknikZiyareti = qdms_MusteriTakipFormu.TeknikZiyareti;
                fisDetays.SikayetKabulRed = qdms_MusteriTakipFormu.SikayetKabulRed;
                fisDetays.TekstureSeciliMi = qdms_MusteriTakipFormu.TekstureSeciliMi;
                fisDetays.TekstureYorum = qdms_MusteriTakipFormu.TekstureYorum;
                fisDetays.TTSeciliMi = qdms_MusteriTakipFormu.TTSeciliMi;
                fisDetays.TTYorum = qdms_MusteriTakipFormu.TTYorum;
                fisDetays.UretimSeciliMi = qdms_MusteriTakipFormu.UretimSeciliMi;
                fisDetays.UretimYorum = qdms_MusteriTakipFormu.UretimYorum;
                fisDetays.YapilanAnalizler = qdms_MusteriTakipFormu.YapilanAnalizler; 
                fisDetays.SikayetSebebiID = qdms_MusteriTakipFormu.SikayetSebebiID; 
                fisDetays.SikayetGrubuID = qdms_MusteriTakipFormu.SikayetGrubuID;
                fisDetays.Hazirlayan = qdms_MusteriTakipFormu.Hazirlayan;

                
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.SikayetSebebiID = new SelectList(db.Qdms_SikayetSebebi, "SikayetSebebiID", "SikayetSebebi", qdms_MusteriTakipFormu.SikayetSebebiID);
            //ViewBag.SikayetGrubuID = new SelectList(db.Qdms_SikayetGrubu, "SikayetGrubuID", "SikayetGrubu", qdms_MusteriTakipFormu.SikayetGrubuID);
            return View(qdms_MusteriTakipFormu);
        }
        [AllowAnonymous]
       
        public FileResult ExcelMusteriTakipFormu(Qdms_MusteriSikayet gelenModel)
        {

            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{

                    new DataColumn("SIRA NO"),

                            new DataColumn("İÇ/DIŞ"),
                                 new DataColumn("ŞİKAYET/REKLAMASYON GELİŞ TARİHİ"),
                                  new DataColumn("ŞİKAYET/REKLAMASYON DUYURU TARİHİ"),
                                  new DataColumn("ŞİKAYET/REKLAMASYON BİLDİREN ÜLKE"),
                                  new DataColumn("MÜŞTERİ"),

                                        new DataColumn("PARTİ NO"),
                                           new DataColumn("DTEX/FLAMAN"),


                    new DataColumn("SEVK MİKTARI(KG)"),
                        new DataColumn("SEVK TARİHİ"),
                        new DataColumn("KABUL EDİLEN ŞİKAYET/REKLAMASYON MİKTARI (KG)"),
                            new DataColumn("KALİTE"),
                             new DataColumn("ŞİKAYET SEBEBİ"),
                                new DataColumn("ŞİKAYET GRUBU"),
                                  new DataColumn("KAPATILMA SÜRESİ"),
                                    new DataColumn("GECİKME NEDENİ"),
                                       new DataColumn("KÖK NEDEN"),
                                  new DataColumn("NUMUNE TİPİ"),
                                      new DataColumn("SORUMLU"),
                                            new DataColumn("SONUÇ"),
                                             new DataColumn("AKSİYON GERÇEKLEŞME TARİHİ"),

                                        new DataColumn("AKSİYON TERMİNİ"),
                                        new DataColumn("TEKNİK ZİYARET"),
                                          new DataColumn("ŞİKAYET KABUL/RED"),
                                        new DataColumn("İADE MİKTARI"),










            });
            var liste = from list in db.Qdms_MusteriSikayet.OrderByDescending(a => a.SikayetReklamasyonGelisTarihi) select list;



            foreach (var list in liste)
            {

             
                dt.Rows.Add(list.ID, list.IcDis, 
                    list.SikayetReklamasyonGelisTarihi.GetValueOrDefault().ToString("dd.MM.yyyy"),
                    list.SikayetReklamasyonDuyuruTarihi.GetValueOrDefault().ToString("dd.MM.yyyy"), 
                    list.Qdms_Cari.Ulke,
                  list.Qdms_Cari.CariAdi,
                  list.Qdms_PartiNo.LotNo, 
                  list.Qdms_PartiNo.StokAdi, 
                  list.SevkMiktari1.Value.ToString("N"),
                  list.SevkTarihi1.GetValueOrDefault().ToString("dd.MM.yyyy"),
                  list.KabulEdilenSikayetReklamasyonMiktari
                  , list.Kalite,
                  list.Qdms_SikayetSebebi.SikayetSebebi,
                  list.Qdms_SikayetSebebi.Qdms_SikayetGrubu.SikayetGrubu,
                  list.KapatilmaSuresi,
                  list.GecikmeNedeni,


                  list.KokNeden,
                  list.NumuneTipi,
                  list.Sorumlu,
                  list.SonucOlarakYapilacakCalisma,
                  list.AksiyonGerceklestirmeTermini.GetValueOrDefault().ToString("dd.MM.yyyy"),
                  list.AksiyonTermini.GetValueOrDefault().ToString("dd.MM.yyyy"),

                  list.TeknikZiyareti.GetValueOrDefault() ? "EVET":"HAYIR",
                  list.SikayetKabulRed.GetValueOrDefault() ? "EVET":"HAYIR",
                  list.IadeMiktari.GetValueOrDefault().ToString("N")





                  );
            }

            dt.Rows.Add("F-142_03");


            using (XLWorkbook wb = new XLWorkbook())
            {

                //var imagePath = @"~/Images/Picture2.png";   
                //var imagePath = @"C:\Users\MERVE\source\repos\POLYTEKS_QDMS_MVC\Views\MusteriTakipFormu\polylogo.jpg";

                var ws = wb.Worksheets.Add(dt, "MusteriSikayetTakipFormu");
                //                var image = ws.AddPicture(imagePath)
                //.MoveTo(ws.Cell("A1"))
                //.Scale(0.5);
                ws.Row(1).InsertRowsAbove(1);
                ws.Cell("A1").Value = "MÜŞTERİ ŞİKAYET-REKLAMASYON VERİ TAKİP FORMU";
                ws.Cell("A1").Style.Font.Bold = true;
                ws.Cell("A1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;

                // ws.Columns().AdjustToContents(); // Sütunların içerigine göre sütünları genişletir
                ws.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "F_142_03 MÜŞTERİ ŞİKAYET-REKLAMASYON VERİ TAKİP FORMU" + ".xlsx");
                }


            }

        }
    }
}
