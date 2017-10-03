using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Facelist.Utilities
{
    public class ImageUtility
    {
        public static string imgBaseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                                   HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/Images/";

        public static string imgBlobUrl = ConfigurationManager.AppSettings["imgBlobUrl"];
        public static string imgBlobFolder = ConfigurationManager.AppSettings["imgBlobFolder"];
        public static string imgBlobAccessKey = ConfigurationManager.AppSettings["BlobAccessKey"];
        public static string imgBlobStorageName = ConfigurationManager.AppSettings["blobStorageName"];
    }
}