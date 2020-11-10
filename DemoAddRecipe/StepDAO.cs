using FoodRecipe.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodRecipe.DAO
{
    class StepDAO
    {
        private static StepDAO instance = null;

        public static StepDAO Instance
        {
            get
            {
                if (instance == null)
                    instance = new StepDAO();
                return instance;
            }

            set => instance = value;
        }

        private StepDAO() { }

        /// <summary>
        /// Lấy dữ liệu các bước làm của món ăn từ databasse SQL Server
        /// Bước 0 là phần giới thiệu món ăn
        /// </summary>
        /// <param name="foodID"></param>
        /// <returns>List các bước</returns>
        public List<Step> GetAllAtRecipe(int foodID)
        {
            List<Step> stepList = new List<Step>();

            string query = "EXEC GetAllStepAtRecipe @foodID";

            try
            {
                DataTable data = DataProvider.Instance.ExcuteQuery(query, new object[] { foodID });
                foreach (DataRow row in data.Rows)
                {
                    int stepIndex = data.Rows.IndexOf(row);
                    List<FoodImage> foodImageList = FoodImageDAO.Instance.GetAllAtStep(foodID, stepIndex);

                    Step Step = new Step(row, foodImageList);
                    stepList.Add(Step);
                }
            }
            catch (Exception e)
            {
                throw new Exception("Excute GetAllStepAtRecipe failed", e);
            }

            return stepList;
        }


        /// <summary>
        /// Thêm list các bước làm vào database SQL Server
        /// </summary>
        /// <param name="foodID"></param>
        /// <param name="stepList"></param>
        /// <returns>Số bước thêm thành công</returns>
        public int InsertList(int foodID, List<Step> stepList)
        {
            string query = "exec InsertStep @foodID , @step , @content";
            int successRows = 0;
            foreach (var item in stepList)
            {
                successRows += DataProvider.Instance.ExcuteNonQuery(query, new object[] { foodID, stepList.IndexOf(item), item.Content });
                FoodImageDAO.Instance.InsertList(foodID, stepList.IndexOf(item), item.FoodImageList);
            }

            return successRows;
        }

    }
}
