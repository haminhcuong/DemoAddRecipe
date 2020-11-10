using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FoodRecipe.DTO
{
    class Step : INotifyPropertyChanged
    {
        public string Content { get; set; }

        public List<FoodImage> FoodImageList { get; set; }

        public Step() { }

        public Step(string content, List<FoodImage> foodImageList)
        {
            this.Content = content;
            this.FoodImageList = foodImageList;
        }

        public Step(DataRow row, List<FoodImage> foodImageList)  //row = FoodID | Step | Content
        {
            this.Content = (string)row["Content"];
            this.FoodImageList = foodImageList;
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
