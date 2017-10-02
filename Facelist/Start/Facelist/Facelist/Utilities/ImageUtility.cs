using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Facelist.Utilities
{
    public class ImageUtility
    {
        public static string imgBaseUrl = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority +
                                   HttpContext.Current.Request.ApplicationPath.TrimEnd('/') + "/Images/";
    }
}