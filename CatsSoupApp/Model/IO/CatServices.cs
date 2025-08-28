using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsSoupApp.Model.IO
{
    public class CatServices
    {
        private readonly Dictionary<string, ICatSerialize> _serializers;

        public CatServices(IEnumerable<ICatSerialize> serializers)
        {
            _serializers = new Dictionary<string, ICatSerialize>();
            foreach (var serializer in serializers)
            {
                _serializers[serializer.FileExtension.ToLower()] = serializer;
            }
        }

        public void Save(string path, IEnumerable<Cat> cats)
        {
            var ext = System.IO.Path.GetExtension(path).ToLower();
            if (_serializers.ContainsKey(ext))
                _serializers[ext].Save(path, cats);
            else
                throw new System.NotSupportedException($"File type {ext} not supported");
        }

        public IEnumerable<Cat> Load(string path)
        {
            var ext = System.IO.Path.GetExtension(path).ToLower();
            if (_serializers.ContainsKey(ext))
                return _serializers[ext].Load(path);
            else
                throw new System.NotSupportedException($"File type {ext} not supported");
        }
    }

    public interface ICatSerialize
    {
        string FileExtension { get; }
        void Save(string path, IEnumerable<Cat> cats);
        IEnumerable<Cat> Load(string path);
    }
}
