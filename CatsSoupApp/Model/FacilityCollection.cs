using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace CatsSoupApp.Model
{
    public class FacilityCollection : ObservableCollection<Facility>
    {
        private const string FacilityFileName = "facilities.txt";

        public FacilityCollection()
        {
            LoadFacilities();
        }

        private void LoadFacilities()
        {
            if (!File.Exists(FacilityFileName))
            {
                File.WriteAllLines(FacilityFileName, new[]
                {
                    "Soup", "Carrots", "Cabbage", "Juice", "Corn", "Acorns",
                    "Broccoli", "Barbecue", "Lemons", "Honey", "Wheat", "Radishes",
                    "Pumpkins", "Mushrooms", "Celery", "Lotus Root", "Grapes", "Sugarcane",
                    "Strawberries", "Oats", "Garlic", "Pineapples", "Papayas", "Sesame",
                    "Cheese", "Apples", "Red Beans", "Mangosteen", "Asparagus", "Potatoes",
                    "Avocado", "Onion", "Peanut", "Tomatoes", "Olives", "Beets",
                    "Oranges", "Sunflower Seeds", "Starfruit", "Bamboo Shoots", "Basil", "Watermelons",
                    "Eggplants", "Tabasco Peppers", "Pomegranates", "Sweet Potatoes", "Ginseng", "Coffee Beans",
                    "Buckwheat", "Figs", "Peaches", "Bananas", "Dragon Fruit"
                });
            }

            var lines = File.ReadAllLines(FacilityFileName);
            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    Add(new Facility(line.Trim()));
                }
            }
        }
    }
}
