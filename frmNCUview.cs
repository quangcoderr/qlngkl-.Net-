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
    public partial class frmNCUview : Form
    {
        public frmNCUview()
        {
            InitializeComponent();
        }

        private void frmNCUview_Load(object sender, EventArgs e)
        {
            DbStaff.DisplayAndSearch("select * from NhaCungUng", guna2DataGridView2);
        }

        private void btnAddNCC_Click(object sender, EventArgs e)
        {
            frmNCUadd frmNCUadd = new frmNCUadd();
            frmNCUadd.ShowDialog();
            frmNCUview_Load(this, null);
        }

        private void guna2DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (guna2DataGridView2.CurrentCell.OwningColumn.Name == "delete")
            {
                // Lấy mã nhân viên từ ô tại cột ID (giả sử cột ID nằm ở cột 0)
                string maNCC = guna2DataGridView2.Rows[e.RowIndex].Cells["Column1"].Value.ToString();

                // Xác nhận việc xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa Nhà cung ứng có mã: " + maNCC + " không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Gọi hàm xóa nhân viên và truyền mã nhân viên cần xóa
                    DbNhaCungUng.DeleteNCU(maNCC);
                    frmNCUview_Load(this, EventArgs.Empty);
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
                    string addr = row.Cells["Column6"].Value.ToString();

                    // Tạo một đối tượng mới của frmStaffadd
                    frmNCUadd frm = new frmNCUadd();


                    // Gán giá trị cho các biến thành viên của frmStaffadd

                    frm.id = id;
                    frm.name = name;
                    frm.phone = phone;
                    frm.addr = addr;

                    // Gọi phương thức UpdateInfo từ đối tượng frmStaffadd
                    frm.UpdateInfo();
                    frm.ShowDialog();
                    frmNCUview_Load(this, EventArgs.Empty);
                }
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchValue = txtSearch.Text.Trim();
            DbNhaCungUng.SearchName(searchValue, guna2DataGridView2);
        }
    }
}
