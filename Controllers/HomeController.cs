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
        POLY_QDMSEntities db = new POLY_QDMSEntities();

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

      
    }
}