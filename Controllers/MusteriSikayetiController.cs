using ClosedXML.Excel;
using PolyteksKaliteKontrolTakip.Models;
using Rotativa;
using Rotativa.Options;
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
    public class MusteriSikayetiController : Controller
    {
        POLY_QDMSEntities db = new POLY_QDMSEntities();

        [AllowAnonymous]
        public ActionResult Formlar()
        {
            return View();
        }

        public ActionResult MusteriRapor(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_MusteriSikayet qdms_MusteriSikayet = db.Qdms_MusteriSikayet.Find(id);
            if (qdms_MusteriSikayet == null)
            {
                return HttpNotFound();
            }
            return View(qdms_MusteriSikayet);
        }
        public ActionResult MusteriFormati(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_MusteriSikayet qdms_MusteriSikayet = db.Qdms_MusteriSikayet.Find(id);
            if (qdms_MusteriSikayet == null)
            {
                return HttpNotFound();
            }
            return View(qdms_MusteriSikayet);
        }
        [AllowAnonymous]
        public ActionResult Analizler()
        {
            return View(db.Qdms_MusteriSikayet.ToList());
        }

        [AllowAnonymous]
        public ActionResult Home()
        {
            return View();
        }
  




        [AllowAnonymous]
        public ActionResult ExportPDF(int? id)
        {
            return new ActionAsPdf("MusteriFormati", new { id = id })
            {
                //FileName=Server.MapPath("~/Temalar")
                FileName = DateTime.Now.ToString("dd MMMM yyyy") + "-" + "MusteriSikayeti.pdf",
                PageSize = Size.A4,

                // PageMargins = { Left = 1, Right = 1 },
                //CustomSwitches = "--page-offset 0 --footer-center [page] --footer-font-size 8"
            };
        }

        [AllowAnonymous]
        public ActionResult TamamlananSikayet()
        {
            var asd = db.Qdms_MusteriSikayet.OrderByDescending(a => a.ID).Where(a => a.Durum.Trim() == "Müşteri Şikayeti Sonuçlandı").ToList();
            return View(asd);
        }
        public ActionResult DevamEdenSikayet()
        {
            return PartialView();
        }

        #region SATIŞ
        [AllowAnonymous]
        public ActionResult _DevamEdenSikayetSatis(Qdms_MusteriSikayet vm)
        {



            var asd = db.Qdms_MusteriSikayet.OrderByDescending(a => a.ID).Where(a => a.Durum.Trim() == "Sonuç Bekleniyor" || a.Durum.Trim() == "Bölümlerin Yorumu Bekleniyor" || a.Durum.Trim() == "Şikayet Birimine Gönderildi").ToList();
            return PartialView(asd);

        }
        [Authorize(Roles = "A,S")]
        public ActionResult _DevamEdenSikayetSatisDuzenle(int? id)
        {
            //var h = db.Qdms_MusteriSikayet.Include(x => x.Resims).SingleOrDefault();
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_MusteriSikayet qdms_MusteriSikayet = db.Qdms_MusteriSikayet.Find(id);
            if (qdms_MusteriSikayet == null)
            {
                return HttpNotFound();
            }
            List<Qdms_SikayetGrubu> CountryList = db.Qdms_SikayetGrubu.ToList();
            ViewBag.CountryList = new SelectList(CountryList, "SikayetGrubuID", "SikayetGrubu");
            ViewBag.Cari = new SelectList(db.Qdms_Cari, "ID", "CariAdi", qdms_MusteriSikayet.Cari);
            ViewBag.PartiNoSecme = new SelectList(db.Qdms_PartiNo, "ID", "LotNo", qdms_MusteriSikayet.PartiNoSecme);
            return View(qdms_MusteriSikayet);
        }
        public string uploadimage(HttpPostedFileBase file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.ContentLength > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Images"), random + Path.GetFileName(file.FileName));
                        file.SaveAs(path);
                        path = "~/Images/" + random + Path.GetFileName(file.FileName);
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }
            return path;
        }
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult _DevamEdenSikayetSatisDuzenle(Qdms_MusteriSikayet qdms_MusteriSikayet, int? id, HttpPostedFileBase imgfile)
        {
            ////var kimlik = db.Qdms_MusteriSikayet.Find(id);
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (imgfile != null && imgfile.ContentLength > 0)
            {
                string extension = Path.GetExtension(imgfile.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        path = Path.Combine(Server.MapPath("~/Images"), random + Path.GetFileName(imgfile.FileName));
                        imgfile.SaveAs(path);
                        path = "~/Images/" + random + Path.GetFileName(imgfile.FileName);
                    }
                    catch (Exception ex)
                    {
                        path = "-1";
                    }
                }
                else
                {
                    Response.Write("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }
            else
            {
                Response.Write("<script>alert('Please select a file'); </script>");
                path = "-1";
            }



            if (ModelState.IsValid)
            {
              
                qdms_MusteriSikayet.Durum = "Şikayet Birimine Gönderildi";

                db.Entry(qdms_MusteriSikayet).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("DevamEdenSikayet");
            }
            List<Qdms_SikayetGrubu> CountryList = db.Qdms_SikayetGrubu.ToList();
            ViewBag.CountryList = new SelectList(CountryList, "SikayetGrubuID", "SikayetGrubu");
            ViewBag.Cari = new SelectList(db.Qdms_Cari, "ID", "CariAdi", qdms_MusteriSikayet.Cari);
            ViewBag.PartiNoSecme = new SelectList(db.Qdms_PartiNo, "ID", "LotNo", qdms_MusteriSikayet.PartiNoSecme);
            return View(qdms_MusteriSikayet);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Actionlar(int code)
        {
            var query = from c in db.Qdms_Cari
                        where c.ID == code
                        select c;

            return Json(query, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Actionlarr(int code)
        {
            var query = from c in db.Qdms_PartiNo
                        where c.ID == code
                        select c;

            return Json(query, JsonRequestBehavior.AllowGet);
        }


        [Authorize(Roles = "A,S")]
        public ActionResult Satis()
        {
            List<Qdms_SikayetGrubu> CountryList = db.Qdms_SikayetGrubu.ToList();
            ViewBag.CountryList = new SelectList(CountryList, "SikayetGrubuID", "SikayetGrubu");
            //ViewData["chain_name"] = new SelectList(db.Qdms_Cari, "CariNo", "CariAdi");
            ViewBag.Cari = new SelectList(db.Qdms_Cari.OrderBy(a => a.CariAdi).AsNoTracking().ToList(), "ID", "CariAdi");
            ViewBag.PartiNoSecme = new SelectList(db.Qdms_PartiNo.OrderBy(a => a.LotNo).AsNoTracking().ToList(), "ID", "LotNo");
            //ViewBag.SikayetSebebi = new SelectList(db.Qdms_MusteriSikayetSebep.AsNoTracking().ToList(), "ID", "SikayetSebebi");
            //ViewBag.SikayetGrubu = new SelectList(db.Qdms_MusteriSikayetSebep.AsNoTracking().ToList(), "ID", "SikayetGrubu");

            return PartialView();
        }
        [Authorize(Roles = "A,S")]

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Satis(Qdms_MusteriSikayet qdms_MusteriSikayet)
        {

            if (ModelState.IsValid)
            {

                qdms_MusteriSikayet.Durum = "Şikayet Birimine Gönderildi";
                qdms_MusteriSikayet.Status = 0;
                db.Qdms_MusteriSikayet.Add(qdms_MusteriSikayet);
                db.SaveChanges();



                ViewBag.Cari = new SelectList(db.Qdms_Cari, "ID", "CariNo", qdms_MusteriSikayet.Cari);
                ViewBag.MusteriSikayetId = new SelectList(db.Qdms_MusteriSikayetAna, "ID", "MusteriTemsilcisi", qdms_MusteriSikayet.MusteriSikayetId);
                ViewBag.PartiNoSecme = new SelectList(db.Qdms_PartiNo, "ID", "LotNo", qdms_MusteriSikayet.PartiNoSecme);
                ViewBag.SikayetSebebiID = new SelectList(db.Qdms_SikayetSebebi, "SikayetSebebiID", "SikayetSebebi", qdms_MusteriSikayet.SikayetSebebiID);
                return View("DevamEdenSikayet");
            }





            return View(qdms_MusteriSikayet);
        }
        [AllowAnonymous]
        public ActionResult SikayetResimEkleme(int? id)

        {

            Qdms_MusteriSikayet qdms_MusteriSikayet = db.Qdms_MusteriSikayet.Find(id);
            ViewBag.SikayetId = new SelectList(db.Qdms_MusteriSikayet, "ID", "Cari");
            return View();
        }
        [HttpPost]
        public ActionResult SikayetResimEkleme(int? id, Qdms_MusteriSikayetResim cokluResims, List<HttpPostedFileBase> ImagePath)
        {

            Qdms_MusteriSikayet fisDetays = db.Qdms_MusteriSikayet.FirstOrDefault(x => x.ID == id);
            if (ModelState.IsValid)
            {
                foreach (var item in ImagePath)

                    if (item.ContentLength > 0)
                    {
                        var image = Path.GetFileName(item.FileName);
                        var path = Path.Combine(Server.MapPath("~/Image"), image);
                        item.SaveAs(path);
                        cokluResims.SikayetId = fisDetays.ID;
                        cokluResims.ImagePath = "/Image/" + image;
                        db.Qdms_MusteriSikayetResim.Add(cokluResims);
                        db.SaveChanges();
                    }
                return RedirectToAction("DevamEdenSikayet");
            }

            ViewBag.SikayetId = new SelectList(db.Qdms_MusteriSikayet, "ID", "Cari", cokluResims.SikayetId);
            return View(cokluResims);

        }

        public JsonResult GetStateList(int SikayetGrubuID)
        {
            db.Configuration.ProxyCreationEnabled = false;
            List<Qdms_SikayetSebebi> StateList = db.Qdms_SikayetSebebi.Where(x => x.SikayetGrubuID == SikayetGrubuID).OrderBy(x => x.SikayetSebebi).ToList();
            return Json(StateList, JsonRequestBehavior.AllowGet);

        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Action(int code)
        {
            var query = from c in db.Qdms_Cari
                        where c.ID == code
                        select c;

            return Json(query, JsonRequestBehavior.AllowGet);
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Actions(int code)
        {
            var query = from c in db.Qdms_PartiNo
                        where c.ID == code
                        select c;

            return Json(query, JsonRequestBehavior.AllowGet);
        }
        #region SİLME
        [Authorize(Roles = "A,S")]
        public ActionResult _DevamEdenSikayetSil(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_MusteriSikayet qdms_MusteriSikayet = db.Qdms_MusteriSikayet.Find(id);
            if (qdms_MusteriSikayet == null)
            {
                return HttpNotFound();
            }
            return View(qdms_MusteriSikayet);
        }

        [HttpPost, ActionName("_DevamEdenSikayetSil")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Qdms_MusteriSikayet qdms_MusteriSikayet = db.Qdms_MusteriSikayet.Find(id);
            db.Qdms_MusteriSikayet.Remove(qdms_MusteriSikayet);
            db.SaveChanges();
            return RedirectToAction("DevamEdenSikayet");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        #endregion
        [AllowAnonymous]
        public ActionResult DevamEdenSikayetSatisDetayi(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_MusteriSikayet qdms_MusteriSikayet = db.Qdms_MusteriSikayet.Find(id);
            if (qdms_MusteriSikayet == null)
            {
                return HttpNotFound();
            }
            return View(qdms_MusteriSikayet);




        }
        #region SATIŞEXCEL
        [AllowAnonymous]
        public ActionResult SatisExcel(int id)
        {
            var model = db.Qdms_MusteriSikayet.Find(id);

            return SatisExcels(model);

        }
        [AllowAnonymous]
        [HttpPost]
        public FileResult SatisExcels(Qdms_MusteriSikayet gelenModels)
        {

            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                 new DataColumn(""),
                     new DataColumn("FİRMA"),
                    new DataColumn("YETKİLİ"),
                        new DataColumn("ADRES"),
                          new DataColumn("TEL"),
                            new DataColumn("E-POSTA"),
                          new DataColumn("ŞİKAYET/REKLAMASYON NO"),
                            new DataColumn("ŞİKAYET/REKLAMASYON GELİŞ TARİHİ"),
                                 new DataColumn("ŞİKAYET/REKLAMASYON KONUSU"),
                                   new DataColumn("ŞİKAYETİN YAPILDIĞI İLETİŞİM ARACI"),
                                     new DataColumn("\n\n"),
                                     new DataColumn("İPLİĞİN KULLANILDIĞI YERLER"),
                                       new DataColumn("\n\n\n"),
                                        new DataColumn("Şikayet ile ilgili olarak POLYTEKS'e gönderilen:"),


                                                              new DataColumn("BOBİN(ADET)"),


                            new DataColumn("KUMAŞ( cm x cm)"),
                                  new DataColumn("ÇİLE( cm x cm)"),


                                        new DataColumn("DİĞER"),
                                          new DataColumn("\n"),

                                           new DataColumn("Müşteri Şikayet isteğine konu olan iplik sevkiyatına ait:"),
                                                 new DataColumn("PARTİ NO"),
                new DataColumn("SEVK TARİHİ"),
               new DataColumn("SEVK MİKTARI(KG)"),
                    new DataColumn("DTEX/FLAMAN İPLİK ÖZELLİĞİ"),
                      new DataColumn("\n\n\n\n"),
                    new DataColumn("SATIŞ/MÜŞTERİ TEMSİLCİSİ:"),
                       new DataColumn("ADI SOYADI"),
                new DataColumn("TARİH"),





            });
            var liste = from list in db.Qdms_MusteriSikayet.Where(a => a.ID == gelenModels.ID) select list;

            foreach (var list in liste)
            {

                dt.Rows.Add(" ",
                    list.Qdms_Cari.CariAdi,
                    list.Qdms_Cari.Yetkili,
                    list.Qdms_Cari.Adres,
                    list.Qdms_Cari.Tel1.ToString(),
                    list.Qdms_Cari.ElektronikPosta,
                    list.ID.ToString()
                 , list.SikayetReklamasyonGelisTarihi.GetValueOrDefault().ToString("dd.MM.yyyy"),
                    list.Qdms_SikayetSebebi.Qdms_SikayetGrubu.SikayetGrubu,
                    list.SikayetIletisim,
                    " ",
                    list.IplikKullanimYeri,
                    " ",
                    " ",
                    list.Bobin.GetValueOrDefault(),
                    list.Kumas.ToString(),

                 list.Cile,
                    list.Diger,
                    " ",
                    " ",
                    list.Qdms_PartiNo.LotNo.ToString(),
                    list.SevkTarihi1.GetValueOrDefault().ToString("dd.MM.yyyy"),
                    list.SevkMiktari1.GetValueOrDefault(),
                    list.Qdms_PartiNo.StokAdi,
                    " ", " ",
                    list.SatisMusteriTemsilcisiAd,
                    list.SatisMusteriTemsilcisiTarih.GetValueOrDefault().ToString("dd.MM.yyyy"));
            }
            //dt.Rows.Add("Sayın Müşterimiz ;" + "\n" + " Bu form Müşteri Memnuniyeti çerçevesinde bilgilendirme amaçlı gönderilmiştir. Hatalı bilgi olması halinde lütfen Müşteri Temsilcimiz ile irtibata geçiniz. Bu bilgiler Şikayet Birimi Bölümüne gönderilecek olup, tüm gerekli bilgi ve numuneler tamamlandıktan en geç 5 iş günü içerisinde şikayetin sonuçlandırılmasına özen gösterilecektir.Sürecin devam etmesi halinde şikayet hakkında bilgilendirme yapılacaktır.");
            //dt.Rows.Add("F-142.01/00");


            using (XLWorkbook wb = new XLWorkbook())
            {

                //var imagePath = @"~/Images/Picture2.png";   
                var imagePath = @"C:\Users\MERVE\source\repos\POLYTEKS_QDMS_MVC\Pictures\poly_logo.png";

                var ws = wb.Worksheets.Add(dt, "MusteriSikayetFormu");

                var image = ws.AddPicture(imagePath).MoveTo(ws.Cell("A1")).Scale(0.1);


                ws.Row(1).InsertRowsAbove(1);


                ws.Cell("B1").Value = "MÜŞTERİ ŞİKAYET/REKLAMASYON BİLGİ FORMU";
                ws.Cell("B1").Style.Font.Bold = true;
                ws.Cell("B1").Style.Font.FontSize = 18;
                ws.Cell("B1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                ws.Cell("A39").Value = "Sayın Müşterimiz; \n Bu form Müşteri Memnuniyeti çerçevesinde bilgilendirme amaçlı gönderilmiştir. " + Environment.NewLine + "Hatalı bilgi olması halinde lütfen Müşteri Temsilcimiz ile irtibata geçiniz. \n Bu bilgiler Şikayet Birimi Bölümüne gönderilecek olup, tüm gerekli bilgi ve numuneler tamamlandıktan en geç 5 iş günü içerisinde şikayetin sonuçlandırılmasına özen gösterilecektir.\n Sürecin devam etmesi halinde şikayet hakkında bilgilendirme yapılacaktır.";
                ws.Cell(1, 1).Style.Font.FontSize = 30;
                ws.Cell("A39").Style.Alignment.Vertical = XLAlignmentVerticalValues.Distributed;


                ws.Cell("A39").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                ws.Cell("A40").Value = "F-142.01/00";
                ws.Cell("A40").Style.Font.Bold = true;
                ws.Cell("A40").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                ws.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.ShowGridLines = false;
                ws.Columns().AdjustToContents();
                //ws.Rows().AdjustToContents();
                ws.Tables.FirstOrDefault().ShowAutoFilter = false;

                ws.Tables.FirstOrDefault().AutoFilter.IsEnabled = false;
                ws.Style.Fill.BackgroundColor = XLColor.White;
        
                var rngTable = ws.Range("A2:AH31");

                rngTable.Transpose(XLTransposeOptions.MoveCells);
                rngTable.Style.Font.FontColor = XLColor.Black;
                rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.None;



                rngTable.Range("A2:B30").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                rngTable.Range("A2:B30").Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
                rngTable.Range("A2:B30").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                rngTable.Range("A2:B30").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                rngTable.Range("A2:B30").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                rngTable.Range("A2:B30").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                rngTable.Range("A2:A30").Style.Font.Bold = true;


                rngTable.Range("A15:B20").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                rngTable.Range("A13:B13").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                rngTable.Range("A22:B26").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                rngTable.Range("A3:B11").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                rngTable.Range("A28:B30").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
           


                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "F_142_01 MÜŞTERİ ŞİKAYET- REKLAMASYON BİLGİ FORMU" + ".xlsx");
                }


            }





        }
        #endregion
        #endregion

        #region ŞİKAYET BİRİMİ
        [AllowAnonymous]
        public ActionResult _DevamEdenSikayetLab(Qdms_MusteriSikayet vm)
        {

            var asd = db.Qdms_MusteriSikayet.OrderByDescending(a => a.ID).Where(a => a.Durum.Trim() == "Şikayet Birimine Gönderildi").ToList();

            return PartialView(asd);

        }



        [AllowAnonymous]
        public ActionResult _DevamEdenSikayetLabDuzenle(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_MusteriSikayet qdms_MusteriSikayet = db.Qdms_MusteriSikayet.Find(id);
            if (qdms_MusteriSikayet == null)
            {
                return HttpNotFound();
            }

            ViewBag.Cari = new SelectList(db.Qdms_Cari, "ID", "CariNo", qdms_MusteriSikayet.Cari);
            ViewBag.PartiNoSecme = new SelectList(db.Qdms_PartiNo, "ID", "LotNo", qdms_MusteriSikayet.PartiNoSecme);
            return PartialView(qdms_MusteriSikayet);
        }


        [AllowAnonymous]
        [HttpPost]
        public ActionResult _DevamEdenSikayetLabDuzenle(Qdms_MusteriSikayet qdms_MusteriSikayet, int? id)
        {


            if (ModelState.IsValid)
            {
                Qdms_MusteriSikayet fisDetays = db.Qdms_MusteriSikayet.FirstOrDefault(x => x.ID == id);



                fisDetays.LabOzet = qdms_MusteriSikayet.LabOzet;
                fisDetays.IcDis = qdms_MusteriSikayet.IcDis;
                fisDetays.NumuneGelisTarihi = qdms_MusteriSikayet.NumuneGelisTarihi;

                fisDetays.UretimSeciliMi = qdms_MusteriSikayet.UretimSeciliMi;
                fisDetays.BukumSeciliMi = qdms_MusteriSikayet.BukumSeciliMi;
                fisDetays.TekstureSeciliMi = qdms_MusteriSikayet.TekstureSeciliMi;
                fisDetays.TTSeciliMi = qdms_MusteriSikayet.TTSeciliMi;
                fisDetays.SevkiyatSeciliMi = qdms_MusteriSikayet.SevkiyatSeciliMi;
                fisDetays.SatinalmaSeciliMi = qdms_MusteriSikayet.SatinalmaSeciliMi;
                fisDetays.MBSeciliMi = qdms_MusteriSikayet.MBSeciliMi;
                fisDetays.EESeciliMi = qdms_MusteriSikayet.EESeciliMi;
                fisDetays.ProsesSeciliMi = qdms_MusteriSikayet.ProsesSeciliMi;
                fisDetays.LabSeciliMi = qdms_MusteriSikayet.LabSeciliMi;
                fisDetays.PaketlemeSeciliMi = qdms_MusteriSikayet.PaketlemeSeciliMi;
                fisDetays.Durum = "Bölümlerin Yorumu Bekleniyor";
                fisDetays.Status = 1;


                db.SaveChanges();
                return RedirectToAction("DevamEdenSikayet");
            }



            ViewBag.BolumID = db.Qdms_Bolumler.Select(c => c.Bolumler).ToList();

            ViewBag.Cari = new SelectList(db.Qdms_Cari, "ID", "CariAdi", qdms_MusteriSikayet.Cari);
            ViewBag.PartiNoSecme = new SelectList(db.Qdms_PartiNo, "ID", "LotNo", qdms_MusteriSikayet.PartiNoSecme);
            ViewBag.SikayetSebebiID = new SelectList(db.Qdms_SikayetSebebi, "SikayetSebebiID", "SikayetSebebi", qdms_MusteriSikayet.SikayetSebebiID);
            return View(qdms_MusteriSikayet);

        }
        public Qdms_Kullanici Kullanici;
        #endregion

        #region ONAYLAMA
        [Authorize(Roles = "A")]
        [HttpPost]
        public ActionResult DevamEdenSikayetSonucOnay(int? id)
        {
            Qdms_MusteriSikayet qdms_MusteriSikayet = db.Qdms_MusteriSikayet.FirstOrDefault(x => x.ID == id);
            qdms_MusteriSikayet.OnaylandiMi = true;
            qdms_MusteriSikayet.Onaylayan = User.Identity.Name;
            qdms_MusteriSikayet.Durum = "Müşteri Şikayeti Sonuçlandı";
            qdms_MusteriSikayet.Status = 3;
            db.SaveChanges();
            return RedirectToAction("DevamEdenSikayet");
        }
        #endregion

        #region YORUMLAR

        [AllowAnonymous]
        public ActionResult _DevamEdenSikayetYorum(Qdms_MusteriSikayet vm)
        {



            var asd = db.Qdms_MusteriSikayet.OrderByDescending(a => a.ID).Where(a => a.Durum.Trim() == "Bölümlerin Yorumu Bekleniyor").ToList();
            return PartialView(asd);

        }
        [AllowAnonymous]
        public ActionResult _DevamEdenSikayetYorumDuzenle(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_MusteriSikayet qdms_MusteriSikayet = db.Qdms_MusteriSikayet.Find(id);
            if (qdms_MusteriSikayet == null)
            {
                return HttpNotFound();
            }

            ViewBag.Cari = new SelectList(db.Qdms_Cari, "ID", "CariNo", qdms_MusteriSikayet.Cari);
            ViewBag.PartiNoSecme = new SelectList(db.Qdms_PartiNo, "ID", "LotNo", qdms_MusteriSikayet.PartiNoSecme);
            return PartialView(qdms_MusteriSikayet);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult _DevamEdenSikayetYorumDuzenle(Qdms_MusteriSikayet qdms_MusteriSikayet, int? id)
        {
            int i = 0;
            Qdms_MusteriSikayet fisDetays = db.Qdms_MusteriSikayet.FirstOrDefault(x => x.ID == id);
            List<Qdms_MusteriSikayet> filteredList = new List<Qdms_MusteriSikayet>();
            var posts = from p in db.Qdms_MusteriSikayet
                        select p;


            if (ModelState.IsValid)
            {
                if (fisDetays.BukumSeciliMi == true ||
                   fisDetays.TekstureSeciliMi == true ||
                  fisDetays.SatinalmaSeciliMi == true ||
                   fisDetays.UretimSeciliMi == true ||
                   fisDetays.TTSeciliMi == true ||
                   fisDetays.ProsesSeciliMi == true ||
                      fisDetays.PaketlemeSeciliMi == true ||
                   fisDetays.MBSeciliMi == true ||
                  fisDetays.EESeciliMi == true ||
                  fisDetays.LabSeciliMi == true ||
                   fisDetays.SevkiyatSeciliMi == true)
                {


                    if (fisDetays.BukumSeciliMi == true && fisDetays.BukumYorum == null)
                    {
                        i++;

                    }
                    if (fisDetays.TekstureSeciliMi == true && fisDetays.TekstureYorum == null)
                    {
                        i++;

                    }
                    if (fisDetays.SatinalmaSeciliMi == true && fisDetays.SatinalmaYorum == null)
                    {
                        i++;

                    }
                    if (fisDetays.UretimSeciliMi == true && fisDetays.UretimYorum == null)
                    {
                        i++;

                    }
                    if (fisDetays.TTSeciliMi == true && fisDetays.TTYorum == null)
                    {
                        i++;

                    }
                    if (fisDetays.ProsesSeciliMi == true && fisDetays.ProsesYorum == null)
                    {
                        i++;

                    }
                    if (fisDetays.PaketlemeSeciliMi == true && fisDetays.PaketlemeYorum == null)
                    {
                        i++;

                    }
                    if (fisDetays.MBSeciliMi == true && fisDetays.MBYorum == null)
                    {
                        i++;

                    }
                    if (fisDetays.EESeciliMi == true && fisDetays.EEYorum == null)
                    {
                        i++;

                    }



                    if (fisDetays.LabSeciliMi == true && fisDetays.YapilanAnalizler == null)
                    {
                        i++;

                    }
                    if (fisDetays.SevkiyatSeciliMi == true && fisDetays.SevkiyatYorum == null)
                    {
                        i++;

                    }

                    if (i > 1)
                    {
                        if (qdms_MusteriSikayet.YapilanAnalizler != null)
                        {
                            fisDetays.YapilanAnalizler = qdms_MusteriSikayet.YapilanAnalizler.ToString();
                        }
                        fisDetays.UretimYorum = qdms_MusteriSikayet.UretimYorum;
                        fisDetays.BukumYorum = qdms_MusteriSikayet.BukumYorum;
                        fisDetays.TekstureYorum = qdms_MusteriSikayet.TekstureYorum;
                        fisDetays.TTYorum = qdms_MusteriSikayet.TTYorum;
                        fisDetays.SevkiyatYorum = qdms_MusteriSikayet.SevkiyatYorum;
                        fisDetays.SatinalmaYorum = qdms_MusteriSikayet.SatinalmaYorum;
                        fisDetays.MBYorum = qdms_MusteriSikayet.MBYorum;
                        fisDetays.EEYorum = qdms_MusteriSikayet.EEYorum;
                        fisDetays.ProsesYorum = qdms_MusteriSikayet.ProsesYorum;
                        fisDetays.LabYorum = qdms_MusteriSikayet.LabYorum;
                        fisDetays.PaketlemeYorum = qdms_MusteriSikayet.PaketlemeYorum;

                        db.SaveChanges();

                    }


                    if (i == 1)
                    {
                        if (qdms_MusteriSikayet.YapilanAnalizler != null)
                        {
                            fisDetays.YapilanAnalizler = qdms_MusteriSikayet.YapilanAnalizler.ToString();
                        }
                        fisDetays.UretimYorum = qdms_MusteriSikayet.UretimYorum;
                        fisDetays.BukumYorum = qdms_MusteriSikayet.BukumYorum;
                        fisDetays.TekstureYorum = qdms_MusteriSikayet.TekstureYorum;
                        fisDetays.TTYorum = qdms_MusteriSikayet.TTYorum;
                        fisDetays.SevkiyatYorum = qdms_MusteriSikayet.SevkiyatYorum;
                        fisDetays.SatinalmaYorum = qdms_MusteriSikayet.SatinalmaYorum;
                        fisDetays.MBYorum = qdms_MusteriSikayet.MBYorum;
                        fisDetays.EEYorum = qdms_MusteriSikayet.EEYorum;
                        fisDetays.ProsesYorum = qdms_MusteriSikayet.ProsesYorum;
                        fisDetays.LabYorum = qdms_MusteriSikayet.LabYorum;
                        fisDetays.PaketlemeYorum = qdms_MusteriSikayet.PaketlemeYorum;

                        fisDetays.Durum = "Sonuç Bekleniyor";
                        fisDetays.Status = 2;

                        db.SaveChanges();

                    }
                    if (i == 0)
                    {
                        if (qdms_MusteriSikayet.YapilanAnalizler != null)
                        {
                            fisDetays.YapilanAnalizler = qdms_MusteriSikayet.YapilanAnalizler.ToString();
                        }
                        fisDetays.UretimYorum = qdms_MusteriSikayet.UretimYorum;
                        fisDetays.BukumYorum = qdms_MusteriSikayet.BukumYorum;
                        fisDetays.TekstureYorum = qdms_MusteriSikayet.TekstureYorum;
                        fisDetays.TTYorum = qdms_MusteriSikayet.TTYorum;
                        fisDetays.SevkiyatYorum = qdms_MusteriSikayet.SevkiyatYorum;
                        fisDetays.SatinalmaYorum = qdms_MusteriSikayet.SatinalmaYorum;
                        fisDetays.MBYorum = qdms_MusteriSikayet.MBYorum;
                        fisDetays.EEYorum = qdms_MusteriSikayet.EEYorum;
                        fisDetays.ProsesYorum = qdms_MusteriSikayet.ProsesYorum;
                        fisDetays.LabYorum = qdms_MusteriSikayet.LabYorum;

                        fisDetays.PaketlemeYorum = qdms_MusteriSikayet.PaketlemeYorum;
                        fisDetays.Durum = "Sonuç Bekleniyor";
                        fisDetays.Status = 2;
                        db.SaveChanges();



                    }
                }
                //if (qdms_MusteriSikayet.BukumSeciliMi == true && qdms_MusteriSikayet.BukumYorum != null ||
                //    qdms_MusteriSikayet.LabSeciliMi == true && qdms_MusteriSikayet.LabYorum != null ||
                //    qdms_MusteriSikayet.SevkiyatSeciliMi == true && qdms_MusteriSikayet.SevkiyatYorum != null ||
                //        qdms_MusteriSikayet.EESeciliMi == true && qdms_MusteriSikayet.EEYorum != null ||
                //    qdms_MusteriSikayet.PaketlemeSeciliMi == true && qdms_MusteriSikayet.PaketlemeYorum != null ||
                //    qdms_MusteriSikayet.ProsesSeciliMi == true && qdms_MusteriSikayet.ProsesYorum != null ||
                //    qdms_MusteriSikayet.TTSeciliMi == true && qdms_MusteriSikayet.TTYorum != null ||
                //    qdms_MusteriSikayet.UretimSeciliMi == true && qdms_MusteriSikayet.UretimYorum != null ||
                //    qdms_MusteriSikayet.SatinalmaSeciliMi == true && qdms_MusteriSikayet.SatinalmaYorum != null ||
                //    qdms_MusteriSikayet.TekstureSeciliMi == true && qdms_MusteriSikayet.TekstureYorum != null ||
                //    qdms_MusteriSikayet.MBSeciliMi == true && qdms_MusteriSikayet.MBYorum != null)
                //{
                //    fisDetays.Durum = "Sonuç Bekleniyor";

                //}
                //else if (fisDetays.SevkiyatSeciliMi == true && fisDetays.SevkiyatYorum == null)
                //{
                //    fisDetays.UretimYorum = qdms_MusteriSikayet.UretimYorum;
                //    fisDetays.BukumYorum = qdms_MusteriSikayet.BukumYorum;
                //    fisDetays.TekstureYorum = qdms_MusteriSikayet.TekstureYorum;
                //    fisDetays.TTYorum = qdms_MusteriSikayet.TTYorum;
                //    fisDetays.SevkiyatYorum = qdms_MusteriSikayet.SevkiyatYorum;
                //    fisDetays.SatinalmaYorum = qdms_MusteriSikayet.SatinalmaYorum;
                //    fisDetays.MBYorum = qdms_MusteriSikayet.MBYorum;
                //    fisDetays.EEYorum = qdms_MusteriSikayet.EEYorum;
                //    fisDetays.ProsesYorum = qdms_MusteriSikayet.ProsesYorum;
                //    fisDetays.LabYorum = qdms_MusteriSikayet.LabYorum;
                //    fisDetays.PaketlemeYorum = qdms_MusteriSikayet.PaketlemeYorum;
                //}

                //    else if ((fisDetays.BukumSeciliMi == true && fisDetays.BukumYorum == null) ||
                //     fisDetays.EEYorum == null && fisDetays.EESeciliMi == true ||
                //     fisDetays.TekstureYorum == null && fisDetays.TekstureSeciliMi == true ||
                //      fisDetays.TTYorum == null && fisDetays.TTSeciliMi == true ||
                //       fisDetays.UretimYorum == null && fisDetays.UretimSeciliMi == true ||
                //        fisDetays.LabYorum == null && fisDetays.LabSeciliMi == true ||
                //         fisDetays.SevkiyatYorum == null && fisDetays.SevkiyatSeciliMi == true ||
                //          fisDetays.SatinalmaYorum == null && fisDetays.SatinalmaSeciliMi == true ||
                //           fisDetays.ProsesYorum == null && fisDetays.ProsesSeciliMi == true ||
                //           fisDetays.MBYorum == null && fisDetays.MBSeciliMi == true ||

                //        fisDetays.PaketlemeYorum == null && fisDetays.PaketlemeSeciliMi == true)
                //{
                //    fisDetays.UretimYorum = qdms_MusteriSikayet.UretimYorum;
                //    fisDetays.BukumYorum = qdms_MusteriSikayet.BukumYorum;
                //    fisDetays.TekstureYorum = qdms_MusteriSikayet.TekstureYorum;
                //    fisDetays.TTYorum = qdms_MusteriSikayet.TTYorum;
                //    fisDetays.SevkiyatYorum = qdms_MusteriSikayet.SevkiyatYorum;
                //    fisDetays.SatinalmaYorum = qdms_MusteriSikayet.SatinalmaYorum;
                //    fisDetays.MBYorum = qdms_MusteriSikayet.MBYorum;
                //    fisDetays.EEYorum = qdms_MusteriSikayet.EEYorum;
                //    fisDetays.ProsesYorum = qdms_MusteriSikayet.ProsesYorum;
                //    fisDetays.LabYorum = qdms_MusteriSikayet.LabYorum;
                //    fisDetays.PaketlemeYorum = qdms_MusteriSikayet.PaketlemeYorum;


                //}


                else
                {
                    fisDetays.Durum = "Sonuç Bekleniyor";
                    fisDetays.Status = 2;
                    db.SaveChanges();
                }




            }


            return RedirectToAction("DevamEdenSikayet");
        }
        #endregion


        #region SONUÇ
        [AllowAnonymous]
        public ActionResult _DevamEdenSikayetSonuc()
        {



            var asd = db.Qdms_MusteriSikayet.OrderByDescending(a => a.ID).Where(a => a.Durum.Trim() == "Sonuç Bekleniyor").ToList();
            return PartialView(asd);

        }
        [AllowAnonymous]
        public ActionResult _DevamEdenSikayetSonucDuzenle(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_MusteriSikayet qdms_MusteriSikayet = db.Qdms_MusteriSikayet.Find(id);
            if (qdms_MusteriSikayet == null)
            {
                return HttpNotFound();
            }
            
            ViewBag.Cari = new SelectList(db.Qdms_Cari, "ID", "CariAdi", qdms_MusteriSikayet.Cari);
            ViewBag.PartiNoSecme = new SelectList(db.Qdms_PartiNo, "ID", "LotNo", qdms_MusteriSikayet.PartiNoSecme);
            return PartialView(qdms_MusteriSikayet);
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult _DevamEdenSikayetSonucDuzenle(Qdms_MusteriSikayet qdms_MusteriSikayet, int? id)
        {

            Qdms_MusteriSikayet fisDetays = db.Qdms_MusteriSikayet.FirstOrDefault(x => x.ID == id);

            if (ModelState.IsValid && fisDetays.YapilanAnalizler != null)
            {
                fisDetays.SikayetSonucu = qdms_MusteriSikayet.SikayetSonucu;

           

                db.SaveChanges();
                return RedirectToAction("DevamEdenSikayet");
            }




            return View(qdms_MusteriSikayet);
        }
        [AllowAnonymous]
        public ActionResult _DevamEdenSikayetSonucDetay(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_MusteriSikayet qdms_MusteriSikayet = db.Qdms_MusteriSikayet.Find(id);
            if (qdms_MusteriSikayet == null)
            {
                return HttpNotFound();
            }
            return View(qdms_MusteriSikayet);
        }

        [AllowAnonymous]
        public ActionResult SonucExcel(int id)
        {
            var model = db.Qdms_MusteriSikayet.Find(id);

            return SonucsExcels(model);

        }
        [AllowAnonymous]
        [HttpPost]
        public FileResult SonucsExcels(Qdms_MusteriSikayet gelenModels)
        {

            DataTable dt = new DataTable("Grid");
            dt.Columns.AddRange(new DataColumn[]{
                 new DataColumn(""),


                            new DataColumn("ŞİKAYET/REKLAMASYON GELİŞ TARİHİ"),
                                new DataColumn("ŞİKAYET/REKLAMASYON NO"),
                                    new DataColumn("İÇ/DIŞ"),
                                 new DataColumn("FİRMA"),
                                   new DataColumn("PARTİ NUMARASI"),
                                       new DataColumn("DTEX/FLAMAN"),
                                       new DataColumn("ŞİKAYET SEBEBİ"),

                                        new DataColumn("ANALİZ SONUÇ ve YORUMLAR:"),
                                    new DataColumn("ÖZET"),
                            //new DataColumn("YAPILAN ANALİZLER ve İNCELEMELER"),
                               new DataColumn("SONUÇ"),





            });
            var liste = from list in db.Qdms_MusteriSikayet.Where(a => a.ID == gelenModels.ID) select list;

            foreach (var list in liste)
            {

                dt.Rows.Add(" ",
                    list.SikayetReklamasyonGelisTarihi.GetValueOrDefault().ToString("dd.MM.yyyy"),
                       list.ID.ToString(),
                 list.IcDis,
                    list.Qdms_Cari.CariAdi,
                    list.Qdms_PartiNo.LotNo.ToString(),
                      list.Qdms_PartiNo.StokAdi.ToString(),



                    list.Qdms_SikayetSebebi.SikayetSebebi,
                       " ",

                    list.LabOzet.ToString(),

                //list.YapilanAnalizler.
                //HttpUtility.HtmlEncode(list.YapilanAnalizler),




                list.SikayetSonucu);

            }

            dt.Rows.Add("F-142.05/00");


            using (XLWorkbook wb = new XLWorkbook())
            {

                //var imagePath = @"~/Images/Picture2.png";   
                //var imagePath = @"C:\Users\MERVE\source\repos\POLYTEKS_QDMS_MVC\Pictures\poly_logo.png";

                var ws = wb.Worksheets.Add(dt, "MusteriSikayetFormu");

                // var image = ws.AddPicture(imagePath).MoveTo(ws.Cell("A1")).Scale(0.1);


                ws.Row(1).InsertRowsAbove(1);


                ws.Cell("B1").Value = "MÜŞTERİ ŞİKAYET/REKLAMASYON BİLGİ FORMU";
                ws.Cell("B1").Style.Font.Bold = true;
                ws.Cell("B1").Style.Font.FontSize = 18;
                ws.Cell("B1").Style.Alignment.Vertical = XLAlignmentVerticalValues.Center;
                //ws.Column(1).CellsUsed().SetDataType(XLDataType.Text);
                //ws.Cell("A39").Value = "Sayın Müşterimiz; \n Bu form Müşteri Memnuniyeti çerçevesinde bilgilendirme amaçlı gönderilmiştir. " + Environment.NewLine + "Hatalı bilgi olması halinde lütfen Müşteri Temsilcimiz ile irtibata geçiniz. \n Bu bilgiler Şikayet Birimi Bölümüne gönderilecek olup, tüm gerekli bilgi ve numuneler tamamlandıktan en geç 5 iş günü içerisinde şikayetin sonuçlandırılmasına özen gösterilecektir.\n Sürecin devam etmesi halinde şikayet hakkında bilgilendirme yapılacaktır.";
                //ws.Cell(1, 1).Style.Font.FontSize = 30;
                //ws.Cell("A39").Style.Alignment.Vertical = XLAlignmentVerticalValues.Distributed;


                //ws.Cell("A39").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                //ws.Cell("A25").Value = "F-142.05/00";
                //ws.Cell("A25").Style.Font.Bold = true;

                ws.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;
                ws.ShowGridLines = false;
                ws.Columns().AdjustToContents();
                //ws.Rows().AdjustToContents();
                ws.Tables.FirstOrDefault().ShowAutoFilter = false;

                ws.Tables.FirstOrDefault().AutoFilter.IsEnabled = false;
                ws.Style.Fill.BackgroundColor = XLColor.White;
                //ws.Cell("C2:V3").Style.Border.OutsideBorder = XLBorderStyleValues.None;
                var rngTable = ws.Range("A2:K18");

                rngTable.Transpose(XLTransposeOptions.MoveCells);
                rngTable.Style.Font.FontColor = XLColor.Black;
                rngTable.Style.Border.OutsideBorder = XLBorderStyleValues.None;
                //rngTable.Column(2).CellsUsed().SetDataType(XLDataType.Text);

                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.AddHeader("content-disposition", "attachment;filename=TrainCatExport.xlsx");

                //rngTable.Range("A2:B30").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //rngTable.Range("A2:B30").Style.Border.InsideBorder = XLBorderStyleValues.Dotted;
                //rngTable.Range("A2:B30").Style.Border.OutsideBorder = XLBorderStyleValues.Thin;
                //rngTable.Range("A2:B30").Style.Border.LeftBorder = XLBorderStyleValues.Thin;
                //rngTable.Range("A2:B30").Style.Border.RightBorder = XLBorderStyleValues.Thin;
                //rngTable.Range("A2:B30").Style.Border.TopBorder = XLBorderStyleValues.Thin;
                //rngTable.Range("A2:A30").Style.Font.Bold = true;


                //rngTable.Range("A15:B20").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                //rngTable.Range("A13:B13").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                //rngTable.Range("A22:B26").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                //rngTable.Range("A3:B11").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                //rngTable.Range("A28:B30").Style.Border.OutsideBorder = XLBorderStyleValues.Thick;
                //IXLRange contents = rngTable.Range("A2:A23");
                //contents.Style.Alignment.WrapText = true;
                //Worksheet worksheet = workbook.Worksheets[0];
                //Cell cell = worksheet.Cells["A1"];
                //cell.HtmlString = "<Font Style=\"FONT-WEIGHT: bold;FONT-STYLE: italic;TEXT-DECORATION: underline;FONT-FAMILY: Arial;FONT-SIZE: 11pt;COLOR: #ff0000;\">This is simple HTML formatted text.</Font>";

                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);


                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", DateTime.Now.ToString("dd MMMM yyyy") + "-" + "F_142_05 MÜŞTERİ ŞİKAYET- REKLAMASYON BİLGİ FORMU" + ".xlsx");
                }


            }





        }
        #endregion
    }
}