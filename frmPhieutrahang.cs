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
    public partial class frmPhieutrahang : Form
    {
        public frmPhieutrahang()
        {
            InitializeComponent();
        }
        public void AddControls(Form f)
        {
            panelBill.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            panelBill.Controls.Add(f);
            f.Show();
        }
        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            AddControls(new frmPhieutrano());
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            frmHenTrahangA frmHenTrahangA = new frmHenTrahangA();
            frmHenTrahangA.ShowDialog();
            frmPhieutrahang_Load(this, null);

        }

        private void frmPhieutrahang_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Mở kết nối
                connection.Open();

                // Truy vấn dữ liệu từ bảng HoadonBanHang
                string query = "SELECT * FROM PhieuHenTraHang";
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
            if (e.ColumnIndex == guna2DataGridView2.Columns["Edit"].Index && e.RowIndex >= 0)
            {
                // Lấy mã hóa đơn từ cột maHD tương ứng với hàng đã chọn
                string maHD = guna2DataGridView2.Rows[e.RowIndex].Cells["MaHD"].Value.ToString();

                // Hiển thị hộp thoại cảnh báo và lấy kết quả
                DialogResult result = MessageBox.Show("Khách hàng đã nhận được hàng?", "Xác nhận", MessageBoxButtons.YesNoCancel);

                // Xử lý kết quả từ hộp thoại cảnh báo
                if (result == DialogResult.Yes)
                {
                    string queryrh = "UPDATE PhieuHenTraHang SET TrangThai = @TrangThai WHERE maHD = @MaHD";

                    using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
                    {
                        SqlCommand command = new SqlCommand(queryrh, connection);
                        command.Parameters.AddWithValue("@MaHD", maHD);
                        command.Parameters.AddWithValue("@TrangThai", "Hoàn tất giao hàng");

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Đã xảy ra lỗi khi cập nhật trạng thái hóa đơn: " + ex.Message);
                        }
                    }
                    // Cập nhật trạng thái hóa đơn thành "Hoàn tất giao hàng" trong cơ sở dữ liệu
                    string queryr = "UPDATE HoaDonBanHang SET TrangThai = @TrangThai WHERE maHD = @MaHD";

                    using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
                    {
                        SqlCommand command = new SqlCommand(queryr, connection);
                        command.Parameters.AddWithValue("@MaHD", maHD);
                        command.Parameters.AddWithValue("@TrangThai", "Hoàn tất giao hàng");

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Đã xảy ra lỗi khi cập nhật trạng thái hóa đơn: " + ex.Message);
                        }
                    }

                    // Giảm số lượng sản phẩm trong kho
                    string query = "UPDATE SanPham SET soLuong = soLuong - (SELECT soLuong FROM ChiTietHoaDon WHERE maHD = @MaHD AND SanPham.maSP = ChiTietHoaDon.maSP) WHERE maSP IN (SELECT maSP FROM ChiTietHoaDon WHERE maHD = @MaHD)";

                    using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
                    {
                        SqlCommand command = new SqlCommand(query, connection);
                        command.Parameters.AddWithValue("@MaHD", maHD);

                        try
                        {
                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Đã xảy ra lỗi khi giảm số lượng sản phẩm trong kho: " + ex.Message);
                        }
                    }
                    MessageBox.Show("Hoàn tất");
                }
                else if (result == DialogResult.Cancel)
                {
                    // Nếu người dùng chọn "Cancel", không làm gì cả hoặc đóng hộp thoại
                }
            }
            frmPhieutrahang_Load(this, null);
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                string query = "SELECT * FROM HoaDonBanHang WHERE TrangThai = N'Thiếu hàng'";
                using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Mở kết nối và thực hiện truy vấn
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        StringBuilder sb = new StringBuilder();
                        if (!reader.HasRows)
                        {
                            MessageBox.Show("Không có dữ liệu phù hợp.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        while (reader.Read())
                        {
                            sb.AppendLine("Mã khách hàng : " + reader["maKH"].ToString());
                            sb.AppendLine("Mã hóa đơn : " + reader["maHD"].ToString());
                            sb.AppendLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                            // Thêm các thông tin khác nếu cần
                        }
                        MessageBox.Show(sb.ToString(), "Thông tin khách hàng chưa nhận được hàng!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
