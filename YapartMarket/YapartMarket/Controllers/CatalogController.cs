using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using YapartMarket.Core.BL;
using YapartMarket.Core.Models;
using YapartMarket.MainApp.ViewModels;

namespace YapartMarket.MainApp.Controllers
{
    public class CatalogController : Controller
    {
        private readonly IMarkService _markService;
        private readonly IModelService _modelService;
        private readonly IModificationService _modificationService;

        public CatalogController(IMarkService markService, IModelService modelService, IModificationService modificationService, IServiceProvider serviceProvider)
        {
            _markService = markService;
            _modelService = modelService;
            _modificationService = modificationService;
            _modificationService = serviceProvider.GetService<IModificationService>();
        }
        public ActionResult Cars(string mark, string model, string modification)
        {
            CarViewModel carViewModel = new CarViewModel();
            //список всех модификаций
            if (!string.IsNullOrEmpty(mark) && !string.IsNullOrEmpty(model) && string.IsNullOrEmpty(modification))
            {
               carViewModel.Modifications = _markService.GetAll(x => x.Name == mark).FirstOrDefault()?.Models.FirstOrDefault(x => x.Name == model).Modifications;
                ViewBag.TypeView = "Modifications";

                //Конкретная модификация
            }else if (!string.IsNullOrEmpty(mark) && !string.IsNullOrEmpty(model) && !string.IsNullOrEmpty(modification))
            {
                var currentModification = _markService.GetAll(m => m.Name == mark, null,
                        x => x.Include(o=> o.Models)).FirstOrDefault()?.Models
                    .FirstOrDefault(x => x.Name == model).Modifications.FirstOrDefault(x=>x.Name == modification);
                ViewBag.TypeView = "Modification";
            }
            else if (!string.IsNullOrEmpty(mark) && string.IsNullOrEmpty(model) && string.IsNullOrEmpty(modification))
            {
               carViewModel.Models = _markService.GetAll(x => x.Name == mark,null, x=> x.Include(o => o.Models)).FirstOrDefault().Models;
               ViewBag.TypeView = "Models";
            }
            else
            {
                carViewModel.Marks = _markService.GetAll();
                ViewBag.TypeView = "Marks";
            }

            return View(carViewModel);
        }

        public IActionResult MenuAccessories()
        {
            return PartialView();
        }
    }
}