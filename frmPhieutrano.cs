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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace QuanLyNGK
{
    public partial class frmPhieutrano : Form
    {
        public frmPhieutrano()
        {
            InitializeComponent();
        }

        private void guna2GradientButton1_Click(object sender, EventArgs e)
        {
            
        }


        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string query = "SELECT * FROM KhachHang WHERE noTien > 0";
            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    // Mở kết nối và thực hiện truy vấn
                    connection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    StringBuilder sb = new StringBuilder();
                    while (reader.Read())
                    {
                        sb.AppendLine("Mã khách hàng : " + reader["maKH"].ToString());
                        sb.AppendLine("Tên khách hàng : " + reader["tenKH"].ToString());
                        sb.AppendLine("Số điện thoại : " + reader["sdt"].ToString());
                        sb.AppendLine("Địa chỉ : " + reader["diaChi"].ToString());
                        sb.AppendLine("Nợ: " + reader["noTien"].ToString());
                        sb.AppendLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                        //Thêm các thông tin khác nếu cần
                    }
                    MessageBox.Show(sb.ToString(), "Thông tin khách hàng nợ tiền!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            frmHenAdd frmHenAdd = new frmHenAdd();
            frmHenAdd.ShowDialog();
            frmPhieutrano_Load(this, null);
        }

        private void frmPhieutrano_Load(object sender, EventArgs e)
        {
            string connectionString = @"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Mở kết nối
                connection.Open();

                // Truy vấn dữ liệu từ bảng HoadonBanHang
                string query = "SELECT * FROM PhieuHen";
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
                // Lấy mã phiếu hẹn từ DataGridView
                string maPH = guna2DataGridView2.Rows[e.RowIndex].Cells["MaPH"].Value.ToString();
                string maHD = guna2DataGridView2.Rows[e.RowIndex].Cells["MaHD"].Value.ToString();
                string maKH = guna2DataGridView2.Rows[e.RowIndex].Cells["maKH"].Value.ToString();
                decimal tongtien = guna2DataGridView2.Rows[e.RowIndex].Cells["tongtien"].Value != null ? Convert.ToDecimal(guna2DataGridView2.Rows[e.RowIndex].Cells["tongtien"].Value) : 0;


                // Hiển thị thông báo xác nhận
                DialogResult result = MessageBox.Show("Khách hàng đã trả nợ?", "Xác nhận", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Chuyển trạng thái PhieuHen thành đã thanh toán
                    UpdatePhieuHenStatus(maPH, "Đã thanh toán");

                    // Chuyển trạng thái trong bảng HoaDonBanHang thành đã thanh toán
                    string query = "UPDATE HoaDonBanHang SET TrangThai = @status WHERE maHD = @maHD";

                    using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
                    {
                        using (SqlCommand command = new SqlCommand(query, connection))
                        {
                            command.Parameters.AddWithValue("@maHD", maHD);
                            command.Parameters.AddWithValue("@status", "Đã thanh toán");

                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }
                    string queryy = "UPDATE KhachHang SET noTien = noTien - @amount WHERE maKH = @maKH";

                    using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
                    {
                        using (SqlCommand command = new SqlCommand(queryy, connection))
                        {
                            command.Parameters.AddWithValue("@maKH", maKH);
                            command.Parameters.AddWithValue("@amount", tongtien);

                            connection.Open();
                            command.ExecuteNonQuery();
                        }
                    }

                    MessageBox.Show("Đã cập nhật thành công.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (result == DialogResult.Cancel)
                {
                    // Huỷ bỏ thao tác
                }
            }
        }
        public void UpdatePhieuHenStatus(string maPH, string status)
        {
            string query = "UPDATE PhieuHen SET trangThai = @status WHERE maPH = @maPH";

            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@maPH", maPH);
                    command.Parameters.AddWithValue("@status", status);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            frmPhieutrahang f = new frmPhieutrahang();
            panelBill.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            panelBill.Controls.Add(f);
            f.Show();
        }
    }
}
