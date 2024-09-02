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
    class DbNhaCungUng
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
        public static void addNCU(NhaCungUng std)
        {
            string sql = @"INSERT INTO NhaCungUng (maNCC, tenNCC, sdtNCC, diaChi) VALUES (@maNCC, @tenNCC, @sdtNCC, @diaChi)";
            SqlConnection con = GetConnection();
            SqlCommand cmd = new SqlCommand(sql, con); // Use SqlCommand for SQL Server
            cmd.Parameters.AddWithValue("@maNCC", std.maNCC);
            cmd.Parameters.AddWithValue("@tenNCC", std.tenNCC);
            cmd.Parameters.AddWithValue("@sdtNCC", std.sdtNCC);
            cmd.Parameters.AddWithValue("@diaChi", std.diaChi);

            try
            {
                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    // Likely a duplicate ID error (consider checking for specific exception types)
                    MessageBox.Show("Không thể thêm Nhà Cung Ứng! Mã Nhà Cung Ứngđã tồn tại.", "Chú ý!", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string sql = "SELECT * FROM NhaCungUng WHERE tenNCC LIKE '%' + @tenNCC + '%'";
            SqlConnection con = GetConnection();
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@tenNCC", "%" + name + "%"); // Tìm tất cả các nhân viên có tên chứa phần của 'name'
            SqlDataAdapter adb = new SqlDataAdapter(cmd);
            DataTable tbl = new DataTable();
            adb.Fill(tbl);
            dgv.DataSource = tbl;
            con.Close();
        }
        public static void EditNCU(NhaCungUng std, string id)
        {
            string sql = @"UPDATE NhaCungUng SET tenNCC = @tenNCC, sdtNCC = @sdtNCC, diaChi = @diaChi WHERE maNCC = @id";
            using (SqlConnection con = GetConnection())
            using (SqlCommand cmd = new SqlCommand(sql, con))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@tenNCC", std.tenNCC);
                cmd.Parameters.AddWithValue("@sdtNCC", std.sdtNCC);
                cmd.Parameters.AddWithValue("@diaChi", std.diaChi);

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
        public static void DeleteNCU(string id)
        {
            string sql = "DELETE FROM NhaCungUng WHERE maNCC = @id";

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
                    MessageBox.Show("Xóa Nhà cung ứng thành công");
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
