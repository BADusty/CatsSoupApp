using CatsSoupApp.Model;
using CatsSoupApp.Model.IO;
using CatsSoupApp.ViewModel.BaseClass;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.Win32;

namespace CatsSoupApp.ViewModel
{
    class CatListVM : ViewModelBase
    {
        private readonly MainVM _mainVM;
        public ObservableCollection<Cat> Cats { get; }
        public FacilityCollection FacilityCollection { get; }
        private readonly CatServices _catService;

        #region Data Binding
        public string WrittenCatName { get; set; }
        public int SelectedGrade { get; set; } = 1;
        public int SelectedHearts { get; set; } = 1;

        private Facility _selectedFacility;
        public Facility SelectedFacility
        {
            get => _selectedFacility;
            set { _selectedFacility = value; OnPropertyChanged(); }
        }

        public ObservableCollection<int> Grades { get; } = new ObservableCollection<int> { 1, 2, 3, 4, 5 };
        public ObservableCollection<int> Hearts { get; } = new ObservableCollection<int> { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
        #endregion

        #region SelectedCat Bind
        private Cat _selectedCat;
        public Cat SelectedCat
        {
            get => _selectedCat;
            set
            {
                _selectedCat = value;
                OnPropertyChanged();

                if (value != null)
                {
                    WrittenCatName = value.CatName;
                    SelectedGrade = value.CatGrade;
                    SelectedHearts = value.CatHearts;
                    SelectedFacility = FacilityCollection.FirstOrDefault(f => f._name == value.CatSkill);

                    OnPropertyChanged(nameof(WrittenCatName), nameof(SelectedGrade), nameof(SelectedHearts), nameof(SelectedFacility));
                }
            }
        }
        #endregion

        #region SaveCat Command
        private ICommand saveCat;
        public ICommand SaveCat => (saveCat ??= new RelayCommand(
            p =>
            {
                if(SelectedCat != null)
                {
                    SelectedCat.CatName = WrittenCatName;
                    SelectedCat.CatGrade = SelectedGrade;
                    SelectedCat.CatHearts = SelectedHearts;
                    SelectedCat.CatSkill = SelectedFacility._name;
                }

                else
                {
                    var NewCat = new Cat();
                    NewCat.ID = (Cats.Count + 1);
                    NewCat.CatName = WrittenCatName;
                    NewCat.CatGrade = SelectedGrade;
                    NewCat.CatHearts = SelectedHearts;
                    NewCat.CatSkill = SelectedFacility._name;

                    Cats.Add(NewCat);
                }

                ClearForm.Execute(null);
                OnPropertyChanged(nameof(Cats));
            }
            ,
            p => !string.IsNullOrEmpty(WrittenCatName) && SelectedFacility != null
            ));
        #endregion

        #region DeleteCat Command
        private ICommand deleteCat;
        public ICommand DeleteCat => deleteCat ??= new RelayCommand(
            p =>
            {
                if (SelectedCat != null)
                {
                    Cats.Remove(SelectedCat);

                    for (int i = 0; i < Cats.Count; i++)
                    {
                        Cats[i].ID = i + 1;
                    }

                    SelectedCat = null;
                    ClearForm.Execute(null);

                    OnPropertyChanged(nameof(Cats));
                }
            }
            ,
            p => SelectedCat != null
            );
        #endregion

        public ICommand SwitchToAssignView => _mainVM.SwitchToAssignView;

        public CatListVM(MainVM mainVM, ObservableCollection<Cat> cats, FacilityCollection facilities)
        {
            _catService = new CatServices(new ICatSerialize[]
            {
                new JSONCats(),
                new TXTCats()
            });

            _mainVM = mainVM;
            Cats = cats;
            FacilityCollection = facilities;
        }

        #region CancelEdit Command
        private ICommand cancelEdit;
        public ICommand CancelEdit => cancelEdit ??= new RelayCommand(
            p =>
            {
                if(SelectedCat != null)
                {
                    SelectedCat = null;
                    ClearForm.Execute(null);

                    OnPropertyChanged(nameof(Cats));
                }
            }
            ,
            p => true
            );
        #endregion

        #region ClearForm
        private ICommand clearForm;
        public ICommand ClearForm => clearForm ??= new RelayCommand(
            p =>
            {
                WrittenCatName = string.Empty;
                SelectedFacility = null;
                SelectedGrade = SelectedHearts = 1;

                OnPropertyChanged(nameof(SelectedFacility), nameof(SelectedGrade), nameof(SelectedHearts), nameof(WrittenCatName));
            }
            ,
            p => true
            );
        #endregion

        #region IO
        private ICommand saveCommand;
        public ICommand SaveCommand => saveCommand ??= new RelayCommand(
            p =>
            {
                var dlg = new SaveFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|JSON Files (*.json)|*.json",
                    DefaultExt = "txt",
                    AddExtension = true
                };

                if (dlg.ShowDialog() == true)
                {
                    _catService.Save(dlg.FileName, Cats);
                }
            }
            ,
            p => Cats.Count != 0
            );

        public void SaveCats(string path)
        {
            _catService.Save(path, Cats);
        }

        private ICommand loadCommand;
        public ICommand LoadCommand => loadCommand ??= new RelayCommand(
            p =>
            {
                var dlg = new OpenFileDialog
                {
                    Filter = "Text Files (*.txt)|*.txt|JSON Files (*.json)|*.json",
                    Multiselect = false
                };

                if (dlg.ShowDialog() == true)
                {
                    var loadedCats = _catService.Load(dlg.FileName);
                    Cats.Clear();
                    foreach (var cat in loadedCats)
                        Cats.Add(cat);
                }
            }
            ,
            p => true
            );

        public void LoadCats(string path)
        {
            var loadedCats = _catService.Load(path);
            Cats.Clear();
            foreach (var cat in loadedCats)
                Cats.Add(cat);
        }
        #endregion
    }
}
