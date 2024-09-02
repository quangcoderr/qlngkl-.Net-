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
    public partial class frmRPdoanhthu : Form
    {
        public frmRPdoanhthu()
        {
            InitializeComponent();
        }

        private void frmRPdoanhthu_Load(object sender, EventArgs e)
        {
            // Thêm các tháng vào ComboBox
            for (int i = 1; i <= 12; i++)
            {
                cbThang.Items.Add($"Tháng {i}");
            }

            // Chọn tháng hiện tại là tháng mặc định
            cbThang.SelectedIndex = DateTime.Now.Month - 1;

            // Thống kê dữ liệu cho tháng hiện tại khi form được tải lần đầu tiên
            ThongKeDoanhThuTheoThang(DateTime.Now.Month);
            
        }
        private void ThongKeDoanhThuTheoThang(int selectedMonth)
        {
            // Lấy dữ liệu hóa đơn từ cơ sở dữ liệu cho tháng được chọn
            dataDoanhthu dataDoanhthu = new dataDoanhthu();
            int currentYear = DateTime.Now.Year;

            List<HoaDonBanHang> listHD = dataDoanhthu.HoaDonBanHangs
                .Where(hd => hd.ngayLapHD.Month == selectedMonth && hd.ngayLapHD.Year == currentYear)
                .ToList();


            // Tạo danh sách dữ liệu thống kê
            List<DoanhthuRP> listDT = listHD.Select(hd => new DoanhthuRP
            {
                ID = hd.maHD,
                Date = hd.ngayLapHD.ToString("dd/MM/yyyy"),
                Total = hd.TongTien,
            }).ToList();

            // Cập nhật dữ liệu lên ReportViewer

            reportViewer1.LocalReport.ReportPath = "rptDoanhthu.rdlc";
            var source = new ReportDataSource("DoanhthuDataset", listDT);
            reportViewer1.LocalReport.DataSources.Clear();
            reportViewer1.LocalReport.DataSources.Add(source);

            this.reportViewer1.RefreshReport();
        }
        private void reportViewer1_Load(object sender, EventArgs e)
        {

        }

        private void cbThang_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Lấy tháng được chọn từ ComboBox
            int selectedMonth = cbThang.SelectedIndex + 1;

            // Thực hiện thống kê dữ liệu cho tháng được chọn
            ThongKeDoanhThuTheoThang(selectedMonth);
        }
    }
}
