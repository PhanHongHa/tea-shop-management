using DocuSign.eSign.Model;
using QuanLyTraSua.DAO;
using QuanLyTraSua.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuanLyTraSua
{
    public partial class fManagerMilkTea : Form
    {

        private Account loginAccount;

        

        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount.Type); }

            
        }

        public fManagerMilkTea(Account acc)
        {
            InitializeComponent();

            this.LoginAccount = acc;

            LoadTable();

            LoadCategory();

            LoadComBoBoxTable(cbSwitchTable);

            
        }

       

        #region Method




        void LoadCategory()
        {
            List<Category> listCategory = CategoryDAO.Instance.GetListCategory(); 
            cbCategory.DataSource = listCategory;

            cbCategory.DisplayMember = "Name";
        }

        void LoadListFoodByIdCategory(int id)
        {
            List<Food> listFood = FoodDAO.Instance.GetFoodByIdCategory(id);
            cbFoodName.DataSource = listFood;


            cbFoodName.DisplayMember = "Name";
            
        }

        void LoadTable()
        {

            flpTable.Controls.Clear();
            List<Table> tableList = TableDAO.Instance.LoadTableList();

            foreach (Table item in tableList)
            {
                Button btn = new Button()
                {
                    Width = TableDAO.TableWidth,
                    Height = TableDAO.TableHeight
                };

                btn.Click += btn_Click;
                btn.Tag = item;

                switch (item.Status) 
                {
                    case "Trống":
                        btn.BackColor = Color.AntiqueWhite;
                        break;
                    default:
                        btn.BackColor = Color.DeepPink;
                        break;

                }


                flpTable.Controls.Add(btn);

                btn.Text = item.Name + Environment.NewLine + item.Status;

            }
        }

        void ShowBill(int id)
        {
            

            List<DTO.Menu> listBillInfo = MenuDAO.Instance.GetListMenuByTable(id);

            lsvBill.Items.Clear();

            float toTalPrice =0;

            foreach (DTO.Menu item in listBillInfo)
            {
                ListViewItem lsvItem = new ListViewItem(item.FoodName.ToString());
                lsvItem.SubItems.Add(item.Count.ToString());
                lsvItem.SubItems.Add(item.Price.ToString());
                lsvItem.SubItems.Add(item.TotalPrice.ToString());

                toTalPrice += item.TotalPrice;

                lsvBill.Items.Add(lsvItem);
            }

            txbTotalPrice.Text = toTalPrice.ToString("c");
        }

        void LoadComBoBoxTable(ComboBox cb)
        {
            cb.DataSource = TableDAO.Instance.LoadTableList();
            cb.DisplayMember = "Name";
        }


        void ChangeAccount(int type)
        {
            adminToolStripMenuItem.Enabled = type == 1;
            infoToolStripMenuItem.Text += "(" + loginAccount.DisplayName + ")";
            
        }


        #endregion

        #region Event

        private void btn_Click(object sender, EventArgs e)
        {
            int idTable = ((sender as Button).Tag as Table).Id;
            lsvBill.Tag = (sender as Button).Tag;
            ShowBill(idTable);
           
        }
        private void logoutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void infomationToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            fInfomation f = new fInfomation( LoginAccount);

            f.UpdateAccount += f_UpdateAccount;
            f.ShowDialog();
        }

        private void f_UpdateAccount(object sender, AccountEvent e)
        {
            infoToolStripMenuItem.Text = "Thông tin tài khoản (" + e.Acc.DisplayName + ")";
        }

        private void adminToolStripMenuItem_Click(object sender, EventArgs e)
        {
            fAdmin fAdmin = new fAdmin();
            fAdmin.loginAccount = LoginAccount;
            fAdmin.InsertFood += f_InsertFood;
            fAdmin.DeleteFood += f_DeleteFood;
            fAdmin.UpdateFood += f_UpdateFood;

            fAdmin.ShowDialog();
        }

        private void f_UpdateFood(object sender, EventArgs e)
        {
            LoadListFoodByIdCategory((cbCategory.SelectedItem as Category).Id);
            if(lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).Id);
        }

        private void f_DeleteFood(object sender, EventArgs e)
        {
            LoadListFoodByIdCategory((cbCategory.SelectedItem as Category).Id);
            if(lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).Id);
            LoadTable();
        }

        private void f_InsertFood(object sender, EventArgs e)
        {
            LoadListFoodByIdCategory((cbCategory.SelectedItem as Category).Id);
            if (lsvBill.Tag != null)
                ShowBill((lsvBill.Tag as Table).Id);
        }

        private void cbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {

            int id = 0;

            ComboBox cb = sender as ComboBox;

            if (cb.SelectedIndex == null)
                return;

            Category selected = cb.SelectedItem as Category;
            id = selected.Id;

            LoadListFoodByIdCategory(id);
        }

        private void btnAddFood_Click(object sender, EventArgs e)
        {


            Table table = lsvBill.Tag as Table;

            if( table == null)
            {
                MessageBox.Show("Hãy chọn bàn");
                return;
            }    


            int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(table.Id);
            int idFood = (cbFoodName.SelectedItem as Food).Id;
            int count = (int)nmFoodCount.Value;

            if(idBill == -1)
            {
                BillDAO.Instance.InsertBill(table.Id);
                BillInfoDAO.Instance.InsertBillInfo(BillDAO.Instance.GetMaxIdBill(), idFood, count);
            }
            else
            {
                BillInfoDAO.Instance.InsertBillInfo(idBill, idFood, count);
            }

            ShowBill(table.Id);
            LoadTable();
        }

        private void btnCheckOut_Click(object sender, EventArgs e)
        {
            Table table = lsvBill.Tag as Table;
            int idBill = BillDAO.Instance.GetUnCheckBillIDByTableID(table.Id);
            int discount = (int)nmDiscount.Value;

            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
          

            if (idBill != -1)
            {
                if (MessageBox.Show("Bạn có muốn thanh toán hóa đơn cho bàn " + table.Name, "Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
                {
                    BillDAO.Instance.CheckOut(idBill, discount, (float)totalPrice);

                    ShowBill(table.Id);
                    LoadTable();
                }
            }
        }


        private void btnDiscount_Click(object sender, EventArgs e)
        {
            
            int discount = (int)nmDiscount.Value;

            double totalPrice = Convert.ToDouble(txbTotalPrice.Text.Split(',')[0]);
            double finalTotalPrice = totalPrice - (totalPrice / 100) * discount;

            txbTotalPrice.Text = finalTotalPrice.ToString("c");
        }

        private void btnSwitchTable_Click(object sender, EventArgs e)
        {
           

            int id1 = (lsvBill.Tag as Table).Id; 
            int id2 = (cbSwitchTable.SelectedItem as Table).Id;

            if (MessageBox.Show(string.Format("Bạn có muốn chuyển từ bàn {0} qua bàn {1}", (lsvBill.Tag as Table).Name, (cbSwitchTable.SelectedItem as Table).Name),"Thông báo", MessageBoxButtons.OKCancel) == System.Windows.Forms.DialogResult.OK)
            {
                TableDAO.Instance.SwitchTable(id1, id2);

                LoadTable();
            }

            
        }
        #endregion


    }
}
