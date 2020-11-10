using FoodRecipe.DAO;
using FoodRecipe.DTO;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DemoAddRecipe
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public string mainImagePath = "";
        public string stepImagePath = "";
        string directoryPath = "";
        Step tempImageGrid;

        Food food;
        Recipe recipe;
        BindingList<Ingredient> ingredients = new BindingList<Ingredient>();
        BindingList<Step> steps = new BindingList<Step>();
        List<FoodImage> image;



        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            productsListView.ItemsSource = steps;
        }

        private void addRecipeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (txtStepDescription.Text == "")
            {
                MessageBox.Show("Please complete all information", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            steps.Add(new Step(txtStepDescription.Text, image));

            txtStepDescription.Text = "";
            tempImageGrid.FoodImageList = new List<FoodImage>();
        }

        private void saveRecipeBtn_Click(object sender, RoutedEventArgs e)
        {
            if (IsNumber(txtRation.Text) == false)
            {
                MessageBox.Show("Ration is a number", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            recipe = new Recipe(new List<Step>(steps));
            recipe.StepList.Insert(0, new Step(txtDescription.Text, new List<FoodImage> { new FoodImage(ConvertLinkYoutube(txtYoutubeLink.Text)) }));
            food = new Food(0, txtName.Text, txtType.Text, txtArea.Text, new DateTime(2020, 1, 1), false,"", int.Parse(txtRation.Text), recipe, new List<Ingredient>(ingredients));

            if (txtName.Text == "" || txtDescription.Text == "" || txtType.Text == "" || txtYoutubeLink.Text == "" || txtArea.Text == "" || txtRation.Text == "" || food.Ingredients.Count == 0 || food.Recipe.StepList.Count == 1)
            {
                MessageBox.Show("Please complete all information", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            SaveImageToFolder();

            FoodDAO.Instance.Insert(food);

            MessageBox.Show("Add New Food Recipe Successfully", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);

            this.Close();

        }

        private void addMainImageBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == true)
            {
                mainImagePath = openFileDialog.FileName;
                var bitmap = new BitmapImage(new Uri(mainImagePath, UriKind.Absolute));
                MainFoodImage.Source = bitmap;
            }

            MainFoodImage.Visibility = System.Windows.Visibility.Visible;
        }

        private void addStepImageBtn_Click(object sender, RoutedEventArgs e)
        {

            var screen = new OpenFileDialog();
            screen.Multiselect = true;

            if (screen.ShowDialog() == true)
            {
                var files = screen.FileNames;
                image = new List<FoodImage>();

                foreach (var file in files)
                {
                    image.Add(new FoodImage(file));
                }

                tempImageGrid = new Step("", image);

                ImagesGrid.DataContext = tempImageGrid;
            }
        }

        private void addIngredientBtn_Click(object sender, RoutedEventArgs e)
        {
            if (txtIngredientName.Text == "" || txtIngredientAmount.Text == "" || txtIngredientUnit.Text == "")
            {
                MessageBox.Show("Please complete all information", "Notification", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (IsNumber(txtIngredientAmount.Text) == false)
            {
                MessageBox.Show("Amount is a number", "Notification", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            ingredients.Add(new Ingredient(txtIngredientName.Text, decimal.Parse(txtIngredientAmount.Text), txtIngredientUnit.Text));

            ListIngredient.Content += String.Format("{0,-15} – {1,3} {2,-10}\n", txtIngredientName.Text, txtIngredientAmount.Text, txtIngredientUnit.Text);

            txtIngredientName.Text = "";
            txtIngredientAmount.Text = "";
            txtIngredientUnit.Text = "";
        }

        public bool IsNumber(string text)
        {
            bool result = true;
            try
            {
                decimal.Parse(text);
            }
            catch
            {
                result = false;
            }
            return result;
        }

        public string ConvertAbsolutePathToRelativePath(string path)
        {
            string absolutePath;
            int temp = path.LastIndexOf("Images");

            absolutePath = path.Substring(temp);

            return absolutePath;
        }

        public string ConvertLinkYoutube(string path)
        {
            string link;
            try
            {
                if (path.Contains("/embed/") == false)
                {
                    int firstIndex = path.IndexOf("=") + 1;
                    int lastIndex = path.IndexOf("&");
                    if (lastIndex != -1)
                    {
                        link = "@https://www.youtube.com/embed/" + path.Substring(firstIndex, lastIndex - firstIndex);
                    }
                    else
                    {
                        link = "@https://www.youtube.com/embed/" + path.Substring(firstIndex);
                    }

                }
                else
                {
                    link = "@" + path;
                }
            }
            catch
            {
                link = @"Images\default.jpg";
            }
            return link;
        }

        public void SaveImageToFolder()
        {
            // Create Folder
            var currentFolder = AppDomain.CurrentDomain.BaseDirectory;
            directoryPath = $"{currentFolder}Images\\{Guid.NewGuid()}";
            if (!System.IO.Directory.Exists(directoryPath))
                System.IO.Directory.CreateDirectory(directoryPath);
            Debug.Write(" ");

            // Add Thumbnail
            var info = new FileInfo(mainImagePath);
            var newName = $"thumbnail{info.Extension}";
            File.Copy(mainImagePath, $"{directoryPath}\\{newName}");

            food.Thumbnail = ConvertAbsolutePathToRelativePath($"{directoryPath}\\{newName}");


            // Add Step Image
            foreach(var step in food.Recipe.StepList.Skip(1))
            {
                foreach(var image in step.FoodImageList)
                {
                    var inFo = new FileInfo(image.ImagePath);
                    var newname = $"{Guid.NewGuid()}{inFo.Extension}";
                    File.Copy(image.ImagePath, $"{directoryPath}\\{newname}");
                    image.ImagePath = ConvertAbsolutePathToRelativePath($"{directoryPath}\\{newname}");
                }
            }
        }
    }
}
