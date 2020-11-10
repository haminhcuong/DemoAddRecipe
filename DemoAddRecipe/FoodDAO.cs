using FoodRecipe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace FoodRecipe.DAO
{
    class FoodDAO
    {
        private static FoodDAO instance = null;

        public static FoodDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new FoodDAO();
                return instance;
            }

            set => instance = value;
        }

        private FoodDAO() { }


        /// <summary>
        /// Lấy toàn bộ dữ liệu các món ăn
        /// </summary>
        /// <returns></returns>
        public List<Food> GetAll()
        {
            List<Food> foodList = new List<Food>();

            string query = "select * from FOOD";

            try
            {
                DataTable data = DataProvider.Instance.ExcuteQuery(query);
                foreach (DataRow row in data.Rows)
                {
                    int foodID = (int)row["FoodID"];

                    Recipe recipe = RecipeDAO.Instance.GetRecipe(foodID);

                    List<Ingredient> ingredients = IngredientDAO.Instance.GetAll(foodID);

                    Food food = new Food(row, recipe, ingredients);
                    foodList.Add(food);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Excute GetAllFood failed", e);
            }

            return foodList;
        }


        /// <summary>
        /// Lấy dữ liệu các món ăn (không có thành phần và hướng dẫn nấu)
        /// </summary>
        /// <returns></returns>
        public List<Food> GetAllWithoutDetail()
        {
            List<Food> foodList = new List<Food>();

            string query = "select * from FOOD";

            try
            {
                DataTable data = DataProvider.Instance.ExcuteQuery(query);
                foreach (DataRow row in data.Rows)
                {
                    Food food = new Food(row);
                    foodList.Add(food);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Excute GetAllWithoutDetail failed", e);
            }

            return foodList;
        }


        /// <summary>
        /// Lấy toàn bộ dữ liệu của món ăn qua ID
        /// </summary>
        /// <param name="foodID"></param>
        /// <returns></returns>
        public Food GetByID(int foodID)
        {
            Food result;
            try
            {
                Recipe recipe = RecipeDAO.Instance.GetRecipe(foodID);

                List<Ingredient> ingredients = IngredientDAO.Instance.GetAll(foodID);

                string query = "select * from FOOD where FoodID = @FoodID";

                DataTable data = DataProvider.Instance.ExcuteQuery(query, new object[] { foodID });

                result = new Food(data.Rows[0], recipe, ingredients);
            }
            catch (Exception e)
            {
                throw new Exception("Excute GetByID failed", e);
            }

            return result;
        }


        /// <summary>
        /// Lấy dữ liệu các món ăn trên một trang với điều kiện lọc
        /// </summary>
        /// <param name="pageIndex">Chỉ số trang</param>
        /// <param name="pageElements">Số món ăn trên một trang</param>
        /// <param name="searchName">Tên món ăn tìm kiếm</param>
        /// <param name="type">Chuỗi các loại món ăn</param>
        /// <param name="area">Chuỗi các khu vực</param>
        /// <returns></returns>
        public List<Food> GetPageOfFoodWithFilter(int pageIndex, int pageElements, string searchName = null, string type = null, string area = null, bool isFavor = false)
        {
            List<Food> foodList = new List<Food>();

            string query = "exec GetPageOfFoodWithFilter @pageIndex , @pageElements , @searchName , @type , @area , @isFavor";

            try
            {
                DataTable data = DataProvider.Instance.ExcuteQuery(query, new object[] { pageIndex, pageElements, searchName, type, area, isFavor });
                foreach (DataRow row in data.Rows)
                {
                    Food food = new Food(row);
                    foodList.Add(food);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Excute GetPageOfFoodWithFilter failed", e);
            }

            return foodList;
        }


        /// <summary>
        /// Lấy số món ăn với điều kiện lọc
        /// </summary>
        /// <param name="searchName">Tên món ăn tìm kiếm</param>
        /// <param name="type">Chuỗi các loại món ăn</param>
        /// <param name="area">Chuỗi các khu vực</param>
        /// <returns></returns>
        public int GetNumOfFoodWithFilter(string searchName = null, string type = null, string area = null, bool isFavor = false)
        {
            string query = "exec GetNumOfFoodWithFilter @searchName , @type , @area , @isFavor";

            int numOfFood;
            try
            {
                numOfFood = (int)DataProvider.Instance.ExcuteScalar(query, new object[] { searchName, type, area, isFavor });
            }
            catch (Exception e)
            {
                throw new Exception("Excute GetNumOfFoodWithFilter failed", e);
            }

            return numOfFood;
        }


        /// <summary>
        /// Thêm món ăn vào database
        /// </summary>
        /// <param name="food"></param>
        /// <returns>1 nếu thêm thành công, 0 nếu thêm thất bại</returns>
        public int Insert(Food food)
        {
            string query = "exec InsertFood @name , @isFavor , @type , @area , @thumbnail , @ration";
            int successRows = 0;

            object data = DataProvider.Instance.ExcuteScalar(query, new object[] { food.Name, food.IsFavor, food.Type, food.Area, food.Thumbnail, food.Ration });
            if (data != null)
            {
                food.FoodID = (int)data;
                successRows = 1;
                RecipeDAO.Instance.Insert(food.FoodID, food.Recipe);
                IngredientDAO.Instance.InsertList(food.FoodID, food.Ingredients);
            }

            return successRows;
        }


        /// <summary>
        /// Đặt trạng thái yêu thích của món ăn trong database
        /// </summary>
        /// <param name="foodID"></param>
        /// <param name="isFavor"></param>
        /// <returns>1 nếu thành công, 0 nếu thất bại</returns>
        public int SetFavor(int foodID, bool isFavor)
        {
            int result;
            string query = "exec SetFavor @foodID , @isFavor";

            result = DataProvider.Instance.ExcuteNonQuery(query, new object[] { foodID, isFavor });

            return result;
        }
    }
}
