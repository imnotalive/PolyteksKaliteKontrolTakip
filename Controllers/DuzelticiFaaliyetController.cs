using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PolyteksKaliteKontrolTakip.Models;
using Rotativa;
using Rotativa.Options;

namespace PolyteksKaliteKontrolTakip.Controllers
{
    public class DuzelticiFaaliyetController : HomeController
    {
     

     
        public ActionResult Indexs()
        {
            return View(db.Qdms_DuzelticiFaaliyetFormu.ToList());
        }

        
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_DuzelticiFaaliyetFormu qdms_DuzelticiFaaliyetFormu = db.Qdms_DuzelticiFaaliyetFormu.Find(id);
            if (qdms_DuzelticiFaaliyetFormu == null)
            {
                return HttpNotFound();
            }
            return View(qdms_DuzelticiFaaliyetFormu);
        }

    
        public ActionResult Create()
        {
            return View();
        }

      
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Qdms_DuzelticiFaaliyetFormu qdms_DuzelticiFaaliyetFormu)
        {
            if (ModelState.IsValid)
            {
                db.Qdms_DuzelticiFaaliyetFormu.Add(qdms_DuzelticiFaaliyetFormu);
                db.SaveChanges();
                return RedirectToAction("Indexs");
            }

            return View(qdms_DuzelticiFaaliyetFormu);
        }

      
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_DuzelticiFaaliyetFormu qdms_DuzelticiFaaliyetFormu = db.Qdms_DuzelticiFaaliyetFormu.Find(id);
            if (qdms_DuzelticiFaaliyetFormu == null)
            {
                return HttpNotFound();
            }
            return View(qdms_DuzelticiFaaliyetFormu);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Qdms_DuzelticiFaaliyetFormu qdms_DuzelticiFaaliyetFormu)
        {
            if (ModelState.IsValid)
            {
                db.Entry(qdms_DuzelticiFaaliyetFormu).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Indexs");
            }
            return View(qdms_DuzelticiFaaliyetFormu);
        }


        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_DuzelticiFaaliyetFormu qdms_DuzelticiFaaliyetFormu = db.Qdms_DuzelticiFaaliyetFormu.Find(id);
            if (qdms_DuzelticiFaaliyetFormu == null)
            {
                return HttpNotFound();
            }
            return View(qdms_DuzelticiFaaliyetFormu);
        }

       
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Qdms_DuzelticiFaaliyetFormu qdms_DuzelticiFaaliyetFormu = db.Qdms_DuzelticiFaaliyetFormu.Find(id);
            db.Qdms_DuzelticiFaaliyetFormu.Remove(qdms_DuzelticiFaaliyetFormu);
            db.SaveChanges();
            return RedirectToAction("Indexs");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult PDFFormat(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Qdms_DuzelticiFaaliyetFormu qdms_DuzelticiFaaliyetFormu = db.Qdms_DuzelticiFaaliyetFormu.Find(id);
            if (qdms_DuzelticiFaaliyetFormu == null)
            {
                return HttpNotFound();
            }
            return View(qdms_DuzelticiFaaliyetFormu);
        }


        [AllowAnonymous]
        public ActionResult ExportPDFS(int? id)
        {
            Qdms_MusteriSikayet qdms_MusteriSikayet = db.Qdms_MusteriSikayet.Find(id);
            return new ActionAsPdf("PDFFormat", new { id = id })
            {
                //FileName=Server.MapPath("~/Temalar")
                FileName = DateTime.Now.ToString("dd MMMM yyyy") + "-" + "DuzelticiFaaliyetFormu.pdf",
                PageSize = Size.A4,
                MinimumFontSize = 8


            };
        }
    }
}
