using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNGK
{
    class DbProduct
    {
        public static SqlConnection GetConnection()
        {
            string connectionString = "Data Source=LAPTOP-5I8I5M5T\\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            try
            {
                connection.Open();
            }
            catch (SqlException ex)
            {
                MessageBox.Show(ex.Message);
            }
            return connection;
        }
     
        public static void DisplayAndSearch(string query, DataGridView dgv)
        {
            string sql = query;
            SqlConnection con = GetConnection();
            SqlCommand cmd = new SqlCommand(sql, con); // Sử dụng SqlCommand cho SQL Server
            SqlDataAdapter adb = new SqlDataAdapter(cmd); // Sử dụng SqlDataAdapter cho SQL Server
            DataTable tbl = new DataTable();
            adb.Fill(tbl);
            dgv.DataSource = tbl;
            con.Close();
        }
        public static void AddProduct(Product product)
        {
            string sql = @"INSERT INTO SanPham (maSP, tenSP, ngaySX, ngayHH, giaSP, maNCC, hinhanh, soLuong) 
                   VALUES (@Id, @Name, @NSX, @NHH, @Price, @MCC, @Image, @Quantity)"; 
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", product.Id);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@NSX", product.NSX);
                command.Parameters.AddWithValue("@NHH", product.NHH);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@MCC", product.MCC);
                command.Parameters.AddWithValue("@Image", product.Image);
                command.Parameters.AddWithValue("@Quantity", product.Quantity); 

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("Thêm sản phẩm không thành công!");
                    }
                    else
                    {
                        MessageBox.Show("Thêm sản phẩm thành công!");
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public static void SearchName(string name, DataGridView dgv)
        {
            string sql = "SELECT * FROM SanPham WHERE tenSP LIKE '%' + @tenSP + '%'";
            SqlConnection con = GetConnection();
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@tenSP", "%" + name + "%"); // Tìm tất cả các nhân viên có tên chứa phần của 'name'
            SqlDataAdapter adb = new SqlDataAdapter(cmd);
            DataTable tbl = new DataTable();
            adb.Fill(tbl);
            dgv.DataSource = tbl;
            con.Close();
        }
        // 4. Cập nhật phương thức EditProduct
        public static void EditProduct(Product product, string id)
        {
            string sql = @"UPDATE SanPham 
                   SET maSP = @Id, tenSP = @Name, ngaySX = @NSX, ngayHH = @NHH, giaSP = @Price, maNCC = @MCC, hinhanh = @Image, soLuong = @Quantity
                   WHERE maSP = @OldId";
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", product.Id);
                command.Parameters.AddWithValue("@Name", product.Name);
                command.Parameters.AddWithValue("@NSX", product.NSX);
                command.Parameters.AddWithValue("@NHH", product.NHH);
                command.Parameters.AddWithValue("@Price", product.Price);
                command.Parameters.AddWithValue("@MCC", product.MCC);
                command.Parameters.AddWithValue("@Image", product.Image);
                command.Parameters.AddWithValue("@Quantity", product.Quantity); // Thêm tham số cho số lượng
                command.Parameters.AddWithValue("@OldId", id);

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm để chỉnh sửa!");
                    }
                    else
                    {
                        MessageBox.Show("Chỉnh sửa sản phẩm thành công!");
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Hãy nhập đúng và đủ thông tin");
                }
            }
        }

        public static void DeleteProduct(string id)
        {
            string sql = "DELETE FROM SanPham WHERE maSP = @Id";

            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand(sql, connection))
            {
                command.Parameters.AddWithValue("@Id", id);

                try
                {
                    int rowsAffected = command.ExecuteNonQuery();
                    if (rowsAffected == 0)
                    {
                        MessageBox.Show("Không tìm thấy sản phẩm để xóa!");
                    }
                    else
                    {
                        MessageBox.Show("Xóa sản phẩm thành công!");
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        // Trong lớp DbProduct

        public static List<Product> GetAllProducts()
        {
            List<Product> productList = new List<Product>();

            string query = "SELECT maSP, tenSP, maNCC, soLuong, ngaySX, ngayHH, giaSP, hinhanh FROM SanPham";

            // Using statement for connection and command
            using (SqlConnection connection = GetConnection())
            using (SqlCommand command = new SqlCommand(query, connection))
            {
                try
                {
                    connection.Open(); // Open connection within the using block

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string id = reader.GetString(0);
                            string name = reader.GetString(1);
                            string nsx = reader.GetString(2);
                            string nhh = reader.GetString(3);
                            int price = reader.GetInt32(4);
                            string mcc = reader.GetString(5);
                            byte[] imageBytes = (byte[])reader["hinhanh"];
                            int quantity = reader.GetInt32(7);

                            Product product = new Product(id, name, nsx, nhh, price, mcc, imageBytes, quantity);
                            productList.Add(product);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    MessageBox.Show("Lỗi: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            } // Connection and command are disposed here automatically

            return productList;
        }











    }
}
