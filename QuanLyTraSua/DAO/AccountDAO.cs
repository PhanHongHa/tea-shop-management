using QuanLyTraSua.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTraSua.DAO
{
    public class AccountDAO
    {
        private static AccountDAO instance;

        public static AccountDAO Instance
        {
            get { if (instance == null) instance = new AccountDAO(); return AccountDAO.instance; }
            private set { AccountDAO.instance = value; }
        }

        private AccountDAO() { }



        public bool Login(string userName, string passWord)
        {

            //// mã hóa mật khẩu

            //byte[] temp = ASCIIEncoding.ASCII.GetBytes(passWord);
            //byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            //string hasPass = "";

            //foreach (byte item in hasData)
            //{
            //    hasPass += item;
            //}

            ////var list = hasData.ToString();
            ////list.Reverse();

            ////
            

            string query = "USP_Login @userName , @passWord ";

            DataTable result = DataProvider.Instance.ExecuteQuery(query, new object[] {userName, passWord});

            return result.Rows.Count >0;
        }

        public Account GetAccountByUserName(string userName)
        {
            DataTable data = DataProvider.Instance.ExecuteQuery("Select * From account where userName = '" + userName + "'");

            foreach (DataRow item in data.Rows)
            {
                return new Account(item);
            }

            return null;
        }

        public bool UpdateAccount( string userName, string displayName, string password, string newPassword ) 
        {

            //// mã hóa mật khẩu

            //byte[] temp = ASCIIEncoding.ASCII.GetBytes(password);
            //byte[] hasData = new MD5CryptoServiceProvider().ComputeHash(temp);

            //string hasPass = "";

            //foreach (byte item in hasData)
            //{
            //    hasPass += item;
            //}

            ////var list = hasData.ToString();
            ////list.Reverse();

            ////

            string query = "USP_UpdateAccount @userName , @displayName , @password , @newPassword ";

            int result = DataProvider.Instance.ExecuteNonQuery(query, new object[] { userName, displayName, password, newPassword });
            return result > 0;
        }

        public DataTable GetListAccount()
        {

            string query = "SELECT UserName, DisplayName, Type FROM dbo.Account";
            return DataProvider.Instance.ExecuteQuery(query);
        }

        public bool InsertAccount(string userName, string displayName, int type)
        {
            string query = string.Format("INSERT dbo.Account ( userName , displayName , type ) VALUES ( N'{0}', N'{1}', {2} , N'{3} )", userName, displayName, type );
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }


        public bool UpdateAccount(string userName, string displayName, int type)
        {
            string query = string.Format("UPDATE dbo.Account SET displayName = N'{1}' , type = {2} WHERE userName = N'{0}'", userName, displayName, type);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteAccount(string userName)
        {

            string query = string.Format("DELETE dbo.Account WHERE userName = N'{0}'", userName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }


        public List<Account> SearchAccountByDisplayName(string displayName)
        {

            List<Account> listAcccount = new List<Account>();
            string query = string.Format("SELECT * FROM dbo.Account WHERE dbo.fuConvertToUnsign1(displayName) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%' ", displayName);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                Account account = new Account(row);
                listAcccount.Add(account);
            }

            return listAcccount;
        }

        public bool ResetPassword(string userName)
        {
            string query = string.Format("UPDATE dbo.Account SET password = N'1' WHERE userName = N'{0}'", userName);
            int result = DataProvider.Instance.ExecuteNonQuery(query);
            return result > 0;
        }

    }
}
