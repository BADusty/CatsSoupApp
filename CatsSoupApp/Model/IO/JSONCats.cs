using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace CatsSoupApp.Model.IO
{
    public class JSONCats : ICatSerialize
    {
        public string FileExtension => ".json";

        public void Save(string path, IEnumerable<Cat> cats)
        {
            var options = new JsonSerializerOptions { WriteIndented = true };
            File.WriteAllText(path, JsonSerializer.Serialize(cats, options));
        }

        public IEnumerable<Cat> Load(string path)
        {
            var json = File.ReadAllText(path);
            return JsonSerializer.Deserialize<List<Cat>>(json) ?? new List<Cat>();
        }
    }
}
