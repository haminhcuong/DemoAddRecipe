using FoodRecipe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodRecipe.DAO
{
    class FoodImageDAO
    {
        private static FoodImageDAO instance =null;

        public static FoodImageDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new FoodImageDAO();
                return instance;
            }

            set => instance = value;
        }

        private FoodImageDAO() { }


        /// <summary>
        /// Lấy toàn bộ ImagePath của một Step từ database SQL Server
        /// </summary>
        /// <param name="foodID"></param>
        /// <param name="step"></param>
        /// <returns>List FoodImage</returns>
        public List<FoodImage> GetAllAtStep(int foodID, int step)
        {
            List<FoodImage> foodImageList = new List<FoodImage>();

            string query = "EXEC GetAllFoodImageAtStep @foodID , @step";

            try
            {
                DataTable data = DataProvider.Instance.ExcuteQuery(query, new object[] { foodID, step });
                foreach (DataRow row in data.Rows)
                {
                    FoodImage foodImage = new FoodImage(row);
                    foodImageList.Add(foodImage);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Excute GetAllFoodImageAtStep failed", e);
            }

            return foodImageList;
        }

        /// <summary>
        /// Thêm list FoodImage vào database SQL Server
        /// </summary>
        /// <param name="foodID"></param>
        /// <param name="step"></param>
        /// <param name="foodImageList"></param>
        /// <returns>Số dòng thêm thành công</returns>
        public int InsertList(int foodID, int step, List<FoodImage> foodImageList)
        {
            string query = "exec InsertFoodImage @foodID , @step , @ordinal , @imagePath";
            int successRows = 0;
            foreach (var item in foodImageList)
            {
                successRows += DataProvider.Instance.ExcuteNonQuery(query, new object[] { foodID, step, foodImageList.IndexOf(item), item.ImagePath });
            }

            return successRows;
        }
    }
}
