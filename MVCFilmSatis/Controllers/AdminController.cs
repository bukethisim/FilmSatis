using MVCFilmSatis.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace MVCFilmSatis.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class AdminController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Slider()
        {
            return View(db.Sliders.ToList());
        }

        [HttpGet]
        public ActionResult SliderCreate()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SliderCreate(HttpPostedFileBase imagefile)
        {
            if (imagefile != null && imagefile.ContentLength != 0)
            {
                //exe dosyası olmadığından dolayı IIS üzerinden hizmet verdiğimiz için sadece relativepath kullanamıyoruz. MapPath çalışan siteye göre absolute path getirir. abs.path ---> C//..... den başlar
                string path = Server.MapPath("/Uploads/Sliders/");
                string thumbpath = path + "thumb/";
                string largepath = path + "large/";

                //HttpPostedFileBase ---> SaveAs
                imagefile.SaveAs(largepath + imagefile.FileName);

                //(dosyadaki image bulduk)
                Image i = Image.FromFile(largepath + imagefile.FileName);

                //size kaç olsun onu verdik
                Size s = new Size(380, 100);

                Image small = Helper.ResizeImage(i, s);

                //Image olduğundan---> Save
                small.Save(thumbpath + imagefile.FileName);

                i.Dispose(); //yeni eklenen resim o anda silinmediğinden Dispose etmeliyiz. İlişkisini keseriz

                Slider slider = new Slider();
                //img src içinde göstereceğimiz için relative path kaydediyoruz.
                slider.LargeImageURL = "/Uploads/Sliders/large/"+imagefile.FileName;
                slider.ThumbnailURL = "/Uploads/Sliders/thumb/" + imagefile.FileName;

                db.Sliders.Add(slider);
                db.SaveChanges();
                return RedirectToAction("Slider");

            }
            return View();
        }
        public ActionResult DeleteSlider(int id)
        {
            var selected = db.Sliders.Find(id);
            //ana dizinin absolute pathi
            var path = Server.MapPath("/");
            var l = path + selected.LargeImageURL;
            var s = path + selected.ThumbnailURL;

            //Dosyalardan resimleri sildik.
            System.IO.File.Delete(l);
            System.IO.File.Delete(s);

            db.Sliders.Remove(selected);
            db.SaveChanges();
            return RedirectToAction("Slider");
        }
    }
}