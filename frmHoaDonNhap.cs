using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNGK
{
    public partial class frmHoaDonNhap : Form
    {
        public frmHoaDonNhap()
        {
            InitializeComponent();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            frmNhapAdd frmNhapAdd = new frmNhapAdd();
            frmNhapAdd.ShowDialog();
            frmHoaDonNhap_Load(this, null);
        }

        private void panelBill_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmHoaDonNhap_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Mở kết nối
                connection.Open();

                // Truy vấn dữ liệu từ bảng HoadonBanHang
                string query = "SELECT * FROM HoaDonNhapHang";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Đổ dữ liệu vào DataTable
                    DataTable dataTable = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    adapter.Fill(dataTable);

                    // Gán DataTable làm nguồn dữ liệu cho DataGridView
                    guna2DataGridView2.DataSource = dataTable;
                }
            }
        }

        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == guna2DataGridView2.Columns["edit"].Index && e.RowIndex >= 0)
            {
                DialogResult result = MessageBox.Show("Đã nhập hàng thành công. Bạn có muốn lưu không?", "Xác nhận", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (result == DialogResult.OK)
                {
                    // Lấy thông tin từ DataGridView
                    int maHDNH = Convert.ToInt32(guna2DataGridView2.Rows[e.RowIndex].Cells["maHDNH"].Value);
                    string maNCC = guna2DataGridView2.Rows[e.RowIndex].Cells["maNCC"].Value.ToString();
                    string maSP = guna2DataGridView2.Rows[e.RowIndex].Cells["maSP"].Value.ToString();
                    string tenSP = guna2DataGridView2.Rows[e.RowIndex].Cells["tenSP"].Value.ToString();
                    int soLuong = Convert.ToInt32(guna2DataGridView2.Rows[e.RowIndex].Cells["soLuong"].Value);
                    int donGia = Convert.ToInt32(guna2DataGridView2.Rows[e.RowIndex].Cells["donGia"].Value);

                    // Thực hiện thêm bản ghi vào bảng sản phẩm và cập nhật trạng thái hóa đơn nhập hàng
                    AddProductAndUpdateStatus(maHDNH, maNCC, maSP, tenSP, soLuong, donGia);
                }
            }
        }
        private void AddProductAndUpdateStatus(int maHDNH, string maNCC, string maSP, string tenSP, int soLuong, int donGia)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
                {
                    connection.Open();

                    // Kiểm tra xem mặt hàng đã tồn tại trong kho chưa
                    string checkProductQuery = "SELECT COUNT(*) FROM SanPham WHERE maSP = @maSP";
                    using (SqlCommand checkProductCommand = new SqlCommand(checkProductQuery, connection))
                    {
                        checkProductCommand.Parameters.AddWithValue("@maSP", maSP);
                        int existingProductCount = (int)checkProductCommand.ExecuteScalar();

                        if (existingProductCount > 0) // Nếu mặt hàng đã tồn tại trong kho
                        {
                            // Cập nhật số lượng của mặt hàng
                            string updateQuantityQuery = "UPDATE SanPham SET soLuong = soLuong + @soLuong WHERE maSP = @maSP";
                            using (SqlCommand updateQuantityCommand = new SqlCommand(updateQuantityQuery, connection))
                            {
                                updateQuantityCommand.Parameters.AddWithValue("@soLuong", soLuong);
                                updateQuantityCommand.Parameters.AddWithValue("@maSP", maSP);
                                updateQuantityCommand.ExecuteNonQuery();
                            }
                        }
                        else // Nếu mặt hàng chưa tồn tại trong kho
                        {
                            // Thêm sản phẩm mới vào kho
                            string insertProductQuery = "INSERT INTO SanPham (maSP, tenSP, maNCC, soLuong, ngaySX, ngayHH, giaSP) VALUES (@maSP, @tenSP, @maNCC, @soLuong, @ngaySX, @ngayHH, @giaSP)";
                            using (SqlCommand insertProductCommand = new SqlCommand(insertProductQuery, connection))
                            {
                                insertProductCommand.Parameters.AddWithValue("@maSP", maSP);
                                insertProductCommand.Parameters.AddWithValue("@tenSP", tenSP);
                                insertProductCommand.Parameters.AddWithValue("@maNCC", maNCC);
                                insertProductCommand.Parameters.AddWithValue("@soLuong", soLuong);
                                insertProductCommand.Parameters.AddWithValue("@ngaySX", DateTime.Now); // Giả sử ngày sản xuất là ngày hiện tại
                                insertProductCommand.Parameters.AddWithValue("@ngayHH", DateTime.Now.AddYears(1)); // Giả sử ngày hết hạn là 1 năm sau
                                insertProductCommand.Parameters.AddWithValue("@giaSP", donGia);
                                insertProductCommand.ExecuteNonQuery();
                            }
                        }
                    }

                    // Cập nhật trạng thái hóa đơn nhập hàng
                    string updateStatusQuery = "UPDATE HoaDonNhapHang SET TrangThai = @TrangThai WHERE maHDNH = @maHDNH";
                    using (SqlCommand updateStatusCommand = new SqlCommand(updateStatusQuery, connection))
                    {
                        updateStatusCommand.Parameters.AddWithValue("@TrangThai", "Đã thành công");
                        updateStatusCommand.Parameters.AddWithValue("@maHDNH", maHDNH);
                        updateStatusCommand.ExecuteNonQuery();
                    }

                    MessageBox.Show("Nhập hàng và cập nhật trạng thái thành công.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            frmHoaDonban f = new frmHoaDonban();
            panelBill.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            panelBill.Controls.Add(f);
            f.Show();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
