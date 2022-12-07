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
    public partial class UpdateCustomer : Form
    {
        public UpdateCustomer()
        {
            InitializeComponent();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(this, "Bạn có muốn thoát ra ngoài không?", "Thông báo", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                this.Close();
            }
        }

        private void UpdateCustomer_Load(object sender, EventArgs e)
        {
            LoadData();
            BackgroundImage = Image.FromFile("\\CSharp\\Project_PRN211\\background.jpg");
        }

        private void LoadData()
        {
            using (MyOrderContext context = new MyOrderContext())
            {
                var data = context.TblKhachHangs.
                    Select(x => new
                    {
                        code = x.MaKh,
                        name = x.TenKh,
                        gender = x.Gt == true ? "Nam" : "Nữ",
                        address = x.Diachi,
                        birthDay = x.NgaySinh.ToString("d")
                    })
                    .
                    ToList();
                dgCustomer.DataSource = data;

                txtCode.DataBindings.Clear();
                txtCode.DataBindings.Add("Text", data, "code");

                txtName.DataBindings.Clear();
                txtName.DataBindings.Add("Text", data, "name");

                txtAddress.DataBindings.Clear();
                txtAddress.DataBindings.Add("Text", data, "address");

                txtDoB.DataBindings.Clear();
                txtDoB.DataBindings.Add("Text", data, "birthDay");



                Boolean check = dgCustomer.CurrentRow.Cells[2].Value.ToString().Equals("Nam") ? true : false;
                if (check)
                {
                    rdMale.Checked = true;
                }
                else
                {
                    rdFemale.Checked = true;
                }
                //rdMale.DataBindings.Clear();
                //rdMale.Checked = !rdFemale.Checked;
            }
        }

        private void blockForm()
        {
            txtCode.ReadOnly = true;
            txtName.ReadOnly = true;
            txtDoB.ReadOnly = true;
            txtAddress.ReadOnly = true;
            rdMale.Enabled = false;
            rdFemale.Enabled = false;
        }

        private void openForm(bool check)
        {
            if (check)
            {
                rdMale.Enabled = true;
                rdFemale.Enabled = true;
                txtName.ReadOnly = false;
                txtAddress.ReadOnly = false;
                txtDoB.ReadOnly = false;
            }
            else
            {
                rdMale.Enabled = true;
                rdFemale.Enabled = true;

                txtCode.ReadOnly = false;
                txtName.ReadOnly = false;
                txtAddress.ReadOnly = false;
                txtDoB.ReadOnly = false;
            }

        }

        private void dgCustomer_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgCustomer.CurrentCell == null ||
                dgCustomer.CurrentCell.Value == null || e.RowIndex == -1)
            {
                return;
            }
            Boolean gender = dgCustomer.Rows[e.RowIndex].Cells[2].FormattedValue.ToString().Equals("Nam") ? rdMale.Checked = true : rdFemale.Checked = true;
            
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            openForm(false);
            txtCode.Text = "";
            txtName.Text = "";
            txtAddress.Text = "";
            txtDoB.Text = "";
        
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            Boolean gender = rdMale.Checked ? true : false;
            DateTime enteredDate = DateTime.Parse(txtDoB.Text);
            using (MyOrderContext context = new MyOrderContext())
            {
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
                    TblKhachHang check = context.TblKhachHangs.FirstOrDefault(x => x.MaKh.Equals(anCus.MaKh));
                    if(check != null)
                    {
                        MessageBox.Show("Đã tồn tại mã khách hàng này, vui lòng nhập mã khách hàng khác!");
                        return;
                    }
                    context.Add(anCus);
                    if (context.SaveChanges() > 0)
                    {
                        MessageBox.Show("Add success!");
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Add error! " + ex.Message);
                }
            }
            blockForm();
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            string code = dgCustomer.CurrentRow.Cells[0].Value.ToString();
            using (MyOrderContext context = new MyOrderContext())
            {
                TblKhachHang anCus = context.TblKhachHangs.FirstOrDefault(x => x.MaKh.Equals(code));
                if (context.TblHoadons.FirstOrDefault(x => x.MaKh.Equals(anCus.MaKh)) != null)
                {
                    MessageBox.Show("Khách hàng này tồn tại đơn hàng, không thể xóa!");
                    return;
                }
                if (anCus != null)
                {
                    context.TblKhachHangs.Remove(anCus);
                    if (context.SaveChanges() > 0)
                    {
                        MessageBox.Show("Delete sucess!");
                        LoadData();
                    }
                }
            }
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            openForm(true);
        }

        private void rdFemale_CheckedChanged(object sender, EventArgs e)
        {
            //rdMale.Checked = !rdFemale.Checked;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            using (MyOrderContext context = new MyOrderContext())
            {
                try
                {
                    Boolean check = dgCustomer.CurrentRow.Cells[2].Value.ToString().Equals("Nam") ? true : false;
                    TblKhachHang khachhang = context.TblKhachHangs.FirstOrDefault(x => x.MaKh.Equals(txtCode.Text));
                    if (khachhang != null)
                    {
                        khachhang.TenKh = txtName.Text;
                        khachhang.Diachi = txtAddress.Text;
                        khachhang.NgaySinh = DateTime.Parse(txtDoB.Text);
                        if (check)
                        {
                            khachhang.Gt = false;
                        }
                        else
                        {
                            khachhang.Gt = true;
                        }
                        if (context.SaveChanges() > 0)
                        {
                            MessageBox.Show("Update success! ");
                            LoadData();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Update error! " + ex.Message);
                }
            }
            blockForm();
        }


    }


}
