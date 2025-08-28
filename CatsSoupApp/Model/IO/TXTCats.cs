using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace CatsSoupApp.Model.IO
{
    public class TXTCats : ICatSerialize
    {
        public string FileExtension => ".txt";

        public void Save(string path, IEnumerable<Cat> cats)
        {
            var lines = cats.Select(c => $"{c.CatName},{c.CatSkill},{c.CatGrade},{c.CatHearts}");
            File.WriteAllLines(path, lines);
        }

        public IEnumerable<Cat> Load(string path)
        {
            var lines = File.ReadAllLines(path);
            var cats = new List<Cat>();
            int i = 1;

            foreach (var line in lines)
            {
                var parts = line.Split(',');
                if (parts.Length == 4)
                {
                    cats.Add(new Cat
                    {
                        ID = i,
                        CatName = parts[0].Trim(),
                        CatSkill = parts[1].Trim(),
                        CatGrade = int.TryParse(parts[2], out var g) ? g : 0,
                        CatHearts = int.TryParse(parts[3], out var h) ? h : 0
                    });
                    i += 1;
                }
            }

            return cats;
        }
    }
}
