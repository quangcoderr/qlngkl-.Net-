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
    public partial class frmHoaDonban : Form
    {
        public frmHoaDonban()
        {
            InitializeComponent();
        }

        private void frmHoaDonban_Load(object sender, EventArgs e)
        {
            // Kết nối đến cơ sở dữ liệu
            string connectionString = @"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                // Mở kết nối
                connection.Open();

                // Truy vấn dữ liệu từ bảng HoadonBanHang
                string query = "SELECT * FROM HoadonBanHang";
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
        string connectionString = @"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True";
        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == guna2DataGridView2.Columns["info"].Index && e.RowIndex >= 0)
            {
                // Lấy giá trị của mã hóa đơn từ hàng tương ứng
                string maHD = guna2DataGridView2.Rows[e.RowIndex].Cells["maHD"].Value.ToString();

                // Truy vấn thông tin chi tiết hóa đơn từ cơ sở dữ liệu
                string query = "SELECT * FROM ChiTietHoaDon WHERE MaHD = @MaHD";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Thêm tham số cho truy vấn
                        command.Parameters.AddWithValue("@MaHD", maHD);

                        // Mở kết nối và thực hiện truy vấn
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        // Hiển thị thông tin chi tiết hóa đơn trong một cửa sổ mới hoặc control khác
                        // Ví dụ:
                        StringBuilder sb = new StringBuilder();
                        while (reader.Read())
                        {
                            sb.AppendLine("Mã sản phẩm: " + reader["MaSP"].ToString());
                            sb.AppendLine("Tên sản phẩm: " + reader["tenSP"].ToString());
                            sb.AppendLine("Số lượng: " + reader["SoLuong"].ToString());
                            sb.AppendLine("Đơn giá: " + reader["DonGia"].ToString());
                            sb.AppendLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                            //Thêm các thông tin khác nếu cần
                        }
                        MessageBox.Show(sb.ToString(), "Thông tin chi tiết hóa đơn", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }

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
            AddControls(new frmHoaDonNhap());
        }
    }
}
