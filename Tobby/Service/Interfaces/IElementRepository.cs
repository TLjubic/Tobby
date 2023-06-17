using Tobby.Data.Enum;
using Tobby.Models;
using Tobby.Models.ViewModels;

namespace Tobby.Service.Interfaces
{
    public interface IElementRepository
    {
        Task<IEnumerable<Element>> GetAll();
        Task<Element> GetByIdAsync(int? id);
        Task<IEnumerable<Element>> GetSectionByRequirements(Category category);
        bool Add(Element element);
        bool Update(Element element);
        bool Delete(Element element);
        bool Save();
    }
}
