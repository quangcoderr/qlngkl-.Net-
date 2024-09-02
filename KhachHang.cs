namespace QuanLyNGK
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KhachHang")]
    public partial class KhachHang
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KhachHang()
        {
            PhieuHens = new HashSet<PhieuHen>();
            PhieuHenTraHangs = new HashSet<PhieuHenTraHang>();
        }

        [Key]
        [StringLength(20)]
        public string maKH { get; set; }

        [Required]
        [StringLength(20)]
        public string kieuKH { get; set; }

        [StringLength(50)]
        public string tenKH { get; set; }

        [StringLength(10)]
        public string gioiTinh { get; set; }

        [StringLength(20)]
        public string sdt { get; set; }

        [Required]
        [StringLength(50)]
        public string diaChi { get; set; }

        public decimal? noTien { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuHen> PhieuHens { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PhieuHenTraHang> PhieuHenTraHangs { get; set; }
    }
}
