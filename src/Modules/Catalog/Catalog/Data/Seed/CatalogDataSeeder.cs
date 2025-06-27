namespace Catalog.Data.Seed
{
    public class CatalogDataSeeder(CatalogDbContext dbContext)
    {
        public async Task SeedAllAsync()
        {
            if (!await dbContext.Products.AnyAsync())
            {
                await dbContext.Products.AddRangeAsync(InitialData.Products);
                await dbContext.SaveChangesAsync();
            }
        }
    }
}
