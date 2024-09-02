namespace QuanLyNGK
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("NhaCungUng")]
    public partial class NhaCungUng
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhaCungUng()
        {
            HoaDonNhapHangs = new HashSet<HoaDonNhapHang>();
            SanPhams = new HashSet<SanPham>();
        }
        public NhaCungUng(string maNCC, string tenNCC, string sdtNCC, string diaChi)
        {
            this.maNCC = maNCC;
            this.tenNCC = tenNCC;
            this.sdtNCC = sdtNCC;
            this.diaChi = diaChi;
        }

        [Key]
        [StringLength(20)]
        public string maNCC { get; set; }

        [Required]
        [StringLength(50)]
        public string tenNCC { get; set; }

        [Required]
        [StringLength(15)]
        public string sdtNCC { get; set; }

        [Required]
        [StringLength(100)]
        public string diaChi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDonNhapHang> HoaDonNhapHangs { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPham> SanPhams { get; set; }
    }
}
