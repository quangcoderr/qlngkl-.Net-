namespace QuanLyNGK
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ChiTietHoaDon")]
    public partial class ChiTietHoaDon
    {
        [Key]
        public int maHDCT { get; set; }

        [Required]
        [StringLength(20)]
        public string maHD { get; set; }

        [Required]
        [StringLength(20)]
        public string maSP { get; set; }

        public int soLuong { get; set; }

        public decimal DonGia { get; set; }

        [Required]
        [StringLength(50)]
        public string tenSP { get; set; }

        public virtual HoaDonBanHang HoaDonBanHang { get; set; }

        public virtual SanPham SanPham { get; set; }
    }
}
