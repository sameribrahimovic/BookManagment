using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Model;
using Models.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace BookManagment.Controllers
{
    public class BookController : Controller
    {
        private readonly ApplicationDbContext _db;

        public BookController(ApplicationDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {
            List<Book> books = _db.Books.ToList();

            //load publisher into Index page of Books
            foreach (var item in books)
            {
                //basic sample, not that good way - search database for every single record(Book)
                //item.Publisher = _db.Publishers.FirstOrDefault(p => p.Publisher_Id == item.Publisher_Id);

                //explicite loading - bether way, because less times to search database, depends how much Publishers is in Book table
                _db.Entry(item).Reference(p=>p.Publisher).Load();
            }
            return View(books);
        }
        public IActionResult Upsert(int? id)
        {
            BookVM obj = new BookVM();

            //projections - to show all publisher in dropdown
            obj.PublisherList = _db.Publishers.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Publisher_Id.ToString()
            });
            if (id == null)
            {
                return View(obj);
            }
            //else edit
            obj.Book = _db.Books.FirstOrDefault(u => u.Book_Id == id);
            if (id == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(BookVM obj)
        {
            if (obj.Book.Book_Id == 0) //if - create
            {
                _db.Books.Add(obj.Book);
            }
            else //else - update
            {
                _db.Books.Update(obj.Book);
            }
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var bookFromDb = _db.Books.FirstOrDefault(b => b.Book_Id == id);
            _db.Books.Remove(bookFromDb);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}
