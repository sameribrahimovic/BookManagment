using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Models.Model;
using System.Linq;

namespace BookManagment.Controllers
{
    public class PublisherController : Controller
    {
        private readonly ApplicationDbContext _db;
        public PublisherController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //show all publishers from db
            List<Publisher> publisherList = _db.Publishers.ToList();
            return View(publisherList);
        }

        public IActionResult Upsert(int? id)
        {
            Publisher publisher = new Publisher();
            if (id == null)
            {
                return View(publisher);
            }

            if (publisher == null)
            {
                return NotFound();
            }
            publisher = _db.Publishers.FirstOrDefault(p => p.Publisher_Id == id);
            return View(publisher);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Publisher publisher)
        {
            if (ModelState.IsValid)
            {
                if (publisher.Publisher_Id == 0)
                {
                    _db.Publishers.Add(publisher);
                }
                else //edit publisher
                {
                    _db.Publishers.Update(publisher);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        public IActionResult Delete(int id)
        {
            var publisherFromDb = _db.Publishers.FirstOrDefault(p =>p.Publisher_Id==id);
            _db.Publishers.Remove(publisherFromDb);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
