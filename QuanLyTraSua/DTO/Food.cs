using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLyTraSua.DTO
{
    public class Food
    {

        private int id;
        private string name;
        private int idCategory;
        private float price;

        public float Price { get => price; set => price = value; }
        public int IdCategory { get => idCategory; set => idCategory = value; }
        public string Name { get => name; set => name = value; }
        public int Id { get => id; set => id = value; }
        

        public Food(int id, string name, int idFoodCategory, float price) 
        {
            this.Id = id;
            this.Name = name;
           
            this.Price = price;
            this.IdCategory = idFoodCategory;

        }

        public Food(DataRow row)
        {
            this.Id = (int)row["id"];
            this.Name = row["name" ].ToString();
         
            this.Price = (float)Convert.ToDouble(row["price"].ToString());
            this.IdCategory =(int)row["idCategory"];

        }


    }
}
