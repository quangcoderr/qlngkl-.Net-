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
    public partial class frmCustomerview : Form
    {
        public frmCustomerview()
        {
            InitializeComponent();
        }

        private void frmCustomerview_Load(object sender, EventArgs e)
        {
            DbCustomer.DisplayAndSearch("select * from KhachHang", guna2DataGridView2);
        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            frmCustomerAdd f = new frmCustomerAdd();
            f.ShowDialog();
            frmCustomerview_Load(this, null);
        }

        private void guna2DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView2.CurrentCell.OwningColumn.Name == "delete")
            {
                // Lấy mã nhân viên từ ô tại cột ID (giả sử cột ID nằm ở cột 0)
                string maKH = guna2DataGridView2.Rows[e.RowIndex].Cells["Column1"].Value.ToString();

                // Xác nhận việc xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa Khách hàng có mã: " + maKH + " không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Gọi hàm xóa nhân viên và truyền mã nhân viên cần xóa
                    DbCustomer.DeleteCustomer(maKH);
                    frmCustomerview_Load(this, EventArgs.Empty);
                }
            }

            if (guna2DataGridView2.CurrentCell.OwningColumn.Name == "edit")
            {

                if (e.RowIndex >= 0) // Kiểm tra xem đã nhấp vào một hàng hợp lệ chưa
                {
                    DataGridViewRow row = guna2DataGridView2.Rows[e.RowIndex]; // Lấy hàng được chọn

                    // Trích xuất thông tin từ hàng được chọn
                    string id = row.Cells["Column1"].Value.ToString();
                    string name = row.Cells["Column2"].Value.ToString();
                    string phone = row.Cells["Column5"].Value.ToString();
                    string addr = row.Cells["Column3"].Value.ToString();
                    string type = row.Cells["Type"].Value.ToString();
                    string gender = row.Cells["Column4"].Value.ToString();

                    // Tạo một đối tượng mới của frmStaffadd
                    frmCustomerAdd fr = new frmCustomerAdd();


                    // Gán giá trị cho các biến thành viên của frmStaffadd

                    fr.id = id;
                    fr.type = type;
                    fr.name = name;
                    fr.phone = phone;
                    fr.addr = addr;
                    
                    fr.gender = gender;

                    // Gọi phương thức UpdateInfo từ đối tượng frmStaffadd
                    fr.UpdateInfo();
                    fr.ShowDialog();
                    frmCustomerview_Load(this, EventArgs.Empty);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchValue = txtSearch.Text.Trim();
            DbCustomer.SearchName(searchValue, guna2DataGridView2);
        }
    }
}
