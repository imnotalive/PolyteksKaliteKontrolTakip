using PagedList;
using PolyteksKaliteKontrolTakip.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace PolyteksKaliteKontrolTakip.Controllers
{
    public class HomeController : Controller
    {
        public POLY_QDMSEntities db = new POLY_QDMSEntities();

        public ActionResult Index()
        {
            bool islogin = User.Identity.IsAuthenticated;
            string username = User.Identity.Name;
            return View();
        }
        [AllowAnonymous]
        public ActionResult Home()
        {
            return View();
        }
        [AllowAnonymous]
        public PartialViewResult Cariler(/*int sayfa=1*/int? page)
        {
            int pageNumber = page ?? 1;
            int pagesize = 10;
            var cari = db.Qdms_Cari.ToList().OrderBy(a => a.CariAdi).ToPagedList(pageNumber, pagesize);
            return PartialView(cari);
        }
        [AllowAnonymous]
        public ActionResult Giris()
        {

            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public ActionResult Giris(Qdms_Kullanici Kullanici)
        {

            var teyit = db.Qdms_Kullanici.FirstOrDefault(a => a.KullaniciKodu == Kullanici.KullaniciKodu && a.Sifre == Kullanici.Sifre);
            if (teyit != null)
            {
                FormsAuthentication.SetAuthCookie(teyit.AdSoyad, false);
                Session["AdSoyad"] = teyit.AdSoyad;
                return RedirectToAction("Home", "Home");
            }
            else
            {
                ViewBag.mesaj = "GEÇERSİZ KULLANICI ADI VEYA ŞİFRE !";
                return View();
            }

        }
        [AllowAnonymous]
        public ActionResult Cikis()
        {
            FormsAuthentication.SignOut();

            return View("Giris");
        }
        public ActionResult Profil()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Giris", "Home");
        }
        [HttpPost]
        public JsonResult KeepSessionAlive()
        {

            return new JsonResult
            {
                Data = "Beat Generated"
            };
        }
        [AllowAnonymous]
        public ActionResult Kullanici()
        {

            return View(db.Qdms_Kullanici.OrderBy(a=>a.AdSoyad).ToList());
        }


        public ActionResult karisik()
        {
            //var monthlists = db.Qdms_MusteriSikayet.OrderBy(a=>a.KokNeden).ToList();



            //ViewBag.Exponate =
            //    Newtonsoft.Json.JsonConvert.SerializeObject(monthlists);
            var groups = db.Qdms_MusteriSikayet.GroupBy(p => p.KokNeden).OrderBy(p => p.Key).Select(g => new { g.Key, Count = g.Count() });

            //var sorgu = from x in db.Qdms_MusteriSikayet
            //            group x by x.KokNeden into g
            //            select(g => new 
            //            {
            //                KokNeden = g,
            //                MusteriSikayetId = g.Count()
            //            };
             ViewBag.asd =
            Newtonsoft.Json.JsonConvert.SerializeObject(groups.ToList());
            return View();
         

        
        }

    }
}