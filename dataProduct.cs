using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace QuanLyNGK
{
    public partial class dataProduct : DbContext
    {
        public dataProduct()
            : base("name=dataProduct")
        {
        }

        public virtual DbSet<SanPham> SanPhams { get; set; }
        
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
        }
    }
}
