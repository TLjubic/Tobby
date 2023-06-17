using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Tobby.Data.Enum;
using Tobby.Models;
using Tobby.Models.ViewModels;
using Tobby.Service.Interfaces;
using static System.Collections.Specialized.BitVector32;

namespace Tobby.Service.Repository
{
    public class ElementFunctions : IElementFunctions
    {
        public Element CreatePartOfTemplate(List<Element> elements, int num)
        {
            return new Element()
            {
                ID = elements[num].ID,
                Title = elements[num].Title,
                Html = elements[num].Html,
                Css = elements[num].Css,
                FloatingElement = elements[num].FloatingElement,
                Color = elements[num].Color,
                ShadowProperty = elements[num].ShadowProperty,
                ElementType = elements[num].ElementType,
                Category = elements[num].Category,
                Theme = elements[num].Theme,
                ShapeDesign = elements[num].ShapeDesign,
                SectionDescription = elements[num].SectionDescription,
                SectionPriority = elements[num].SectionPriority
            };
        }

        public void ClassifySections(IEnumerable<Element> categoryElements, List<Element> sectionElements, TemplateExampleViewModel templateExample, ElementType type)
        {
            foreach (Element item in categoryElements)
            {
                if(item.ElementType == ElementType.Intro)
                {
                    if ((item.ElementType == type) && (item.FloatingElement == templateExample.Header.FloatingElement))
                    {
                        sectionElements.Add(item);
                    }
                }
                else if ((item.ElementType == type))
                {
                    sectionElements.Add(item);
                }


                /*
                if ((item.ElementType == type))
                {
                    sectionElements.Add(item);
                }
                else if ((item.ElementType == type) && (item.FloatingElement == templateExample.Header.FloatingElement))
                {
                    sectionElements.Add(item);
                }
                else if ((item.ElementType == type))
                {
                    sectionElements.Add(item);
                }
                else if ((item.ElementType == type))
                {
                    sectionElements.Add(item);
                }
                */
            }
        }

        public bool IsDuplication(Element section, TemplateExampleViewModel templateExample)
        {
            for (var i = 0; i < templateExample.Sections.Count(); i++)
            {
                if (section.SectionDescription == templateExample.Sections[i].SectionDescription)
                {
                    // Duplication founded
                    return true;
                }
            }
            return false;    
        }

        public void ModifyHtmlClasses(Element element)
        {
            string[] separator = { "class=\"" };
            string[] stringParts = SplitThisString(element.Html, separator);

            // String to work with further
            string newHtml = element.Html;

            // Loop through separate strings
            foreach (string stringPart in stringParts)
            {
                // Get index of closing tab position
                int indexOfClosingTab = stringPart.IndexOf("\">");

                // If there is closing tab inside string
                if (indexOfClosingTab >= 0)
                {

                    // Remove the rest of string from position of closing tab
                    string listOfClasses = stringPart.Remove(indexOfClosingTab);

                    //// Make separator to divide class names
                    //string[] spaceSeparator = { " " }; 

                    //// Split class names into separate strings
                    //string[] strClassName = listOfClasses.Split(spaceSeparator,
                    //   StringSplitOptions.RemoveEmptyEntries);

                    // Make separator to divide class names
                    //string[] spaceSeparator = { " " };
                    //string[] classes = _elementFunctions.SplitThisString(listOfClasses, spaceSeparator);

                    ////ViewBag.KK = strClassName;

                    //if (classes.Count() > 1)
                    //{
                    //    string allNamesReplaced = "";
                    //    string allNames = "";

                    //    foreach (string className in classes)
                    //    {
                    //        allNames = allNames + className + " ";
                    //        // Add random sufix at the end of class name
                    //        string replacedString = className.Replace(className, className + "--randomBroj");

                    //        allNamesReplaced = allNamesReplaced + replacedString + " ";

                    //    }
                    //    allNames = allNames.Remove(allNames.Length - 1, 1);
                    //    allNamesReplaced = allNamesReplaced.Remove(allNamesReplaced.Length - 1, 1);
                    //    // Logic to replace all class names in HTML
                    //    string classTag = "class=\"" + allNames;
                    //    string classTagToReplace = "class=\"" + allNamesReplaced;

                    //    newHtml = newHtml.Replace(classTag, classTagToReplace);
                    //}
                    //else if(classes.Count() <= 1)
                    //{

                    //    // Add random sufix at the end of class name
                    //    string replacedString = listOfClasses.Replace(listOfClasses, listOfClasses + "--randomBroj");

                    //    // Logic to replace all class names in HTML
                    //    string classTag = "class=\"" + listOfClasses;
                    //    string classTagToReplace = "class=\"" + replacedString;

                    //    newHtml = newHtml.Replace(classTag, classTagToReplace);

                    //}


                    // Generate sufix of 8 characters
                    var rand = new Random();
                    var sufix = "--";

                    for (int i=0; i<8; i++)
                    {
                        sufix += rand.Next(10).ToString();
                    }

                    AddSufixOnClassName(listOfClasses, newHtml, sufix, element);
                }
            }

            //return element;
        }

        public string[] SplitThisString(string rawString, string[] separator)
        {
            // Return HTML into separate strings
            return rawString.Split(separator,
               StringSplitOptions.RemoveEmptyEntries);
        }

        public void AddSufixOnClassName(string listOfClasses, string html1, string sufix, Element element)
        {
            // Make separator to divide class names
            string[] spaceSeparator = { " " };
            string[] classNames = SplitThisString(listOfClasses, spaceSeparator);

            // If there are more than one class
            if (classNames.Count() > 1)
            {
                string allNamesReplaced = "";
                string allNames = "";

                foreach (string className in classNames)
                {
                    allNames = allNames + className + " ";

                    // Add random sufix at the end of class name
                    string replacedString = "";

                    if(className.Equals("loadImg"))
                    {
                        replacedString = className;
                    }
                    else
                    {
                        replacedString = className.Replace(className, className + sufix);
                    }
                    allNamesReplaced = allNamesReplaced + replacedString + " ";

                    // Replace all class names in CSS
                    element.Css = Regex.Replace(element.Css, @"\." + className + " ", "." + replacedString + " ");
                    element.Css = Regex.Replace(element.Css, @"\." + className + ":", "." + replacedString + ":");
                    element.Css = Regex.Replace(element.Css, @"\." + className + "{", "." + replacedString + "{");

                }
                allNames = allNames.Remove(allNames.Length - 1, 1);
                allNamesReplaced = allNamesReplaced.Remove(allNamesReplaced.Length - 1, 1);
                
                // Replace all class names in HTML
                element.Html = RenameClassNamesInHtml(allNames, element.Html, allNamesReplaced);
                
            }
            else if (classNames.Count() <= 1)
            {
                string replacedString = "";
                // Add random sufix at the end of class name
                if(listOfClasses.Equals("loadImg"))
                {
                    replacedString = listOfClasses;
                }
                else
                {
                    replacedString = listOfClasses.Replace(listOfClasses, listOfClasses + sufix);

                    // Replace all class names in HTML
                    element.Html = RenameClassNamesInHtml(listOfClasses, element.Html, replacedString);
                }
                

                //string newstr = Regex.Replace(".apple .applefruitsapple|turnipapple .apple .apple-ap", @"\b.apple\b", " .mango ");

                //string newstr = Regex.Replace(".apple .applefruitsapple|turnipapple .apple .apple-ap", @"\.apple ", ".mango ");

                //element.Css = Regex.Replace(element.Css, @"\.header ", "." + replacedString + " ");

                // Replace all class names in CSS
                element.Css = Regex.Replace(element.Css, @"\." + listOfClasses + " ", "." + replacedString + " ");
                element.Css = Regex.Replace(element.Css, @"\." + listOfClasses + ":", "." + replacedString + ":");
                element.Css = Regex.Replace(element.Css, @"\." + listOfClasses + "{", "." + replacedString + "{");

            }

            //return html;

        }


        public string RenameClassNamesInHtml(string className, string html, string replacedString)
        {
            string classTag = "class=\"" + className;
            string classTagToReplace = "class=\"" + replacedString;

            return Regex.Replace(html, @"\""" + className + "\"", "\"" + replacedString + "\"");

            //return html.Replace(classTag, classTagToReplace);
        }
    }
}
