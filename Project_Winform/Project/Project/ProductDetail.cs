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
    public partial class ProductDetail : Form
    {
        public ProductDetail()
        {
            InitializeComponent();
        }

        private void openForm(bool check)
        {
            if (check)
            {
                txtName.ReadOnly = false;
                txtPrice.ReadOnly = false;
                cboDVT.Enabled = true;
            }
            else
            {
                txtCode.ReadOnly = false;
                txtName.ReadOnly = false;
                txtPrice.ReadOnly = false;
                cboDVT.Enabled = true;
            }
        }

        private void blockForm()
        {
            txtName.ReadOnly = true;
            txtPrice.ReadOnly = true;
            txtCode.ReadOnly = true;
            cboDVT.Enabled = false;
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            openForm(false);
            txtCode.Text = "";
            txtName.Text = "";
            txtPrice.Text = "";
        }

        private void ProductDetail_Load(object sender, EventArgs e)
        {
            BackgroundImage = Image.FromFile("\\CSharp\\Project_PRN211\\background.jpg");
            LoadData();
        }

        private void LoadData()
        {
            using (MyOrderContext context = new MyOrderContext())
            {
                var data2 = context.TblMatHangs.Select(x => x.Dvt).Distinct().ToList();

                var data = context.TblMatHangs.
                    Select(p => new
                    {
                        MaHang = p.MaHang,
                        TenHang = p.TenHang,
                        DVT = p.Dvt,
                        Gia = p.Gia.ToString()
                    }).ToList();
                dgProduct.DataSource = data;

                cboDVT.DataSource = null;
                cboDVT.Items.Clear();
                cboDVT.DataSource = data2;

                cboDVT.DataBindings.Clear();
                cboDVT.DataBindings.Add("Text", data, "DVT");

                txtCode.DataBindings.Clear();
                txtCode.DataBindings.Add("Text", data, "MaHang");

                txtName.DataBindings.Clear();
                txtName.DataBindings.Add("Text", data, "TenHang");

                txtPrice.DataBindings.Clear();
                txtPrice.DataBindings.Add("Text", data, "Gia");
            }
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            using (MyOrderContext context = new MyOrderContext())
            {
                try
                {
                    TblMatHang mathang = new TblMatHang
                    {
                        MaHang = txtCode.Text,
                        TenHang = txtName.Text,
                        Dvt = (string)cboDVT.Text,
                        Gia = float.Parse(txtPrice.Text)
                    };
                    context.TblMatHangs.Add(mathang);
                    if (context.SaveChanges() > 0)
                    {
                        MessageBox.Show("Add success!");
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Add failed! " + ex.Message);
                }
            }
            blockForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            using (MyOrderContext context = new MyOrderContext())
            {
                try
                {
                    var matHang = context.TblChiTietHds.Where(x => x.MaHang == txtCode.Text).ToList();
                    if(matHang != null)
                    {
                        MessageBox.Show("Mặt hàng đang tồn tại trong đơn hàng nào đó, không thể xóa!");
                        return;
                    }
                    TblMatHang mathang = context.TblMatHangs.FirstOrDefault(x => x.MaHang.Equals(txtCode.Text));
                    if (mathang != null)
                    {
                        context.TblMatHangs.Remove(mathang);
                        if (context.SaveChanges() > 0)
                        {
                            MessageBox.Show("Remove success!");
                            LoadData();
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Remove failed! " + ex.Message);
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

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (MyOrderContext context = new MyOrderContext())
            {
                try
                {
                    TblMatHang mathang = context.TblMatHangs.FirstOrDefault(x => x.MaHang.Equals(txtCode.Text));
                    if (mathang != null)
                    {
                        mathang.MaHang = txtCode.Text;
                        mathang.TenHang = txtName.Text;
                        mathang.Gia = float.Parse(txtPrice.Text);
                        mathang.Dvt = (string)cboDVT.Text;
                        if (context.SaveChanges() > 0)
                        {
                            MessageBox.Show("Update success!");
                            LoadData();
                        }
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update failed! " + ex.Message);
                }
            }
            blockForm();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            openForm(true);
        }

        private void txtPrice_TextChanged(object sender, EventArgs e)
        {

        }
    }


}
