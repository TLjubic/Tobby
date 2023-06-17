using Microsoft.Build.Evaluation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection.Metadata.Ecma335;
using System.Security.Principal;
using Tobby.Data.Enum;

namespace Tobby.Models
{
    public class Element
    {
        [Key]
        public int ID { get; set; }
        public string? Title { get; set; } = default!;
        public string? Html { get; set; } = default!;
        public string? Css { get; set; } = default!;
        public bool FloatingElement { get; set; } = default!;
        public string? Color { get; set; } = default!;
        public bool ShadowProperty { get; set; } = default!;
        public int SectionPriority { get; set; }

        public ElementType ElementType { get; set; } = default!;
        public Category Category { get; set; } = default!;
        public Theme Theme { get; set; } = default!;
        public ShapeDesign ShapeDesign { get; set; } = default!;
        public SectionDescription SectionDescription { get; set; }
    }
}
