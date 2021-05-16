using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Store.Lib.Entities;
using System.Data.Entity;
 

namespace Store.Lib.Concrete
{
   public  class EFDbContext : DbContext
    {
        public DbSet<Part> Parts { get; set; }
    }
}
