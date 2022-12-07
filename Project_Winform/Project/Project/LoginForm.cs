using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Project.Models;
namespace Project
{
    public partial class LoginForm : Form
    {
        public LoginForm()
        {
            InitializeComponent();
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            using (MyOrderContext context = new MyOrderContext())
            {
                
                TblUser anUser = context.TblUsers.FirstOrDefault(u => u.Username == txtUser.Text && u.Pass == Int32.Parse(txtPw.Text));
                if (anUser != null)
                {
                    MessageBox.Show("Đăng nhập thành công, chào mừng bạn đến với chương trình");
                    MainForm mForm = new MainForm();
                    //this.Close();
                    mForm.Show();
                } else
                {
                    MessageBox.Show("Đăng nhập không thành công, vui lòng kiểm tra tại thông tin!");
                }
            }
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            BackgroundImage = Image.FromFile("\\CSharp\\Project_PRN211\\background.jpg");
        }
    }

}
