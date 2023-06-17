using Microsoft.EntityFrameworkCore;
using Tobby.Data;
using Tobby.Data.Enum;
using Tobby.Models;
using Tobby.Models.ViewModels;
using Tobby.Service.Interfaces;

namespace Tobby.Service.Repository
{
    public class ElementRepository : IElementRepository
    {
        private readonly TobbyDbContext _context;
        public ElementRepository(TobbyDbContext context)
        {
            _context = context;
        }

        //Get all elements
        public async Task<IEnumerable<Element>> GetAll()
        {
            return await _context.Element.ToListAsync();
        }

        //Get element by ID
        public async Task<Element> GetByIdAsync(int? id)
        {
            return await _context.Element.FirstOrDefaultAsync(i => i.ID == id);
        }

        //Create new element
        public bool Add(Element element)
        {
            _context.Add(element);
            return Save();
        }

        //Remove element
        public bool Delete(Element element)
        {
            _context.Remove(element);
            return Save();
        }

        //Update element
        public bool Update(Element element)
        {
            _context.Update(element);
            return Save();
        }

        //Save changes
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public async Task<IEnumerable<Element>> GetSectionByRequirements(Category category)
        {
            return await _context.Element.Where(t => t.Category == category).ToListAsync();
        }
    }
}