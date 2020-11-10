using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodRecipe.DTO
{
    class FoodImage
    {
        public string ImagePath { get; set; }

        public FoodImage() { }

        public FoodImage(string imagePath)
        {
            this.ImagePath = imagePath;
        }

        public FoodImage(DataRow row) //row = Ordinal|ImagePath
        {
            this.ImagePath = (string)row["ImagePath"];
        }

        
    }
}
