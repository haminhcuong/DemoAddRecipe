using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodRecipe.DTO
{
    class Ingredient
    {
        public string Name { get; set; }
        public decimal Amount { get; set; }
        public string Unit { get; set; }

        public Ingredient() { }

        public Ingredient(string name, decimal amount, string unit)
        {
            Name = name;
            Amount = amount;
            Unit = unit;
        }

        public Ingredient(DataRow row) // row = FoodID | IngredientName | Amount | Unit
        {
            Name = (string)row["IngredientName"];
            Amount = (decimal)row["Amount"];
            Unit = (string)row["Unit"];
        }

    }
}
