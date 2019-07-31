using Core3.Persistence.Inftrastructure;
using Microsoft.EntityFrameworkCore;

namespace Core3.Persistence
{
    public class Core3DbContextFactory : DesignTimeDbContextFactoryBase<Core3DbContext>
    {
        protected override Core3DbContext CreateNewInstance(DbContextOptions<Core3DbContext> options)
        {
            return new Core3DbContext(options);
        }
    }
}
