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
    public partial class frmNCUadd : Form
    {
        public frmNCUadd()
        {
            InitializeComponent();
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public void clear()
        {
            txtID.Text = txtName.Text  = txtPhone.Text = txtAddr.Text = string.Empty;
        }
        public string id, name, addr, phone;

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void UpdateInfo()
        {
            label6.Text = "Edit Infomation Supplier";
            guna2Button1.Text = "Update";
            txtID.Text = id;
            txtID.ReadOnly = true;
            txtName.Text = name;
            txtPhone.Text = phone;
            txtAddr.Text = addr;
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2Button1.Text == "Add") // Chế độ thêm mới
            {
                // Kiểm tra dữ liệu nhập vào có hợp lệ không
                if (txtID.Text != "" && txtName.Text != "" && txtPhone.Text != "" && txtAddr.Text != "")
                {
                    // Tạo đối tượng ClassStaff mới từ dữ liệu nhập vào
                    NhaCungUng std = new NhaCungUng(txtID.Text, txtName.Text, txtPhone.Text, txtAddr.Text);

                    // Thêm nhân viên mới vào cơ sở dữ liệu
                    DbNhaCungUng.addNCU(std);

                    // Xóa dữ liệu nhập vào sau khi thêm thành công
                    clear();
                }
                else
                {
                    MessageBox.Show("Hãy nhập đủ thông tin!!!");
                }
            }
            else if (guna2Button1.Text == "Update") // Chế độ chỉnh sửa
            {
                // Kiểm tra dữ liệu nhập vào có hợp lệ không
                if (txtID.Text != "" && txtName.Text != "" && txtPhone.Text != "" && txtAddr.Text != "")
                {
                    // Tạo đối tượng ClassStaff mới từ dữ liệu nhập vào
                    NhaCungUng std = new NhaCungUng(txtID.Text, txtName.Text, txtPhone.Text, txtAddr.Text);

                    // Gọi phương thức EditStaff để chỉnh sửa thông tin nhân viên
                    DbNhaCungUng.EditNCU(std, id);

                    // Xóa dữ liệu nhập vào sau khi chỉnh sửa thành công
                    clear();
                }
                else
                {
                    MessageBox.Show("Hãy nhập đủ thông tin!!!");
                }
            }
        }
    }
}
