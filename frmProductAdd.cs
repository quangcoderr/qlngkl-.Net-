using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Globalization;
using System.Data.SqlClient;

namespace QuanLyNGK
{
    public partial class frmProductAdd : Form
    {
        public frmProductAdd()
        {
            InitializeComponent();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
       
        private void picProduct_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.bmp;*.jpg;*.jpeg,*.png)|*.BMP;*.JPG;*.JPEG;*.PNG";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                string selectedFile = openFileDialog.FileName;
                textBoxImagePath.Text = selectedFile;
                picProduct.Image = Image.FromFile(selectedFile);
                
            }
           
        }
        public void clear()
        {
            txtID.Text = txtName.Text = txtMCC.Text = txtPrice.Text =txtQuantity.Text= string.Empty;
            
        }

        public string id, name, nsx, nhh, price, mcc,sl;

        private void cbNCC_DropDown(object sender, EventArgs e)
        {
            string query = "SELECT maNCC FROM NhaCungUng";

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
                    txtMCC.Items.Clear(); // Đã sửa chỗ này

                    // Lặp qua DataTable và thêm các mục vào ComboBox
                    foreach (DataRow row in dataTable.Rows)
                    {
                        txtMCC.Items.Add(row["maNCC"].ToString());
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Đã xảy ra lỗi: " + ex.Message);
                }
            }
        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
        public string NSX { get; set; }
        public string NHH { get; set; }
        public byte[] productImage;
        public void UpdateInfo()
        {
            label6.Text = "Edit Infomation Product";
            guna2Button1.Text = "Update";
            txtID.Visible = true;
            txtID.ReadOnly = true;
            txtID.Text = id;
            txtName.Text = name;
            
            txtPrice.Text = price;
            txtQuantity.Text = sl;
            if (DateTime.TryParseExact(nsx, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedNSX))
            {
                dateNSX.Value = parsedNSX;
            }
            if (DateTime.TryParseExact(nhh, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedNHH))
            {
                dateNHH.Value = parsedNHH;
            }
            if (productImage != null)
            {
                // Chuyển đổi byte[] thành Image object (có thể cần thư viện bổ sung)
                Image image = byteArrayToImage(productImage);
                picProduct.Image = image;
            }
        }
        private Image byteArrayToImage(byte[] byteArray)
        {
            using (var ms = new MemoryStream(byteArray))
            {
                return Image.FromStream(ms);
            }
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            string ngaySX = dateNSX.Value.ToString("yyyy-MM-dd");
            string ngayHH = dateNHH.Value.ToString("yyyy-MM-dd");
            string hinhanh = textBoxImagePath.Text; // Đường dẫn của hình ảnh

            // Kiểm tra xem có đường dẫn ảnh mới không
            if (!string.IsNullOrEmpty(hinhanh))
            {
                // Nếu có đường dẫn mới, thực hiện cập nhật ảnh
                byte[] imageBytes = File.ReadAllBytes(hinhanh);

                if (guna2Button1.Text == "Add") // Chế độ thêm mới
                {
                    if (txtID.Text != "")
                    {
                        Product std = new Product(txtID.Text, txtName.Text, ngaySX, ngayHH, int.Parse(txtPrice.Text), txtMCC.Text, imageBytes, int.Parse(txtQuantity.Text));

                        // Thêm sản phẩm mới vào cơ sở dữ liệu
                        DbProduct.AddProduct(std);
                    }
                    else
                    {
                        MessageBox.Show("Hãy nhập đủ thông tin!!!");
                    }
                }
                else if (guna2Button1.Text == "Update") // Chế độ chỉnh sửa
                {
                    Product std = new Product(txtID.Text, txtName.Text, ngaySX, ngayHH, int.Parse(txtPrice.Text), txtMCC.Text, imageBytes, int.Parse(txtQuantity.Text));

                    // Chỉnh sửa thông tin sản phẩm trong cơ sở dữ liệu
                    DbProduct.EditProduct(std, id);

                    // Xóa dữ liệu nhập vào sau khi chỉnh sửa thành công
                    clear();
                }
            }
            else
            {
                // Nếu không có đường dẫn mới, giữ nguyên ảnh đã có
                if (guna2Button1.Text == "Add") // Chế độ thêm mới
                {
                    if (txtID.Text != "")
                    {
                        Product std = new Product(txtID.Text, txtName.Text, ngaySX, ngayHH, int.Parse(txtPrice.Text), txtMCC.Text, productImage, int.Parse(txtQuantity.Text));

                        // Thêm sản phẩm mới vào cơ sở dữ liệu
                        DbProduct.AddProduct(std);
                    }
                    else
                    {
                        MessageBox.Show("Hãy nhập đủ thông tin!!!");
                    }
                }
                else if (guna2Button1.Text == "Update") // Chế độ chỉnh sửa
                {
                    Product std = new Product(txtID.Text, txtName.Text, ngaySX, ngayHH, int.Parse(txtPrice.Text), txtMCC.Text, productImage, int.Parse(txtQuantity.Text));

                    // Chỉnh sửa thông tin sản phẩm trong cơ sở dữ liệu
                    DbProduct.EditProduct(std, id);

                    // Xóa dữ liệu nhập vào sau khi chỉnh sửa thành công
                    clear();
                }
            }
        }
    }
}
