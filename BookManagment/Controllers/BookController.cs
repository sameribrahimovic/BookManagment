using DataAccess.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Model;
using Models.ViewModel;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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
            //List<Book> books = _db.Books.ToList();

            //3. Way to load publishers - Eager Loading - does one query per loading
            List<Book> books = _db.Books.Include(p => p.Publisher)
                                        .Include(ba => ba.BookAuthors).ThenInclude(a => a.Author).ToList();


            //load publisher into Index page of Books
            //foreach (var item in books)
            //{
            //    //basic sample, not that good way - search database for every single record(Book)
            //    //item.Publisher = _db.Publishers.FirstOrDefault(p => p.Publisher_Id == item.Publisher_Id);

            //    //explicite loading - bether way, because less times to search database, depends how much Publishers is in Book table
            //    //_db.Entry(item).Reference(p=>p.Publisher).Load();
            //}
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
        public IActionResult Details(int? id)
        {
            BookVM obj = new BookVM();

            if (id == null)
            {
                return View(obj);
            }
            //else edit
            obj.Book = _db.Books.Include(bd => bd.BookDetail).FirstOrDefault(u => u.Book_Id == id); //incude - eager loading
            //book load
            //obj.Book.BookDetail = _db.BookDetails.FirstOrDefault(b => b.BookDetail_Id == obj.Book.BookDetail_Id); // we dont need this if we use eager loading - line abowe
            if (id == null)
            {
                return NotFound();
            }
            return View(obj);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(BookVM obj)
        {
            if (obj.Book.BookDetail.BookDetail_Id == 0) //if - create
            {
                _db.BookDetails.Add(obj.Book.BookDetail);
                _db.SaveChanges();

                var bookFromDb = _db.Books.FirstOrDefault(b => b.Book_Id == obj.Book.Book_Id); //base on this, we will retrive a book
                bookFromDb.BookDetail_Id = obj.Book.BookDetail.BookDetail_Id; //update bookFromDb and save, manualy populate book detail
                _db.SaveChanges();
            }
            else //else - update
            {
                _db.BookDetails.Update(obj.Book.BookDetail);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }

        public IActionResult ManageAuthors(int id)
        {
            BookAuthorVM obj = new BookAuthorVM
            {
                BookAuthorsList = _db.BookAuthors.Include(a => a.Author).Include(a => a.Book)
                                                .Where(a => a.Book_Id == id).ToList(),
                BookAuthor = new BookAuthor()
                {
                    Book_Id = id
                },
                Book = _db.Books.FirstOrDefault(b => b.Book_Id == id)
            };
            List<int> tempListOfAssignedAuthors = obj.BookAuthorsList.Select(x => x.Author_Id).ToList();
            var tempList = _db.Authors.Where(x => !tempListOfAssignedAuthors.Contains(x.Author_Id)).ToList();

            //populate dropdown
            obj.AuthorList = tempList.Select(x => new SelectListItem
            {
                Text = x.FullName,
                Value = x.Author_Id.ToString()
            });

            return View(obj);
        }

        [HttpPost]
        public IActionResult ManageAuthors(BookAuthorVM bookAuthorVM)
        {
            if (bookAuthorVM.BookAuthor.Book_Id !=0 && bookAuthorVM.BookAuthor.Author_Id != 0)
            {
                _db.BookAuthors.Add(bookAuthorVM.BookAuthor);
                _db.SaveChanges();
            }
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookAuthorVM.BookAuthor.Book_Id });
        }

        [HttpPost]
        public IActionResult RemoveAuthors(int authorId, BookAuthorVM bookAuthorVM)
        {
            int bookId = bookAuthorVM.Book.Book_Id;
            BookAuthor bookAuthor = _db.BookAuthors.FirstOrDefault(x => x.Author_Id == authorId && x.Book_Id == bookId);
            _db.BookAuthors.Remove(bookAuthor);
            _db.SaveChanges();
            
            return RedirectToAction(nameof(ManageAuthors), new { @id = bookId });
        }
    }
}
