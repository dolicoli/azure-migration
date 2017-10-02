using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facelist.Models;

namespace Facelist.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            ViewBag.listFaces = GetFaces();
            return View();
        }

        [HttpPost]
        public ActionResult Upload()
        {
            if (HttpContext.Request.Files.Count > 0)
            {
                UploadFiles(HttpContext.Request.Files[0]);
            }
            return RedirectToAction("Index");
        }

        private List<Face> GetFaces()
        {
            List<Face> listFaces = new List<Face>();
            using (var ctx = new facedbEntities())
            {
                listFaces = ctx.Faces.ToList();
            }
            return listFaces;
        }

        private void UploadFiles(HttpPostedFileBase file)
        {
            string filename = file.FileName;
            string imageLocation = HttpContext.Server.MapPath("~/Images/");
            file.SaveAs(imageLocation + filename);
            Face face = new Face();
            face.filename = filename;
            face.name = "face";
            using (var ctx = new facedbEntities())
            {
                ctx.Faces.Add(face);
                ctx.SaveChanges();
            }
        }
    }
}