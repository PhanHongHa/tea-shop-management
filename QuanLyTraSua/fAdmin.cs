using QuanLyTraSua.DAO;
using QuanLyTraSua.DTO;
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
using System.Xml.Linq;

namespace QuanLyTraSua
{
    public partial class fAdmin : Form
    {
        BindingSource foodList = new BindingSource();


        BindingSource categoryList = new BindingSource();

        BindingSource tableList = new BindingSource();

        BindingSource accountList = new BindingSource();

        public Account loginAccount;
        public fAdmin()
        {
            InitializeComponent();
            Load();
        }


        #region Methods
        void Load()
        {
            dtgvFood.DataSource = foodList;
            dtgvCategory.DataSource = categoryList;
            dtgvTable.DataSource = tableList;
            dtgvAccount.DataSource = accountList;

            LoadListFood();
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
            LoadDateTmePickerBill();
            AddFoodBinding();
            LoadCategoryIntoCombobox(cbCategory);
            LoadListCategory();
            AddCategoryBinding();
            LoadListTable();
            AddTableBinding();
            LoadAccount();
            AddAccountBinding();
        }


        void LoadListBillByDate(DateTime checkIn, DateTime checkOut)
        {
            dtgvBill.DataSource = BillDAO.Instance.GetBillListByDate(checkIn, checkOut);
        }

        void LoadDateTmePickerBill()
        {
            DateTime today = DateTime.Now;
            dtpkFromDate.Value = new DateTime(today.Year, today.Month, 1);
            dtpkToDate.Value = dtpkFromDate.Value.AddMonths(1).AddDays(-1);
        }


        private void LoadListFood()
        {
            foodList.DataSource = FoodDAO.Instance.GetListFood();
            
            dtgvFood.Columns["Name"].DisplayIndex = 0;
            dtgvFood.Columns["Name"].HeaderText = "Tên món";

            dtgvFood.Columns["Price"].DisplayIndex = 3;
            dtgvFood.Columns["Price"].HeaderText = "Giá";

            dtgvFood.Columns["IdCategory"].HeaderText = "Id Loại";


        }

        void AddFoodBinding()
        {
            txbFoodName.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Name", true , DataSourceUpdateMode.Never));
            txbIdFood.DataBindings.Add(new Binding("Text", dtgvFood.DataSource, "Id",true, DataSourceUpdateMode.Never));
            nmFoodPrice.DataBindings.Add(new Binding("Value", dtgvFood.DataSource, "Price", true, DataSourceUpdateMode.Never));

        }

        void LoadCategoryIntoCombobox(ComboBox cb)
        {
            cb.DataSource = CategoryDAO.Instance.GetListCategory();
            cb.DisplayMember = "Name";
        }


        List<Food> SearchFoodByName(string name)
        {
        
            List<Food> listFood = FoodDAO.Instance.SearchFoodByName(name);

            return listFood;

        }

        private void LoadListCategory()
        {
            categoryList.DataSource = CategoryDAO.Instance.GetListCategory();
            dtgvCategory.Columns["Name"].HeaderText = "Tên Loại";
        }

        void AddCategoryBinding()
        {
          
            txbCategoryName.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbCategoryId.DataBindings.Add(new Binding("Text", dtgvCategory.DataSource, "Id", true, DataSourceUpdateMode.Never));

        }


        private void LoadListTable()
        {
            tableList.DataSource = TableDAO.Instance.LoadTableList();
            dtgvTable.Columns["Name"].HeaderText = "Tên bàn";
            dtgvTable.Columns["Status"].HeaderText = "Trạng thái";
        }

        void AddTableBinding()
        {

            txbTableName.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Name", true, DataSourceUpdateMode.Never));
            txbTableId.DataBindings.Add(new Binding("Text", dtgvTable.DataSource, "Id", true, DataSourceUpdateMode.Never));
            txbTableStatus.DataBindings.Add(new Binding("Text", dtgvTable.DataSource,"Status", true, DataSourceUpdateMode.Never));
        }

        List<Category> SearchCategoryByName(string name)
        {

            List<Category> listCategory = CategoryDAO.Instance.SearchCategoryByName(name);

            return listCategory;

        }

        private object SearchTableByStatus(string status)
        {
            List<Table> listTable = TableDAO.Instance.SearchTableByStatus(status);

            return listTable;
        }


        private void LoadAccount()
        {
            accountList.DataSource = AccountDAO.Instance.GetListAccount();
            dtgvAccount.Columns["UserName"].HeaderText = "Tên tài khoản";
            dtgvAccount.Columns["DisplayName"].HeaderText = "Tên hiển thị";
            dtgvAccount.Columns["Type"].HeaderText = "Loại tài khoản";
        }

        void AddAccountBinding()
        {

            txbAccountName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "UserName", true, DataSourceUpdateMode.Never));
            txbDisplayName.DataBindings.Add(new Binding("Text", dtgvAccount.DataSource, "DisplayName", true, DataSourceUpdateMode.Never));
            nmAccountType.DataBindings.Add(new Binding("Value", dtgvAccount.DataSource, "Type", true, DataSourceUpdateMode.Never));
        }


        private object SearchAccountByDisplayName(string displayName)
        {
            List<Account> listAccount = AccountDAO.Instance.SearchAccountByDisplayName(displayName);

            return listAccount;
        }



        private void ResetPassword(string userName)
        {
            if (AccountDAO.Instance.ResetPassword(userName))
            {
                MessageBox.Show("Đặt lại tài khoản thành công");

            }
            else
            {
                MessageBox.Show("Đặt lại mật khẩu thành công");
            }
        }
        #endregion




        #region Events
        private void btnView_Click(object sender, EventArgs e)
        {
            LoadListBillByDate(dtpkFromDate.Value, dtpkToDate.Value);
        }


        private void btnViewFood_Click(object sender, EventArgs e)
        {
            LoadListFood();
        }

        private void txbFood_TextChange(object sender, EventArgs e)
        {
            try
            {
                if (dtgvFood.SelectedCells.Count > 0)
                {
                    int id = (int)dtgvFood.SelectedCells[0].OwningRow.Cells["IdCategory"].Value;

                    Category category = CategoryDAO.Instance.GetCategoryById(id);

                    cbCategory.SelectedItem = category;

                    int index = -1;
                    int i = 0;
                    foreach (Category item in cbCategory.Items)
                    {
                        if (item.Id == category.Id)
                        {
                            index = i;
                            break;
                        }
                        i++;
                    }

                    cbCategory.SelectedIndex = index;
                }

            }
            catch { }
            
            

            
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categpryId = (cbCategory.SelectedItem as Category).Id;
            float price = (float)nmFoodPrice.Value;

            if (FoodDAO.Instance.InsertFood(name, categpryId, price))
            {
                MessageBox.Show("Thêm món thành công");
                //LoadListFood();

                if (insertFood != null)
                    insertFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món");
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            string name = txbFoodName.Text;
            int categpryId = (cbCategory.SelectedItem as Category).Id;
            float price = (float)nmFoodPrice.Value;

            int id = Convert.ToInt32(txbIdFood.Text);

            if (FoodDAO.Instance.UpdateFood(name, categpryId, price, id))
            {
                MessageBox.Show("Sửa món thành công");
                LoadListFood();

                if (updateFood != null)
                    updateFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa món");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbIdFood.Text);

            if (FoodDAO.Instance.DeleteFood(id))
            {
                MessageBox.Show("Xóa món thành công");
                LoadListFood();

                if(deleteFood != null)
                    deleteFood(this, new EventArgs());
            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa món");
            }
        }


        private event EventHandler insertFood;
        public event EventHandler InsertFood 
        {
            add { insertFood += value; } 
            remove { insertFood -= value; }
        }

        private event EventHandler deleteFood;
        public event EventHandler DeleteFood
        {
            add { deleteFood += value; }
            remove { deleteFood -= value; }
        }

        private event EventHandler updateFood;
        public event EventHandler UpdateFood
        {
            add { updateFood += value; }
            remove { updateFood -= value; }
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {
            
            foodList.DataSource= SearchFoodByName(txbSearchFood.Text);
        }


        private void btnViewCategory_Click(object sender, EventArgs e)
        {
            LoadListCategory();
        }

        private void btnViewTable_Click(object sender, EventArgs e)
        {
            LoadListTable();
        }


        private void btnAddCategory_Click(object sender, EventArgs e)
        {
            string name = txbCategoryName.Text;
         
           

            if (CategoryDAO.Instance.InsertCategory(name))
            {
                MessageBox.Show("Thêm loại món thành công");
                LoadListCategory();

                
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm món");
            }
        }

        private void btnDeleteCategory_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbCategoryId.Text);
            try
            {
                if (CategoryDAO.Instance.DeleteCategory(id))
                {
                    MessageBox.Show("Xóa loại món thành công");
                    LoadListCategory();


                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa loại món");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message + " n/ Không xóa được loại ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

           
        }

        private void btnUpdateCategory_Click(object sender, EventArgs e)
        {

            string name = txbCategoryName.Text;
           

            int id = Convert.ToInt32(txbCategoryId.Text);

            if (CategoryDAO.Instance.UpdateCategory(name,  id))
            {
                MessageBox.Show("Sửa loại món thành công");
                LoadListCategory();

                
            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa loại món");
            }

        }

        private void btnSearchCategory_Click(object sender, EventArgs e)
        {
            categoryList.DataSource = SearchCategoryByName(txbCategorySearch.Text);
        }



        private void btnAddTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;



            if (TableDAO.Instance.InsertTable(name))
            {
                MessageBox.Show("Thêm bàn thành công");
                LoadListTable();


            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm bàn");
            }
        }

        private void btnDeleteTable_Click(object sender, EventArgs e)
        {
            int id = Convert.ToInt32(txbTableId.Text);
            try
            {
                if (TableDAO.Instance.DeleteTable(id))
                {
                    MessageBox.Show("Xóa bàn thành công");
                    LoadListTable();


                }
                else
                {
                    MessageBox.Show("Có lỗi khi xóa loại món");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi: " + ex.Message + " n/ Không xóa được bàn ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void btnUpdataTable_Click(object sender, EventArgs e)
        {
            string name = txbTableName.Text;


            int id = Convert.ToInt32(txbTableId.Text);

            if (TableDAO.Instance.UpdateTable(name, id))
            {
                MessageBox.Show("Sửa bàn thành công");
                LoadListTable();


            }
            else
            {
                MessageBox.Show("Có lỗi khi sửa bàn");
            }
        }

        private void btnSearchTable_Click(object sender, EventArgs e)
        {
            tableList.DataSource = SearchTableByStatus(txbSearchTable.Text);
        }

        private void btnViewAccount_Click(object sender, EventArgs e)
        {
            LoadAccount();
        }



        private void btnAddAccount_Click(object sender, EventArgs e)
        {
            string userName = txbAccountName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nmAccountType.Value;

            if (AccountDAO.Instance.InsertAccount(userName, displayName, type))
            {
                MessageBox.Show("Thêm tài khoản thành công");
                LoadAccount();
            }
            else
            {
                MessageBox.Show("Có lỗi khi thêm tài khoản");
            }
        }

        private void btnDeleteAccount_Click(object sender, EventArgs e)
        {
            string userName = txbAccountName.Text;


            if (loginAccount.UserName.Equals(userName))
            {
                MessageBox.Show("Vui lòng đừng xóa chính bạn chứ!");
                return;
            }
            if (AccountDAO.Instance.DeleteAccount(userName))
            {
                MessageBox.Show("Xóa tài khoản thành công");
                LoadAccount();


            }
            else
            {
                MessageBox.Show("Có lỗi khi xóa tài khoản");
            }
        }

        private void btnUpdateAccount_Click(object sender, EventArgs e)
        {
            string userName = txbAccountName.Text;
            string displayName = txbDisplayName.Text;
            int type = (int)nmAccountType.Value;



            if (AccountDAO.Instance.UpdateAccount(userName, displayName, type))
            {
                MessageBox.Show("Sửa tài khoản thành công");
                LoadAccount();


            }
            else
            {
                MessageBox.Show("Tên tài khoản không được sửa!");
            }
        }

        private void btnSearchAccount_Click(object sender, EventArgs e)
        {
            accountList.DataSource = SearchAccountByDisplayName(txbSearchAccount.Text);
        }

        private void btnResetPassword_Click(object sender, EventArgs e)
        {
            string userName = txbAccountName.Text;
            ResetPassword(userName);
        }



        #endregion


    }
}
