using QuanLyTraSua.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTraSua.DAO
{
    public class TableDAO
    {
        public static int TableWidth = 85;
        public static int TableHeight = 85;

        private static TableDAO instance;

        public static TableDAO Instance {
            get { if (instance == null) instance = new TableDAO(); return TableDAO.instance; }
            private set { TableDAO.instance = value; }
        }

        private TableDAO() { }

        public List<Table> LoadTableList()
        {
            List<Table> tableList = new List<Table>();

            DataTable data = DataProvider.Instance.ExecuteQuery("USP_GetTableList");

            foreach (DataRow item in data.Rows)
            {
                Table table = new Table(item);
                tableList.Add(table);
            }

            return tableList;
        }

        public void SwitchTable(int id1, int id2)
        {
            DataProvider.Instance.ExecuteQuery("USP_SwitchTable @idTable1 , @idTable2", new object[] { id1, id2 });

        }

        public bool InsertTable(string name)
        {
            string query = string.Format("INSERT dbo.TableFood   (name) VALUES ( N'{0}')", name);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }


        public bool UpdateTable(string name, int id)
        {
            string query = string.Format("UPDATE dbo.TableFood  SET name = N'{0}' WHERE id = {1}", name, id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }

        public bool DeleteTable(int id)
        {

            string query = string.Format("DELETE dbo.TableFood WHERE id = {0}", id);
            int result = DataProvider.Instance.ExecuteNonQuery(query);

            return result > 0;
        }


        public List<Table> SearchTableByStatus(string status)
        {

            List<Table> listTable = new List<Table>();
            string query = string.Format("SELECT * FROM dbo.TableFood  WHERE dbo.fuConvertToUnsign1(status) LIKE N'%' + dbo.fuConvertToUnsign1(N'{0}') + '%' ", status);

            DataTable data = DataProvider.Instance.ExecuteQuery(query);
            foreach (DataRow row in data.Rows)
            {
                Table table = new Table(row);
                listTable.Add(table);
            }

            return listTable;
        }
    }
}
