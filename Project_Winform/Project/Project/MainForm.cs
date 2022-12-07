using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Project
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnProduct_Click(object sender, EventArgs e)
        {
            ProductDetail productDetail = new ProductDetail();
            productDetail.Show();
        }

        private void btnCustomer_Click(object sender, EventArgs e)
        {
            UpdateCustomer up = new UpdateCustomer();
            up.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            FormSale fSale = new FormSale();
            fSale.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void btnReport_Click(object sender, EventArgs e)
        {
            FormReport fReport = new FormReport();
            fReport.Show();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            pictureBox1.Image = Image.FromFile("\\CSharp\\Project_PRN211\\customer_care.jpg");
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
        }
    }
}
