using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatsSoupApp.Model
{
    public class Cat : INotifyPropertyChanged
    {
        public int ID { get; set; }

        private string _catName { get; set; }
        public string CatName
        {
            get => _catName;
            set
            {
                _catName = value;
                OnPropertyChanged(nameof(CatName));
            }
        }

        private int _catGrade { get; set; }
        public int CatGrade
        {
            get => _catGrade;
            set
            {
                _catGrade = value;
                OnPropertyChanged(nameof(CatGrade));
            }
        }

        private int _catHearts { get; set; }
        public int CatHearts
        {
            get => _catHearts;
            set
            {
                _catHearts = value;
                OnPropertyChanged(nameof(CatHearts));
            }
        }

        private string _catSkill { get; set; }
        public string CatSkill
        {
            get => _catSkill;
            set
            {
                _catSkill = value;
                OnPropertyChanged(nameof(CatSkill));
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string name) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
