using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace QuanLyNGK
{
    public partial class frmRPproduct : Form
    {
        public frmRPproduct()
        {
            InitializeComponent();
        }

        private void frmRPproduct_Load(object sender, EventArgs e)
        {
            dataProduct dataProduct = new dataProduct();
            List<SanPham> listSP = dataProduct.SanPhams.ToList();
            List<Product> listDT = new List<Product>();

            foreach (SanPham sanPham in listSP)
            {
                Product temp = new Product
                (
                    sanPham.maSP,
                    sanPham.tenSP,
                    sanPham.ngaySX.ToString("dd/MM/yyyy"),
                    sanPham.ngayHH.ToString("dd/MM/yyyy"),
                    sanPham.giaSP,
                    sanPham.maNCC,
                    sanPham.hinhanh,
                    sanPham.soLuong
                );
                listDT.Add(temp);
            }

            reportViewer1.LocalReport.ReportPath = "rptSanPham.rdlc";
            var source = new ReportDataSource("ProductDataSet", listDT);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(source);

            this.reportViewer1.RefreshReport();
        }

        private void guna2PictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }
    }
}
