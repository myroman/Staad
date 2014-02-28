using System.Web.Mvc;
using Staad.Domain.Abstract;
using Staad.Domain.Entities;
using Staad.Web.Models;

namespace Staad.Web.Controllers
{
    using System;

    public class DictionaryController : Controller
    {
        private readonly IDictionaryRepository dictionaryRepository;

        public DictionaryController(IDictionaryRepository dictionaryRepository)
        {
            this.dictionaryRepository = dictionaryRepository;
        }

        public ActionResult List()
        {
            var dictionaries = dictionaryRepository.GetList();

            return View(new DictionaryListViewModel(dictionaries, routeValues => Url.Action("Show", routeValues)));
        }

        [HttpGet]
        public ViewResult New()
        {
            return View(new Dictionary());
        }

        [HttpPost]
        public ActionResult New(Dictionary dictionary)
        {
            if (ModelState.IsValid)
            {
                dictionaryRepository.Create(dictionary);
                return RedirectToAction("Show", new { dictionary.Id });
            }

            return View(dictionary);
        }

        public ActionResult Show(int id = 0)
        {
            var dictionary = dictionaryRepository.Read(id);
            if (dictionary != null)
            {
                return View(new DictionaryViewModel(dictionary));
            }
            return RedirectToAction("List");
        }

        public ActionResult Delete(string dictionaryIdList)
        {
            if (!string.IsNullOrEmpty(dictionaryIdList))
            {
                var idsToDelete = dictionaryIdList.Split(',');
                foreach (var dictionaryId in idsToDelete)
                {
                    var id = Convert.ToInt32(dictionaryId);

                    dictionaryRepository.Delete(id);
                }
            }
            
            return RedirectToAction("List");
        }
    }
}
