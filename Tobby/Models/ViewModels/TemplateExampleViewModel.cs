using Tobby.Data.Enum;

namespace Tobby.Models.ViewModels
{
    public class TemplateExampleViewModel
    {
        public Element Header { get; set; } = default!;
        public Element Intro { get; set; } = default!;
        public List<Element> Sections { get; set; } = default!;
        public Element Footer { get; set; } = default!; 
    }
}
