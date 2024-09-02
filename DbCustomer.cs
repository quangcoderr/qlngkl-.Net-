using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNGK
{
    class DbCustomer
    {
        public static SqlConnection GetConnection()
        {
            string sql = "Data Source=LAPTOP-5I8I5M5T\\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True";
            SqlConnection con = new SqlConnection(sql); // Pass connection string to SqlConnection constructor
            try
            {
                con.Open();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message); // Display exception message as string
            }
            return con;
        }
        public static void addCustomer(Customer std)
        {
            string sql = @"INSERT INTO KhachHang (maKH, kieuKH, tenKH, gioiTinh, sdt, diaChi, noTien) VALUES (@maKH, @kieuKH, @tenKH, @gioiTinh, @sdt, @diaChi, @no)";
            SqlConnection con = GetConnection();
            SqlCommand cmd = new SqlCommand(sql, con); // Use SqlCommand for SQL Server
            cmd.Parameters.AddWithValue("@maKH", std.Id);
            cmd.Parameters.AddWithValue("@kieuKH", std.Type);
            cmd.Parameters.AddWithValue("@tenKH", std.Name);
            cmd.Parameters.AddWithValue("@gioiTinh", std.Gender);
            cmd.Parameters.AddWithValue("@sdt", std.Phone);
            cmd.Parameters.AddWithValue("@diaChi", std.Addr);
            cmd.Parameters.AddWithValue("@no",std.Unpaid);
            try
            {
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    // Likely a duplicate ID error (consider checking for specific exception types)
                    MessageBox.Show("Không thể thêm Khách Hàng! Mã Khách Hàng đã tồn tại.", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Thêm thành công");


                }
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                con.Close(); // Ensure connection is closed even if exceptions occur
            }
        }

        public static void DisplayAndSearch(string query, DataGridView dgv)
        {
            string sql = query;
            SqlConnection con = GetConnection();
            SqlCommand cmd = new SqlCommand(sql, con); // Use SqlCommand for SQL Server
            SqlDataAdapter adb = new SqlDataAdapter(cmd); // Use SqlDataAdapter for SQL Server
            DataTable tbl = new DataTable();
            adb.Fill(tbl);
            dgv.DataSource = tbl;
            con.Close();


        }
        public static void SearchName(string name, DataGridView dgv)
        {
            string sql = "SELECT * FROM KhachHang WHERE tenKH LIKE '%' + @tenKH + '%'";
            SqlConnection con = GetConnection();
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@tenKH", "%" + name + "%"); // Tìm tất cả các nhân viên có tên chứa phần của 'name'
            SqlDataAdapter adb = new SqlDataAdapter(cmd);
            DataTable tbl = new DataTable();
            adb.Fill(tbl);
            dgv.DataSource = tbl;
            con.Close();
        }
        public static void EditCustomer(Customer std, string id)
        {
            string sql = @"UPDATE KhachHang SET kieuKH = @kieuKH, tenKH = @tenKH, gioiTinh = @gioiTinh, sdt = @sdt, diaChi = @diaChi, noTien = @noTien WHERE maKH = @id";
            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@kieuKH", std.Type);
                cmd.Parameters.AddWithValue("@tenKH", std.Name);
                cmd.Parameters.AddWithValue("@gioiTinh", std.Gender);
                cmd.Parameters.AddWithValue("@sdt", std.Phone);
                cmd.Parameters.AddWithValue("@diaChi", std.Addr);
                cmd.Parameters.AddWithValue("@noTien",std.Unpaid);


                try
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    MessageBox.Show("Cập nhật thành công");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public static void DeleteCustomer(string id)
        {
            string sql = "DELETE FROM KhachHang WHERE maKH = @id";

            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                try
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                    }

                    cmd.ExecuteNonQuery();
                    MessageBox.Show("Xóa Khách hàng thành công");
                }
                catch (SqlException ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    if (con.State == ConnectionState.Open)
                    {
                        con.Close();
                    }
                }
            }

        }
    }
}
