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
    public partial class frmNhapAdd : Form
    {
        public frmNhapAdd()
        {
            InitializeComponent();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            try
            {
                // Lấy thông tin từ các textbox
                string maNCC = txtMaNCC.Text;
                string maSP = txtMaSP.Text;
                string tenSP = txtTenSP.Text;
                int giaSP = int.Parse(txtGia.Text);
                int soLuong = int.Parse(txtSoluong.Text);
                DateTime ngayLapHDNH = DateTime.Now;
                string trangThai = "Mới";

                // Tính tổng tiền
                int tongTien = giaSP * soLuong;
                
                // Tạo kết nối đến cơ sở dữ liệu
                using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
                {
                    connection.Open();

                    // Tạo lệnh chèn dữ liệu
                    string query = "INSERT INTO HoaDonNhapHang (ngayLapHDNH, maNCC, maSP, tenSP, soLuong, donGia, TrangThai) " +
                                   "VALUES (@ngayLapHDNH, @maNCC, @maSP, @tenSP, @soLuong, @donGia, @TrangThai)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm tham số vào lệnh
                        command.Parameters.AddWithValue("@ngayLapHDNH", ngayLapHDNH);
                        command.Parameters.AddWithValue("@maNCC", maNCC);
                        command.Parameters.AddWithValue("@maSP", maSP);
                        command.Parameters.AddWithValue("@tenSP", tenSP);
                        command.Parameters.AddWithValue("@soLuong", soLuong);
                        command.Parameters.AddWithValue("@donGia", giaSP);
                        command.Parameters.AddWithValue("@TrangThai", trangThai);

                        // Thực thi lệnh
                        command.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Lưu thông tin thành công!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
            }
        }
        private bool CheckMaSPExist(string maSP)
        {
            // Kiểm tra xem mã sản phẩm có tồn tại trong kho không
            string query = "SELECT COUNT(*) FROM SanPham WHERE maSP = @MaSP";
            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@MaSP", maSP);

                try
                {
                    connection.Open();
                    int count = (int)command.ExecuteScalar();
                    return count > 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                    return false;
                }
            }
        }

        private void txtMaSP_TextChanged(object sender, EventArgs e)
        {
            string maSP = txtMaSP.Text;
            string tenSP = "";
            string giaSP = "";

            // Kiểm tra xem mã sản phẩm đã tồn tại trong kho chưa
            if (CheckMaSPExist(maSP))
            {
                // Nếu tồn tại, lấy thông tin sản phẩm từ cơ sở dữ liệu
                string query = "SELECT tenSP, giaSP FROM SanPham WHERE maSP = @MaSP";
                using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@MaSP", maSP);

                    try
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        if (reader.Read())
                        {
                            // Lấy thông tin sản phẩm
                            tenSP = reader["tenSP"].ToString();
                            giaSP = reader["giaSP"].ToString();
                        }
                        reader.Close();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                    }
                }

                // Hiển thị thông tin sản phẩm lên các TextBox
                txtTenSP.Text = tenSP;
                txtGia.Text = giaSP;
            }
            else
            {
                // Nếu không tồn tại, xóa thông tin đã hiển thị trên các TextBox
                txtTenSP.Text = "";
                txtGia.Text = "";
            }
        }

    
}
}
