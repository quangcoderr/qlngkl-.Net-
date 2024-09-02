using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;
using Guna.UI2.WinForms;

namespace QuanLyNGK.View
{

    public partial class frmStaffview : Form
    {
        
        public frmStaffview()
        {
            InitializeComponent();
        }

        

        public void frmStaffview_Load_1(object sender, EventArgs e)
        {           
            DbStaff.DisplayAndSearch("select * from NhanVien", guna2DataGridView2);
            guna2DataGridView2.Cursor = Cursors.Default;



        }

        private void guna2ImageButton1_Click(object sender, EventArgs e)
        {
            frmStaffadd frm =new frmStaffadd();
            frm.ShowDialog();
            frmStaffview_Load_1(this, null);
        }

        private void guna2DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        
        private void guna2DataGridView2_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            string searchValue = txtSearch.Text.Trim();
            DbStaff.SearchName(searchValue, guna2DataGridView2);
        }

        private void guna2DataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }

        private void guna2DataGridView2_MouseHover(object sender, EventArgs e)
        {
            

        }


        
        private void guna2DataGridView2_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (guna2DataGridView2.CurrentCell.OwningColumn.Name=="delete")
            {
                // Lấy mã nhân viên từ ô tại cột ID (giả sử cột ID nằm ở cột 0)
                string maNV = guna2DataGridView2.Rows[e.RowIndex].Cells["Column1"].Value.ToString();

                // Xác nhận việc xóa
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa nhân viên có mã: " + maNV + " không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // Gọi hàm xóa nhân viên và truyền mã nhân viên cần xóa
                    DbStaff.DeleteStaff(maNV);
                    frmStaffview_Load_1(this, EventArgs.Empty);
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
                    int age = Convert.ToInt32(row.Cells["Column3"].Value);
                    string gender = row.Cells["Column4"].Value.ToString();

                    // Tạo một đối tượng mới của frmStaffadd
                    frmStaffadd frm = new frmStaffadd();


                    // Gán giá trị cho các biến thành viên của frmStaffadd
                    
                    frm.id = id;
                    frm.name = name;
                    frm.phone =phone;
                    frm.addr = addr;
                    frm.age = age;
                    frm.gender = gender;

                    // Gọi phương thức UpdateInfo từ đối tượng frmStaffadd
                    frm.UpdateInfo();
                    frm.ShowDialog();
                    frmStaffview_Load_1(this, EventArgs.Empty);
                }
            }
                
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void guna2Separator1_Click(object sender, EventArgs e)
        {

        }
    }
    }

