using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Facelist.Models;
using Facelist.Utilities;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Facelist.Controllers
{
    public class HomeController : Controller
    {
        private string BlobAccountName = ImageUtility.imgBlobStorageName;

        private string BlobAccessKey = ImageUtility.imgBlobAccessKey;
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
               // UploadFiles(HttpContext.Request.Files[0]);
                UploadFiletoBlob(HttpContext.Request.Files[0]);
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

        private void UploadFiletoBlob(HttpPostedFileBase file)
        {
            StorageCredentials cred = new StorageCredentials(BlobAccountName, BlobAccessKey);
            CloudStorageAccount acc = new CloudStorageAccount(cred, true);
            CloudBlobClient client = acc.CreateCloudBlobClient();
            CloudBlobContainer container = client.GetContainerReference("images");
            container.CreateIfNotExists();
            container.SetPermissions(new BlobContainerPermissions()
            {
                PublicAccess = BlobContainerPublicAccessType.Blob
            });
            CloudBlockBlob blob = container.GetBlockBlobReference(file.FileName);
            using (Stream stream = file.InputStream)
            {
                blob.Properties.ContentType = file.ContentType;
                blob.UploadFromStream(stream);
            }
            Face face = new Face();
            face.filename = file.FileName;
            face.name = "face";
            using (var ctx = new facedbEntities())
            {
                ctx.Faces.Add(face);
                ctx.SaveChanges();
            }
        }
    }
}