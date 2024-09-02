using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace QuanLyNGK
{
    public partial class dataDoanhthu : DbContext
    {
        public dataDoanhthu()
            : base("name=dataDoanhthu1")
        {
        }

        public virtual DbSet<HoaDonBanHang> HoaDonBanHangs { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<HoaDonBanHang>()
                .Property(e => e.maHD)
                .IsUnicode(false);

            modelBuilder.Entity<HoaDonBanHang>()
                .Property(e => e.TongTien)
                .HasPrecision(10, 2);

            modelBuilder.Entity<HoaDonBanHang>()
                .Property(e => e.maKH)
                .IsUnicode(false);
        }
    }
}
