using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phonelist.Models;

namespace Phonelist.Controllers
{

    public class PersonController : Controller
    {
        SourceManager _sourceManager = new SourceManager();

        private int RecordsPerPage = 10;

        public IActionResult Index(int page = 1)
        {
            
            int records = _sourceManager.NumberOfRecords();
            int pages = (int)Math.Ceiling((double)(records / RecordsPerPage));
            ViewBag.Pages = pages;
            ViewBag.ActualPage = page;

            var personListAll = _sourceManager.Get(1, records);

            int skip = page == 1 ? 0 : (RecordsPerPage * page);

            IEnumerable<PersonModel> personsList = personListAll.Skip(skip).Take(RecordsPerPage);

            return View(personsList);
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(PersonModel model)
        {
            if (ModelState.IsValid)
            {
                _sourceManager.Add(model);

                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Remove(int id)
        {
            var removeMe = _sourceManager.GetByID(id);

            return View(removeMe);
        }

        [HttpPost]
        public IActionResult RemoveConfirmed(PersonModel model)
        {
            _sourceManager.Remove(model.ID);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Search(string searchText)
        {
            var results = _sourceManager.Search(searchText);

            if (results != null)
            {
                ViewBag.SearchText = searchText;
                return View("Index", results);
            }
            else
            {
                return View("Info", "Brak wyników");
            }
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var editMe = _sourceManager.GetByID(id);
            return View("Edit", editMe);
        }

        [HttpPost]
        public IActionResult EditUpdate(PersonModel model)
        {
            if (ModelState.IsValid)
            {
                _sourceManager.Update(model);
                return Redirect("Index");
            }
            else
            {
                return View("Edit", model);
            }
        }


    }
}