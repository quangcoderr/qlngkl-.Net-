using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNGK
{
    public partial class frmProductView : Form
    {
       
        public frmProductView()
        {
            InitializeComponent();
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            frmProductAdd frmProductAdd = new frmProductAdd();
            frmProductAdd.ShowDialog();
            frmProductView_Load(this, null);
            
        }

        private void frmProductView_Load(object sender, EventArgs e)
        {
            DbProduct.DisplayAndSearch("select * from SanPham", guna2DataGridView2);
        }

        private void guna2DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView2.CurrentCell.OwningColumn.Name == "delete")
            {
                // Lấy mã nhân viên từ ô tại cột ID (giả sử cột ID nằm ở cột 0)
                string maSP = guna2DataGridView2.Rows[e.RowIndex].Cells["maSP"].Value.ToString();

                // Xác nhận việc xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa Sản phẩm có mã: " + maSP + " không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Gọi hàm xóa nhân viên và truyền mã nhân viên cần xóa
                    DbProduct.DeleteProduct(maSP);
                    frmProductView_Load(this, EventArgs.Empty);
                }
            }

            if (guna2DataGridView2.CurrentCell.OwningColumn.Name == "edit")
            {

                if (e.RowIndex >= 0) // Kiểm tra xem đã nhấp vào một hàng hợp lệ chưa
                {
                    DataGridViewRow row = guna2DataGridView2.Rows[e.RowIndex]; // Lấy hàng được chọn

                    // Trích xuất thông tin từ hàng được chọn
                    string id = row.Cells["maSP"].Value.ToString();
                    string name = row.Cells["Column3"].Value.ToString();
                    string mcc = row.Cells["Column2"].Value.ToString();
                    string price = row.Cells["Column5"].Value.ToString();
                    string nsx = row.Cells["Column7"].Value.ToString();
                    string nhh = row.Cells["Column8"].Value.ToString();
                    string sl = row.Cells["Column4"].Value.ToString();

                    byte[] imageData = null;
                    if (row.Cells["Column6"].Value != DBNull.Value)
                    {
                        imageData = (byte[])row.Cells["Column6"].Value;
                    }

                    
                    frmProductAdd frmProductAdd = new frmProductAdd();

                    frmProductAdd.id = id;
                    frmProductAdd.name = name;
                    frmProductAdd.price = price;
                    frmProductAdd.mcc = mcc;
                    frmProductAdd.sl = sl;
                    frmProductAdd.productImage = imageData;

                    
                    frmProductAdd.UpdateInfo();
                    frmProductAdd.ShowDialog();
                    frmProductView_Load(this, EventArgs.Empty);
                }

            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchValue = txtSearch.Text.Trim();
            DbProduct.SearchName(searchValue, guna2DataGridView2);
        }
    }
}
