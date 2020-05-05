using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.EfCode
{
    public class ContextFactory
    {
        private const string ConnectionString =
            "Data Source=MAJDINURHASAN-L\\SQLEXPRESS;Integrated Security=True;Database=JournalDB;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        public EfCoreContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<EfCoreContext>();
            optionsBuilder.UseSqlServer(ConnectionString, b => b.MigrationsAssembly("DataLayer"));

            return new EfCoreContext(optionsBuilder.Options);
        }

    }
}
