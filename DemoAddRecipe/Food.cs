using FoodRecipe.DAO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodRecipe.DTO
{
    class Food
    {
        public int FoodID { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Area { get; set; }
        public DateTime CreateDate { get; set; }
        public bool IsFavor { get; set; }
        public string Thumbnail { get; set; }
        public int Ration { get; set; }
        public Recipe Recipe { get; set; }
        public List<Ingredient> Ingredients { get; set; }

        public Food() { }

        public Food(int foodID, string name, string type, string area, DateTime createDate, bool isFavor, string thumbnail, int ration, Recipe recipe, List<Ingredient> ingredients)
        {
            FoodID = foodID;
            Name = name;
            Type = type;
            Area = area;
            CreateDate = createDate;
            IsFavor = isFavor;
            Thumbnail = thumbnail;
            Ration = ration;
            Recipe = recipe;
            Ingredients = ingredients;
        }

        public Food(DataRow row, Recipe recipe = null, List<Ingredient> ingredients = null)
        {
            FoodID = (int)row["FoodID"];
            Name = (string)row["FoodName"];
            Type = (string)row["Type"];
            Area = (string)row["Area"];
            CreateDate = (DateTime)row["CreateDate"];
            IsFavor = (bool)row["IsFavor"];
            Ration = (byte)row["Ration"];
            Recipe = recipe;
            Ingredients = ingredients;
        }


        /// <summary>
        /// Gọi khi isFavor thay đổi
        /// </summary>
        public void FavorChanged()
        {
            FoodDAO.Instance.SetFavor(this.FoodID, this.IsFavor);
        }

    }
}
