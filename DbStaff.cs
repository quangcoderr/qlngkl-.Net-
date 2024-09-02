using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using QuanLyNGK.View;

namespace QuanLyNGK
{
    class DbStaff
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
        public static void addStaff(ClassStaff std)
        {
            string sql = @"INSERT INTO NhanVien (maNV, tenNV, tuoi, gioiTinh, sdt, diaChi) VALUES (@maNV, @tenNV, @tuoi, @gioiTinh, @sdt, @diaChi)";
            SqlConnection con = GetConnection();
            SqlCommand cmd = new SqlCommand(sql, con); // Use SqlCommand for SQL Server
            cmd.Parameters.AddWithValue("@maNV", std.Id);
            cmd.Parameters.AddWithValue("@tenNV", std.Name);
            cmd.Parameters.AddWithValue("@tuoi", std.age);
            cmd.Parameters.AddWithValue("@gioiTinh", std.gender);
            cmd.Parameters.AddWithValue("@sdt", std.Phone);
            cmd.Parameters.AddWithValue("@diaChi", std.Addr);

            try
            {
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    // Likely a duplicate ID error (consider checking for specific exception types)
                    MessageBox.Show("Không thể thêm nhân viên! Mã nhân viên đã tồn tại.", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    MessageBox.Show("Thêm thành công");
                    

                }
            }
            catch (SqlException ex)
            {
                // Handle other potential SQL errors
                MessageBox.Show("Không thể thêm nhân viên! Mã nhân viên đã tồn tại.", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string sql = "SELECT * FROM NhanVien WHERE tenNV LIKE '%' + @tenNV + '%'";
            SqlConnection con = GetConnection();
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@tenNV", "%" + name + "%"); // Tìm tất cả các nhân viên có tên chứa phần của 'name'
            SqlDataAdapter adb = new SqlDataAdapter(cmd);
            DataTable tbl = new DataTable();
            adb.Fill(tbl);
            dgv.DataSource = tbl;
            con.Close();
        }
        public static void EditStaff(ClassStaff std, string id)
        {
            string sql = @"UPDATE NhanVien SET tenNV = @tenNV, tuoi = @tuoi, gioiTinh = @gioiTinh, sdt = @sdt, diaChi = @diaChi WHERE maNV = @id";
            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@tenNV", std.Name);
                cmd.Parameters.AddWithValue("@tuoi", std.age);
                cmd.Parameters.AddWithValue("@gioiTinh", std.gender);
                cmd.Parameters.AddWithValue("@sdt", std.Phone);
                cmd.Parameters.AddWithValue("@diaChi", std.Addr);

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
        public static void DeleteStaff(string id)
        {
            string sql = "DELETE FROM NhanVien WHERE maNV = @id";

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
                    MessageBox.Show("Xóa nhân viên thành công");
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
