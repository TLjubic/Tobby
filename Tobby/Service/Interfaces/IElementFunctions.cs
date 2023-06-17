using Microsoft.AspNetCore.Razor.Language.Extensions;
using Tobby.Data.Enum;
using Tobby.Models;
using Tobby.Models.ViewModels;

namespace Tobby.Service.Interfaces
{
    public interface IElementFunctions
    {
        public Element CreatePartOfTemplate(List<Element> elements, int num);
        public void ClassifySections(IEnumerable<Element> categoryElements, List<Element> sectionElements, TemplateExampleViewModel templateExample, ElementType type);
        public bool IsDuplication(Element section, TemplateExampleViewModel templateExample);
        public void ModifyHtmlClasses(Element element);
        public string[] SplitThisString(string rawString, string[] separator);
        public void AddSufixOnClassName(string listOfClasses, string html1, string sufix, Element element);
        public string RenameClassNamesInHtml(string className, string html, string replacedString);
    }
}
