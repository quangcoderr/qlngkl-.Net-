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
    public partial class frmHenAdd : Form
    {
        public frmHenAdd()
        {
            InitializeComponent();
            cbMKH.DropDown += new EventHandler(guna2ComboBox1_DropDown);
            cbHD.DropDown += new EventHandler(cbHD_DropDown);
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }


        private void guna2ComboBox1_DropDown(object sender, EventArgs e)
        {
            
                
                string query = "SELECT maKH FROM KhachHang WHERE noTien > 0";

                using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dataTable = new DataTable();

                    try
                    {
                        connection.Open();
                        adapter.Fill(dataTable);

                        // Clear the ComboBox before adding new items
                        cbMKH.Items.Clear();

                        // Loop through the DataTable and add items to the ComboBox
                        foreach (DataRow row in dataTable.Rows)
                        {
                            cbMKH.Items.Add(row["maKH"].ToString());
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
            string selectedMaKH = cbMKH.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedMaKH))
            {
                MessageBox.Show("Vui lòng chọn một mã khách hàng trước.");
                return;
            }

         
            string query = "SELECT maHD FROM HoaDonBanHang WHERE maKH = @maKH";

            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@maKH", selectedMaKH);
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dataTable = new DataTable();

                try
                {
                    connection.Open();
                    adapter.Fill(dataTable);

                    // Clear the ComboBox before adding new items
                    cbHD.Items.Clear();

                    // Loop through the DataTable and add items to the ComboBox
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

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            
                // Kiểm tra xem tất cả các trường cần thiết đã được chọn hay chưa
                if (cbMKH.SelectedItem == null || cbHD.SelectedItem == null || string.IsNullOrWhiteSpace(txtPH.Text) || DTHen.Value == null)
                {
                    MessageBox.Show("Vui lòng điền đầy đủ thông tin.");
                    return;
                }

                string maPH = txtPH.Text;
                string maHD = cbHD.SelectedItem.ToString();
                string maKH = cbMKH.SelectedItem.ToString();
                DateTime ngayLapPH = DateTime.Now; // Lấy thời gian hiện tại
                DateTime ngayHenTra = DTHen.Value; // Lấy giá trị từ DateTimePicker
            decimal tongTien = 0;
            string queryTongTien = "SELECT TongTien FROM HoaDonBanHang WHERE maHD = @maHD";
            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                using (SqlCommand commandTongTien = new SqlCommand(queryTongTien, connection))
                {
                    commandTongTien.Parameters.AddWithValue("@maHD", maHD);
                    try
                    {
                        connection.Open();
                        object result = commandTongTien.ExecuteScalar();
                        if (result != null && result != DBNull.Value)
                        {
                            tongTien = Convert.ToDecimal(result);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi khi lấy tổng tiền từ bảng HoaDonBanHang: " + ex.Message);
                        return;
                    }
                }
            }
            string query = "INSERT INTO PhieuHen (maPH, maHD, TongTien, maKH, ngaylapPH, ngayhenTra) VALUES (@maPH, @maHD, @TongTien, @maKH, @ngaylapPH, @ngayhenTra)";

                using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
                {
                    SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@maPH", maPH);
                    command.Parameters.AddWithValue("@maHD", maHD);
                    command.Parameters.AddWithValue("@TongTien", tongTien);
                    command.Parameters.AddWithValue("@maKH", maKH);
                    command.Parameters.AddWithValue("@ngaylapPH", ngayLapPH);
                    command.Parameters.AddWithValue("@ngayhenTra", ngayHenTra);

                    try
                    {
                        connection.Open();
                        int result = command.ExecuteNonQuery();
                        if (result > 0)
                        {
                            MessageBox.Show("Thêm phiếu hẹn thành công.");
                        }
                        else
                        {
                            MessageBox.Show("Thêm phiếu hẹn thất bại.");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                    }
                }
            

        }
    }
}
