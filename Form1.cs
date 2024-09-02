using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyNGK
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();
        }

        private void guna2HtmlLabel1_Click(object sender, EventArgs e)
        {

        }

        private void guna2HtmlLabel2_Click(object sender, EventArgs e)
        {

        }

        private void guna2PictureBox4_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có muốn thoát chương trình", "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                this.Close();
            }
        }

        private void txtUsername_TextChanged(object sender, EventArgs e)
        {
            txtUsername.ForeColor = System.Drawing.Color.Black;
        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            txtPassword.ForeColor = System.Drawing.Color.Black;
            txtPassword.PasswordChar = '*';
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
                txtPassword.PasswordChar = '\0';
            }
            else
                txtPassword.PasswordChar = '*';
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {

            if (txtUsername.Text == "" )
            {
                MessageBox.Show("Hãy nhập tên tài khoản", "Thông báo");
            }
            else if (txtPassword.Text == "" )
            {
                MessageBox.Show("Hãy nhập mật khẩu", "Thông báo");
            }
            else
            {
                try
                {
                    SqlConnection connection = new SqlConnection("Data Source=LAPTOP-5I8I5M5T\\SQLEXPRESS;Initial Catalog=QLNGK;Integrated Security=True");
                    SqlCommand cmd = new SqlCommand("select * from Account where username = @username and password = @password", connection);
                    cmd.Parameters.AddWithValue("@username", txtUsername.Text);
                    cmd.Parameters.AddWithValue("@password", txtPassword.Text);
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count > 0)
                    {
                        USER = dt.Rows[0]["username"].ToString();                       
                        this.Hide();
                        fManage fManage = new fManage();
                        fManage.ShowDialog();
                        this.Show();

                    }
                    else
                    {
                        MessageBox.Show("Tài khoản hoặc mật khẩu không đúng", "Thông báo");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(" " + ex);
                }
            }
        }
        public static string user;
        public static string USER
        {
            get { return user; }
            private set {user=value;}
        } 
        private void txtkeydow(object sender, KeyEventArgs e)
        {
            if(e.KeyCode== Keys.Enter)
            {
               txtPassword.Focus();
                txtPassword.Text = "";
                txtPassword.ForeColor = System.Drawing.Color.Black;
                txtPassword.PasswordChar = '*';
            }
        }

        private void txtkey(object sender, KeyEventArgs e)
        {
            if(e.KeyCode== Keys.Enter)
            {
                guna2Button1_Click(sender, e);
            }
        }

        private void guna2GradientPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void guna2CirclePictureBox1_Click(object sender, EventArgs e)
        {

        }
    }
}
