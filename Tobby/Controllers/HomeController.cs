using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGeneration.Templating;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Xml.Linq;
using Tobby.Models;
using Tobby.Models.ViewModels;
using Tobby.Service.Interfaces;

namespace Tobby.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IElementRepository _elementRepository;
        private readonly IElementFunctions _elementFunctions;

        public HomeController(IElementRepository elementRepository, IElementFunctions elementFunctions, ILogger<HomeController> logger)
        {
            _elementRepository = elementRepository;
            _elementFunctions = elementFunctions;
            _logger = logger;
        }

        public IActionResult Index(Element element)
        {
            return RedirectToAction("NewTemplate", "Elements", element);
            //return RedirectToAction("yourAnotherActionName", "yourAnotherControllerName");
            //return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}