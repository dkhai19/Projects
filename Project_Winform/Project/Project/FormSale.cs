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
    public partial class FormSale : Form
    {
        public FormSale()
        {
            InitializeComponent();
        }

        private void FormSale_Load(object sender, EventArgs e)
        {
            btnOrder.Enabled = false;
            btnAct1.Enabled = false;
            btnAct2.Enabled = false;
            txtPrice.ReadOnly = true;
            txtCode.ReadOnly = true;
            txtDate.ReadOnly = true;
            txtMaHD.ReadOnly = true;
            txtName.ReadOnly = true;
            txtQuantity.ReadOnly = true;
            txtAddress.ReadOnly = true;
            LoadData();
            BackgroundImage = Image.FromFile("\\CSharp\\Project_PRN211\\background.jpg");
        }

        private void LoadData()
        {
            using (MyOrderContext context = new MyOrderContext())
            {
                var data = context.TblMatHangs.ToList();
                cboMH.DataSource = data;
                cboMH.ValueMember = "MaHang";
                cboMH.DisplayMember = "TenHang";
                txtPrice.DataBindings.Clear();
                txtPrice.DataBindings.Add("Text", data, "Gia");

            }
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            txtPrice.ReadOnly = false;
            txtCode.ReadOnly = false;
            txtDate.ReadOnly = false;
            txtMaHD.ReadOnly = false;
            txtName.ReadOnly = false;
            txtDoB.ReadOnly = false;
            txtQuantity.ReadOnly = false;
            txtAddress.ReadOnly = false;
        }

        private void txtCode_TextChanged(object sender, EventArgs e)
        {
            using (MyOrderContext context = new MyOrderContext())
            {
                var data = context.TblKhachHangs.FirstOrDefault(x => x.MaKh.Equals(txtCode.Text));
                if (data != null)
                {
                    txtCode.Text = data.MaKh;
                    txtAddress.Text = data.Diachi;
                    txtName.Text = data.TenKh;
                    txtDoB.Text = data.NgaySinh.ToString();
                    Boolean gender = data.Gt.ToString().Equals("True") ? rdMale.Checked = true : rdFemale.Checked = true;    
                }
            }
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            using (MyOrderContext context = new MyOrderContext())
            {
                var data = context.TblKhachHangs.FirstOrDefault(x => x.TenKh.Equals(txtName.Text));
                if (data != null)
                {
                    txtCode.Text = data.MaKh;
                    txtAddress.Text = data.Diachi;
                    txtName.Text = data.TenKh;
                }

            }
        }

        private void txtMaHD_TextChanged(object sender, EventArgs e)
        {
            using (MyOrderContext context = new MyOrderContext())
            {
                try
                {
                    if (txtMaHD.Text == "")
                    {
                        return;
                    }
                    TblHoadon data = context.TblHoadons.FirstOrDefault(x => x.MaHd.Equals(Int32.Parse(txtMaHD.Text)));
                    if (data != null)
                    {
                        btnOrder.Enabled = false;
                        btnAct1.Enabled = true;
                        btnAct2.Enabled = true;
                        txtDate.ReadOnly = true;
                        txtDate.Text = data.NgayHd.ToString();

                        var data2 = context.TblChiTietHds.Select(x => new
                        {
                            MaChiTietHD = x.MaChiTietHd,
                            MaHD = x.MaHd,
                            MaHang = x.MaHang,
                            Soluong = x.Soluong
                        }).Where(x => x.MaHD == data.MaHd).ToList();
                        
                        dgHoaDon.DataSource = data2;
                        txtCode.Text = data.MaKh;
                        checkHang(Int32.Parse(txtMaHD.Text), cboMH.SelectedValue.ToString());
                        return;
                    }
                    dgHoaDon.DataSource = null;
                    dgHoaDon.Rows.Clear();
                    btnOrder.Enabled = true;
                    btnAct2.Enabled = false;
                    txtDate.ReadOnly = false;
                } catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            } 
        }

        private void checkHang(int maHD, string maHang)
        {
            using (MyOrderContext context = new MyOrderContext())
            {
                TblChiTietHd hoaDon = context.TblChiTietHds.FirstOrDefault(x => x.MaHang.Equals(maHang) && x.MaHd == maHD);
                if (hoaDon != null)
                {
                    txtQuantity.Text = hoaDon.Soluong.ToString();
                }
                else
                {
                    txtQuantity.Text = "";
                }
            }
        }

        private void btnAct1_Click(object sender, EventArgs e)
        {
            using (MyOrderContext context = new MyOrderContext())
            {
                try
                {
                    if (txtQuantity.Text == "")
                    {
                        MessageBox.Show("Cần nhập số lượng ít nhất là 1!");
                        return;
                    }
                    TblChiTietHd t = context.TblChiTietHds.FirstOrDefault(x => x.MaHd == Int32.Parse(txtMaHD.Text) && x.MaHang == cboMH.SelectedValue.ToString());
                    if (t == null)
                    {
                        TblChiTietHd invoiceDetail = new TblChiTietHd
                        {
                            MaHang = cboMH.SelectedValue.ToString(),
                            MaHd = Int32.Parse(txtMaHD.Text),
                            Soluong = Int32.Parse(txtQuantity.Text)
                        };
                        context.TblChiTietHds.Add(invoiceDetail);
                        if (context.SaveChanges() > 0)
                        {
                            MessageBox.Show("Thêm chi tiết hóa đơn mã " + txtMaHD.Text + " thành công!");
                        }
                    }
                    else
                    {
                        t.Soluong += Int32.Parse(txtQuantity.Text);
                        if (context.SaveChanges() > 0)
                        {
                            MessageBox.Show("Cập nhật chi tiết đơn hàng thành công!");
                        }
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
                
            }

        }

        private void cboMH_SelectedValueChanged(object sender, EventArgs e)
        {
            if (txtMaHD.Text != "")
            {
                checkHang(Int32.Parse(txtMaHD.Text), cboMH.SelectedValue.ToString());
            }
        }

        private void btnOrder_Click(object sender, EventArgs e)
        {
            using (MyOrderContext context = new MyOrderContext())
            {
                Boolean gender = rdMale.Checked ? true : false;
                DateTime enteredDate = DateTime.Parse(txtDoB.Text);
                if (txtCode.Text == "")
                {
                    MessageBox.Show("Yêu cầu thông tin khách hàng!");
                    return;
                }
                try
                {
                    TblKhachHang anCus = new TblKhachHang
                    {
                        MaKh = txtCode.Text,
                        TenKh = txtName.Text,
                        Gt = gender,
                        Diachi = txtAddress.Text,
                        NgaySinh = enteredDate
                    };
                    context.TblKhachHangs.Add(anCus);
                    TblKhachHang check = context.TblKhachHangs.FirstOrDefault(x => x.MaKh.Equals(anCus.MaKh));
                    if (check != null)
                    {
                        MessageBox.Show("Đã tồn tại mã khách hàng này, vui lòng nhập mã khách hàng khác!");
                        return;
                    }
                    TblHoadon invoice = new TblHoadon
                    {
                        MaHd = Int32.Parse(txtMaHD.Text),
                        MaKh = (txtCode.Text),
                        NgayHd = DateTime.Parse(txtDate.Text)
                    };
                    context.TblHoadons.Add(invoice);
                    if (context.SaveChanges() > 0)
                    {
                        MessageBox.Show("Thêm một hóa đơn thành công!");
                        txtMaHD.ReadOnly = true;
                        txtDate.ReadOnly = true;
                        txtCode.ReadOnly = true;
                        txtAddress.ReadOnly = true;
                        txtName.ReadOnly = true;
                    }

                } catch(Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show(this, "Bạn có muốn thoát ra ngoài không?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void btnAct2_Click(object sender, EventArgs e)
        {
            using(MyOrderContext context = new MyOrderContext())
            {
                
                TblChiTietHd t = context.TblChiTietHds.FirstOrDefault(x => x.MaHd == Int32.Parse(txtMaHD.Text) && x.MaHang == cboMH.SelectedValue.ToString());
                if(t != null)
                {
                    context.TblChiTietHds.Remove(t);
                    if(context.SaveChanges() > 0)
                    {
                        MessageBox.Show("Gỡ bỏ đơn hàng thành công!");
                        return;
                    }
                }
                MessageBox.Show("Không có đơn hàng nào để gỡ bỏ cả!");
            }
            
        }

        private void dgHoaDon_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgHoaDon.CurrentCell == null ||
                dgHoaDon.CurrentCell.Value == null || e.RowIndex == -1)
            {
                return;
            }
            cboMH.SelectedValue = dgHoaDon.Rows[e.RowIndex].Cells[2].Value;
        }

        private void txtDoB_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
