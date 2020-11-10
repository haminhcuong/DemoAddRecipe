using FoodRecipe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodRecipe.DAO
{
    class IngredientDAO
    {
        private static IngredientDAO instance = null;

        public static IngredientDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new IngredientDAO();
                return instance;
            }

            set => instance = value;
        }

        private IngredientDAO() { }

        public List<Ingredient> GetAll(int foodID)
        {
            List<Ingredient> ingredientList = new List<Ingredient>();

            string query = "EXEC GetAllIngredient @foodID";

            try
            {

                DataTable data = DataProvider.Instance.ExcuteQuery(query, new object[] { foodID });
                foreach (DataRow row in data.Rows)
                {
                    Ingredient ingredient = new Ingredient(row);
                    ingredientList.Add(ingredient);
                }

            }
            catch (Exception e)
            {
                throw new Exception("Excute GetAllIngredient failed", e);
            }

            return ingredientList;
        }

        public int InsertList(int foodID, List<Ingredient> ingredianList)
        {
            string query = "exec InsertIngredient @foodID , @name , @amount , @unit";
            int successRows = 0;
            foreach (var item in ingredianList)
            {
                successRows += DataProvider.Instance.ExcuteNonQuery(query, new object[] { foodID, item.Name, item.Amount, item.Unit });
            }

            return successRows;
        }
    }
}
