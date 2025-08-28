using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using CatsSoupApp.Model;
using CatsSoupApp.View;
using CatsSoupApp.ViewModel.BaseClass;

namespace CatsSoupApp.ViewModel
{
    class MainVM : ViewModelBase
    {
        private object _currentView;
        public object CurrentView
        {
            get => _currentView;
            set
            {
                if (_currentView != value)
                {
                    _currentView = value;
                    OnPropertyChanged(nameof(CurrentView));
                }
            }
        }

        public ICommand SwitchToAssignView { get; }
        public ICommand SwitchToListView { get; }

        private readonly CatListView _catListView;
        private readonly CatAssignView _catAssignView;
        private CatListVM _catlistVM;
        private CatAssignVM _catassignVM;
        public ObservableCollection<Cat> Cats { get; } = new();

        private FacilityCollection _facilityCollection = new FacilityCollection();
        public FacilityCollection FacilityCollection
        {
            get => _facilityCollection;
            set { _facilityCollection = value; OnPropertyChanged(); }
        }

        public MainVM()
        {
            _catlistVM = new CatListVM(this, Cats, FacilityCollection);
            _catassignVM = new CatAssignVM(this, Cats, FacilityCollection);

            _catListView = new CatListView { DataContext = _catlistVM };
            _catAssignView = new CatAssignView { DataContext = _catassignVM };

            SwitchToAssignView = new RelayCommand(_ => CurrentView = _catAssignView, p => true);
            SwitchToListView = new RelayCommand(_ => CurrentView = _catListView, p => true);

            CurrentView = _catListView;
        }
    }
}
