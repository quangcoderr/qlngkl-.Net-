namespace QuanLyNGK
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoaDonBanHang")]
    public partial class HoaDonBanHang
    {
        [Key]
        [StringLength(20)]
        public string maHD { get; set; }

        public DateTime ngayLapHD { get; set; }

        [Required]
        [StringLength(100)]
        public string tenNV { get; set; }

        public decimal TongTien { get; set; }

        [Required]
        [StringLength(255)]
        public string TrangThai { get; set; }

        [StringLength(100)]
        public string maKH { get; set; }
    }
}
