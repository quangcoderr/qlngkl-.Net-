using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNGK.View;

namespace QuanLyNGK
{
    public partial class frmStaffadd : Form
    {
        public frmStaffadd()
        {
            InitializeComponent();
        }

        private void guna2Panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }
        public void clear()
        {
            txtID.Text = txtName.Text = txtAge.Text = txtPhone.Text = txtAddr.Text = string.Empty;
            cbGender.SelectedIndex = -1;
        }

        public string id, name, gender, addr,phone;
        public int  age;
        public void UpdateInfo()
        {
            label6.Text = "Edit Infomation Staff";
            guna2Button1.Text = "Update";
            txtID.Text = id;
            txtID.ReadOnly = true;
            txtName.Text = name;
            txtPhone.Text = phone;
            txtAddr.Text = addr;
            txtAge.Text = age.ToString();
            cbGender.Text = gender;
        }
        private void guna2Button1_Click(object sender, EventArgs e)
        {
            if (guna2Button1.Text == "Add") // Chế độ thêm mới
            {              
                // Kiểm tra dữ liệu nhập vào có hợp lệ không
                if (txtID.Text != "" && txtName.Text != "" && txtAge.Text != "" && cbGender.SelectedIndex != -1 && txtPhone.Text != "" && txtAddr.Text != "")
                {
                    // Tạo đối tượng ClassStaff mới từ dữ liệu nhập vào
                    ClassStaff std = new ClassStaff(txtID.Text, txtName.Text, int.Parse(txtAge.Text), cbGender.Text, txtPhone.Text, txtAddr.Text);

                    // Thêm nhân viên mới vào cơ sở dữ liệu
                    DbStaff.addStaff(std);

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
                if (txtID.Text != "" && txtName.Text != "" && txtAge.Text != "" && cbGender.SelectedIndex != -1 && txtPhone.Text != "" && txtAddr.Text != "")
                {
                    // Tạo đối tượng ClassStaff mới từ dữ liệu nhập vào
                    ClassStaff std = new ClassStaff(txtID.Text, txtName.Text, int.Parse(txtAge.Text), cbGender.Text, txtPhone.Text, txtAddr.Text);

                    // Gọi phương thức EditStaff để chỉnh sửa thông tin nhân viên
                    DbStaff.EditStaff(std, id);

                    // Xóa dữ liệu nhập vào sau khi chỉnh sửa thành công
                    clear();
                }
                else
                {
                    MessageBox.Show("Hãy nhập đủ thông tin!!!");
                }
            }
        }

        private void guna2Panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2Panel2_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
