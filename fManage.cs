using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QuanLyNGK.View;

namespace QuanLyNGK
{
    public partial class fManage : Form
    {
        public Color LavenderBlush { get; private set; }

        public fManage()
        {
            InitializeComponent();
            PanelMenu.Visible = false;
            btnPOS.Location = new Point(0, btnCategories.Location.Y + btnCategories.Height + 10);
            btnStatistics.Location = new Point(0, btnPOS.Location.Y + btnPOS.Height + 10);
            PanelReport.Visible=false;
            PanelReport.Location = new Point(0, btnStatistics.Location.Y + btnStatistics.Height + 10);
        }

        private void label1_Click(object sender, EventArgs e)
        {
            
        }

        public void AddControls(Form f)
        {
            CenterPanel.Controls.Clear();
            f.Dock = DockStyle.Fill;
            f.TopLevel = false;
            CenterPanel.Controls.Add(f);
            f.Show();
        }

        private void guna2ControlBox1_Click(object sender, EventArgs e)
        {

        }

        private void fManage_Load(object sender, EventArgs e)
        {
            label1.Text = "Hello,"+ Login.USER ;
        }
        public static string user=Login.USER ;

        private void ClickCategories(object sender, EventArgs e)
        {
            PanelReport.Visible = false;    
            PanelMenu.Visible = !PanelMenu.Visible;
            
        }

        private void Guna2Button5_Click(object sender, EventArgs e)
        {
            AddControls(new frmNCUview());
        }

        private void MenuChange(object sender, EventArgs e)
        {
            if (!PanelMenu.Visible)
            {
                // Di chuyển btnPOS lên khi menu ẩn
                btnPOS.Location = new Point(0, btnCategories.Location.Y + btnCategories.Height + 10);
                btnStatistics.Location = new Point(0, btnPOS.Location.Y + btnPOS.Height + 10);// Khoảng cách 10px
            }
            else
            { 
                // Di chuyển btnPOS xuống khi menu hiện
                btnPOS.Location = new Point(3,750); // Khoảng cách 10px
                btnStatistics.Location = new Point(0, btnPOS.Location.Y + btnPOS.Height + 10);
            }
        }

        private void btnPOS_Click(object sender, EventArgs e)
        {
            PanelReport.Visible = false;
            PanelMenu.Visible=false;
            this.WindowState = FormWindowState.Maximized;
            AddControls(new frmPOSview());
           
        }

        private void btnStaff_Click(object sender, EventArgs e)
        {
            AddControls(new frmStaffview());
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            AddControls(new frmCustomerview());
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            AddControls(new frmProductView());
        }

        private void btnBill_Click(object sender, EventArgs e)
        {
            AddControls(new frmHoaDonban());
            
        }

        private void guna2Button1_Click(object sender, EventArgs e)
        {
            AddControls(new frmPhieutrahang());
        }

        private void btnStatistics_Click(object sender, EventArgs e)
        {
            
            this.WindowState = FormWindowState.Normal;
            
            PanelMenu.Visible = false;
            PanelReport.Visible = !PanelReport.Visible;
        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            AddControls(new frmRPdoanhthu());   
            
        }

        private void guna2Button3_Click(object sender, EventArgs e)
        {
            AddControls(new frmRPproduct());
        }

        private void guna2ControlBox3_Click(object sender, EventArgs e)
        {
            
        }
    }
}
