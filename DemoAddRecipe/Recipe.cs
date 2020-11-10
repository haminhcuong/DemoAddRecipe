using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodRecipe.DTO
{
    class Recipe
    {
        public List<Step> StepList { get; set; }

        public Recipe() { }

        public Recipe(List<Step> stepList)
        {
            this.StepList = stepList;
        }

    }
}
