using Challenge1.Data;
using Challenge1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;

namespace Challenge1.Controllers
{
    public class HomeController : Controller
    {
        private static string BatchId;

        public HomeController()
        {
            if (BatchId == null)
                BatchId = Guid.NewGuid().ToString();
        }

        public ActionResult Index()
        {
            var _entries = AddressBook.Book;

            _entries = _entries.Where(w => w.BatchId == BatchId).ToList();
            return View(_entries);
        }

        public ActionResult NewBatch()
        {
            BatchId = Guid.NewGuid().ToString();
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public ActionResult Index(string name, string phone)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrEmpty(phone))
            {
                ModelState.AddModelError("", "Name and phone number are required.");
            }
            else if (!Regex.IsMatch(name, @"^[a-zA-Z\s]+$"))
            {
                ModelState.AddModelError("name", "Name can only contain letters and spaces.");
            }
            else if (!Regex.IsMatch(phone, @"^(\+44)?\(?0?\d{4}\)?\d{6}$"))
            {
                ModelState.AddModelError("phone", "Invalid phone number format.");
            }
            else if (AddressBook.Book.Where(w => w.BatchId == BatchId).ToList().Count >= 50)
            {
                ModelState.AddModelError("", "Maximum number of 50 entries reached.");
            }
            else if (AddressBook.Book.Count >= 300)
            {
                ModelState.AddModelError("", "Maximum number of 300 entries reached.");
            }
            else
            {
                AddressBook.Book.Add(new AddressModel()
                {
                    BatchId = BatchId,
                    Name = name,
                    PhoneNumber = phone
                });
            }

            var _entries = AddressBook.Book;

            _entries = _entries.Where(w => w.BatchId == BatchId).ToList();

            return View(_entries);
        }

        public ActionResult About()
        {
            var _entries = AddressBook.Book;

            return View(_entries);
        }

        public ActionResult Contact()
        {

            return View( new List<AddressModel>());
        }

        [HttpPost]
        public ActionResult Contact(string phone)
        {
           var _entries = AddressBook.Book.Where(w => w.PhoneNumber == phone).ToList();
            return View(_entries);
        }
    }
}