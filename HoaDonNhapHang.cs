namespace QuanLyNGK
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoaDonNhapHang")]
    public partial class HoaDonNhapHang
    {
        [Key]
        public int maHDNH { get; set; }

        public DateTime ngayLapHDNH { get; set; }

        [Required]
        [StringLength(20)]
        public string maNCC { get; set; }

        [Required]
        [StringLength(20)]
        public string maSP { get; set; }

        [Required]
        [StringLength(20)]
        public string tenSP { get; set; }

        public int soLuong { get; set; }

        public int donGia { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int? tongTien { get; set; }

        [StringLength(255)]
        public string TrangThai { get; set; }

        public virtual NhaCungUng NhaCungUng { get; set; }
    }
}
