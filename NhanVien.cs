namespace QuanLyNGK
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhanVien")]
    public partial class NhanVien
    {
        [Key]
        [StringLength(20)]
        public string maNV { get; set; }

        [Required]
        [StringLength(50)]
        public string tenNV { get; set; }

        public int tuoi { get; set; }

        [Required]
        [StringLength(10)]
        public string gioiTinh { get; set; }

        [Required]
        [StringLength(20)]
        public string sdt { get; set; }

        [Required]
        [StringLength(50)]
        public string diaChi { get; set; }
    }
}
