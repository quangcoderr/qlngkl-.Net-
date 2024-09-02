using System;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Windows.Forms;

namespace QuanLyNGK
{
    public partial class frmHenTrahangA : Form
    {
        public frmHenTrahangA()
        {
            InitializeComponent();
            cbHD.DropDown += new EventHandler(cbHD_DropDown);
            cbHD.SelectedIndexChanged += new EventHandler(cbHD_SelectedIndexChanged); // Thêm sự kiện SelectedIndexChanged

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string maPH =txtPH.Text;
            string maHD = cbHD.SelectedItem.ToString(); // Lấy mã hóa đơn từ ComboBox

            // Lấy mã khách hàng từ thông tin hiển thị trên Label
            string[] lines = lblthongtin.Text.Split('\n');
            string maKH = lines[0].Replace("Mã Khách Hàng: ", "");

            // Lấy tổng tiền từ thông tin hiển thị trên Label
            string tongTien = lines[1].Replace("Tổng Tiền: ", "");

            // Lấy ngày hiện tại
            DateTime ngayLapPH = DateTime.Now;

            // Lấy ngày hẹn trả từ DateTimePicker
            DateTime ngayHenTra = DTHen.Value;

            // Chuẩn bị truy vấn SQL để thêm bản ghi vào bảng PhieuHenTraHang
            string query = "INSERT INTO PhieuHenTraHang (maPH, maHD, maKH, ngaylapPH, ngayhenTra) VALUES (@maPH, @MaHD, @MaKH, @NgayLapPH, @NgayHenTra)";

            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@maPH", maPH);
                command.Parameters.AddWithValue("@MaHD", maHD);
                command.Parameters.AddWithValue("@MaKH", maKH);
                command.Parameters.AddWithValue("@NgayLapPH", ngayLapPH);
                command.Parameters.AddWithValue("@NgayHenTra", ngayHenTra);

                try
                {
                    connection.Open();
                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        MessageBox.Show("Thêm bản ghi thành công vào bảng PhieuHenTraHang.");
                    }
                    else
                    {
                        MessageBox.Show("Thêm bản ghi không thành công.");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }
        }


        private void cbHD_DropDown(object sender, EventArgs e)
        {
            string query = "SELECT maHD FROM HoaDonBanHang WHERE TrangThai = N'Thiếu hàng'";

            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(dataTable);

                    // Xóa các mục trước khi thêm các mục mới vào ComboBox
                    cbHD.Items.Clear(); // Đã sửa chỗ này

                    // Lặp qua DataTable và thêm các mục vào ComboBox
                    foreach (DataRow row in dataTable.Rows)
                    {
                        cbHD.Items.Add(row["maHD"].ToString());
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }
        }

        private void cbHD_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void cbHD_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            string selectedHD = cbHD.SelectedItem.ToString();

            string query = "SELECT HoaDonBanHang.maKH, HoaDonBanHang.TongTien FROM HoaDonBanHang WHERE HoaDonBanHang.maHD = @MaHD";

            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MaHD", selectedHD);

                try
                {
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();

                    string thongTin = ""; // Chuỗi thông tin chi tiết hóa đơn
                    string maKH = ""; // Mã khách hàng
                    string tongTien = ""; // Tổng tiền của hóa đơn

                    // Đọc thông tin từ kết quả truy vấn
                    if (reader.Read())
                    {
                        maKH = reader["maKH"].ToString();
                        tongTien = reader["TongTien"].ToString();
                    }

                    reader.Close();

                    // Lấy chi tiết hóa đơn
                    query = "SELECT ChiTietHoaDon.maSP, SanPham.tenSP, ChiTietHoaDon.soLuong, ChiTietHoaDon.DonGia FROM ChiTietHoaDon INNER JOIN SanPham ON ChiTietHoaDon.maSP = SanPham.maSP WHERE ChiTietHoaDon.maHD = @MaHD";
                    command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaHD", selectedHD);

                    reader = command.ExecuteReader();

                    // Đọc từng dòng dữ liệu và thêm vào chuỗi thông tin
                    while (reader.Read())
                    {
                        thongTin += "Mã Sản Phẩm: " + reader["maSP"].ToString() + "\n" +
                                    "Tên Sản Phẩm: " + reader["tenSP"].ToString() + "\n" +
                                    "Số Lượng: " + reader["soLuong"].ToString() + "\n" +
                                    "Đơn Giá: " + reader["DonGia"].ToString() + "\n\n";
                    }

                    // Đóng kết nối và đóng đối tượng SqlDataReader
                    reader.Close();

                    // Cập nhật thông tin trong Label
                    lblthongtin.Text = "Mã Khách Hàng: " + maKH + "\n" +
                                       "Tổng Tiền: " + tongTien + "\n\n" +
                                       "Thông Tin Sản Phẩm:\n" + thongTin;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }
        }

        private void lblthongtin_Click(object sender, EventArgs e)
        {

        }
    }
}
