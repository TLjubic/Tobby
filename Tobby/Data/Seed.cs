using Tobby.Models;

namespace Tobby.Data
{
    public class Seed
    {
        public static void SeedData(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<TobbyDbContext>();

                context.Database.EnsureCreated();

                if(!context.Element.Any())
                {
                    context.Element.AddRange(new List<Element>()
                    {
                        new Element()
                        {
                            Title = "Test 1",
                            Color = "#111111",
                            Category = Enum.Category.Photography
                        },
                        new Element()
                        {
                            Title = "Test 2",
                            Color = "#222222",
                            Category = Enum.Category.Photography
                        }
                    });

                    context.SaveChanges();
                }
            }
        }
    }
}
