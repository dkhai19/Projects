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
    public partial class FormReport : Form
    {
        public FormReport()
        {
            InitializeComponent();
        }

        private void FormReport_Load(object sender, EventArgs e)
        {
            BackgroundImage = Image.FromFile("\\CSharp\\Project_PRN211\\background.jpg");
            LoadData();
        }

        private void LoadData()
        {
            using (MyOrderContext context = new MyOrderContext())
            {
                var data = context.TblMatHangs.ToList();
                comboBox1.DataSource = data;
                comboBox1.ValueMember = "MaHang";
                comboBox1.DisplayMember = "MaHang";
            }
        }

        private void comboBox1_SelectedValueChanged(object sender, EventArgs e)
        {
            string code = comboBox1.SelectedValue.ToString();
            using(MyOrderContext context = new MyOrderContext())
            {
                TblMatHang matHang = context.TblMatHangs.FirstOrDefault(x => x.MaHang == code);
                if(matHang != null)
                {
                    txtName.Text = matHang.TenHang;
                }
            } 
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Bạn có muốn thoát ra ngoài không?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            using(MyOrderContext context = new MyOrderContext())
            {
                var data = context.TblHoadons.Select(p => new
                {
                    MaHD = p.MaHd,
                    MaKhachHang = p.MaKh,
                    NgayTao = p.NgayHd
                }).Where(x => dtFrom.Value < x.NgayTao && x.NgayTao < dtTo.Value).ToList();
                if(data != null)
                {
                    var data2 = context.TblChiTietHds.Select(p => new
                    {
                        MaChiTietHD = p.MaChiTietHd,
                        MaHoaDon = p.MaHd,
                        MaHang = p.MaHang,
                        SoLuong = p.Soluong
                    }).Where(x => comboBox1.SelectedValue.ToString() == x.MaHang).ToList();
                    dataGridView1.DataSource = data2;
                    return;
                }
                MessageBox.Show("Không có sản phẩm nào");
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            
        }
    }
}
