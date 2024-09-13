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
    public partial class fInfomation : Form
    {

        private Account loginAccount;



        public Account LoginAccount
        {
            get { return loginAccount; }
            set { loginAccount = value; ChangeAccount(loginAccount); }


        }
        public fInfomation( Account acc)
        {
            InitializeComponent();

            LoginAccount = acc;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        void ChangeAccount(Account acc)
        {
            txbLoginName.Text = LoginAccount.UserName;
            txbDisplayName.Text = LoginAccount.DisplayName;
        }
        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdataAccount();
        }

        void UpdataAccount()
        {
            string displayName = txbDisplayName.Text;
            string password = txbPassword.Text;
            string newPassword = txbNewPassword.Text;
            string confirmPassword = txbComfirm.Text;
            string userName = txbLoginName.Text;

            if(!newPassword.Equals(confirmPassword))
            {
                MessageBox.Show("Vui lòng nhập lại mật khẩu đúng với mất khẩu mới");
            }
            else
            {
                if(AccountDAO.Instance.UpdateAccount(userName, displayName, password, newPassword))
                {
                    MessageBox.Show("Cập nhật thành công");
                    if (updateAccount != null)
                        updateAccount(this, new AccountEvent(AccountDAO.Instance.GetAccountByUserName(userName)));

                }
                else
                {
                    MessageBox.Show("Vui lòng điền đúng mật khẩu");
                }
            }
        }

        private event EventHandler<AccountEvent> updateAccount;

        public event EventHandler<AccountEvent> UpdateAccount
        {
            add { updateAccount += value; }
            remove { updateAccount -= value; }
        }


    }

    public class AccountEvent : EventArgs
    {
        private Account acc;

        public Account Acc { get => acc; set => acc = value; }

        public AccountEvent(Account acc)
        {
            this.Acc = acc;
        }
    }
}
