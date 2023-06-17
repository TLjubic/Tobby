using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using Tobby.Data.Enum;

namespace Tobby.Models.ViewModels
{
    public class TemplateRequirementsViewModel
    {
        
        //public Element Element { get; set; } = default!;
        public Category Category { get; set; } = default!;
        
        [Display(Name = "Total sections")]
        public int? NumberOfSections { get; set; }
        public string? Color { get; set; }
        public string? Font { get; set; }
    }
}
