using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;


namespace Production_Controll
{
    public class MyDbContext : DbContext
    {

        public MyDbContext() : base("myConnectionString") 
        {
            this.Database.Connection.ConnectionString = "Server=localhost;port=3306;Database=production_control;Uid=root;Pwd=garomysql1852;";
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<Modification> Modifications { get; set; }
    }
}
