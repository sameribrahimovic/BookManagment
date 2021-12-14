using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using System.Collections.Generic;
using System.Linq;

namespace BookManagment.Controllers
{
    public class AuthorController : Controller
    {
        private readonly ApplicationDbContext _db;
        public AuthorController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //to show all Authors
            List<Author> authors = _db.Authors.ToList();
            return View(authors);
        }

        public IActionResult Upsert(int? id)
        {
            Author author = new Author();
            if (id == null)
            {
                return View(author);
            }

            if (author == null)
            {
                return NotFound();
            }
            author = _db.Authors.FirstOrDefault(a => a.Author_Id == id);
            return View(author);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Author author)
        {
            if (ModelState.IsValid)
            {
                if (author.Author_Id == 0)
                {
                    //Create
                    _db.Authors.Add(author);
                }
                else //update
                {
                    _db.Authors.Update(author);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(author);
        }

        public IActionResult Delete(int id)
        {
            var objFromDb = _db.Authors.FirstOrDefault(a => a.Author_Id == id);
            _db.Authors.Remove(objFromDb);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
