using FoodRecipe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodRecipe.DAO
{
    class RecipeDAO
    {
        private static RecipeDAO instance = null;

        public static RecipeDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new RecipeDAO();
                return instance;
            }

            set => instance = value;
        }

        private RecipeDAO() { }


        /// <summary>
        /// Lấy dữ liệu hướng dẫn nấu từ database
        /// </summary>
        /// <param name="foodID"></param>
        /// <returns></returns>
        public Recipe GetRecipe(int foodID)
        {
            Recipe recipe;

            try
            {

                List<Step> stepList = StepDAO.Instance.GetAllAtRecipe(foodID);
                recipe = new Recipe(stepList);

            }
            catch (Exception e)
            {
                throw new Exception("Excute GetRecipe failed", e);
            }

            return recipe;
        }


        /// <summary>
        /// Thêm dữ liệu hướng dẫn nấu vào database
        /// </summary>
        /// <param name="foodID"></param>
        /// <param name="recipe"></param>
        /// <returns>Số bước thêm thành công</returns>
        public int Insert(int foodID , Recipe recipe)
        {

            int successRows = 0;

            successRows = StepDAO.Instance.InsertList(foodID, recipe.StepList);

            return successRows;
        }

    }
}
