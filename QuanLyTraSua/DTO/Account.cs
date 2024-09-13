using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTraSua.DTO
{
    public class Account
    {

        private string userName;
        private string password;
        private string displayName;
        private int type;

        public string UserName { get => userName; set => userName = value; }
        public string Password { get => password; set => password = value; }
        public string DisplayName { get => displayName; set => displayName = value; }
        public int Type { get => type; set => type = value; }
    
    
        public Account(string userName, int type, string displayName, string password = null) 
        {
            this.UserName = userName;
            this.Password = password;
            this.DisplayName = displayName;
            this.Type = type;
        

        }

        public Account(DataRow row) 
        {
            this.UserName = row["userName"].ToString();
            this.Password = row["password"].ToString();
            this.DisplayName = row["displayName"].ToString();
            this.Type =  (int)row["type"];

        }
    
    }
}
