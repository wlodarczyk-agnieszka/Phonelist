using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Phonelist.Models;

namespace Phonelist.Controllers
{
    /*TODO:
     - Wyszukiwarka (najchetniej jako partial view?)
     - Edycja osób
     - Paginacja
     - Poprawić wygląd
     - Dodać stopkę?
    */ 



    public class PersonController : Controller
    {
        SourceManager _sourceManager = new SourceManager();

        public IActionResult Index()
        {
            var personsList = _sourceManager.Get(1, 50);

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
            return Content($"Jeszcze nie działa. Próbujesz wyszukać: {searchText}");
        }
    }
}