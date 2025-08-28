using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsSoupApp.Model
{
    public class StationAssigner
    {
        private readonly FacilityCollection _facilities;

        public StationAssigner(FacilityCollection facilities)
        {
            _facilities = facilities;
        }

        public string AssignCats(ObservableCollection<Cat> Cats, string lastUnlockedStation)
        {
            var output = new StringBuilder();

            var unlockedFacilities = _facilities
                .TakeWhile(f => !string.Equals(f._name, lastUnlockedStation, StringComparison.OrdinalIgnoreCase))
                .Concat(_facilities.Where(f => string.Equals(f._name, lastUnlockedStation, StringComparison.OrdinalIgnoreCase)).Take(1))
                .ToList();

            var matchedCats = new HashSet<Cat>();

            foreach (var facility in unlockedFacilities)
            {
                var catsForFacility = Cats
                    .Where(c => string.Equals(c.CatSkill, facility._name, StringComparison.OrdinalIgnoreCase))
                    .OrderByDescending(c => c.CatGrade)
                    .ThenByDescending(c => c.CatHearts)
                    .ToList();

                if (catsForFacility.Count > 0)
                {
                    var displayName = facility._name;
                    if (displayName.Equals("Soup", StringComparison.OrdinalIgnoreCase) ||
                        displayName.Equals("Juice", StringComparison.OrdinalIgnoreCase) ||
                        displayName.Equals("Barbecue", StringComparison.OrdinalIgnoreCase))
                    {
                        displayName = displayName.ToUpper();
                    }
                    output.AppendLine(displayName);
                    foreach (var cat in catsForFacility)
                    {
                        output.AppendLine($"   - {cat.CatName}, {cat.CatGrade}★, {cat.CatHearts}❤️");
                        matchedCats.Add(cat);
                    }
                    output.AppendLine();
                }
            }

            var unmatched = Cats
                    .Except(matchedCats)
                    .OrderByDescending(c => c.CatHearts)
                    .ToList();

            if (unmatched.Count > 0)
            {
                output.AppendLine("== Unmatched Cats ==");
                foreach (var cat in unmatched)
                {
                    output.AppendLine($"   - {cat.CatName}: {cat.CatSkill}, {cat.CatGrade}★, {cat.CatHearts}❤️");
                }
            }

            if (output.Length == 0)
            {
                output.AppendLine("No cats found!");
            }

            return output.ToString();
        }
    }
}
