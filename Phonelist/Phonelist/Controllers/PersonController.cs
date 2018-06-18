using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phonelist.Models;

namespace Phonelist.Controllers
{
    /*TODO:
 
     - Paginacja

    */


    public class PersonController : Controller
    {
        SourceManager _sourceManager = new SourceManager();

        private int RecordsPerPage = 10;

        public IActionResult Index(int page = 1)
        {
            ViewBag.SearchText = "";

            int records = _sourceManager.NumberOfRecords();
            int pages = (int)Math.Ceiling((double)(records / RecordsPerPage));
            ViewBag.Pages = pages;
            ViewBag.ActualPage = page;

            int start = (RecordsPerPage * page) - (RecordsPerPage - 1); 
            
            var personsList = _sourceManager.Get(start, RecordsPerPage);

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
        public IActionResult Search()
        {
            return View("/Shared/_SearchForm.cshtml");
        }

        [HttpPost]
        public IActionResult Search(string searchText)
        {
            var results = _sourceManager.Search(searchText);

            if (results != null)
            {
                ViewBag.SearchText = $"Wyniki dla \"{searchText}\"";
                return View("Index", results);
            }
            else
            {
                //return RedirectToAction("Index");
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
                return RedirectToAction("Index");
            }
            else
            {
                return View("Edit", model);
            }
        }


    }
}