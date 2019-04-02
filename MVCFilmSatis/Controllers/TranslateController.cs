using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Mvc;

namespace MVCFilmSatis.Controllers
{
    public class TranslateController : Controller
    {
        // GET: Translate
        //?id=tr
        //Translate/Index/tr
        public ActionResult Index(string id,string page)
        {
            HttpCookie langCookie = new HttpCookie("lang");
            langCookie.Value = id;
            Response.Cookies.Add(langCookie);

            //CultureInfo("tr")
            //CultureInfo("tr-TR")
            //----> Cookie ye cevap verme global.asax sayfasında

            return Redirect(page);

            //RedirectToPermanent bir kez yönlendirme yapıyor
        }
    }
}