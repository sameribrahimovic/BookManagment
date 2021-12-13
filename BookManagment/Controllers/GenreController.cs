using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Models.Model;
using System.Collections.Generic;
using System.Linq;

namespace BookManagment.Controllers
{
    public class GenreController : Controller
    {
        private readonly ApplicationDbContext _db;

        public GenreController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            //to show all genres
            List<Genre> genres = _db.Genres.ToList();
            return View(genres);
        }

        public IActionResult Upsert(int? id) //Insert or update
        {
            Genre obj = new Genre();
            if (id == null)
            {
                return View(obj);
            }

            //else Edit
            if (obj == null)
            {
                return NotFound();
            }
            obj = _db.Genres.FirstOrDefault(g => g.Genre_Id == id);
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Genre genre) //Insert or update
        {
            if (ModelState.IsValid) //related to data Anotations
            {
                if (genre.Genre_Id == 0)
                {
                    //Create
                    _db.Genres.Add(genre);
                }
                else
                {
                    //Update
                    _db.Genres.Update(genre);
                }
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }
            return View(genre);
        }

        public IActionResult Delete(int id)
        {
            var objFromDb = _db.Genres.FirstOrDefault(g => g.Genre_Id == id);
            _db.Genres.Remove(objFromDb);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
