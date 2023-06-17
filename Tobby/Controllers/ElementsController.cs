using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Razor.Language.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration.Internal;
using Tobby.Data;
using Tobby.Data.Enum;
using Tobby.Models;
using Tobby.Models.ViewModels;
using Tobby.Service.Interfaces;

namespace Tobby.Controllers
{
    public class ElementsController : Controller
    {
        private readonly IElementRepository _elementRepository;
        private readonly IElementFunctions _elementFunctions;

        public ElementsController(IElementRepository elementRepository, IElementFunctions elementFunctions)
        {
            _elementRepository = elementRepository;
            _elementFunctions = elementFunctions;
        }

        // GET: Elements
        public async Task<IActionResult> Index()
        { 
              return View(await _elementRepository.GetAll());
        }

        // GET: Elements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _elementRepository.GetByIdAsync(id);
            if (element == null)
            {
                return NotFound();
            }

            string globalColor = "black";
            string globalFont = "Poppins";

            ViewBag.GlobalStyle = ":root {--primary-color: " + globalColor + "; --primary-font: " + globalFont + " ;}";
            ViewBag.ImportFont = globalFont;


            return View(element);
        }

        // GET: Elements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Elements/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Element element)
        {
            if (ModelState.IsValid)
            {
                // Modify html classes
                _elementFunctions.ModifyHtmlClasses(element);

                _elementRepository.Add(element);
                var savingResult = _elementRepository.Save();
                if(!savingResult)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    //Error message
                    return View(element);
                }
            }
            return View(element);
        }

        // GET: Elements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _elementRepository.GetByIdAsync(id);
            if (element == null)
            {
                return NotFound();
            }
            return View(element);
        }

        // POST: Elements/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Element element)
        {
            if (id != element.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _elementRepository.Update(element);
                    var savingResult = _elementRepository.Save();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (_elementRepository.GetByIdAsync(element.ID) == null)
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(element);
        }

        // GET: Elements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var element = await _elementRepository.GetByIdAsync(id);
            if (element == null)
            {
                return NotFound();
            }

            return View(element);
        }

        // POST: Elements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var element = await _elementRepository.GetByIdAsync(id);
            if (element != null)
            {
                _elementRepository.Delete(element);
            }

            return RedirectToAction(nameof(Index));
        }



        public IActionResult NewTemplate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> NewTemplate(TemplateRequirementsViewModel model)
        {
            if (ModelState.IsValid)
            {

                IEnumerable<Element> elements = await _elementRepository.GetSectionByRequirements(model.Category);

                if (elements != null)
                {
                    return RedirectToAction(nameof(TemplateResult), model);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            return View();
        }

        public async Task<IActionResult> TemplateResult(TemplateRequirementsViewModel model)
        {
            IEnumerable<Element> elements = await _elementRepository.GetSectionByRequirements(model.Category);

            List<Element> elementHeaders = new List<Element>();
            List<Element> elementIntros = new List<Element>();
            List<Element> elementSections = new List<Element>();
            List<Element> elementFooters = new List<Element>();

            //Number of sections
            int numberOfSections = 1;
            if (model.NumberOfSections > 0)
            {
                numberOfSections = (int)model.NumberOfSections;
            } 

            Random randomNumber = new Random();
            TemplateExampleViewModel templateExample = new TemplateExampleViewModel();

            //Classify header sections
            _elementFunctions.ClassifySections(elements, elementHeaders, templateExample, ElementType.Header);

            //Choose header
            if (elementHeaders.Count() > 0)
            {
                templateExample.Header = _elementFunctions.CreatePartOfTemplate(elementHeaders, randomNumber.Next(elementHeaders.Count()));
            }


            //Classify intro sections
            _elementFunctions.ClassifySections(elements, elementIntros, templateExample, ElementType.Intro);

            //Choose intro
            if (elementIntros.Count() > 0)
            {
                templateExample.Intro = _elementFunctions.CreatePartOfTemplate(elementIntros, randomNumber.Next(elementIntros.Count()));
            }


            //Classify sections
            _elementFunctions.ClassifySections(elements, elementSections, templateExample, ElementType.Section);

            //Choose sections
            if (elementSections.Count() > 0)
            {
                templateExample.Sections = new List<Element>();
                while (numberOfSections > 0)
                {
                    Element section = _elementFunctions.CreatePartOfTemplate(elementSections, randomNumber.Next(elementSections.Count()));

                    //Check if section with same description already exists in list              
                    if(!_elementFunctions.IsDuplication(section, templateExample))
                    {
                        templateExample.Sections.Add(section);
                        numberOfSections--;
                    }
                };

                //Re-order positions of sections
                templateExample.Sections = templateExample.Sections.OrderBy(x => x.SectionPriority).ToList();
            }

            //Classify footer sections
            _elementFunctions.ClassifySections(elements, elementFooters, templateExample, ElementType.Footer);

            //Create footer
            if (elementFooters.Count() > 0)
            {
                templateExample.Footer = _elementFunctions.CreatePartOfTemplate(elementFooters, randomNumber.Next(elementFooters.Count()));
            }


            string globalColor = "black";
            string globalFont = "Poppins";

            if (model.Color != null)
            {
                globalColor = model.Color;
            }
            if (model.Font != null)
            {
                globalFont = ((Data.Enum.Font)Int32.Parse(model.Font)).ToString();
            }

            ViewBag.ImportFont = globalFont;
            ViewBag.GlobalStyle = ":root {--primary-color: " + globalColor + "; --primary-font: " + globalFont + " ;}";
            


            //// Get HTML to test
            //string stringToSeparate = templateExample.Header.Html;
            //// Make separator
            //string[] separator = { "class=\"" };

            //// Split HTML into separate strings
            //string[] stringParts = stringToSeparate.Split(separator,
            //   StringSplitOptions.RemoveEmptyEntries);

            //string[] separator = { "class=\"" };
            //string[] stringParts = _elementFunctions.SplitThisString(templateExample.Header.Html, separator);

            //ViewBag.Lista = stringParts;

            //// String to work with further
            //string newHtml = templateExample.Header.Html;

            //// Loop through separate strings
            //foreach(string stringPart in stringParts)
            //{
            //    // Get index of closing tab position
            //    int indexOfClosingTab = stringPart.IndexOf("\">");

            //    // If there is closing tab inside string
            //    if(indexOfClosingTab >= 0)
            //    {

            //        // Remove the rest of string from position of closing tab
            //        string listOfClasses = stringPart.Remove(indexOfClosingTab);

            //        //// Make separator to divide class names
            //        //string[] spaceSeparator = { " " }; 

            //        //// Split class names into separate strings
            //        //string[] strClassName = listOfClasses.Split(spaceSeparator,
            //        //   StringSplitOptions.RemoveEmptyEntries);

            //        // Make separator to divide class names
            //        //string[] spaceSeparator = { " " };
            //        //string[] classes = _elementFunctions.SplitThisString(listOfClasses, spaceSeparator);

            //        ////ViewBag.KK = strClassName;

            //        //if (classes.Count() > 1)
            //        //{
            //        //    string allNamesReplaced = "";
            //        //    string allNames = "";

            //        //    foreach (string className in classes)
            //        //    {
            //        //        allNames = allNames + className + " ";
            //        //        // Add random sufix at the end of class name
            //        //        string replacedString = className.Replace(className, className + "--randomBroj");

            //        //        allNamesReplaced = allNamesReplaced + replacedString + " ";

            //        //    }
            //        //    allNames = allNames.Remove(allNames.Length - 1, 1);
            //        //    allNamesReplaced = allNamesReplaced.Remove(allNamesReplaced.Length - 1, 1);
            //        //    // Logic to replace all class names in HTML
            //        //    string classTag = "class=\"" + allNames;
            //        //    string classTagToReplace = "class=\"" + allNamesReplaced;

            //        //    newHtml = newHtml.Replace(classTag, classTagToReplace);
            //        //}
            //        //else if(classes.Count() <= 1)
            //        //{

            //        //    // Add random sufix at the end of class name
            //        //    string replacedString = listOfClasses.Replace(listOfClasses, listOfClasses + "--randomBroj");

            //        //    // Logic to replace all class names in HTML
            //        //    string classTag = "class=\"" + listOfClasses;
            //        //    string classTagToReplace = "class=\"" + replacedString;

            //        //    newHtml = newHtml.Replace(classTag, classTagToReplace);

            //        //}

            //        newHtml = _elementFunctions.AddSufixOnClassName(listOfClasses, newHtml, "sufix");
            //    }
            //}

            //ViewBag.NewHtml = _elementFunctions.ModifyHtmlClasses(templateExample.Header.Html);
            //ViewBag.Strings = stringParts;

            return View(templateExample);
        }
    }
}