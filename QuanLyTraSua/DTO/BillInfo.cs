using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTraSua.DTO
{
    public class BillInfo
    {
        private int id;
        private int idFood;
        private int idBill;
        private int count;

        public int Id { get => id; set => id = value; }
        public int IdFood { get => idFood; set => idFood = value; }
        public int IdBill { get => idBill; set => idBill = value; }
        public int Count { get => count; set => count = value; }

        public BillInfo(int id, int idFood, int idBill, int count) 
        {
            this.Id = id;
            this.IdFood = idFood;
            this.IdBill = idBill;
            this.Count = count;
        }

        public BillInfo(DataRow row)
        {
            this.Id = (int)row["id"];
            this.IdBill = (int)row["idBill"];
            this.IdFood = (int)row["idFood"];
            this.Count = (int)row["count"];
        }
    }
}
