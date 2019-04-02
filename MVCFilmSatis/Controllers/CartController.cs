using Microsoft.AspNet.Identity;
using MVCFilmSatis.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVCFilmSatis.Controllers
{
    public class CartController : Controller
    {
        ApplicationDbContext db = new ApplicationDbContext();
        // GET: Cart
        public ActionResult Index()
        {
            string uid = User.Identity.GetUserId();
            var u = db.Users.Find(uid);
            if (u.ShoppingCart == null)
            {
                u.ShoppingCart = new ShoppingCart();
                u.ShoppingCart.Movies = new List<Movie>();
            }
            var list = u.ShoppingCart.Movies.ToList();
            decimal total = 0;
            foreach (var item in list)
            {
                total = total + item.Price;
            }
            ViewBag.Total = total.ToString("C");
            return View(list);
        }

        public ActionResult AddToCart(int id)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home", new { error = "Login to but movies" });
            //Giriş yapmış kişinin id si
            string uid = User.Identity.GetUserId();

            //Giriş yapmış kişinin tüm bilgileri
            Customer c = db.Users.Find(uid);

            //null reference exception önlemleri
            if (c.ShoppingCart == null)
                c.ShoppingCart = new ShoppingCart();

            if (c.ShoppingCart.Movies == null)
                c.ShoppingCart.Movies = new List<Movie>();

            if (c.ShoppingCart.Movies.Any(x => x.MovieId == id))
            {
                //seçilen film zaten sepette var
                return RedirectToAction("Index", "Home", new { error = "You already have this movie in your cart." });
                //Başka controller'a veri gönderme
            }
            else
            {
                //Film sepette yok, işleme devam ediyoruz.
                //Seçilmiş olan film
                Movie choosenMovie = db.Movies.Find(id);
                c.ShoppingCart.Movies.Add(choosenMovie);
                db.Entry(c).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

        }

        public ActionResult Delete(int id)
        {   //int id :sepetten kaldırılacak film idsi

            //giriş yapmış kişini idsi
            string uid = User.Identity.GetUserId();
            //giriş yapmış kişinin tüm bilgileri
            var u = db.Users.Find(uid);
            //silinecek filmi bulduk
            var movie = u.ShoppingCart.Movies.Where(x => x.MovieId == id).FirstOrDefault();
            //kişinin sepetinden o filmi kaldrdık
            u.ShoppingCart.Movies.Remove(movie);
            //kişiyi değişti olarak işaretle
            db.Entry(u).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Checkout()
        {
            string uid = User.Identity.GetUserId();
            Customer c = db.Users.Find(uid);

            ViewBag.Total = c.ShoppingCart.SubTotal;
            ViewBag.CartNo = c.ShoppingCart.ShoppingCartId;

            return View();
        }

        [HttpPost]
        public ActionResult PayBankTransfer(int? approve)
        {
            if (approve.HasValue && approve.Value == 1)
            {

                BankTransferPayment p1 = new BankTransferPayment();
                p1.IsApproved = false;
                p1.NameSurname = User.Identity.GetNameSurname();
                p1.TC = User.Identity.GetTC();

                BankTransferService service = new BankTransferService();
                bool isPaid = service.MakePayment(p1);
                
                if (isPaid)
                {
                    CreateOrder(isPaid);
                    ResetShoppingCart();
                }

                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("Chechout");
        }
        public void ResetShoppingCart()
        {
            string uid = User.Identity.GetUserId();
            Customer c = db.Users.Find(uid);
            c.ShoppingCart.Movies.Clear();
            db.Entry(c).State = EntityState.Modified;
            db.SaveChanges();
        }
        public Order CreateOrder(bool isPaid)
        {
            string uid = User.Identity.GetUserId();
            Customer c = db.Users.Find(uid);

            Order order = new Order();
            order.Customer = c;
            order.IsPaid = isPaid; //ödenip ödenmediğine bakıyoruz
            order.OrderItems = new List<OrderItem>();
            foreach (var item in c.ShoppingCart.Movies)
            {
                OrderItem oi = new OrderItem();
                order.Date = DateTime.Now;
                oi.Movie = item;
                oi.Count = 1;
                oi.Price = item.Price;
                order.OrderItems.Add(oi);
            }
            order.SubTotal = c.ShoppingCart.SubTotal;
            db.Orders.Add(order);
            db.SaveChanges();
            return order;
        }
    }
}