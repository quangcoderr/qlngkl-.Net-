using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNGK
{
    public partial class frmPOSview : Form
    {
        public frmPOSview()
        {
            InitializeComponent();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {
        }

        private void frmPOSview_Load(object sender, EventArgs e)
        {
            // Dispose các controls không còn cần thiết
            while (ProductPanel.Controls.Count > 0)
            {
                ProductPanel.Controls[0].Dispose();
            }
            Loaddata();
        }

        private void UcProduct_onSelect(object sender, EventArgs e)
        {
            if (sender is UcProduct selectedProduct)
            {
                string productName = selectedProduct.PName;
                string productPrice = selectedProduct.PPrice.Replace(" Đ", "");
                Image productImage = selectedProduct.PImage;

                bool productExists = false;
                foreach (DataGridViewRow row in guna2DataGridView2.Rows)
                {
                    if (row.Cells[0].Value != null && row.Cells[0].Value.ToString() == productName)
                    {
                        int currentQty = Convert.ToInt32(row.Cells[1].Value);
                        row.Cells[1].Value = currentQty + 1;
                        decimal productAmount = (currentQty + 1) * Convert.ToDecimal(productPrice);
                        row.Cells[3].Value = productAmount;
                        productExists = true;
                        break;
                    }
                }

                if (!productExists)
                {
                    guna2DataGridView2.Rows.Add(productName, 1, productPrice, productPrice);
                }

                CalculateTotal();
            }
        }

        public void Loaddata()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                con.Open();

                using (SqlCommand cmd = new SqlCommand("SELECT maSP, tenSP, hinhanh, giaSP FROM SanPham", con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            foreach (DataRow row in dt.Rows)
            {
                UcProduct ucProduct = new UcProduct
                {
                    id = row["maSp"].ToString(),
                    PName = row["tenSP"].ToString(),
                    PPrice = row["giaSP"].ToString()
                };

                byte[] imageBytes = row["hinhanh"] as byte[];
                if (imageBytes != null && imageBytes.Length > 0)
                {
                    using (MemoryStream ms = new MemoryStream(imageBytes))
                    {
                        ucProduct.PImage = Image.FromStream(ms);
                    }
                }
                else
                {
                    ucProduct.PImage = Image.FromFile(@"C:\Users\lengu\Downloads\dang-cap-nhat-san-pham-446.png");
                }

                ucProduct.onSelect += UcProduct_onSelect;
                this.ProductPanel.Controls.Add(ucProduct);
            }
        }

        private string GetProductIDByName(string productName)
        {
            string selectQuery = @"SELECT maSP FROM SanPham WHERE tenSP = @TenSP";
            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@TenSP", productName);
                    return command.ExecuteScalar()?.ToString();
                }
            }
        }

        private void CalculateTotal()
        {
            decimal totalAmount = 0;

            foreach (DataGridViewRow row in guna2DataGridView2.Rows)
            {
                if (!row.IsNewRow)
                {
                    totalAmount += Convert.ToDecimal(row.Cells["amount"].Value);
                }
            }

            lblTotal.Text = totalAmount.ToString();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            foreach (var item in ProductPanel.Controls)
            {
                var pro = (UcProduct)item;
                pro.Visible = pro.PName.ToLower().Contains(txtSearch.Text.Trim().ToLower());
            }
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            guna2DataGridView2.Rows.Clear();
            lblTotal.Text = "0";
            txtMaKH.Text = "";
            cbPay.TabIndex = 0;
        }

        string user = fManage.user;

        private void AddOrderToHoaDonBanHang(string maNV, bool insufficientStock)
        {
            string maHD = GenerateRandomHD();
            decimal totalAmount = Convert.ToDecimal(lblTotal.Text);
            decimal debtAmount = 0;

            if (cbPay.Text == "Chưa thanh toán")
            {
                string maKH = txtMaKH.Text;

                if (!IsCustomerExists(maKH))
                {
                    MessageBox.Show("Bạn chưa đăng ký khách hàng để có thể nợ.", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                debtAmount = totalAmount;
                UpdateCustomerDebt(maKH, debtAmount);
            }

            string trangThai = insufficientStock ? "Thiếu hàng" : cbPay.Text + " (" + txtMaKH.Text + ")";

            string insertQuery = @"INSERT INTO HoaDonBanHang (maHD, ngayLapHD, tenNV, TongTien, TrangThai, maKH)
                                   VALUES (@MaHD, @NgayLapHD, @user, @TongTien, @TrangThai, @maKH)";

            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@MaHD", maHD);
                    command.Parameters.AddWithValue("@NgayLapHD", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    command.Parameters.AddWithValue("@user", user);
                    command.Parameters.AddWithValue("@TongTien", totalAmount);
                    command.Parameters.AddWithValue("@TrangThai", trangThai);
                    command.Parameters.AddWithValue("@maKH", txtMaKH.Text.ToString());

                    command.ExecuteNonQuery();
                }
            }

            foreach (DataGridViewRow row in guna2DataGridView2.Rows)
            {
                string productName = row.Cells[0].Value.ToString();
                int soLuong = Convert.ToInt32(row.Cells[1].Value);
                decimal donGia = Convert.ToDecimal(row.Cells[2].Value);

                string maSP = GetProductIDByName(productName);

                AddOrderDetailToChiTietHoaDon(maHD, maSP, soLuong, donGia, productName);

                if (!insufficientStock)
                {
                    UpdateProductQuantity(maSP, soLuong);
                }
            }
            MessageBox.Show("Tạo hóa đơn thành công", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private bool IsUniqueOrderID(string orderID)
        {
            string selectQuery = @"SELECT COUNT(*) FROM HoaDonBanHang WHERE maHD = @MaHD";
            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@MaHD", orderID);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count == 0;
                }
            }
        }

        private string GenerateRandomHD()
        {
            Random random = new Random();
            string orderID;
            do
            {
                int randomNumber = random.Next(1000, 9999);
                orderID = "HD" + randomNumber.ToString();
            } while (!IsUniqueOrderID(orderID));
            return orderID;
        }

        private void AddOrderDetailToChiTietHoaDon(string maHD, string maSP, int soLuong, decimal donGia, string tenSP)
        {
            string insertQuery = @"INSERT INTO ChiTietHoaDon (maHD, maSP, soLuong, DonGia, tenSP)
                                   VALUES (@MaHD, @MaSP, @SoLuong, @DonGia, @TenSP)";
            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(insertQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@MaHD", maHD);
                    command.Parameters.AddWithValue("@MaSP", maSP);
                    command.Parameters.AddWithValue("@SoLuong", soLuong);
                    command.Parameters.AddWithValue("@DonGia", donGia);
                    command.Parameters.AddWithValue("@TenSP", tenSP);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void UpdateProductQuantity(string maSP, int soLuong)
        {
            string updateQuery = @"UPDATE SanPham SET soLuong = soLuong - @SoLuong WHERE maSP = @MaSP";
            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@SoLuong", soLuong);
                    command.Parameters.AddWithValue("@MaSP", maSP);
                    command.ExecuteNonQuery();
                }
            }
        }

        private void UpdateCustomerDebt(string maKH, decimal debtAmount)
        {
            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                string updateQuery = "UPDATE KhachHang SET noTien = noTien + @DebtAmount WHERE maKH = @MaKH";

                using (SqlCommand command = new SqlCommand(updateQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@DebtAmount", debtAmount);
                    command.Parameters.AddWithValue("@MaKH", maKH);
                    command.ExecuteNonQuery();
                }
            }
        }

        private bool IsCustomerExists(string maKH)
        {
            string selectQuery = "SELECT COUNT(*) FROM KhachHang WHERE maKH = @MaKH";
            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@MaKH", maKH);
                    int count = Convert.ToInt32(command.ExecuteScalar());
                    return count > 0;
                }
            }
        }

        private bool ktrSoluongSP(string productName, int quantity)
        {
            string selectQuery = "SELECT soLuong FROM SanPham WHERE tenSP = @ProductName";
            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@ProductName", productName);
                    int availableQuantity = Convert.ToInt32(command.ExecuteScalar());
                    return availableQuantity >= quantity;
                }
            }
        }

        private int GetAvailableQuantity(string productName)
        {
            string selectQuery = "SELECT soLuong FROM SanPham WHERE tenSP = @ProductName";
            using (SqlConnection connection = new SqlConnection(@"Data Source=LAPTOP-5I8I5M5T\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True"))
            {
                using (SqlCommand command = new SqlCommand(selectQuery, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@ProductName", productName);
                    return Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string maKH=txtMaKH.Text;
            bool insufficientStock = false;
            foreach (DataGridViewRow row in guna2DataGridView2.Rows)
            {
                string productName = row.Cells[0].Value.ToString();
                int quantity = Convert.ToInt32(row.Cells[1].Value);
                if (!ktrSoluongSP(productName, quantity))
                {
                    int availableQuantity = GetAvailableQuantity(productName);
                    DialogResult result = MessageBox.Show(
                        "Sản phẩm " + productName + " chỉ còn " + availableQuantity + " sản phẩm trong kho. Bạn vẫn muốn tạo hóa đơn chứ?",
                        "Thông báo",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning
                    );

                    if (result == DialogResult.Yes)
                    {
                        if (!IsCustomerExists(maKH))
                        {
                            MessageBox.Show("Hãy đăng ký khách hàng để chúng tôi giao hàng!", "Cảnh báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }
                        else
                        {
                            insufficientStock = true;
                            break;
                        }
                        
                    }
                    else
                    {
                        return;
                    }
                }
            }

            string maNV = fManage.user;
            AddOrderToHoaDonBanHang(maNV, insufficientStock);
            guna2Button1_Click(this, new EventArgs());
        }
    }
}
