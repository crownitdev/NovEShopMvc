using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace NovEShop.Data
{
    //public class NovEShopContextDesignTime : IDesignTimeDbContextFactory<NovEShopDbContext>
    //{
    //    public NovEShopDbContext CreateDbContext(string[] args)
    //    {
    //        var configuration = new ConfigurationBuilder()
    //            .SetBasePath(Directory.GetCurrentDirectory())
    //            .AddJsonFile("appsettings.json")
    //            .Build();

    //        DbContextOptionsBuilder<NovEShopDbContext> options = new DbContextOptionsBuilder<NovEShopDbContext>();
    //        options.UseSqlServer(configuration.GetConnectionString("NovEShop"));

    //        return new NovEShopDbContext(options.Options);
    //    }
    //}
}
