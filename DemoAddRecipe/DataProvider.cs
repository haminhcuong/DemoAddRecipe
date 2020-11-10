using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodRecipe.DAO
{
    public class DataProvider
    {
        string connectionStr = @"Data Source=.\SQLEXPRESS;Initial Catalog=FoodRecipes;Integrated Security=True";
        //string connectionStr = @"Data Source=LAPTOP-U9GVUPKL;Initial Catalog=FoodRecipes;Integrated Security=True";

        private static DataProvider instance = null;

        // Có thể cải tiến singleton dùng đa thread... sau
        public static DataProvider Instance
        {
            get
            {
                if (instance == null)
                    instance = new DataProvider();
                return instance;
            }
            set => instance = value;
        }

        private DataProvider() { }

        /// <summary>
        /// Truy vấn trên SQL Server
        /// </summary>
        /// <param name="query">Câu truy vấn</param>
        /// <param name="parameter">Tham số khi câu truy vấn là Stored Procedure</param>
        /// <returns>Bảng kết quả</returns>
        public DataTable ExcuteQuery(string query, object[] parameter = null)
        {
            DataTable data = new DataTable();

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (var item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i] ?? DBNull.Value);
                            i++;
                        }
                    }
                }


                SqlDataAdapter adapter = new SqlDataAdapter(command);
                adapter.Fill(data);

                connection.Close();
            }

            return data;
        }

        
        /// <summary>
        /// Truy vấn trên SQL Server không trả về bảng
        /// </summary>
        /// <param name="query">Câu truy vấn</param>
        /// <param name="parameter">Tham số khi câu truy vấn là Stored Procedure</param>
        /// <returns>Số dòng bị ảnh hưởng</returns>
        public int ExcuteNonQuery(string query, object[] parameter = null)
        {
            int successedRows;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (var item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i] ?? DBNull.Value);
                            i++;
                        }
                    }
                }

                successedRows = command.ExecuteNonQuery();

                connection.Close();
            }

            return successedRows;
        }


        
        /// <summary>
        /// Truy vấn trên SQL Server trả về 1 ô
        /// </summary>
        /// <param name="query">Câu truy vấn</param>
        /// <param name="parameter">Tham số khi câu truy vấn là Stored Procedure</param>
        /// <returns>Ô đầu tiên của bảng kết quả</returns>
        public object ExcuteScalar(string query, object[] parameter = null)
        {
            object data;

            using (SqlConnection connection = new SqlConnection(connectionStr))
            {
                connection.Open();

                SqlCommand command = new SqlCommand(query, connection);

                if (parameter != null)
                {
                    string[] listPara = query.Split(' ');
                    int i = 0;
                    foreach (var item in listPara)
                    {
                        if (item.Contains('@'))
                        {
                            command.Parameters.AddWithValue(item, parameter[i] ?? DBNull.Value);
                            i++;
                        }
                    }
                }

                 data = command.ExecuteScalar();

                connection.Close();
            }

            return data;
        }


    }
}
