using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Skylakias.Models;

namespace Skylakias.Controllers
{
    public class OrdersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.ApplicationUser).Include(o => o.Service);
            return View(orders.ToList());
        }

        // GET: Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // GET: Orders/Create
        public ActionResult Create()
        {
            //ViewBag.CustomerId = new SelectList(db.Users, "Id", "Name");
            //ViewBag.ServiceId = new SelectList(db.Services, "Id", "Name");
            //var userId = User.Identity.GetUserId();
            //var user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
            //ViewBag.Disc = user.MembershipType.DiscountRate;

            var list = db.Services.ToList();

            return View(list);
        }

        // POST: Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id)
        {
            
            if (ModelState.IsValid)
            {
                var service = db.Services.Where(s => s.Id == id).FirstOrDefault();
                var order = new Order();
                order.Service = service;
                var userId = User.Identity.GetUserId();
                var user = db.Users.Where(x => x.Id == userId).FirstOrDefault();
                var disc = user.MembershipType.DiscountRate;
                var price = order.Service.Price;
                order.TotalPrice = Math.Round(price - (disc * price), 2);
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            //ViewBag.CustomerId = new SelectList(db.Users, "Id", "Name", order.ApplicationUserId);
            //ViewBag.ServiceId = new SelectList(db.Services, "Id", "Name", order.ServiceId);
            return View("Index");
        }

        // GET: Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.CustomerId = new SelectList(db.Users, "Id", "Name", order.ApplicationUserId);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Name", order.ServiceId);
            return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "CustomerId,ServiceId,TotalPrice")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CustomerId = new SelectList(db.Users, "Id", "Name", order.ApplicationUserId);
            ViewBag.ServiceId = new SelectList(db.Services, "Id", "Name", order.ServiceId);
            return View(order);
        }

        // GET: Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
